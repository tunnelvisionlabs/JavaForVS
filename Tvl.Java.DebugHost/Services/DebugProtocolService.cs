namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.ServiceModel;
    using Tvl.Collections;
    using Tvl.Java.DebugHost.Interop;
    using Tvl.Java.DebugInterface.Types;
    using ExceptionTableEntry = Tvl.Java.DebugInterface.Types.Loader.ExceptionTableEntry;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true)]
    public partial class DebugProtocolService : IDebugProtocolService
    {
        private readonly JavaVM _virtualMachine;
        private readonly EventProcessor _eventProcessor;
        private readonly JvmtiEnvironment _environment;

        private IDebugProcotolCallback _callback;

        public DebugProtocolService()
        {
            // this is used for metadata only
        }

        public DebugProtocolService(JavaVM virtualMachine)
        {
            Contract.Requires<ArgumentNullException>(virtualMachine != null, "virtualMachine");

            _virtualMachine = virtualMachine;
            _eventProcessor = new EventProcessor(this);

            JniErrorHandler.ThrowOnFailure(_virtualMachine.GetEnvironment(out _environment));

            jvmtiCapabilities previousCapabilities;
            JvmtiErrorHandler.ThrowOnFailure(_environment.GetCapabilities(out previousCapabilities));

            var capabilities =
                new jvmtiCapabilities(
                    jvmtiCapabilities.CapabilityFlags1.CanTagObjects
                    | jvmtiCapabilities.CapabilityFlags1.CanGetSyntheticAttribute
                    | jvmtiCapabilities.CapabilityFlags1.CanGetSourceFileName
                    | jvmtiCapabilities.CapabilityFlags1.CanGetLineNumbers
                    | jvmtiCapabilities.CapabilityFlags1.CanGetSourceDebugExtension
                    | jvmtiCapabilities.CapabilityFlags1.CanAccessLocalVariables
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateSingleStepEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateExceptionEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGenerateBreakpointEvents
                    | jvmtiCapabilities.CapabilityFlags1.CanGetBytecodes
                    | jvmtiCapabilities.CapabilityFlags1.CanSuspend
                    );
            JvmtiErrorHandler.ThrowOnFailure(RawInterface.AddCapabilities(_environment, ref capabilities));
        }

        internal JvmtiEnvironment Environment
        {
            get
            {
                return _environment;
            }
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
                return _environment.RawInterface;
            }
        }

        public Error Attach()
        {
            if (_callback != null)
                return Error.Duplicate;

            _callback = OperationContext.Current.GetCallbackChannel<IDebugProcotolCallback>();
            _eventProcessor.Attach();
            AgentExports.DebuggerAttachComplete.Set();
            return Error.None;
        }

        public Error GetVersion(out string description, out int majorVersion, out int minorVersion, out string vmVersion, out string vmName)
        {
            description = "Java Debug Protocol Service";
            majorVersion = 1;
            minorVersion = 0;
            vmVersion = null;
            vmName = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = Environment.GetSystemProperty("java.version", out vmVersion);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = Environment.GetSystemProperty("java.vm.name", out vmName);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            return Error.None;
        }

        public Error GetClassesBySignature(string signature, out ReferenceTypeData[] classes)
        {
            classes = null;

            ReferenceTypeData[] tempClasses;
            Error result = GetAllClasses(out tempClasses);
            if (result != Error.None)
                return result;

            classes = tempClasses.Where(i => i.Signature == signature).ToArray();
            return Error.None;
        }

        public Error GetAllClasses(out ReferenceTypeData[] classes)
        {
            classes = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            TaggedReferenceTypeId[] taggedClasses;
            error = environment.GetLoadedClasses(nativeEnvironment, out taggedClasses);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            List<ReferenceTypeData> dataList = new List<ReferenceTypeData>();
            for (int i = 0; i < taggedClasses.Length; i++)
            {
                ClassStatus status;
                Error error2 = GetReferenceTypeStatus(taggedClasses[i].TypeId, out status);
                if (error2 != Error.None)
                    return error2;

                string signature;
                string genericSignature;
                error2 = GetSignature(taggedClasses[i].TypeId, out signature, out genericSignature);
                if (error2 != Error.None)
                    return error2;

                ReferenceTypeData data = new ReferenceTypeData(taggedClasses[i], signature, genericSignature, status);
                dataList.Add(data);
            }

            classes = dataList.ToArray();
            return Error.None;
        }

        public Error GetAllThreads(out ThreadId[] threads)
        {
            threads = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetAllThreads(nativeEnvironment, out threads);
            return GetStandardError(error);
        }

        public Error GetTopLevelThreadGroups(out ThreadGroupId[] groups)
        {
            groups = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetTopThreadGroups(nativeEnvironment, out groups);
            return GetStandardError(error);
        }

        public Error Dispose()
        {
            throw new NotImplementedException();
        }

        public Error Suspend()
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            ThreadId[] threads;
            error = environment.GetAllThreads(nativeEnvironment, out threads);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jvmtiError[] errors;
            error = environment.SuspendThreads(nativeEnvironment, threads, out errors);

            return GetStandardError(error);
        }

        public Error Resume()
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            ThreadId[] threads;
            error = environment.GetAllThreads(nativeEnvironment, out threads);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jvmtiError[] errors;
            error = environment.ResumeThreads(nativeEnvironment, threads, out errors);

            return GetStandardError(error);
        }

        public Error Exit(int exitCode)
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = Environment.DisposeEnvironment();
            return GetStandardError(error);
        }

        public Error CreateString(string value, out StringId stringObject)
        {
            stringObject = default(StringId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jobject obj = nativeEnvironment.NewString(value);
            stringObject = (StringId)VirtualMachine.TrackLocalObjectReference(obj, environment, nativeEnvironment, false);
            VirtualMachine.AddGlobalReference(environment, nativeEnvironment, obj);
            nativeEnvironment.DeleteLocalReference(obj);
            return Error.None;
        }

        public Error GetCapabilities(out Capabilities capabilities)
        {
            capabilities = default(Capabilities);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jvmtiCapabilities jvmCapabilities;
            error = Environment.GetCapabilities(out jvmCapabilities);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            ulong capabilityValue = (ulong)jvmCapabilities.Capabilities1 | ((ulong)jvmCapabilities.Capabilities2 << 32);
            capabilities = (Capabilities)capabilityValue
                | Capabilities.CanStepByStatement
                | Capabilities.CanInvokeWithoutThread;
            return GetStandardError(error);
        }

        public Error GetClassPaths(out string baseDirectory, out string[] classPaths, out string[] bootClassPaths)
        {
            throw new NotImplementedException();
        }

        public Error DisposeObjects(ObjectReferenceCountData[] requests)
        {
            throw new NotImplementedException();
        }

        public Error HoldEvents()
        {
            throw new NotImplementedException();
        }

        public Error ReleaseEvents()
        {
            throw new NotImplementedException();
        }

        public Error RedefineClasses(ClassDefinitionData[] definitions)
        {
            throw new NotImplementedException();
        }

        public Error SetDefaultStratum(string stratumId)
        {
            throw new NotImplementedException();
        }

        public Error GetSignature(ReferenceTypeId referenceType, out string signature, out string genericSignature)
        {
            signature = null;
            genericSignature = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetClassSignature(nativeEnvironment, referenceType, out signature, out genericSignature);
            return GetStandardError(error);
        }

        public Error GetClassLoader(ReferenceTypeId referenceType, out ClassLoaderId classLoader)
        {
            classLoader = default(ClassLoaderId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                jobject classLoaderObject;
                error = Environment.GetClassLoader(classHandle.Value, out classLoaderObject);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                classLoader = (ClassLoaderId)VirtualMachine.TrackLocalObjectReference(classLoaderObject, environment, nativeEnvironment, true);
                return Error.None;
            }
        }

        public Error GetModifiers(ReferenceTypeId referenceType, out AccessModifiers modifiers)
        {
            modifiers = default(AccessModifiers);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetClassModifiers(nativeEnvironment, referenceType, out modifiers);
            return GetStandardError(error);
        }

        public Error GetFields(ReferenceTypeId referenceType, out DeclaredFieldData[] fields)
        {
            fields = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            FieldId[] fieldIds;
            error = environment.GetClassFields(nativeEnvironment, referenceType, out fieldIds);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                List<DeclaredFieldData> fieldsList = new List<DeclaredFieldData>();
                foreach (var fieldId in fieldIds)
                {
                    string name;
                    string signature;
                    string genericSignature;
                    error = environment.GetFieldName(classHandle.Value, fieldId, out name, out signature, out genericSignature);
                    if (error != jvmtiError.None)
                        return GetStandardError(error);

                    JvmAccessModifiers modifiers;
                    error = environment.GetFieldModifiers(classHandle.Value, fieldId, out modifiers);

                    fieldsList.Add(new DeclaredFieldData(fieldId, name, signature, genericSignature, (AccessModifiers)modifiers));
                }

                fields = fieldsList.ToArray();
                return Error.None;
            }
        }

        public Error GetMethods(ReferenceTypeId referenceType, out DeclaredMethodData[] methods)
        {
            methods = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            MethodId[] methodIds;
            error = environment.GetClassMethods(nativeEnvironment, referenceType, out methodIds);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            List<DeclaredMethodData> methodsList = new List<DeclaredMethodData>();
            foreach (var methodId in methodIds)
            {
                string name;
                string signature;
                string genericSignature;
                error = environment.GetMethodName(methodId, out name, out signature, out genericSignature);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                JvmAccessModifiers modifiers;
                error = environment.GetMethodModifiers(methodId, out modifiers);

                methodsList.Add(new DeclaredMethodData(methodId, name, signature, genericSignature, (AccessModifiers)modifiers));
            }

            methods = methodsList.ToArray();
            return Error.None;
        }

        [HandleProcessCorruptedStateExceptions]
        public Error GetReferenceTypeValues(ReferenceTypeId referenceType, FieldId[] fields, out Value[] values)
        {
            values = null;

            try
            {
                JniEnvironment nativeEnvironment;
                JvmtiEnvironment environment;
                jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                using (LocalClassReferenceHolder classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
                {
                    if (!classHandle.IsAlive)
                        return Error.InvalidClass;

                    Value[] valuesArray = new Value[fields.Length];
                    for (int i = 0; i < valuesArray.Length; i++)
                    {
                        string name;
                        string signature;
                        string genericSignature;

                        error = environment.GetFieldName(classHandle.Value, fields[i], out name, out signature, out genericSignature);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        switch (signature[0])
                        {
                        case 'Z':
                            valuesArray[i] = nativeEnvironment.GetStaticBooleanField(classHandle.Value, fields[i]);
                            break;

                        case 'B':
                            valuesArray[i] = nativeEnvironment.GetStaticByteField(classHandle.Value, fields[i]);
                            break;

                        case 'C':
                            valuesArray[i] = nativeEnvironment.GetStaticCharField(classHandle.Value, fields[i]);
                            break;

                        case 'D':
                            valuesArray[i] = nativeEnvironment.GetStaticDoubleField(classHandle.Value, fields[i]);
                            break;

                        case 'F':
                            valuesArray[i] = nativeEnvironment.GetStaticFloatField(classHandle.Value, fields[i]);
                            break;

                        case 'I':
                            valuesArray[i] = nativeEnvironment.GetStaticIntField(classHandle.Value, fields[i]);
                            break;

                        case 'J':
                            valuesArray[i] = nativeEnvironment.GetStaticLongField(classHandle.Value, fields[i]);
                            break;

                        case 'S':
                            valuesArray[i] = nativeEnvironment.GetStaticShortField(classHandle.Value, fields[i]);
                            break;

                        case 'V':
                            return Error.InvalidFieldid;

                        case '[':
                        case 'L':
                            jobject value = nativeEnvironment.GetStaticObjectField(classHandle.Value, fields[i]);
                            valuesArray[i] = VirtualMachine.TrackLocalObjectReference(value, environment, nativeEnvironment, true);
                            break;

                        default:
                            throw new FormatException();
                        }
                    }

                    values = valuesArray;
                    return Error.None;
                }
            }
            catch (Exception)
            {
                return Error.Internal;
            }
        }

        public Error GetSourceFile(ReferenceTypeId referenceType, out string sourceFile)
        {
            sourceFile = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = Environment.GetSourceFileName(nativeEnvironment, referenceType, out sourceFile);
            return GetStandardError(error);
        }

        public Error GetNestedTypes(ReferenceTypeId referenceType, out TaggedReferenceTypeId[] classes)
        {
            throw new NotImplementedException();
        }

        public Error GetReferenceTypeStatus(ReferenceTypeId referenceType, out ClassStatus status)
        {
            status = default(ClassStatus);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                jvmtiClassStatus classStatus;
                error = environment.GetClassStatus(classHandle.Value, out classStatus);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                status = (ClassStatus)((int)classStatus & 0xF);
                return Error.None;
            }
        }

        public Error GetInterfaces(ReferenceTypeId referenceType, out InterfaceId[] interfaces)
        {
            interfaces = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                TaggedReferenceTypeId[] taggedInterfaces;
                error = environment.GetImplementedInterfaces(nativeEnvironment, classHandle.Value, out taggedInterfaces);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                interfaces = Array.ConvertAll(taggedInterfaces, i => (InterfaceId)i);
                return Error.None;
            }
        }

        public Error GetClassObject(ReferenceTypeId referenceType, out ClassObjectId classObject)
        {
            throw new NotImplementedException();
        }

        public Error GetSourceDebugExtension(ReferenceTypeId referenceType, out string extension)
        {
            extension = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                error = environment.GetSourceDebugExtension(classHandle.Value, out extension);
                return GetStandardError(error);
            }
        }

        public Error GetInstances(ReferenceTypeId referenceType, int maxInstances, out TaggedObjectId[] instances)
        {
            throw new NotImplementedException();
        }

        public Error GetClassFileVersion(ReferenceTypeId referenceType, out int majorVersion, out int minorVersion)
        {
            majorVersion = 0;
            minorVersion = 0;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                error = environment.GetClassVersionNumbers(classHandle.Value, out minorVersion, out majorVersion);
                return GetStandardError(error);
            }
        }

        public Error GetConstantPool(ReferenceTypeId referenceType, out int constantPoolCount, out byte[] data)
        {
            constantPoolCount = 0;
            data = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                error = environment.GetConstantPool(classHandle.Value, out constantPoolCount, out data);
                return GetStandardError(error);
            }

            throw new NotImplementedException();
        }

        public Error GetSuperclass(ClassId classId, out ClassId superclass)
        {
            superclass = default(ClassId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, classId))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                jclass superclassHandle = nativeEnvironment.GetSuperclass(classHandle.Value);
                superclass = (ClassId)VirtualMachine.TrackLocalClassReference(superclassHandle, environment, nativeEnvironment, true);
                return Error.None;
            }
        }

        public Error SetClassValues(ClassId @class, FieldId[] fields, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error InvokeClassMethod(ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments, out Value returnValue, out TaggedObjectId thrownException)
        {
            returnValue = default(Value);
            thrownException = default(TaggedObjectId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            string name;
            string signature;
            string genericSignature;
            error = environment.GetMethodName(method, out name, out signature, out genericSignature);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            if (thread != default(ThreadId))
                throw new NotImplementedException();

            List<string> argumentTypeSignatures;
            string returnTypeSignature;
            SignatureHelper.ParseMethodSignature(signature, out argumentTypeSignatures, out returnTypeSignature);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, @class))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                // don't do argument conversion if the signature is invalid
                switch (returnTypeSignature[0])
                {
                case 'Z':
                case 'B':
                case 'C':
                case 'D':
                case 'F':
                case 'I':
                case 'J':
                case 'S':
                case '[':
                case 'L':
                    break;

                case 'V':
                default:
                    return Error.InvalidMethodid;
                }

                jvalue[] args = arguments.Select(value => new jvalue(VirtualMachine, environment, nativeEnvironment, value)).ToArray();

                switch (returnTypeSignature[0])
                {
                case 'Z':
                    returnValue = nativeEnvironment.CallStaticBooleanMethodA(classHandle.Value, method, args);
                    break;

                case 'B':
                    returnValue = nativeEnvironment.CallStaticByteMethodA(classHandle.Value, method, args);
                    break;

                case 'C':
                    returnValue = nativeEnvironment.CallStaticCharMethodA(classHandle.Value, method, args);
                    break;

                case 'D':
                    returnValue = nativeEnvironment.CallStaticDoubleMethodA(classHandle.Value, method, args);
                    break;

                case 'F':
                    returnValue = nativeEnvironment.CallStaticFloatMethodA(classHandle.Value, method, args);
                    break;

                case 'I':
                    returnValue = nativeEnvironment.CallStaticIntMethodA(classHandle.Value, method, args);
                    break;

                case 'J':
                    returnValue = nativeEnvironment.CallStaticLongMethodA(classHandle.Value, method, args);
                    break;

                case 'S':
                    returnValue = nativeEnvironment.CallStaticShortMethodA(classHandle.Value, method, args);
                    break;

                case '[':
                case 'L':
                    jobject result = nativeEnvironment.CallStaticObjectMethodA(classHandle.Value, method, args);
                    returnValue = VirtualMachine.TrackLocalObjectReference(result, environment, nativeEnvironment, false);
                    VirtualMachine.AddGlobalReference(environment, nativeEnvironment, result);
                    nativeEnvironment.DeleteLocalReference(result);
                    break;

                case 'V':
                default:
                    Contract.Assert(false, "not reachable");
                    break;
                }

                if (nativeEnvironment.ExceptionOccurred() != jthrowable.Null)
                {
                    throw new NotImplementedException();
                }

                for (int i = 0; i < arguments.Length; i++)
                {
                    switch (arguments[i].Tag)
                    {
                    case Tag.Array:
                    case Tag.Object:
                    case Tag.String:
                    case Tag.Thread:
                    case Tag.ThreadGroup:
                    case Tag.ClassLoader:
                    case Tag.ClassObject:
                        nativeEnvironment.DeleteLocalReference(args[i].ObjectValue);
                        break;

                    default:
                        break;
                    }
                }

                return Error.None;
            }
        }

        public Error CreateClassInstance(ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments, out TaggedObjectId newObject, out TaggedObjectId thrownException)
        {
            throw new NotImplementedException();
        }

        public Error CreateArrayInstance(ArrayTypeId arrayType, int length, out TaggedObjectId newArray)
        {
            newArray = default(TaggedObjectId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var arrayTypeHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, arrayType))
            {
                if (!arrayTypeHandle.IsAlive)
                    return Error.InvalidClass;

                string signature;
                string genericSignature;
                error = environment.GetClassSignature(arrayTypeHandle.Value, out signature, out genericSignature);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                jobject array;
                switch (signature[1])
                {
                case 'Z':
                    array = nativeEnvironment.NewBooleanArray(length);
                    break;

                case 'B':
                    array = nativeEnvironment.NewByteArray(length);
                    break;

                case 'C':
                    array = nativeEnvironment.NewCharArray(length);
                    break;

                case 'D':
                    array = nativeEnvironment.NewDoubleArray(length);
                    break;

                case 'F':
                    array = nativeEnvironment.NewFloatArray(length);
                    break;

                case 'I':
                    array = nativeEnvironment.NewIntegerArray(length);
                    break;

                case 'J':
                    array = nativeEnvironment.NewLongArray(length);
                    break;

                case 'S':
                    array = nativeEnvironment.NewShortArray(length);
                    break;

                case '[':
                case 'L':
                    throw new NotImplementedException();
                    //array = nativeEnvironment.NewObjectArray(length, elementType, jobject.Null);
                    //break;

                case 'V':
                default:
                    throw new FormatException();
                }

                if (array == jobject.Null)
                    throw new NotImplementedException();

                newArray = VirtualMachine.TrackLocalObjectReference(array, environment, nativeEnvironment, false);
                VirtualMachine.AddGlobalReference(environment, nativeEnvironment, array);
                nativeEnvironment.DeleteLocalReference(array);
                return Error.None;
            }
        }

        public Error GetMethodExceptionTable(ReferenceTypeId referenceType, MethodId methodId, out ExceptionTableEntry[] entries)
        {
            entries = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, referenceType))
            {
                if (!classHandle.IsAlive)
                    return Error.InvalidClass;

                bool native;
                error = environment.IsMethodNative(methodId, out native);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                if (native)
                    return Error.NativeMethod;

                JvmAccessModifiers modifiers;
                error = environment.GetMethodModifiers(methodId, out modifiers);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                if ((modifiers & JvmAccessModifiers.Abstract) != 0)
                    return Error.AbsentInformation;

                string classSignature;
                string classGenericSignature;
                error = environment.GetClassSignature(classHandle.Value, out classSignature, out classGenericSignature);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                string methodName;
                string methodSignature;
                string methodGenericSignature;
                error = environment.GetMethodName(methodId, out methodName, out methodSignature, out methodGenericSignature);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                jobject classLoader;
                error = environment.GetClassLoader(classHandle.Value, out classLoader);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                long classLoaderTag;
                error = environment.TagClassLoader(classLoader, out classLoaderTag);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                ReadOnlyCollection<ExceptionTableEntry> exceptionTable;
                error = environment.VirtualMachine.GetExceptionTable(classLoaderTag, classSignature, methodName, methodSignature, out exceptionTable);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                entries = exceptionTable.ToArray();
                return Error.None;
            }
        }

        public Error GetMethodLineTable(ReferenceTypeId referenceType, MethodId methodId, out long start, out long end, out LineNumberData[] lines)
        {
            start = 0;
            end = 0;
            lines = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetLineNumberTable(methodId, out lines);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jlocation startLocation;
            jlocation endLocation;
            error = environment.GetMethodLocation(methodId, out startLocation, out endLocation);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            start = startLocation.Value;
            end = endLocation.Value;
            return Error.None;
        }

        public Error GetMethodVariableTable(ReferenceTypeId referenceType, MethodId method, out VariableData[] slots)
        {
            slots = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetLocalVariableTable(method, out slots);
            return GetStandardError(error);
        }

        public Error GetMethodBytecodes(ReferenceTypeId referenceType, MethodId method, out byte[] bytecode)
        {
            bytecode = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetBytecodes(method, out bytecode);
            return GetStandardError(error);
        }

        public Error GetMethodIsObsolete(ReferenceTypeId referenceType, MethodId method, out bool result)
        {
            throw new NotImplementedException();
        }

        public Error GetObjectReferenceType(ObjectId objectId, out TypeTag typeTag, out ReferenceTypeId typeId)
        {
            typeTag = default(TypeTag);
            typeId = default(ReferenceTypeId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, objectId))
            {
                if (!objectHandle.IsAlive)
                    return Error.InvalidObject;

                jclass @class = nativeEnvironment.GetObjectClass(objectHandle.Value);
                TaggedReferenceTypeId result = VirtualMachine.TrackLocalClassReference(@class, environment, nativeEnvironment, true);
                typeTag = result.TypeTag;
                typeId = result.TypeId;
                return Error.None;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public Error GetObjectValues(ObjectId @object, FieldId[] fields, out Value[] values)
        {
            values = null;

            try
            {
                JniEnvironment nativeEnvironment;
                JvmtiEnvironment environment;
                jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                using (LocalObjectReferenceHolder objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, @object))
                {
                    if (!objectHandle.IsAlive)
                        return Error.InvalidObject;

                    Value[] valuesArray = new Value[fields.Length];
                    for (int i = 0; i < valuesArray.Length; i++)
                    {
                        string name;
                        string signature;
                        string genericSignature;

                        using (LocalClassReferenceHolder declaringType = new LocalClassReferenceHolder(nativeEnvironment, nativeEnvironment.GetObjectClass(objectHandle.Value)))
                        {
                            error = environment.GetFieldName(declaringType.Value, fields[i], out name, out signature, out genericSignature);
                            if (error != jvmtiError.None)
                                return GetStandardError(error);

                            switch (signature[0])
                            {
                            case 'Z':
                                valuesArray[i] = nativeEnvironment.GetBooleanField(objectHandle.Value, fields[i]);
                                break;

                            case 'B':
                                valuesArray[i] = nativeEnvironment.GetByteField(objectHandle.Value, fields[i]);
                                break;

                            case 'C':
                                valuesArray[i] = nativeEnvironment.GetCharField(objectHandle.Value, fields[i]);
                                break;

                            case 'D':
                                valuesArray[i] = nativeEnvironment.GetDoubleField(objectHandle.Value, fields[i]);
                                break;

                            case 'F':
                                valuesArray[i] = nativeEnvironment.GetFloatField(objectHandle.Value, fields[i]);
                                break;

                            case 'I':
                                valuesArray[i] = nativeEnvironment.GetIntField(objectHandle.Value, fields[i]);
                                break;

                            case 'J':
                                valuesArray[i] = nativeEnvironment.GetLongField(objectHandle.Value, fields[i]);
                                break;

                            case 'S':
                                valuesArray[i] = nativeEnvironment.GetShortField(objectHandle.Value, fields[i]);
                                break;

                            case 'V':
                                return Error.InvalidFieldid;

                            case '[':
                            case 'L':
                                jobject value = nativeEnvironment.GetObjectField(objectHandle.Value, fields[i]);
                                valuesArray[i] = VirtualMachine.TrackLocalObjectReference(value, environment, nativeEnvironment, true);
                                break;

                            default:
                                throw new FormatException();
                            }
                        }
                    }

                    values = valuesArray;
                    return Error.None;
                }
            }
            catch (Exception)
            {
                return Error.Internal;
            }
        }

        public Error SetObjectValues(ObjectId @object, FieldId[] fields, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error GetObjectMonitorInfo(ObjectId @object, out ThreadId owner, out int entryCount, out ThreadId[] waiters)
        {
            throw new NotImplementedException();
        }

        public Error InvokeObjectMethod(ObjectId objectId, ThreadId thread, ClassId @class, MethodId method, InvokeOptions options, Value[] arguments, out Value returnValue, out TaggedObjectId thrownException)
        {
            returnValue = default(Value);
            thrownException = default(TaggedObjectId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            string name;
            string signature;
            string genericSignature;
            error = environment.GetMethodName(method, out name, out signature, out genericSignature);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            if (thread != default(ThreadId))
                throw new NotImplementedException();

            List<string> argumentTypeSignatures;
            string returnTypeSignature;
            SignatureHelper.ParseMethodSignature(signature, out argumentTypeSignatures, out returnTypeSignature);

            using (var instanceHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, objectId))
            {
                if (!instanceHandle.IsAlive)
                    return Error.InvalidObject;

                using (var classHandle = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, @class))
                {
                    if (!classHandle.IsAlive)
                        return Error.InvalidClass;

                    // don't do argument conversion if the signature is invalid
                    switch (returnTypeSignature[0])
                    {
                    case 'Z':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'F':
                    case 'I':
                    case 'J':
                    case 'S':
                    case '[':
                    case 'L':
                        break;

                    case 'V':
                    default:
                        return Error.InvalidMethodid;
                    }

                    jvalue[] args = arguments.Select(value => new jvalue(VirtualMachine, environment, nativeEnvironment, value)).ToArray();

                    bool nonVirtual = (options & InvokeOptions.NonVirtual) != 0;

                    switch (returnTypeSignature[0])
                    {
                    case 'Z':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallBooleanMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualBooleanMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'B':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallByteMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualByteMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'C':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallCharMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualCharMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'D':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallDoubleMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualDoubleMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'F':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallFloatMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualFloatMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'I':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallIntMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualIntMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'J':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallLongMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualLongMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'S':
                        if (!nonVirtual)
                            returnValue = nativeEnvironment.CallShortMethodA(instanceHandle.Value, method, args);
                        else
                            returnValue = nativeEnvironment.CallNonvirtualShortMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case 'V':
                        if (!nonVirtual)
                            nativeEnvironment.CallVoidMethodA(instanceHandle.Value, method, args);
                        else
                            nativeEnvironment.CallNonvirtualVoidMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        break;

                    case '[':
                    case 'L':
                        jobject result;
                        if (!nonVirtual)
                            result = nativeEnvironment.CallObjectMethodA(instanceHandle.Value, method, args);
                        else
                            result = nativeEnvironment.CallNonvirtualObjectMethodA(instanceHandle.Value, classHandle.Value, method, args);

                        returnValue = VirtualMachine.TrackLocalObjectReference(result, environment, nativeEnvironment, false);
                        VirtualMachine.AddGlobalReference(environment, nativeEnvironment, result);
                        nativeEnvironment.DeleteLocalReference(result);
                        break;

                    default:
                        Contract.Assert(false, "not reachable");
                        break;
                    }

                    if (nativeEnvironment.ExceptionOccurred() != jthrowable.Null)
                    {
                        throw new NotImplementedException();
                    }

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        switch (arguments[i].Tag)
                        {
                        case Tag.Array:
                        case Tag.Object:
                        case Tag.String:
                        case Tag.Thread:
                        case Tag.ThreadGroup:
                        case Tag.ClassLoader:
                        case Tag.ClassObject:
                            nativeEnvironment.DeleteLocalReference(args[i].ObjectValue);
                            break;

                        default:
                            break;
                        }
                    }

                    return Error.None;
                }
            }
        }

        public Error DisableObjectCollection(ObjectId objectId)
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, objectId))
            {
                if (!objectHandle.IsAlive)
                    return Error.InvalidObject;

                error = VirtualMachine.AddGlobalReference(environment, nativeEnvironment, objectHandle.Value);
                return GetStandardError(error);
            }
        }

        public Error EnableObjectCollection(ObjectId objectId)
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, objectId))
            {
                if (!objectHandle.IsAlive)
                    return Error.InvalidObject;

                error = VirtualMachine.RemoveGlobalReference(environment, nativeEnvironment, objectHandle.Value);
                return GetStandardError(error);
            }
        }

        public Error GetIsObjectCollected(ObjectId objectId, out bool result)
        {
            if (objectId == default(ObjectId))
            {
                result = true;
                return Error.None;
            }

            result = false;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, objectId))
            {
                result = !objectHandle.IsAlive;
                return Error.None;
            }
        }

        public Error GetStringValue(ObjectId stringObject, out string stringValue)
        {
            stringValue = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var stringHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, stringObject))
            {
                if (!stringHandle.IsAlive)
                    return Error.InvalidString;

                int length = nativeEnvironment.GetStringUTFLength(stringHandle.Value);
                byte[] buffer = new byte[length + 1];
                nativeEnvironment.GetStringUTFRegion(stringHandle.Value, 0, length, buffer);
                stringValue = ModifiedUTF8Encoding.GetString(buffer, 0, length);
            }

            return Error.None;
        }

        public Error GetThreadName(ThreadId threadId, out string name)
        {
            name = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return Error.InvalidThread;

                jvmtiThreadInfo threadInfo;
                error = environment.RawInterface.GetThreadInfo(environment, thread.Value, out threadInfo);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                name = threadInfo.Name;
                nativeEnvironment.DeleteLocalReference(threadInfo._contextClassLoader);
                nativeEnvironment.DeleteLocalReference(threadInfo._threadGroup);
                environment.Deallocate(threadInfo._name);
                return Error.None;
            }
        }

        public Error SuspendThread(ThreadId thread)
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.SuspendThread(nativeEnvironment, thread);
            return GetStandardError(error);
        }

        public Error ResumeThread(ThreadId thread)
        {
            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.ResumeThread(nativeEnvironment, thread);
            return GetStandardError(error);
        }

        public Error GetThreadStatus(ThreadId threadId, out ThreadStatus threadStatus, out SuspendStatus suspendStatus)
        {
            threadStatus = 0;
            suspendStatus = 0;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return Error.InvalidThread;

                jvmtiThreadState state;
                error = environment.RawInterface.GetThreadState(environment, thread.Value, out state);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                suspendStatus = SuspendStatus.None;
                if ((state & jvmtiThreadState.Suspended) != 0)
                    suspendStatus = SuspendStatus.Suspended;

                if ((state & jvmtiThreadState.BlockedOnMonitorEnter) != 0)
                    threadStatus = ThreadStatus.Monitor;
                else if ((state & jvmtiThreadState.Sleeping) != 0)
                    threadStatus = ThreadStatus.Sleeping;
                else if ((state & jvmtiThreadState.Terminated) != 0)
                    threadStatus = ThreadStatus.Zombie;
                else if ((state & jvmtiThreadState.Waiting) != 0)
                    threadStatus = ThreadStatus.Wait;
                else
                    threadStatus = ThreadStatus.Running;

                return Error.None;
            }
        }

        public Error GetThreadGroup(ThreadId threadId, out ThreadGroupId threadGroup)
        {
            threadGroup = default(ThreadGroupId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, threadId))
            {
                if (!thread.IsAlive)
                    return Error.InvalidThread;

                jvmtiThreadInfo threadInfo;
                error = environment.GetThreadInfo(thread.Value, out threadInfo);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                threadGroup = (ThreadGroupId)VirtualMachine.TrackLocalObjectReference(threadInfo._threadGroup, environment, nativeEnvironment, true);
                nativeEnvironment.DeleteLocalReference(threadInfo._contextClassLoader);
                environment.Deallocate(threadInfo._name);
                return Error.None;
            }
        }

        public Error GetThreadFrames(ThreadId threadId, int startFrame, int length, out FrameLocationData[] frames)
        {
            frames = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetStackTrace(nativeEnvironment, threadId, startFrame, length, out frames);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            return Error.None;
        }

        public Error GetThreadFrameCount(ThreadId threadId, out int frameCount)
        {
            frameCount = 0;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            error = environment.GetFrameCount(nativeEnvironment, threadId, out frameCount);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            return Error.None;
        }

        public Error GetThreadOwnedMonitors(ThreadId thread, out TaggedObjectId[] monitors)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadCurrentContendedMonitor(ThreadId thread, out TaggedObjectId monitor)
        {
            throw new NotImplementedException();
        }

        public Error StopThread(ThreadId thread, ObjectId throwable)
        {
            throw new NotImplementedException();
        }

        public Error InterruptThread(ThreadId thread)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadSuspendCount(ThreadId thread, out int suspendCount)
        {
            suspendCount = 0;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            suspendCount = environment.GetSuspendCount(thread);
            return Error.None;
        }

        public Error GetThreadGroupName(ThreadGroupId group, out string groupName)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadGroupParent(ThreadGroupId group, out ThreadGroupId parentGroup)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadGroupChildren(ThreadGroupId group, out ThreadId[] childThreads, out ThreadGroupId[] childGroups)
        {
            throw new NotImplementedException();
        }

        public Error GetArrayLength(ArrayId arrayObject, out int arrayLength)
        {
            arrayLength = 0;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (var arrayHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, arrayObject))
            {
                if (!arrayHandle.IsAlive)
                    return Error.InvalidObject;

                arrayLength = nativeEnvironment.GetArrayLength(arrayHandle.Value);
                return Error.None;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public Error GetArrayValues(ArrayId arrayObject, int firstIndex, int length, out Value[] values)
        {
            values = null;

            try
            {
                JniEnvironment nativeEnvironment;
                JvmtiEnvironment environment;
                jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                using (LocalObjectReferenceHolder objectHandle = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, arrayObject))
                {
                    if (!objectHandle.IsAlive)
                        return Error.InvalidObject;

                    Value[] valuesArray = new Value[length];

                    using (LocalClassReferenceHolder declaringType = new LocalClassReferenceHolder(nativeEnvironment, nativeEnvironment.GetObjectClass(objectHandle.Value)))
                    {
                        string signature;
                        string genericSignature;

                        error = environment.GetClassSignature(declaringType.Value, out signature, out genericSignature);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        // the array element type signature starts after the '['
                        Contract.Assert(signature[0] == '[');
                        switch (signature[1])
                        {
                        case 'Z':
                            {
                                bool[] buffer = new bool[length];
                                nativeEnvironment.GetBooleanArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'B':
                            {
                                byte[] buffer = new byte[length];
                                nativeEnvironment.GetByteArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'C':
                            {
                                char[] buffer = new char[length];
                                nativeEnvironment.GetCharArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'D':
                            {
                                double[] buffer = new double[length];
                                nativeEnvironment.GetDoubleArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'F':
                            {
                                float[] buffer = new float[length];
                                nativeEnvironment.GetFloatArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'I':
                            {
                                int[] buffer = new int[length];
                                nativeEnvironment.GetIntArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'J':
                            {
                                long[] buffer = new long[length];
                                nativeEnvironment.GetLongArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'S':
                            {
                                short[] buffer = new short[length];
                                nativeEnvironment.GetShortArrayRegion(objectHandle.Value, firstIndex, length, buffer);
                                for (int i = 0; i < length; i++)
                                    valuesArray[i] = (Value)buffer[i];

                                break;
                            }

                        case 'V':
                            return Error.InvalidFieldid;

                        case '[':
                        case 'L':
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    jobject value = nativeEnvironment.GetObjectArrayElement(objectHandle.Value, firstIndex + i);
                                    valuesArray[i] = (Value)VirtualMachine.TrackLocalObjectReference(value, environment, nativeEnvironment, true);
                                }

                                break;
                            }

                        default:
                            throw new FormatException();
                        }
                    }

                    values = valuesArray;
                    return Error.None;
                }
            }
            catch (Exception)
            {
                return Error.Internal;
            }
        }

        public Error SetArrayValues(ArrayId arrayObject, int firstIndex, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error GetClassLoaderVisibleClasses(ClassLoaderId classLoaderObject, out TaggedReferenceTypeId[] classes)
        {
            throw new NotImplementedException();
        }

        public Error SetEvent(EventKind eventKind, SuspendPolicy suspendPolicy, EventRequestModifier[] modifiers, out RequestId requestId)
        {
            requestId = default(RequestId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            return _eventProcessor.SetEvent(environment, nativeEnvironment, eventKind, suspendPolicy, new ImmutableList<EventRequestModifier>(modifiers), false, out requestId);
        }

        public Error ClearEvent(EventKind eventKind, RequestId requestId)
        {
            return _eventProcessor.ClearEvent(eventKind, requestId);
        }

        public Error ClearAllBreakpoints()
        {
            return _eventProcessor.ClearAllBreakpoints();
        }

        public Error GetValues(ThreadId thread, FrameId frame, int[] slots, out Value[] values)
        {
            values = null;

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (LocalThreadReferenceHolder threadHandle = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, thread))
            {
                if (!threadHandle.IsAlive)
                    return Error.InvalidThread;

                int depth = (int)frame.Handle;

                jmethodID method;
                jlocation location;
                error = environment.GetFrameLocation(threadHandle.Value, depth, out method, out location);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                VariableData[] variables;
                error = environment.GetLocalVariableTable(method, out variables);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                Value[] valuesArray = new Value[slots.Length];
                for (int i = 0; i < valuesArray.Length; i++)
                {
                    string signature = variables.First(j => j.Slot == slots[i] && j.CodeIndex <= (ulong)location.Value && (ulong)location.Value < j.CodeIndex + j.Length).Signature;

                    int integerValue;
                    double doubleValue;
                    float floatValue;
                    long longValue;
                    TaggedObjectId objectValue;

                    switch (signature[0])
                    {
                    case 'Z':
                        error = environment.GetLocalInt(threadHandle.Value, depth, slots[i], out integerValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = integerValue != 0;
                        break;

                    case 'B':
                        error = environment.GetLocalInt(threadHandle.Value, depth, slots[i], out integerValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = (byte)integerValue;
                        break;

                    case 'C':
                        error = environment.GetLocalInt(threadHandle.Value, depth, slots[i], out integerValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = (char)integerValue;
                        break;

                    case 'D':
                        error = environment.GetLocalDouble(threadHandle.Value, depth, slots[i], out doubleValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = doubleValue;
                        break;

                    case 'F':
                        error = environment.GetLocalFloat(threadHandle.Value, depth, slots[i], out floatValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = floatValue;
                        break;

                    case 'I':
                        error = environment.GetLocalInt(threadHandle.Value, depth, slots[i], out integerValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = integerValue;
                        break;

                    case 'J':
                        error = environment.GetLocalLong(threadHandle.Value, depth, slots[i], out longValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = longValue;
                        break;

                    case 'S':
                        error = environment.GetLocalInt(threadHandle.Value, depth, slots[i], out integerValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = (short)integerValue;
                        break;

                    case 'V':
                        return Error.InvalidFieldid;

                    case '[':
                    case 'L':
                        error = environment.GetLocalObject(nativeEnvironment, threadHandle.Value, depth, slots[i], out objectValue);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        valuesArray[i] = objectValue;
                        break;

                    default:
                        throw new FormatException();
                    }
                }

                values = valuesArray;
                return Error.None;
            }
        }

        public Error SetValues(ThreadId thread, FrameId frame, int[] slots, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error GetThisObject(ThreadId thread, FrameId frame, out TaggedObjectId thisObject)
        {
            thisObject = default(TaggedObjectId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            using (LocalThreadReferenceHolder threadHandle = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, thread))
            {
                if (!threadHandle.IsAlive)
                    return Error.InvalidThread;

                int depth = (int)frame.Handle;

                jmethodID method;
                jlocation location;
                error = environment.GetFrameLocation(threadHandle.Value, depth, out method, out location);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                JvmAccessModifiers modifiers;
                error = environment.GetMethodModifiers(method, out modifiers);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                if ((modifiers & JvmAccessModifiers.Static) != 0)
                    return Error.None;

                error = environment.GetLocalObject(nativeEnvironment, threadHandle.Value, depth, 0, out thisObject);
                if (error != jvmtiError.None)
                    return GetStandardError(error);

                return Error.None;
            }
        }

        public Error PopFrames(ThreadId thread, FrameId frame)
        {
            throw new NotImplementedException();
        }

        public Error GetReflectedType(ClassObjectId classObjectId, out TypeTag typeTag, out ReferenceTypeId typeId)
        {
            typeTag = default(TypeTag);
            typeId = default(ReferenceTypeId);

            JniEnvironment nativeEnvironment;
            JvmtiEnvironment environment;
            jvmtiError error = GetEnvironment(out environment, out nativeEnvironment);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            TaggedReferenceTypeId[] classes;
            error = environment.GetLoadedClasses(nativeEnvironment, out classes);
            if (error != jvmtiError.None)
                return GetStandardError(error);

            jmethodID getNameMethod = nativeEnvironment.GetMethodId(VirtualMachine.ClassClass, "getName", "()Ljava/lang/String;");

            using (var classObject = VirtualMachine.GetLocalReferenceForObject(nativeEnvironment, classObjectId))
            {
                if (!classObject.IsAlive)
                    return Error.InvalidObject;

                jobject nameObject = nativeEnvironment.CallObjectMethodA(classObject.Value, getNameMethod);
                int length = nativeEnvironment.GetStringUTFLength(nameObject);
                byte[] buffer = new byte[length + 1];
                nativeEnvironment.GetStringUTFRegion(nameObject, 0, length, buffer);
                string name = ModifiedUTF8Encoding.GetString(buffer, 0, length);
                nativeEnvironment.DeleteLocalReference(nameObject);

                string signature;

                switch (name)
                {
                case "boolean":
                case "byte":
                case "char":
                case "double":
                case "float":
                case "int":
                case "long":
                case "short":
                case "void":
                    return Error.IllegalArgument;

                default:
                    signature = name.Replace('.', '/');
                    if (name[0] != '[')
                        signature = 'L' + signature + ';';

                    break;
                }

                foreach (var loadedTypeId in classes)
                {
                    using (var loadedType = VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, loadedTypeId.TypeId))
                    {
                        if (!loadedType.IsAlive)
                            continue;

                        string loadedTypeSignature;
                        string loadedTypeGenericSignature;
                        error = environment.GetClassSignature(loadedType.Value, out loadedTypeSignature, out loadedTypeGenericSignature);
                        if (error == jvmtiError.None && loadedTypeSignature == signature)
                        {
                            typeTag = loadedTypeId.TypeTag;
                            typeId = loadedTypeId.TypeId;
                            return Error.None;
                        }
                    }
                }
            }

            return Error.InvalidClass;
        }

        private jvmtiError GetEnvironment(out JvmtiEnvironment environment, out JniEnvironment nativeEnvironment)
        {
            environment = null;
            nativeEnvironment = null;

            environment = _environment;
            int error = VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, true);
            if (error != 0)
                return GetToolsErrorForJniError(error);

            //error = VirtualMachine.GetEnvironment(out environment);
            //if (error != 0)
            //    return GetToolsErrorForJniError(error);

            return jvmtiError.None;
        }

        internal static jvmtiError GetToolsErrorForJniError(int jniError)
        {
            if (jniError == 0)
                return jvmtiError.None;

            return jvmtiError.Internal;
        }

        internal static Error GetStandardError(jvmtiError internalError)
        {
            switch (internalError)
            {
            case jvmtiError.None:
                return Error.None;

            case jvmtiError.NullPointer:
                return Error.NullPointer;

            case jvmtiError.OutOfMemory:
                return Error.OutOfMemory;

            case jvmtiError.AccessDenied:
                return Error.AccessDenied;

            case jvmtiError.UnattachedThread:
                return Error.UnattachedThread;

            case jvmtiError.InvalidPriority:
                return Error.InvalidPriority;

            case jvmtiError.ThreadNotSuspended:
                return Error.ThreadNotSuspended;

            case jvmtiError.ThreadSuspended:
                return Error.ThreadSuspended;

            case jvmtiError.ClassNotPrepared:
                return Error.ClassNotPrepared;

            case jvmtiError.NoMoreFrames:
                return Error.NoMoreFrames;

            case jvmtiError.OpaqueFrame:
                return Error.OpaqueFrame;

            case jvmtiError.Duplicate:
                return Error.Duplicate;

            case jvmtiError.NotFound:
                return Error.NotFound;

            case jvmtiError.NotMonitorOwner:
                return Error.NotMonitorOwner;

            case jvmtiError.Interrupt:
                return Error.Interrupt;

            case jvmtiError.AbsentInformation:
                return Error.AbsentInformation;

            case jvmtiError.InvalidEventType:
                return Error.InvalidEventType;

            case jvmtiError.NativeMethod:
                return Error.NativeMethod;

            case jvmtiError.ClassLoaderUnsupported:
                return Error.InvalidClassLoader;

            case jvmtiError.InvalidThread:
                return Error.InvalidThread;

            case jvmtiError.InvalidFieldid:
                return Error.InvalidFieldid;

            case jvmtiError.InvalidMethodid:
                return Error.InvalidMethodid;

            case jvmtiError.InvalidLocation:
                return Error.InvalidLocation;

            case jvmtiError.InvalidObject:
                return Error.InvalidObject;

            case jvmtiError.InvalidClass:
                return Error.InvalidClass;

            case jvmtiError.TypeMismatch:
                return Error.TypeMismatch;

            case jvmtiError.InvalidSlot:
                return Error.InvalidSlot;

            case jvmtiError.InvalidThreadGroup:
                return Error.InvalidThreadGroup;

            case jvmtiError.InvalidMonitor:
                return Error.InvalidMonitor;

            case jvmtiError.IllegalArgument:
                return Error.IllegalArgument;

            case jvmtiError.InvalidTypestate:
                return Error.InvalidTypestate;

            case jvmtiError.UnsupportedVersion:
                return Error.UnsupportedVersion;

            case jvmtiError.InvalidClassFormat:
                return Error.InvalidClassFormat;

            case jvmtiError.CircularClassDefinition:
                return Error.CircularClassDefinition;

            case jvmtiError.FailsVerification:
                return Error.FailsVerification;

            case jvmtiError.NamesDontMatch:
                return Error.NamesDontMatch;

            case jvmtiError.UnsupportedRedefinitionMethodAdded:
                return Error.AddMethodNotImplemented;

            case jvmtiError.UnsupportedRedefinitionSchemaChanged:
                return Error.SchemaChangeNotImplemented;

            case jvmtiError.UnsupportedRedefinitionHierarchyChanged:
                return Error.HierarchyChangeNotImplemented;

            case jvmtiError.UnsupportedRedefinitionMethodDeleted:
                return Error.DeleteMethodNotImplemented;

            case jvmtiError.UnsupportedRedefinitionClassModifiersChanged:
                return Error.ClassModifiersChangeNotImplemented;

            case jvmtiError.UnsupportedRedefinitionMethodModifiersChanged:
                return Error.MethodModifiersChangeNotImplemented;

            case jvmtiError.Internal:
                return Error.Internal;

            case jvmtiError.MustPossessCapability:
                return Error.NotImplemented;

            case jvmtiError.ThreadNotAlive:
            case jvmtiError.InvalidEnvironment:
            case jvmtiError.UnmodifiableClass:
            case jvmtiError.WrongPhase:
            case jvmtiError.NotAvailable:
            default:
                return Error.Internal;
            }
        }
    }
}
