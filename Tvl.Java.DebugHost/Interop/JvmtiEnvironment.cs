namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Tvl.Java.DebugInterface.Types;
    using Interlocked = System.Threading.Interlocked;

    public sealed class JvmtiEnvironment
    {
        private static readonly ConcurrentDictionary<jvmtiEnvHandle, JvmtiEnvironment> _instances =
            new ConcurrentDictionary<jvmtiEnvHandle, JvmtiEnvironment>();

        private readonly JavaVM _virtualMachine;
        private readonly jvmtiEnvHandle _handle;
        private readonly jvmtiInterface _rawInterface;

        private JvmtiEnvironment(JavaVM virtualMachine, jvmtiEnvHandle handle)
        {
            Contract.Requires<ArgumentNullException>(virtualMachine != null, "virtualMachine");

            _virtualMachine = virtualMachine;
            _handle = handle;
            _rawInterface = (jvmtiInterface)Marshal.PtrToStructure(Marshal.ReadIntPtr(handle.Handle), typeof(jvmtiInterface));

            jvmtiCapabilities previousCapabilities;
            JvmtiErrorHandler.ThrowOnFailure(RawInterface.GetCapabilities(this, out previousCapabilities));

            jvmtiCapabilities potentialCapabilities;
            JvmtiErrorHandler.ThrowOnFailure(RawInterface.GetPotentialCapabilities(this, out potentialCapabilities));

            var capabilities =
                new jvmtiCapabilities(
                    jvmtiCapabilities.CapabilityFlags1.CanTagObjects
                    | jvmtiCapabilities.CapabilityFlags1.CanGetSourceFileName
                    | jvmtiCapabilities.CapabilityFlags1.CanGetLineNumbers
                    | jvmtiCapabilities.CapabilityFlags1.CanGetSourceDebugExtension
                    | jvmtiCapabilities.CapabilityFlags1.CanAccessLocalVariables
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateSingleStepEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateExceptionEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateBreakpointEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateFramePopEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateAllClassHookEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGetBytecodes
                    | jvmtiCapabilities.CapabilityFlags1.CanSuspend,
                    jvmtiCapabilities.CapabilityFlags2.CanGetConstantPool
                    );
            JvmtiErrorHandler.ThrowOnFailure(RawInterface.AddCapabilities(this, ref capabilities));

            jvmtiCapabilities newCapabilities;
            JvmtiErrorHandler.ThrowOnFailure(RawInterface.GetCapabilities(this, out newCapabilities));
        }

        internal JavaVM VirtualMachine
        {
            get
            {
                return _virtualMachine;
            }
        }

        internal jvmtiInterface RawInterface
        {
            get
            {
                return _rawInterface;
            }
        }

        public static implicit operator jvmtiEnvHandle(JvmtiEnvironment environment)
        {
            return environment._handle;
        }

        internal static JvmtiEnvironment GetOrCreateInstance(JavaVM virtualMachine, jvmtiEnvHandle handle)
        {
            return _instances.GetOrAdd(handle, i => CreateVirtualMachine(virtualMachine, i));
        }

        private static JvmtiEnvironment CreateVirtualMachine(JavaVM virtualMachine, jvmtiEnvHandle handle)
        {
            return new JvmtiEnvironment(virtualMachine, handle);
        }

        public jvmtiError Deallocate(IntPtr ptr)
        {
            return RawInterface.Deallocate(this, ptr);
        }

        public jvmtiError GetVersionNumber(out int version)
        {
            return RawInterface.GetVersionNumber(this, out version);
        }

        public jvmtiError GetSystemProperty(string name, out string value)
        {
            value = null;

            using (ModifiedUTF8StringData property = new ModifiedUTF8StringData(name))
            {
                IntPtr valuePtr;
                jvmtiError error = RawInterface.GetSystemProperty(this, property, out valuePtr);
                if (error != jvmtiError.None)
                    return error;

                unsafe
                {
                    if (valuePtr != IntPtr.Zero)
                    {
                        value = ModifiedUTF8Encoding.GetString((byte*)valuePtr);
                        Deallocate(valuePtr);
                    }
                }

                return jvmtiError.None;
            }
        }

        public jvmtiError GetSourceDebugExtension(jclass classHandle, out string extension)
        {
            extension = null;

            IntPtr sourceDebugExtensionPtr;
            jvmtiError error = RawInterface.GetSourceDebugExtension(this, classHandle, out sourceDebugExtensionPtr);
            if (error != jvmtiError.None)
                return error;

            if (sourceDebugExtensionPtr != IntPtr.Zero)
            {
                unsafe
                {
                    extension = ModifiedUTF8Encoding.GetString((byte*)sourceDebugExtensionPtr);
                    Deallocate(sourceDebugExtensionPtr);
                }
            }

            return jvmtiError.None;
        }

        public jvmtiError DisposeEnvironment()
        {
            return RawInterface.DisposeEnvironment(this);
        }

        public jvmtiError GetCurrentThread(JniEnvironment nativeEnvironment, out ThreadId thread)
        {
            thread = default(ThreadId);

            jthread threadPtr;
            jvmtiError error = RawInterface.GetCurrentThread(this, out threadPtr);
            if (error != jvmtiError.None)
                return error;

            thread = VirtualMachine.TrackLocalThreadReference(threadPtr, this, nativeEnvironment, true);
            return jvmtiError.None;
        }

        internal jvmtiError GetThreadInfo(jthread thread, out jvmtiThreadInfo threadInfo)
        {
            return RawInterface.GetThreadInfo(this, thread, out threadInfo);
        }

        public jvmtiError GetAllThreads(JniEnvironment nativeEnvironment, out ThreadId[] threads)
        {
            threads = null;

            int threadsCount;
            IntPtr threadsPtr;
            jvmtiError error = RawInterface.GetAllThreads(this, out threadsCount, out threadsPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                HashSet<ThreadId> threadSet = new HashSet<ThreadId>();
                unsafe
                {
                    jthread* threadHandles = (jthread*)threadsPtr;
                    for (int i = 0; i < threadsCount; i++)
                    {
                        if (threadHandles[i] == jthread.Null)
                            continue;

                        threadSet.Add(VirtualMachine.TrackLocalThreadReference(threadHandles[i], this, nativeEnvironment, true));
                    }
                }

                threadSet.ExceptWith(_virtualMachine.AgentThreads);
                threads = threadSet.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(threadsPtr);
            }
        }

        internal int GetSuspendCount(ThreadId threadId)
        {
            int result;
            VirtualMachine.SuspendCounts.TryGetValue(threadId, out result);
            return result;
        }

        public jvmtiError SuspendThread(JniEnvironment nativeEnvironment, ThreadId threadId)
        {
            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return jvmtiError.InvalidThread;

                int suspendCount = VirtualMachine.SuspendCounts.AddOrUpdate(threadId, 1, (existingId, existingValue) => existingValue + 1);
                if (suspendCount > 1)
                    return jvmtiError.None;

                jvmtiError error = RawInterface.SuspendThread(this, thread.Value);
                if (error != jvmtiError.None)
                    VirtualMachine.SuspendCounts.AddOrUpdate(threadId, 0, (existingId, existingValue) => existingValue - 1);

                return error;
            }
        }

        public jvmtiError SuspendThreads(JniEnvironment nativeEnvironment, ThreadId[] threads, out jvmtiError[] errors)
        {
            List<Tuple<int, LocalThreadReferenceHolder>> threadsToSuspend = new List<Tuple<int, LocalThreadReferenceHolder>>();

            errors = new jvmtiError[threads.Length];
            for (int i = 0; i < threads.Length; i++)
            {
                LocalThreadReferenceHolder thread;
                errors[i] = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threads[i], out thread);
                if (errors[i] == jvmtiError.None && thread.IsAlive)
                {
                    int suspendCount = VirtualMachine.SuspendCounts.AddOrUpdate(threads[i], 1, (existingId, existingValue) => existingValue + 1);
                    if (suspendCount == 1)
                        threadsToSuspend.Add(Tuple.Create(i, thread));
                }
            }

            if (threadsToSuspend.Count == 0)
                return jvmtiError.None;

            jthread[] requestList = threadsToSuspend.Select(i => i.Item2.Value).ToArray();
            jvmtiError[] intermediateErrors = new jvmtiError[requestList.Length];
            jvmtiError error = RawInterface.SuspendThreadList(this, requestList.Length, requestList, intermediateErrors);

            for (int i = 0; i < intermediateErrors.Length; i++)
            {
                errors[threadsToSuspend[i].Item1] = intermediateErrors[i];
                if (error != jvmtiError.None || intermediateErrors[i] != jvmtiError.None)
                    VirtualMachine.SuspendCounts.AddOrUpdate(threads[threadsToSuspend[i].Item1], 0, (existingId, existingValue) => existingValue - 1);
            }

            foreach (var referencePair in threadsToSuspend)
            {
                referencePair.Item2.Dispose();
            }

            return error;
        }

        public jvmtiError ResumeThread(JniEnvironment nativeEnvironment, ThreadId threadId)
        {
            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return jvmtiError.InvalidThread;

                int suspendCount = VirtualMachine.SuspendCounts.AddOrUpdate(threadId, 0, (existingId, existingValue) => Math.Max(0, existingValue - 1));
                if (suspendCount > 0)
                    return jvmtiError.None;

                jvmtiError error = RawInterface.ResumeThread(this, thread.Value);
                if (error != jvmtiError.None)
                    VirtualMachine.SuspendCounts.AddOrUpdate(threadId, 0, (existingId, existingValue) => existingValue + 1);

                return error;
            }
        }

        public jvmtiError ResumeThreads(JniEnvironment nativeEnvironment, ThreadId[] threads, out jvmtiError[] errors)
        {
            List<Tuple<int, LocalThreadReferenceHolder>> threadsToResume = new List<Tuple<int, LocalThreadReferenceHolder>>();

            errors = new jvmtiError[threads.Length];
            for (int i = 0; i < threads.Length; i++)
            {
                LocalThreadReferenceHolder thread;
                errors[i] = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threads[i], out thread);
                if (errors[i] == jvmtiError.None && thread.IsAlive)
                {
                    int suspendCount = VirtualMachine.SuspendCounts.AddOrUpdate(threads[i], 0, (existingId, existingValue) => existingValue - 1);
                    if (suspendCount == 0)
                        threadsToResume.Add(Tuple.Create(i, thread));
                }
            }

            if (threadsToResume.Count == 0)
                return jvmtiError.None;

            jthread[] requestList = threadsToResume.Select(i => i.Item2.Value).ToArray();
            jvmtiError[] intermediateErrors = new jvmtiError[requestList.Length];
            jvmtiError error = RawInterface.ResumeThreadList(this, requestList.Length, requestList, intermediateErrors);

            for (int i = 0; i < intermediateErrors.Length; i++)
            {
                errors[threadsToResume[i].Item1] = intermediateErrors[i];
                if (error != jvmtiError.None || intermediateErrors[i] != jvmtiError.None)
                    VirtualMachine.SuspendCounts.AddOrUpdate(threads[threadsToResume[i].Item1], 0, (existingId, existingValue) => existingValue + 1);
            }

            foreach (var referencePair in threadsToResume)
                referencePair.Item2.Dispose();

            return error;
        }

        public jvmtiError GetTopThreadGroups(JniEnvironment nativeEnvironment, out ThreadGroupId[] threadGroups)
        {
            threadGroups = null;

            int threadsGroupsCount;
            IntPtr threadsGroupsPtr;
            jvmtiError error = RawInterface.GetTopThreadGroups(this, out threadsGroupsCount, out threadsGroupsPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<ThreadGroupId> threadGroupList = new List<ThreadGroupId>();
                unsafe
                {
                    jthreadGroup* threadGroupHandles = (jthreadGroup*)threadsGroupsPtr;
                    for (int i = 0; i < threadsGroupsCount; i++)
                    {
                        if (threadGroupHandles[i] == jthreadGroup.Null)
                            continue;

                        threadGroupList.Add((ThreadGroupId)VirtualMachine.TrackLocalObjectReference(threadGroupHandles[i], this, nativeEnvironment, true));
                    }
                }

                threadGroups = threadGroupList.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(threadsGroupsPtr);
            }
        }

        public jvmtiError GetLoadedClasses(JniEnvironment nativeEnvironment, out TaggedReferenceTypeId[] classes)
        {
            classes = null;

            int classCount;
            IntPtr classesPtr;
            jvmtiError error = RawInterface.GetLoadedClasses(this, out classCount, out classesPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<TaggedReferenceTypeId> classList = new List<TaggedReferenceTypeId>();
                unsafe
                {
                    jclass* classHandles = (jclass*)classesPtr;
                    for (int i = 0; i < classCount; i++)
                    {
                        if (classHandles[i] == jclass.Null)
                            continue;

                        classList.Add(VirtualMachine.TrackLocalClassReference(classHandles[i], this, nativeEnvironment, true));
                    }
                }

                classes = classList.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(classesPtr);
            }
        }

        public jvmtiError GetClassStatus(jclass classHandle, out jvmtiClassStatus status)
        {
            return RawInterface.GetClassStatus(this, classHandle, out status);
        }

        public jvmtiError GetClassModifiers(JniEnvironment nativeEnvironment, ReferenceTypeId declaringType, out AccessModifiers modifiers)
        {
            modifiers = 0;

            using (var @class = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, declaringType))
            {
                if (!@class.IsAlive)
                    return jvmtiError.InvalidClass;

                JvmAccessModifiers modifiersPtr;
                jvmtiError error = RawInterface.GetClassModifiers(this, @class.Value, out modifiersPtr);
                if (error != jvmtiError.None)
                    return error;

                modifiers = (AccessModifiers)modifiersPtr;
                return jvmtiError.None;
            }
        }

        public jvmtiError GetClassVersionNumbers(jclass classHandle, out int minorVersion, out int majorVersion)
        {
            return RawInterface.GetClassVersionNumbers(this, classHandle, out minorVersion, out majorVersion);
        }

        public jvmtiError GetConstantPool(jclass classHandle, out int constantPoolCount, out byte[] data)
        {
            data = null;

            int constantPoolByteCount;
            IntPtr constantPoolBytesPtr;
            jvmtiError error = RawInterface.GetConstantPool(this, classHandle, out constantPoolCount, out constantPoolByteCount, out constantPoolBytesPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                data = new byte[constantPoolByteCount];
                Marshal.Copy(constantPoolBytesPtr, data, 0, constantPoolByteCount);
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(constantPoolBytesPtr);
            }
        }

        public jvmtiError GetConstantPool(jclass classHandle, out ConstantPoolEntry[] entries)
        {
            entries = null;

            int constantPoolCount;
            int constantPoolByteCount;
            IntPtr constantPoolBytesPtr;
            jvmtiError error = RawInterface.GetConstantPool(this, classHandle, out constantPoolCount, out constantPoolByteCount, out constantPoolBytesPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<ConstantPoolEntry> entryList = new List<ConstantPoolEntry>();
                IntPtr currentPosition = constantPoolBytesPtr;
                for (int i = 0; i < constantPoolCount; i++)
                {
                    entryList.Add(ConstantPoolEntry.FromMemory(ref currentPosition));
                }

                entries = entryList.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(constantPoolBytesPtr);
            }
        }

        public jvmtiError GetClassLoader(jclass @class, out jobject classLoader)
        {
            return RawInterface.GetClassLoader(this, @class, out classLoader);
        }

        public jvmtiError GetClassFields(JniEnvironment nativeEnvironment, ReferenceTypeId declaringType, out FieldId[] fields)
        {
            fields = null;

            using (var @class = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, declaringType))
            {
                if (!@class.IsAlive)
                    return jvmtiError.InvalidClass;

                int fieldsCount;
                IntPtr fieldsPtr;
                jvmtiError error = RawInterface.GetClassFields(this, @class.Value, out fieldsCount, out fieldsPtr);
                if (error != jvmtiError.None)
                    return error;

                try
                {
                    List<FieldId> fieldList = new List<FieldId>();
                    unsafe
                    {
                        jfieldID* fieldHandles = (jfieldID*)fieldsPtr;
                        for (int i = 0; i < fieldsCount; i++)
                        {
                            if (fieldHandles[i] == jfieldID.Null)
                                continue;

                            fieldList.Add(fieldHandles[i]);
                        }
                    }

                    fields = fieldList.ToArray();
                    return jvmtiError.None;
                }
                finally
                {
                    Deallocate(fieldsPtr);
                }
            }
        }

        public jvmtiError GetClassMethods(JniEnvironment nativeEnvironment, ReferenceTypeId declaringType, out MethodId[] methods)
        {
            methods = null;

            using (var @class = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, declaringType))
            {
                if (!@class.IsAlive)
                    return jvmtiError.InvalidClass;

                int methodsCount;
                IntPtr methodsPtr;
                jvmtiError error = RawInterface.GetClassMethods(this, @class.Value, out methodsCount, out methodsPtr);
                if (error != jvmtiError.None)
                    return error;

                try
                {
                    List<MethodId> methodList = new List<MethodId>();
                    unsafe
                    {
                        jmethodID* methodHandles = (jmethodID*)methodsPtr;
                        for (int i = 0; i < methodsCount; i++)
                        {
                            if (methodHandles[i] == jmethodID.Null)
                                continue;

                            methodList.Add(new MethodId(methodHandles[i].Handle));
                        }
                    }

                    methods = methodList.ToArray();
                    return jvmtiError.None;
                }
                finally
                {
                    Deallocate(methodsPtr);
                }
            }
        }

        public jvmtiError GetClassSignature(JniEnvironment nativeEnvironment, ReferenceTypeId classId, out string signature, out string genericSignature)
        {
            signature = null;
            genericSignature = null;

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, classId))
            {
                if (!classHandle.IsAlive)
                    return jvmtiError.InvalidClass;

                return GetClassSignature(classHandle.Value, out signature, out genericSignature);
            }
        }

        public jvmtiError GetClassSignature(jclass classHandle, out string signature, out string genericSignature)
        {
            signature = null;
            genericSignature = null;

            IntPtr signaturePtr;
            IntPtr genericPtr;
            RawInterface.GetClassSignature(this, classHandle, out signaturePtr, out genericPtr);

            try
            {
                unsafe
                {
                    if (signaturePtr != IntPtr.Zero)
                        signature = ModifiedUTF8Encoding.GetString((byte*)signaturePtr);
                    if (genericPtr != IntPtr.Zero)
                        genericSignature = ModifiedUTF8Encoding.GetString((byte*)genericPtr);
                }

                return jvmtiError.None;
            }
            finally
            {
                Deallocate(signaturePtr);
                Deallocate(genericPtr);
            }
        }

        public jvmtiError IsArrayClass(jclass classHandle, out bool result)
        {
            byte arrayClass;
            jvmtiError error = RawInterface.IsArrayClass(this, classHandle, out arrayClass);
            result = arrayClass != 0;
            return error;
        }

        public jvmtiError IsInterface(jclass classHandle, out bool result)
        {
            byte isInterface;
            jvmtiError error = RawInterface.IsInterface(this, classHandle, out isInterface);
            result = isInterface != 0;
            return error;
        }

        public jvmtiError GetObjectHashCode(jobject obj, out int result)
        {
            return RawInterface.GetObjectHashCode(this, obj, out result);
        }

        public jvmtiError GetImplementedInterfaces(JniEnvironment nativeEnvironment, jclass classHandle, out TaggedReferenceTypeId[] interfaces)
        {
            interfaces = null;

            int interfacesCount;
            IntPtr interfacesPtr;
            jvmtiError  error = RawInterface.GetImplementedInterfaces(this, classHandle, out interfacesCount, out interfacesPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<TaggedReferenceTypeId> interfaceList = new List<TaggedReferenceTypeId>();
                unsafe
                {
                    jclass* interfaceHandles = (jclass*)interfacesPtr;
                    for (int i = 0; i < interfacesCount; i++)
                    {
                        if (interfaceHandles[i] == jclass.Null)
                            continue;

                        interfaceList.Add(VirtualMachine.TrackLocalClassReference(interfaceHandles[i], this, nativeEnvironment, true));
                    }
                }

                interfaces = interfaceList.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(interfacesPtr);
            }
        }

        public jvmtiError GetFieldName(jclass classHandle, FieldId fieldId, out string name, out string signature, out string genericSignature)
        {
            name = null;
            signature = null;
            genericSignature = null;

            IntPtr namePtr;
            IntPtr signaturePtr;
            IntPtr genericPtr;
            jvmtiError error = RawInterface.GetFieldName(this, classHandle, fieldId, out namePtr, out signaturePtr, out genericPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                unsafe
                {
                    if (namePtr != IntPtr.Zero)
                        name = ModifiedUTF8Encoding.GetString((byte*)namePtr);
                    if (signaturePtr != IntPtr.Zero)
                        signature = ModifiedUTF8Encoding.GetString((byte*)signaturePtr);
                    if (genericPtr != IntPtr.Zero)
                        genericSignature = ModifiedUTF8Encoding.GetString((byte*)genericPtr);
                }
            }
            finally
            {
                Deallocate(namePtr);
                Deallocate(signaturePtr);
                Deallocate(genericPtr);
            }

            return jvmtiError.None;
        }

        public jvmtiError GetFieldModifiers(jclass classHandle, FieldId fieldId, out JvmAccessModifiers modifiers)
        {
            return RawInterface.GetFieldModifiers(this, classHandle, fieldId, out modifiers);
        }

        public jvmtiError GetFieldDeclaringClass(JniEnvironment nativeEnvironment, jclass classHandle, FieldId fieldId, out TaggedReferenceTypeId declaringClass)
        {
            declaringClass = default(TaggedReferenceTypeId);

            jclass declaringClassHandle;
            jvmtiError error = RawInterface.GetFieldDeclaringClass(this, classHandle, fieldId, out declaringClassHandle);
            if (error != jvmtiError.None)
                return error;

            declaringClass = VirtualMachine.TrackLocalClassReference(declaringClassHandle, this, nativeEnvironment, true);
            return jvmtiError.None;
        }

        public jvmtiError GetMethodName(MethodId methodId, out string name, out string signature, out string genericSignature)
        {
            name = null;
            signature = null;
            genericSignature = null;

            IntPtr namePtr;
            IntPtr signaturePtr;
            IntPtr genericPtr;
            jvmtiError error = RawInterface.GetMethodName(this, new jmethodID((IntPtr)methodId.Handle), out namePtr, out signaturePtr, out genericPtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                unsafe
                {
                    if (namePtr != IntPtr.Zero)
                        name = ModifiedUTF8Encoding.GetString((byte*)namePtr);
                    if (signaturePtr != IntPtr.Zero)
                        signature = ModifiedUTF8Encoding.GetString((byte*)signaturePtr);
                    if (genericPtr != IntPtr.Zero)
                        genericSignature = ModifiedUTF8Encoding.GetString((byte*)genericPtr);
                }
            }
            finally
            {
                Deallocate(namePtr);
                Deallocate(signaturePtr);
                Deallocate(genericPtr);
            }

            return jvmtiError.None;
        }

        public jvmtiError GetMethodModifiers(MethodId methodId, out JvmAccessModifiers modifiers)
        {
            return RawInterface.GetMethodModifiers(this, (jmethodID)methodId, out modifiers);
        }

        public jvmtiError IsMethodNative(MethodId methodId, out bool result)
        {
            result = false;

            byte native;
            jvmtiError error = RawInterface.IsMethodNative(this, methodId, out native);
            if (error != jvmtiError.None)
                return error;

            result = native != 0;
            return jvmtiError.None;
        }

        public jvmtiError GetBytecodes(MethodId methodId, out byte[] bytecode)
        {
            bytecode = null;

            int bytecodeCount;
            IntPtr bytecodePtr;
            jvmtiError error = RawInterface.GetBytecodes(this, methodId, out bytecodeCount, out bytecodePtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                if (bytecodeCount > 0)
                {
                    bytecode = new byte[bytecodeCount];
                    Marshal.Copy(bytecodePtr, bytecode, 0, bytecodeCount);
                }
            }
            finally
            {
                Deallocate(bytecodePtr);
            }

            return jvmtiError.None;
        }

        public jvmtiError GetMethodDeclaringClass(JniEnvironment nativeEnvironment, MethodId methodId, out TaggedReferenceTypeId declaringClass)
        {
            declaringClass = default(TaggedReferenceTypeId);

            jclass classHandle;
            jvmtiError error = RawInterface.GetMethodDeclaringClass(this, (jmethodID)methodId, out classHandle);
            if (error != jvmtiError.None)
                return error;

            declaringClass = VirtualMachine.TrackLocalClassReference(classHandle, this, nativeEnvironment, true);
            return jvmtiError.None;
        }

        public jvmtiError GetMethodLocation(MethodId methodId, out jlocation startLocation, out jlocation endLocation)
        {
            return RawInterface.GetMethodLocation(this, (jmethodID)methodId, out startLocation, out endLocation);
        }

        public jvmtiError GetLineNumberTable(MethodId methodId, out LineNumberData[] lines)
        {
            lines = null;

            int entryCount;
            IntPtr table;
            jvmtiError error = RawInterface.GetLineNumberTable(this, (jmethodID)methodId, out entryCount, out table);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<LineNumberData> lineData = new List<LineNumberData>();
                unsafe
                {
                    jvmtiLineNumberEntry* entryTable = (jvmtiLineNumberEntry*)table;
                    for (int i = 0; i < entryCount; i++)
                    {
                        long lineCodeIndex = entryTable[i].StartLocation.Value;
                        LineNumberData line = new LineNumberData(lineCodeIndex, entryTable[i].LineNumber);
                        lineData.Add(line);
                    }
                }

                lines = lineData.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(table);
            }
        }

        public jvmtiError GetLocalVariableTable(MethodId methodId, out VariableData[] variables)
        {
            variables = null;

            int entryCountPtr;
            IntPtr tablePtr;
            jvmtiError error = RawInterface.GetLocalVariableTable(this, methodId, out entryCountPtr, out tablePtr);
            if (error != jvmtiError.None)
                return error;

            try
            {
                List<VariableData> variableData = new List<VariableData>();
                unsafe
                {
                    jvmtiLocalVariableEntry* entryTable = (jvmtiLocalVariableEntry*)tablePtr;
                    for (int i = 0; i < entryCountPtr; i++)
                    {
                        VariableData data = new VariableData(entryTable[i].Slot, (ulong)entryTable[i].StartLocation.Value, (uint)entryTable[i].Length, entryTable[i].Name, entryTable[i].Signature, entryTable[i].GenericSignature);
                        variableData.Add(data);

                        Deallocate(entryTable[i]._name);
                        Deallocate(entryTable[i]._signature);
                        Deallocate(entryTable[i]._genericSignature);
                    }
                }

                variables = variableData.ToArray();
                return jvmtiError.None;
            }
            finally
            {
                Deallocate(tablePtr);
            }
        }

        public jvmtiError GetSourceFileName(JniEnvironment nativeEnvironment, ReferenceTypeId classId, out string sourceName)
        {
            sourceName = null;

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, classId))
            {
                if (!classHandle.IsAlive)
                    return jvmtiError.InvalidClass;

                IntPtr sourceNamePtr;
                jvmtiError error = RawInterface.GetSourceFileName(this, classHandle.Value, out sourceNamePtr);
                if (error != jvmtiError.None)
                    return error;

                try
                {
                    unsafe
                    {
                        if (sourceNamePtr != IntPtr.Zero)
                            sourceName = ModifiedUTF8Encoding.GetString((byte*)sourceNamePtr);
                    }

                    return jvmtiError.None;
                }
                finally
                {
                    Deallocate(sourceNamePtr);
                }
            }
        }

        public jvmtiError GetStackTrace(JniEnvironment nativeEnvironment, ThreadId threadId, int startDepth, int maxFrameCount, out FrameLocationData[] frames)
        {
            frames = null;

            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return jvmtiError.InvalidThread;

                jvmtiFrameInfo[] frameBuffer = new jvmtiFrameInfo[maxFrameCount];
                int frameCount;
                jvmtiError error = RawInterface.GetStackTrace(this, thread.Value, startDepth, maxFrameCount, frameBuffer, out frameCount);
                if (error != jvmtiError.None)
                    return error;

                var frameData = new FrameLocationData[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    TaggedReferenceTypeId declaringClass;
                    MethodId method = new MethodId(frameBuffer[i]._method.Handle);
                    ulong index = (ulong)frameBuffer[i]._location.Value;
                    error = GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass);
                    if (error != jvmtiError.None)
                        return error;

                    FrameId frameId = new FrameId(startDepth + i);
                    Location location = new Location(declaringClass, method, index);
                    frameData[i] = new FrameLocationData(frameId, location);
                }

                frames = frameData;
                return jvmtiError.None;
            }
        }

        public jvmtiError GetFrameCount(JniEnvironment nativeEnvironment, ThreadId threadId, out int frameCount)
        {
            frameCount = 0;

            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return jvmtiError.InvalidThread;

                return RawInterface.GetFrameCount(this, thread.Value, out frameCount);
            }
        }

        public jvmtiError GetFrameCount(jthread thread, out int frameCount)
        {
            return RawInterface.GetFrameCount(this, thread, out frameCount);
        }

        public jvmtiError GetFrameLocation(jthread thread, int depth, out jmethodID method, out jlocation location)
        {
            return RawInterface.GetFrameLocation(this, thread, depth, out method, out location);
        }

        internal jvmtiError SetBreakpoint(jmethodID methodId, jlocation location)
        {
            return RawInterface.SetBreakpoint(this, methodId, location);
        }

        internal jvmtiError ClearBreakpoint(jmethodID methodId, jlocation location)
        {
            return RawInterface.ClearBreakpoint(this, methodId, location);
        }

        internal jvmtiError SetEventCallbacks(jvmtiEventCallbacks callbacks)
        {
            return RawInterface.SetEventCallbacks(this, ref callbacks, Marshal.SizeOf(callbacks));
        }

        public jvmtiError SetEventNotificationMode(JvmEventMode mode, JvmEventType eventType)
        {
            return RawInterface.SetEventNotificationMode(this, mode, eventType, jthread.Null);
        }

        public jvmtiError GetCapabilities(out jvmtiCapabilities capabilities)
        {
            return RawInterface.GetCapabilities(this, out capabilities);
        }

        public jvmtiError GetLocalInt(jthread thread, int depth, int slot, out int value)
        {
            return RawInterface.GetLocalInt(this, thread, depth, slot, out value);
        }

        public jvmtiError GetLocalLong(jthread thread, int depth, int slot, out long value)
        {
            return RawInterface.GetLocalLong(this, thread, depth, slot, out value);
        }

        public jvmtiError GetLocalFloat(jthread thread, int depth, int slot, out float value)
        {
            return RawInterface.GetLocalFloat(this, thread, depth, slot, out value);
        }

        public jvmtiError GetLocalDouble(jthread thread, int depth, int slot, out double value)
        {
            return RawInterface.GetLocalDouble(this, thread, depth, slot, out value);
        }

        public jvmtiError GetLocalObject(jthread thread, int depth, int slot, out jobject value)
        {
            return RawInterface.GetLocalObject(this, thread, depth, slot, out value);
        }

        public jvmtiError GetLocalObject(JniEnvironment nativeEnvironment, jthread thread, int depth, int slot, out TaggedObjectId value)
        {
            value = default(TaggedObjectId);

            jobject obj;
            jvmtiError error = RawInterface.GetLocalObject(this, thread, depth, slot, out obj);
            if (error != jvmtiError.None)
                return error;

            value = VirtualMachine.TrackLocalObjectReference(obj, this, nativeEnvironment, true);
            return jvmtiError.None;
        }

        public jvmtiError GetTag(jobject objectHandle, out long tag)
        {
            return RawInterface.GetTag(this, objectHandle, out tag);
        }

        public jvmtiError SetTag(jobject objectHandle, long tag)
        {
            return RawInterface.SetTag(this, objectHandle, tag);
        }

        private long _nextClassLoaderTag = 1;

        public jvmtiError TagClassLoader(jobject classLoaderHandle, out long tag)
        {
            if (classLoaderHandle == jobject.Null)
            {
                tag = 0;
                return jvmtiError.None;
            }

            var error = GetTag(classLoaderHandle, out tag);
            if (error != jvmtiError.None)
                return error;

            if (tag != 0)
                return jvmtiError.None;

            tag = Interlocked.Increment(ref _nextClassLoaderTag);
            error = SetTag(classLoaderHandle, tag);
            return error;
        }
    }
}
