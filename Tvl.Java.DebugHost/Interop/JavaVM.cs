namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Tvl.Java.DebugHost.Services;

    using Dispatcher = System.Windows.Threading.Dispatcher;
    using DispatcherFrame = System.Windows.Threading.DispatcherFrame;
    using DispatcherOperation = System.Windows.Threading.DispatcherOperation;
    using DispatcherPriority = System.Windows.Threading.DispatcherPriority;
    using ExceptionTableEntry = Tvl.Java.DebugInterface.Types.Loader.ExceptionTableEntry;
    using Interlocked = System.Threading.Interlocked;
    using IntPtr = System.IntPtr;
    using ObjectId = Tvl.Java.DebugInterface.Types.ObjectId;
    using ReferenceTypeId = Tvl.Java.DebugInterface.Types.ReferenceTypeId;
    using Tag = Tvl.Java.DebugInterface.Types.Tag;
    using TaggedObjectId = Tvl.Java.DebugInterface.Types.TaggedObjectId;
    using TaggedReferenceTypeId = Tvl.Java.DebugInterface.Types.TaggedReferenceTypeId;
    using ThreadId = Tvl.Java.DebugInterface.Types.ThreadId;
    using TypeTag = Tvl.Java.DebugInterface.Types.TypeTag;

    public class JavaVM
    {
        private static readonly ConcurrentDictionary<JavaVMHandle, JavaVM> _instances =
            new ConcurrentDictionary<JavaVMHandle, JavaVM>();

        private readonly JavaVMHandle _handle;
        private readonly JniInvokeInterface _jniInvokeInterface;
        private readonly List<Tuple<Dispatcher, DispatcherFrame, jvmtiEnvHandle, IAsyncResult>> _dispatchers =
            new List<Tuple<Dispatcher, DispatcherFrame, jvmtiEnvHandle, IAsyncResult>>();
        private readonly HashSet<ThreadId> _agentThreads = new HashSet<ThreadId>();

        private readonly ConcurrentDictionary<ThreadId, int> _suspendCounts = new ConcurrentDictionary<ThreadId, int>();

        // class loader tag -> class signature -> full method signature -> exception table
        private readonly Dictionary<long, Dictionary<string, Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>>> _exceptionTableEntries
            = new Dictionary<long, Dictionary<string, Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>>>();

        #region Object tracking

        private readonly Dictionary<ThreadId, jthread> _threads = new Dictionary<ThreadId, jthread>();
        private readonly Dictionary<ObjectId, jweak> _objects = new Dictionary<ObjectId, jweak>();
        private readonly Dictionary<ReferenceTypeId, jclass> _classes = new Dictionary<ReferenceTypeId, jclass>();

        private long _nextTag = 1000;

        private jclass _stringClass;
        private jclass _threadClass;
        private jclass _threadGroupClass;
        private jclass _classClass;
        private jclass _classLoaderClass;

        #endregion

        internal readonly System.Threading.ThreadLocal<bool> IsAgentThread = new System.Threading.ThreadLocal<bool>(() => false);

        private JavaVM(JavaVMHandle vmHandle)
        {
            _handle = vmHandle;
            _jniInvokeInterface = (JniInvokeInterface)Marshal.PtrToStructure(Marshal.ReadIntPtr(vmHandle.Handle), typeof(JniInvokeInterface));
        }

        internal static bool DisableStatementStepping
        {
            get;
            set;
        }

        public JniInvokeInterface RawInterface
        {
            get
            {
                return _jniInvokeInterface;
            }
        }

        public ThreadId[] AgentThreads
        {
            get
            {
                lock (_agentThreads)
                {
                    return _agentThreads.ToArray();
                }
            }
        }

        internal jclass StringClass
        {
            get
            {
                return _stringClass;
            }
        }

        internal jclass ThreadClass
        {
            get
            {
                return _threadClass;
            }
        }

        internal jclass ThreadGroupClass
        {
            get
            {
                return _threadGroupClass;
            }
        }

        internal jclass ClassClass
        {
            get
            {
                return _classClass;
            }
        }

        internal jclass ClassLoaderClass
        {
            get
            {
                return _classLoaderClass;
            }
        }

        internal ConcurrentDictionary<ThreadId, int> SuspendCounts
        {
            get
            {
                return _suspendCounts;
            }
        }

        internal jvmtiError GetExceptionTable(long classLoaderTag, string classSignature, string methodName, string methodSignature, out ReadOnlyCollection<ExceptionTableEntry> exceptionTable)
        {
            Contract.Requires<ArgumentNullException>(classSignature != null, "classSignature");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");
            Contract.Requires<ArgumentNullException>(methodSignature != null, "methodSignature");

            exceptionTable = null;
            lock (_exceptionTableEntries)
            {
                Dictionary<string, Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>> loaderClasses;
                if (!_exceptionTableEntries.TryGetValue(classLoaderTag, out loaderClasses))
                    return jvmtiError.AbsentInformation;

                Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>> classMethods;
                if (!loaderClasses.TryGetValue(classSignature, out classMethods))
                    return jvmtiError.AbsentInformation;

                string fullMethodSignature = methodName + methodSignature;
                if (!classMethods.TryGetValue(fullMethodSignature, out exceptionTable))
                    return jvmtiError.AbsentInformation;

                if (exceptionTable == null)
                {
                    // the exception table was intentionally disabled for this method 
                    return jvmtiError.AbsentInformation;
                }

                return jvmtiError.None;
            }
        }

        internal void SetExceptionTable(long classLoaderTag, string classSignature, string methodName, string methodSignature, ReadOnlyCollection<ExceptionTableEntry> exceptionTable)
        {
            if (exceptionTable == null)
                return;

            lock (_exceptionTableEntries)
            {
                Dictionary<string, Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>> classExceptionTables;
                if (!_exceptionTableEntries.TryGetValue(classLoaderTag, out classExceptionTables))
                {
                    classExceptionTables = new Dictionary<string, Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>>();
                    _exceptionTableEntries.Add(classLoaderTag, classExceptionTables);
                }

                Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>> classMethods;
                if (!classExceptionTables.TryGetValue(classSignature, out classMethods))
                {
                    classMethods = new Dictionary<string, ReadOnlyCollection<ExceptionTableEntry>>();
                    classExceptionTables.Add(classSignature, classMethods);
                }

                string fullMethodSignature = methodName + methodSignature;

                ReadOnlyCollection<ExceptionTableEntry> existingExceptionTable;
                if (classMethods.TryGetValue(fullMethodSignature, out existingExceptionTable))
                {
                    if (existingExceptionTable != null && !exceptionTable.SequenceEqual(existingExceptionTable))
                    {
                        // disable the exception table for all methods which reach this point due to aliasing issues
                        classMethods[fullMethodSignature] = null;
                    }

                    return;
                }

                classMethods.Add(fullMethodSignature, exceptionTable);
            }
        }

        public static implicit operator JvmVirtualMachineRemoteHandle(JavaVM virtualMachine)
        {
            return new JvmVirtualMachineRemoteHandle(virtualMachine._handle);
        }

        public static implicit operator JavaVMHandle(JavaVM vm)
        {
            return vm._handle;
        }

        public static JavaVM GetInstance(JvmVirtualMachineRemoteHandle virtualMachineHandle)
        {
            JavaVM vm = _instances[new JavaVMHandle((IntPtr)virtualMachineHandle.Handle)];

            //JNIEnvHandle env;
            //JavaVMAttachArgs args = new JavaVMAttachArgs(jniVersion.Version1_6, IntPtr.Zero, jthreadGroup.Null);
            //int result = vm.GetRawInterface().AttachCurrentThread(vm, out env, ref args);

            //JvmEnvironment environment = vm.GetEnvironment(jvmtiVersion.Version1_1);
            //if (environment != null)
            //{
            //    jvmtiPhase phase = environment.GetPhase();
            //    if (phase == jvmtiPhase.Live)
            //    {
            //    }
            //}

            //if (attachCurrentThread)
            //{
            //    JNIEnvHandle env;
            //    JavaVMAttachArgs args = new JavaVMAttachArgs(jvmtiVersion.Version1_1, IntPtr.Zero, jthreadGroup.Null);
            //    vm.GetRawInterface().AttachCurrentThread(vm, out env, ref args);
            //}

            return vm;
        }

        internal static JavaVM GetOrCreateInstance(JavaVMHandle handle)
        {
            return _instances.GetOrAdd(handle, CreateVirtualMachine);
        }

        private static JavaVM CreateVirtualMachine(JavaVMHandle handle)
        {
            return new JavaVM(handle);
        }

        public void ShutdownAgentDispatchers()
        {
            List<Dispatcher> dispatchers = new List<Dispatcher>();
            List<DispatcherFrame> frames = new List<DispatcherFrame>();

            lock (_dispatchers)
            {
                for (int i = _dispatchers.Count - 1; i >= 0; i--)
                {
                    if (_dispatchers[i].Item4 != null)
                        continue;

                    dispatchers.Add(_dispatchers[i].Item1);
                    frames.Add(_dispatchers[i].Item2);
                    _dispatchers.RemoveAt(i);
                }
            }

            for (int i = 0; i < dispatchers.Count; i++)
            {
                // queue a low priority operation to end processing
                var frame = frames[i];
                dispatchers[i].BeginInvoke((Action)(() => frame.Continue = false), DispatcherPriority.Background);
            }
        }

        public void PushDispatcherFrame(DispatcherFrame frame, jvmtiEnvHandle environment, IAsyncResult asyncResult)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<ArgumentNullException>(environment != null, "environment");
            Contract.Requires<ArgumentNullException>(asyncResult != null, "asyncResult");

            lock (_dispatchers)
            {
                _dispatchers.Add(Tuple.Create(Dispatcher.CurrentDispatcher, frame, environment, asyncResult));
            }

            Dispatcher.PushFrame(frame);
        }

        public void PushAgentDispatcherFrame(DispatcherFrame frame, jvmtiEnvHandle environment)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<ArgumentNullException>(environment != null, "environment");

            lock (_dispatchers)
            {
                _dispatchers.Add(Tuple.Create(Dispatcher.CurrentDispatcher, frame, environment, default(IAsyncResult)));
            }

            Dispatcher.PushFrame(frame);
        }

        public void InvokeOnJvmThread(Action<jvmtiEnvHandle> action)
        {
            Contract.Requires<ArgumentNullException>(action != null, "action");

            DispatcherOperation operationResult;
            lock (_dispatchers)
            {
                if (_dispatchers.Count == 0)
                    throw new InvalidOperationException("No JVM dispatchers are available.");

                var dispatcher = _dispatchers[0];
                operationResult = dispatcher.Item1.BeginInvoke(action, dispatcher.Item3);
            }

            operationResult.Wait();
        }

        public void HandleAsyncOperationComplete(IAsyncResult result)
        {
            Dispatcher dispatcher = null;
            DispatcherFrame frame = null;

            lock (_dispatchers)
            {
                // find and remove the appropriate dispatcher
                int index = _dispatchers.FindIndex(i => i.Item4 == result);
                if (index < 0)
                    throw new ArgumentException();

                dispatcher = _dispatchers[index].Item1;
                frame = _dispatchers[index].Item2;
                _dispatchers.RemoveAt(index);
            }

            // queue a low priority operation to end processing
            dispatcher.BeginInvoke((Action)(() => frame.Continue = false), DispatcherPriority.Background);
        }

        public int GetEnvironment(out JvmtiEnvironment environment)
        {
            jvmtiEnvHandle env;
            int error = RawInterface.GetEnv(this, out env, jvmtiVersion.Version1_1);
            if (error == 0)
                environment = JvmtiEnvironment.GetOrCreateInstance(this, env);
            else
                environment = null;

            return error;
        }

        public int AttachCurrentThread(JvmtiEnvironment environment, out JniEnvironment nativeEnvironment, bool preventSuspend)
        {
            JavaVMAttachArgs args = new JavaVMAttachArgs(jniVersion.Version1_6, IntPtr.Zero, jthreadGroup.Null);

            if (preventSuspend)
                IsAgentThread.Value = true;

            JNIEnvHandle jniEnv;
            int error = RawInterface.AttachCurrentThread(this, out jniEnv, ref args);
            if (error == 0)
            {
                bool created;
                nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv, out created);
                if (created && preventSuspend)
                {
                    if (environment == null)
                        GetEnvironment(out environment);

                    jthread thread;
                    environment.RawInterface.GetCurrentThread(environment, out thread);
                    if (thread != jthread.Null)
                    {
                        ThreadId threadId = TrackLocalThreadReference(thread, environment, nativeEnvironment, true);
                        lock (_agentThreads)
                        {
                            _agentThreads.Add(threadId);
                        }
                    }
                }
            }
            else
            {
                nativeEnvironment = null;
            }

            return error;
        }

        public int AttachCurrentThreadAsDaemon(JvmtiEnvironment environment, out JniEnvironment nativeEnvironment, bool agentThread)
        {
            JavaVMAttachArgs args = new JavaVMAttachArgs(jniVersion.Version1_6, IntPtr.Zero, jthreadGroup.Null);

            bool alreadyAgent = IsAgentThread.Value;
            if (agentThread && !alreadyAgent)
                IsAgentThread.Value = true;

            JNIEnvHandle jniEnv;
            int error = RawInterface.AttachCurrentThreadAsDaemon(this, out jniEnv, ref args);
            if (error == 0)
            {
                bool created;
                nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv, out created);
                if (agentThread && !alreadyAgent)
                {
                    if (environment == null)
                        GetEnvironment(out environment);

                    jthread thread;
                    JvmtiErrorHandler.ThrowOnFailure(environment.RawInterface.GetCurrentThread(environment, out thread));
                    if (thread != jthread.Null)
                    {
                        ThreadId threadId = TrackLocalThreadReference(thread, environment, nativeEnvironment, true);
                        lock (_agentThreads)
                        {
                            _agentThreads.Add(threadId);
                        }
                    }
                }
            }
            else
            {
                nativeEnvironment = null;
            }

            return error;
        }

#if false
        public JvmEnvironment GetEnvironment(jvmtiVersion version)
        {
            JniInvokeInterface jniInvokeInterface = GetRawInterface();

            jvmtiEnvHandle env;
            int result = jniInvokeInterface.GetEnv(this, out env, version);
            JniErrorHandler.ThrowOnFailure(result);

            return JvmEnvironment.GetOrCreateEnvironment(this, env);
        }

        public JvmNativeEnvironment AttachCurrentThread(ref JavaVMAttachArgs args, jvmtiVersion toolsVersion)
        {
            JniInvokeInterface jniInvokeInterface = GetRawInterface();

            JNIEnvHandle env;
            int result = jniInvokeInterface.AttachCurrentThread(this, out env, ref args);

            return GetEnvironment(toolsVersion).GetNativeFunctionTable(env);
        }
#endif

        #region Object tracking

        internal jvmtiError GetThread(ThreadId threadId, out jthread thread)
        {
            lock (_threads)
            {
                if (!_threads.TryGetValue(threadId, out thread))
                    return jvmtiError.InvalidThread;

                return jvmtiError.None;
            }
        }

        internal jvmtiError GetLocalReferenceForThread(JniEnvironment nativeEnvironment, ThreadId threadId, out LocalThreadReferenceHolder thread)
        {
            thread = default(LocalThreadReferenceHolder);

            jthread threadHandle;
            jvmtiError error = GetThread(threadId, out threadHandle);
            if (error != jvmtiError.None)
                return error;

            thread = new LocalThreadReferenceHolder(nativeEnvironment, threadHandle);
            return jvmtiError.None;
        }

        internal LocalThreadReferenceHolder GetLocalReferenceForThread(JniEnvironment nativeEnvironment, ThreadId threadId)
        {
            LocalThreadReferenceHolder thread;
            var error = GetLocalReferenceForThread(nativeEnvironment, threadId, out thread);
            if (error != jvmtiError.None)
                return new LocalThreadReferenceHolder();

            return thread;
        }

        internal jvmtiError GetClass(ReferenceTypeId classId, out jclass @class)
        {
            lock (_classes)
            {
                if (!_classes.TryGetValue(classId, out @class))
                    return jvmtiError.InvalidClass;

                return jvmtiError.None;
            }
        }

        internal jvmtiError GetLocalReferenceForClass(JniEnvironment nativeEnvironment, ReferenceTypeId typeId, out LocalClassReferenceHolder thread)
        {
            thread = default(LocalClassReferenceHolder);

            jclass threadHandle;
            jvmtiError error = GetClass(typeId, out threadHandle);
            if (error != jvmtiError.None)
                return error;

            thread = new LocalClassReferenceHolder(nativeEnvironment, threadHandle);
            return jvmtiError.None;
        }

        internal LocalClassReferenceHolder GetLocalReferenceForClass(JniEnvironment nativeEnvironment, ReferenceTypeId classId)
        {
            LocalClassReferenceHolder thread;
            var error = GetLocalReferenceForClass(nativeEnvironment, classId, out thread);
            if (error != jvmtiError.None)
                return new LocalClassReferenceHolder();

            return thread;
        }

        internal ThreadId TrackLocalThreadReference(jthread thread, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, bool freeLocalReference)
        {
            if (thread == jthread.Null)
                return default(ThreadId);

            int hashCode;
            JvmtiErrorHandler.ThrowOnFailure(environment.RawInterface.GetObjectHashCode(environment, thread, out hashCode));

            ThreadId threadId = new ThreadId(hashCode);
            lock (_threads)
            {
                if (!_threads.ContainsKey(threadId))
                {
                    jweak weak = nativeEnvironment.NewWeakGlobalReference(thread);
                    bool added = false;
                    if (!_threads.ContainsKey(threadId))
                    {
                        _threads.Add(threadId, new jthread(weak.Handle));
                        added = true;
                    }

                    if (!added)
                    {
                        nativeEnvironment.DeleteWeakGlobalReference(weak);
                    }
                }
            }

            if (freeLocalReference)
                nativeEnvironment.DeleteLocalReference(thread);

            return threadId;
        }

        internal jvmtiError GetObject(ObjectId objectId, out jobject @object)
        {
            @object = jobject.Null;

            lock (_objects)
            {
                jweak weak;
                if (!_objects.TryGetValue(objectId, out weak))
                {
                    jthread thread;
                    if (!_threads.TryGetValue((ThreadId)objectId, out thread))
                    {
                        jclass @class;
                        if (!_classes.TryGetValue((ReferenceTypeId)objectId, out @class))
                            return jvmtiError.InvalidObject;

                        @object = @class;
                        return jvmtiError.None;
                    }

                    @object = thread;
                    return jvmtiError.None;
                }

                @object = new jobject(weak.Handle);
                return jvmtiError.None;
            }
        }

        internal jvmtiError GetLocalReferenceForObject(JniEnvironment nativeEnvironment, ObjectId objectId, out LocalObjectReferenceHolder thread)
        {
            thread = default(LocalObjectReferenceHolder);

            jobject threadHandle;
            jvmtiError error = GetObject(objectId, out threadHandle);
            if (error != jvmtiError.None)
                return error;

            thread = new LocalObjectReferenceHolder(nativeEnvironment, threadHandle);
            return jvmtiError.None;
        }

        internal LocalObjectReferenceHolder GetLocalReferenceForObject(JniEnvironment nativeEnvironment, ObjectId objectId)
        {
            LocalObjectReferenceHolder thread;
            jvmtiError error = GetLocalReferenceForObject(nativeEnvironment, objectId, out thread);
            if (error != jvmtiError.None)
                return new LocalObjectReferenceHolder();

            return thread;
        }

        internal TaggedObjectId TrackLocalObjectReference(jobject @object, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, bool freeLocalReference)
        {
            if (@object == jobject.Null)
                return new TaggedObjectId(Tag.Object, new ObjectId(0), default(TaggedReferenceTypeId));

            long tag;
            JvmtiErrorHandler.ThrowOnFailure(environment.GetTag(@object, out tag));

            TaggedReferenceTypeId type = default(TaggedReferenceTypeId);
            Tag objectKind;
            if (tag == 0)
            {
                long uniqueTag = Interlocked.Increment(ref _nextTag);

                /* first figure out what type of object we're dealing with. could be:
                 *  - String
                 *  - Thread
                 *  - ThreadGroup
                 *  - ClassLoader
                 *  - ClassObject
                 *  - Array
                 *  - Object
                 */

                // check for array
                jclass objectClass = nativeEnvironment.GetObjectClass(@object);
                try
                {
                    type = TrackLocalClassReference(objectClass, environment, nativeEnvironment, false);

                    bool isArray = type.TypeTag == TypeTag.Array;
                    if (isArray)
                    {
                        objectKind = Tag.Array;
                    }
                    else
                    {
                        if (_stringClass != jclass.Null && nativeEnvironment.IsInstanceOf(@object, _stringClass))
                            objectKind = Tag.String;
                        else if (_threadClass != jclass.Null && nativeEnvironment.IsInstanceOf(@object, _threadClass))
                            objectKind = Tag.Thread;
                        else if (_threadGroupClass != jclass.Null && nativeEnvironment.IsInstanceOf(@object, _threadGroupClass))
                            objectKind = Tag.ThreadGroup;
                        else if (_classClass != jclass.Null && nativeEnvironment.IsInstanceOf(@object, _classClass))
                            objectKind = Tag.ClassObject;
                        else if (_classLoaderClass != jclass.Null && nativeEnvironment.IsInstanceOf(@object, _classLoaderClass))
                            objectKind = Tag.ClassLoader;
                        else
                            objectKind = Tag.Object;
                    }
                }
                finally
                {
                    nativeEnvironment.DeleteLocalReference(objectClass);
                }

                tag = (uniqueTag << 8) | (uint)objectKind;
                JvmtiErrorHandler.ThrowOnFailure(environment.SetTag(@object, tag));

                lock (_objects)
                {
                    _objects.Add(new ObjectId(tag), nativeEnvironment.NewWeakGlobalReference(@object));
                }
            }
            else
            {
                jclass objectClass = nativeEnvironment.GetObjectClass(@object);
                type = TrackLocalClassReference(objectClass, environment, nativeEnvironment, true);
            }

            if (freeLocalReference)
                nativeEnvironment.DeleteLocalReference(@object);

            objectKind = (Tag)(tag & 0xFF);
            return new TaggedObjectId(objectKind, new ObjectId(tag), type);
        }

        internal void HandleVMInit(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, jthread thread)
        {
            jclass threadClass = nativeEnvironment.GetObjectClass(thread);
            _threadClass = FindBaseClass(environment, nativeEnvironment, threadClass, "Ljava/lang/Thread;");

            {
                jclass classClass = nativeEnvironment.GetObjectClass(threadClass);
                _classClass = FindBaseClass(environment, nativeEnvironment, classClass, "Ljava/lang/Class;");
                nativeEnvironment.DeleteLocalReference(classClass);
            }

            {
                jvmtiThreadInfo threadInfo;
                JvmtiErrorHandler.ThrowOnFailure(environment.GetThreadInfo(thread, out threadInfo));
                jclass threadGroupClass = nativeEnvironment.GetObjectClass(threadInfo._threadGroup);
                _threadGroupClass = FindBaseClass(environment, nativeEnvironment, threadGroupClass, "Ljava/lang/ThreadGroup;");

                jclass classLoaderClass = nativeEnvironment.GetObjectClass(threadInfo._contextClassLoader);
                _classLoaderClass = FindBaseClass(environment, nativeEnvironment, classLoaderClass, "Ljava/lang/ClassLoader;");
                nativeEnvironment.DeleteLocalReference(classLoaderClass);

                nativeEnvironment.DeleteLocalReference(threadGroupClass);
                nativeEnvironment.DeleteLocalReference(threadInfo._contextClassLoader);
                nativeEnvironment.DeleteLocalReference(threadInfo._threadGroup);
                environment.Deallocate(threadInfo._name);
            }

            nativeEnvironment.DeleteLocalReference(threadClass);

            jobject stringObject = nativeEnvironment.NewString(string.Empty);
            jclass stringClass = nativeEnvironment.GetObjectClass(stringObject);
            _stringClass = FindBaseClass(environment, nativeEnvironment, stringClass, "Ljava/lang/String;");
            nativeEnvironment.DeleteLocalReference(stringObject);
            nativeEnvironment.DeleteLocalReference(stringClass);
        }

        internal jclass FindBaseClass(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, jclass classHandle, string signature)
        {
            string currentSignature;
            string genericSignature;
            JvmtiErrorHandler.ThrowOnFailure(environment.GetClassSignature(classHandle, out currentSignature, out genericSignature));
            if (currentSignature == signature)
            {
                return (jclass)nativeEnvironment.NewGlobalReference(classHandle);
            }

            jclass superClass = nativeEnvironment.GetSuperclass(classHandle);
            if (superClass == jclass.Null)
                return jclass.Null;

            jclass result = FindBaseClass(environment, nativeEnvironment, superClass, signature);
            nativeEnvironment.DeleteLocalReference(superClass);
            return result;
        }

        internal TaggedReferenceTypeId TrackLocalClassReference(jclass classHandle, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, bool freeLocalReference)
        {
            if (classHandle == jclass.Null)
                return default(TaggedReferenceTypeId);

            bool isArrayClass;
            JvmtiErrorHandler.ThrowOnFailure(environment.IsArrayClass(classHandle, out isArrayClass));
            bool isInterface;
            JvmtiErrorHandler.ThrowOnFailure(environment.IsInterface(classHandle, out isInterface));

            TypeTag typeTag = isArrayClass ? TypeTag.Array : (isInterface ? TypeTag.Interface : TypeTag.Class);

            int hashCode;
            JvmtiErrorHandler.ThrowOnFailure(environment.GetObjectHashCode(classHandle, out hashCode));

            ReferenceTypeId typeId = new ReferenceTypeId(hashCode);
            TaggedReferenceTypeId taggedTypeId = new TaggedReferenceTypeId(typeTag, typeId);
            lock (_classes)
            {
                if (!_classes.ContainsKey(typeId))
                {
                    jweak weak = nativeEnvironment.NewWeakGlobalReference(classHandle);
                    bool added = false;
                    if (!_classes.ContainsKey(typeId))
                    {
                        _classes.Add(typeId, new jclass(weak.Handle));
                        added = true;
                    }

                    if (!added)
                    {
                        nativeEnvironment.DeleteWeakGlobalReference(weak);
                    }
                }
            }

            if (freeLocalReference)
                nativeEnvironment.DeleteLocalReference(classHandle);

            return taggedTypeId;
        }

        private readonly ConcurrentDictionary<long, Tuple<jobject, int>> _globalReferences = new ConcurrentDictionary<long, Tuple<jobject, int>>();

        internal jvmtiError AddGlobalReference(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, jobject obj)
        {
            long tag;
            jvmtiError error = environment.GetTag(obj, out tag);
            if (error != jvmtiError.None)
                return error;

            if (tag == 0)
                return jvmtiError.IllegalArgument;

            _globalReferences.AddOrUpdate(tag,
                t =>
                {
                    return Tuple.Create(nativeEnvironment.NewGlobalReference(obj), 1);
                },
                (t, v) =>
                {
                    if (v.Item2 == 0)
                        return Tuple.Create(nativeEnvironment.NewGlobalReference(obj), 1);
                    else
                        return Tuple.Create(v.Item1, v.Item2 + 1);
                });

            return jvmtiError.None;
        }

        internal jvmtiError RemoveGlobalReference(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, jobject obj)
        {
            long tag;
            jvmtiError error = environment.GetTag(obj, out tag);
            if (error != jvmtiError.None)
                return error;

            if (tag == 0)
                return jvmtiError.IllegalArgument;

            var result = _globalReferences.AddOrUpdate(tag,
                t =>
                {
                    return Tuple.Create(jobject.Null, 0);
                },
                (t, v) =>
                {
                    if (v.Item2 == 0)
                    {
                        return Tuple.Create(jobject.Null, 0);
                    }
                    else if (v.Item2 == 1)
                    {
                        nativeEnvironment.DeleteGlobalReference(v.Item1);
                        return Tuple.Create(jobject.Null, 0);
                    }
                    else
                    {
                        return Tuple.Create(v.Item1, v.Item2 - 1);
                    }
                });

            return jvmtiError.None;
        }

        #endregion
    }
}
