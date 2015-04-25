namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Threading;
    using Tvl.Collections;
    using Tvl.Java.DebugHost.Interop;
    using Tvl.Java.DebugInterface.Types;
    using Tvl.Java.DebugInterface.Types.Loader;

    using Interlocked = System.Threading.Interlocked;
    using ManualResetEventSlim = System.Threading.ManualResetEventSlim;
    using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
    using MessageBoxDefaultButton = System.Windows.Forms.MessageBoxDefaultButton;
    using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

    partial class DebugProtocolService
    {
        internal class EventProcessor
        {
            private readonly DebugProtocolService _service;
            private readonly jvmtiEventCallbacks _eventCallbacks;

            private ManualResetEventSlim _agentStartedEvent = new ManualResetEventSlim();
            private jthread _agentThread;
            private UnsafeNativeMethods.jvmtiStartFunction _agentCallbackDelegate;
            private Dispatcher _agentEventDispatcher;

            private readonly Dictionary<EventKind, Dictionary<RequestId, EventFilter>> _eventRequests =
                new Dictionary<EventKind, Dictionary<RequestId, EventFilter>>();
            private int _nextRequestId = 1;

            public EventProcessor(DebugProtocolService service)
            {
                Contract.Requires(service != null);
                _service = service;

                _eventCallbacks = new jvmtiEventCallbacks()
                {
                    VMInit = HandleVMInit,
                    VMDeath = HandleVMDeath,
                    ThreadStart = HandleThreadStart,
                    ThreadEnd = HandleThreadEnd,
                    ClassFileLoadHook = HandleClassFileLoadHook,
                    ClassLoad = HandleClassLoad,
                    ClassPrepare = HandleClassPrepare,
                    VMStart = HandleVMStart,
                    Exception = HandleException,
                    ExceptionCatch = HandleExceptionCatch,
                    SingleStep = HandleSingleStep,
                    FramePop = HandleFramePop,
                    Breakpoint = HandleBreakpoint,
                    FieldAccess = HandleFieldAccess,
                    FieldModification = HandleFieldModification,
                    MethodEntry = HandleMethodEntry,
                    MethodExit = HandleMethodExit,
                    NativeMethodBind = HandleNativeMethodBind,
                    CompiledMethodLoad = HandleCompiledMethodLoad,
                    CompiledMethodUnload = HandleCompiledMethodUnload,
                    DynamicCodeGenerated = HandleDynamicCodeGenerated,
                    DataDumpRequest = HandleDataDumpRequest,
                    MonitorWait = HandleMonitorWait,
                    MonitorWaited = HandleMonitorWaited,
                    MonitorContendedEnter = HandleMonitorContendedEnter,
                    MonitorContendedEntered = HandleMonitorContendedEntered,
                    ResourceExhausted = HandleResourceExhausted,
                    GarbageCollectionStart = HandleGarbageCollectionStart,
                    GarbageCollectionFinish = HandleGarbageCollectionFinish,
                    ObjectFree = HandleObjectFree,
                    VMObjectAlloc = HandleVMObjectAlloc
                };
            }

            private IDebugProcotolCallback Callback
            {
                get
                {
                    return _service._callback;
                }
            }

            private JvmtiEnvironment Environment
            {
                get
                {
                    return _service.Environment;
                }
            }

            private jvmtiInterface RawInterface
            {
                get
                {
                    return _service.RawInterface;
                }
            }

            internal JavaVM VirtualMachine
            {
                get
                {
                    return _service.VirtualMachine;
                }
            }

            private Dispatcher AgentEventDispatcher
            {
                get
                {
                    return _agentEventDispatcher;
                }
            }

            internal void Attach()
            {
                VirtualMachine.InvokeOnJvmThread(AttachImpl);
            }

            private void AttachImpl(jvmtiEnvHandle env)
            {
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(VirtualMachine, env);

                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventCallbacks(_eventCallbacks));

                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.VMStart));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.VMInit));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.VMDeath));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ThreadStart));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ThreadEnd));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ClassLoad));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ClassPrepare));
                JvmtiErrorHandler.ThrowOnFailure(environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ClassFileLoadHook));

#if false
                jvmtiCapabilities capabilities;
                JvmtiErrorHandler.ThrowOnFailure(environment.GetCapabilities(out capabilities));

                environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ClassFileLoadHook);
                environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.DynamicCodeGenerated);
                environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.DataDumpRequest);
                environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ResourceExhausted);
                //environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.DataResetRequest);

                if (capabilities.CanGenerateExceptionEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.Exception);
                if (capabilities.CanGenerateExceptionEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ExceptionCatch);
                if (capabilities.CanGenerateSingleStepEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.SingleStep);
                if (capabilities.CanGenerateFramePopEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.FramePop);
                if (capabilities.CanGenerateBreakpointEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.Breakpoint);
                if (capabilities.CanGenerateFieldAccessEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.FieldAccess);
                if (capabilities.CanGenerateFieldModificationEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.FieldModification);
                if (capabilities.CanGenerateMethodEntryEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MethodEntry);
                if (capabilities.CanGenerateMethodExitEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MethodExit);
                if (capabilities.CanGenerateNativeMethodBindEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.NativeMethodBind);
                if (capabilities.CanGenerateCompiledMethodLoadEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.CompiledMethodLoad);
                if (capabilities.CanGenerateCompiledMethodLoadEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.CompiledMethodUnload);
                if (capabilities.CanGenerateMonitorEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MonitorWait);
                if (capabilities.CanGenerateMonitorEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MonitorWaited);
                if (capabilities.CanGenerateMonitorEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MonitorContendedEnter);
                if (capabilities.CanGenerateMonitorEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.MonitorContendedEntered);
                if (capabilities.CanGenerateGarbageCollectionEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.GarbageCollectionStart);
                if (capabilities.CanGenerateGarbageCollectionEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.GarbageCollectionFinish);
                if (capabilities.CanGenerateObjectFreeEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.ObjectFree);
                if (capabilities.CanGenerateVmObjectAllocEvents)
                    environment.SetEventNotificationMode(JvmEventMode.Enable, JvmEventType.VMObjectAlloc);
#endif
            }

            internal void Detach()
            {
                _service.Environment.SetEventCallbacks(default(jvmtiEventCallbacks));
            }

            public Error SetEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventKind eventKind, SuspendPolicy suspendPolicy, ImmutableList<EventRequestModifier> modifiers, bool internalRequest, out RequestId requestId)
            {
                Contract.Requires<ArgumentNullException>(modifiers != null, "modifiers");

                requestId = default(RequestId);

                EventKind internalEventKind = eventKind;

                EventRequestModifier locationModifier = default(EventRequestModifier);
                EventRequestModifier stepModifier = default(EventRequestModifier);

                switch (eventKind)
                {
                case EventKind.Breakpoint:
                    // we're going to need the location modifier to set the breakpoint
                    if (modifiers.Count == 0 || modifiers[0].Kind != ModifierKind.LocationFilter)
                        return Error.IllegalArgument;

                    locationModifier = modifiers[0];
                    break;

                case EventKind.SingleStep:
                    // the first modifier contains the step properties
                    if (modifiers.Count == 0 || modifiers[0].Kind != ModifierKind.Step)
                        return Error.IllegalArgument;

                    stepModifier = modifiers[0];
                    if (stepModifier.StepDepth == StepDepth.Out)
                    {
                        // we want to attach the filter as a frame pop request instead of a step request
                        eventKind = EventKind.FramePop;
                        internalEventKind = EventKind.SingleStep;
                    }

                    break;

                default:
                    break;
                }

                requestId = new RequestId(Interlocked.Increment(ref _nextRequestId));
                if (internalRequest)
                    requestId = new RequestId(-requestId.Id);

                EventFilter filter = EventFilter.CreateFilter(internalEventKind, environment, nativeEnvironment, requestId, suspendPolicy, modifiers);
                return SetEventInternal(environment, nativeEnvironment, eventKind, filter);
            }

            public Error SetEventInternal(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventKind eventKind, EventFilter filter)
            {
                EventRequestModifier locationModifier = default(EventRequestModifier);
                EventRequestModifier stepModifier = default(EventRequestModifier);

                switch (eventKind)
                {
                case EventKind.Breakpoint:
                    // we're going to need the location modifier to set the breakpoint
                    if (filter.Modifiers.Count == 0 || filter.Modifiers[0].Kind != ModifierKind.LocationFilter)
                        return Error.IllegalArgument;

                    locationModifier = filter.Modifiers[0];
                    break;

                case EventKind.SingleStep:
                    // the first modifier contains the step properties
                    if (filter.Modifiers.Count == 0 || filter.Modifiers[0].Kind != ModifierKind.Step)
                        return Error.IllegalArgument;

                    stepModifier = filter.Modifiers[0];
                    break;

                case EventKind.FramePop:
                    if (filter.InternalEventKind == EventKind.SingleStep)
                        goto case EventKind.SingleStep;

                    break;

                default:
                    break;
                }

                lock (_eventRequests)
                {
                    Dictionary<RequestId, EventFilter> requests;
                    if (!_eventRequests.TryGetValue(eventKind, out requests))
                    {
                        requests = new Dictionary<RequestId, EventFilter>();
                        _eventRequests.Add(eventKind, requests);
                    }

                    requests.Add(filter.RequestId, filter);
                    if (requests.Count == 1)
                    {
                        JvmEventType? eventToEnable = GetJvmEventType(eventKind);

                        if (eventToEnable != null)
                        {
                            jvmtiError error = Environment.SetEventNotificationMode(JvmEventMode.Enable, eventToEnable.Value);
                            if (error != jvmtiError.None)
                                return GetStandardError(error);
                        }
                    }
                }

                switch (eventKind)
                {
                case EventKind.Breakpoint:
                    {
                        Contract.Assert(locationModifier.Kind == ModifierKind.LocationFilter);
                        jmethodID methodId = locationModifier.Location.Method;
                        jlocation location = new jlocation((long)locationModifier.Location.Index);
                        jvmtiError error = Environment.SetBreakpoint(methodId, location);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);

                        break;
                    }

                case EventKind.FramePop:
                    if (filter.InternalEventKind == EventKind.SingleStep)
                    {
                        using (var thread = VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, stepModifier.Thread))
                        {
                            if (!thread.IsAlive)
                                return Error.InvalidThread;

                            jvmtiError error = environment.RawInterface.NotifyFramePop(environment, thread.Value, 0);
                            if (error != jvmtiError.None)
                                return GetStandardError(error);
                        }
                    }

                    break;

                default:
                    break;
                }

                return Error.None;
            }

            public Error ClearEvent(EventKind eventKind, RequestId requestId)
            {
                if (eventKind == EventKind.SingleStep)
                {
                    // this event might also be registered as a frame pop event with the same request ID
                    Error error = ClearEventInternal(EventKind.FramePop, requestId);
                    if (error != Error.None)
                        return error;
                }

                return ClearEventInternal(eventKind, requestId);
            }

            public Error ClearEventInternal(EventKind eventKind, RequestId requestId)
            {
                lock (_eventRequests)
                {
                    Dictionary<RequestId, EventFilter> requests;
                    if (!_eventRequests.TryGetValue(eventKind, out requests))
                        return Error.None;

                    EventFilter eventFilter;
                    if (!requests.TryGetValue(requestId, out eventFilter))
                        return Error.None;

                    requests.Remove(requestId);
                    if (requests.Count == 0)
                    {
                        JvmEventType? eventToDisable = GetJvmEventType(eventKind);
                        if (eventToDisable != null)
                        {
                            jvmtiError error = Environment.SetEventNotificationMode(JvmEventMode.Disable, eventToDisable.Value);
                            if (error != jvmtiError.None)
                                return GetStandardError(error);
                        }
                    }

                    if (eventKind == EventKind.Breakpoint)
                    {
                        LocationEventFilter locationFilter = eventFilter as LocationEventFilter;
                        if (locationFilter == null)
                        {
                            AggregateEventFilter aggregateFilter = eventFilter as AggregateEventFilter;
                            Contract.Assert(aggregateFilter != null);
                            locationFilter = aggregateFilter.Filters.OfType<LocationEventFilter>().FirstOrDefault();
                        }

                        Contract.Assert(locationFilter != null);
                        jmethodID methodId = locationFilter.Location.Method;
                        jlocation location = new jlocation((long)locationFilter.Location.Index);
                        jvmtiError error = Environment.ClearBreakpoint(methodId, location);
                        if (error != jvmtiError.None)
                            return GetStandardError(error);
                    }

                    return Error.None;
                }
            }

            private static JvmEventType? GetJvmEventType(EventKind eventKind)
            {
                switch (eventKind)
                {
                case EventKind.Breakpoint:
                    return JvmEventType.Breakpoint;

                case EventKind.SingleStep:
                    return JvmEventType.SingleStep;

                case EventKind.ThreadStart:
                    return JvmEventType.ThreadStart;

                case EventKind.ThreadEnd:
                    return JvmEventType.ThreadEnd;

                case EventKind.ClassLoad:
                    return JvmEventType.ClassLoad;

                case EventKind.ClassPrepare:
                    return JvmEventType.ClassPrepare;

                case EventKind.ClassUnload:
                    throw new NotSupportedException();

                case EventKind.Exception:
                    return JvmEventType.Exception;

                case EventKind.ExceptionCatch:
                    return JvmEventType.ExceptionCatch;

                case EventKind.FieldAccess:
                    return JvmEventType.FieldAccess;

                case EventKind.FieldModification:
                    return JvmEventType.FieldModification;

                case EventKind.FramePop:
                    return JvmEventType.FramePop;

                case EventKind.MethodEntry:
                    return JvmEventType.MethodEntry;

                case EventKind.MethodExit:
                    return JvmEventType.MethodExit;

                default:
                    return null;
                }
            }

            public Error ClearAllBreakpoints()
            {
                throw new NotImplementedException();
            }

            private EventFilter[] GetEventFilters(EventKind eventKind)
            {
                lock (_eventRequests)
                {
                    Dictionary<RequestId, EventFilter> requests;
                    if (!_eventRequests.TryGetValue(eventKind, out requests))
                        return new EventFilter[0];

                    return requests.Values.ToArray();
                }
            }

            private jvmtiError ApplySuspendPolicy(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, SuspendPolicy policy, ThreadId eventThread)
            {
                if (policy == SuspendPolicy.EventThread && eventThread == default(ThreadId))
                {
                    return jvmtiError.InvalidThread;
                }

                switch (policy)
                {
                case SuspendPolicy.None:
                    return jvmtiError.None;

                case SuspendPolicy.EventThread:
                    return environment.SuspendThread(nativeEnvironment, eventThread);

                case SuspendPolicy.All:
                    ThreadId[] requestList;
                    JvmtiErrorHandler.ThrowOnFailure(environment.GetAllThreads(nativeEnvironment, out requestList));
                    jvmtiError[] errors;
                    return environment.SuspendThreads(nativeEnvironment, requestList, out errors);

                default:
                    throw new ArgumentException("Invalid suspend policy.");
                }
            }

            private void HandleVMInit(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle)
            {
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment;

                if (AgentEventDispatcher == null)
                {
                    if (VirtualMachine.IsAgentThread.Value)
                        throw new InvalidOperationException();

                    nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv);
                    VirtualMachine.HandleVMInit(environment, nativeEnvironment, threadHandle);
                    InitializeAgentThread(environment, nativeEnvironment);
                }

                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread> method = HandleVMInit;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle);
                    return;
                }

                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));
                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);

                bool sent = false;
                EventFilter[] filters = GetEventFilters(EventKind.VirtualMachineStart);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), default(Location?)))
                    {
                        ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                        Callback.VirtualMachineStart(filter.SuspendPolicy, filter.RequestId, threadId);
                        sent = true;
                    }
                }

                if (!sent)
                {
                    ApplySuspendPolicy(environment, nativeEnvironment, SuspendPolicy.All, threadId);
                    Callback.VirtualMachineStart(SuspendPolicy.All, default(RequestId), threadId);
                }

            }

            private void InitializeAgentThread(JvmtiEnvironment environment, JniEnvironment nativeEnvironment)
            {
                _agentStartedEvent = new ManualResetEventSlim(false);
                _agentThread = CreateAgentThread(nativeEnvironment);
                _agentCallbackDelegate = AgentDispatcherThread;
                JvmtiErrorHandler.ThrowOnFailure(environment.RawInterface.RunAgentThread(environment, _agentThread, _agentCallbackDelegate, IntPtr.Zero, JvmThreadPriority.Maximum));
                _agentStartedEvent.Wait();
            }

            private jthread CreateAgentThread(JniEnvironment nativeEnvironment)
            {
                jclass @class = nativeEnvironment.FindClass("java/lang/Thread");
                if (@class == jclass.Null)
                    throw new Exception("ERROR: JNI: Cannot find java/lang/Thread with FindClass.");

                jmethodID method = nativeEnvironment.GetMethodId(@class, "<init>", "()V");
                if (method == jmethodID.Null)
                    throw new Exception("Cannot find Thread constructor method.");

                jthread result = (jthread)nativeEnvironment.NewObject(@class, method);
                if (result == jthread.Null)
                    throw new Exception("Cannot create new Thread object");

                jthread agentThread = (jthread)nativeEnvironment.NewGlobalReference(result);
                if (result == jthread.Null)
                    throw new Exception("Cannot create a new global reference for the agent thread.");

                // don't need to keep the local reference around
                nativeEnvironment.DeleteLocalReference(result);

                return agentThread;
            }

            private void AgentDispatcherThread(jvmtiEnvHandle env, JNIEnvHandle jniEnv, IntPtr arg)
            {
                JniEnvironment nativeEnvironment;
                VirtualMachine.AttachCurrentThreadAsDaemon(Environment, out nativeEnvironment, true);
                _agentEventDispatcher = Dispatcher.CurrentDispatcher;
                _agentStartedEvent.Set();
                Dispatcher.Run();
            }

            private void HandleVMDeath(jvmtiEnvHandle env, JNIEnvHandle jniEnv)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle> method = HandleVMDeath;
                    AgentEventDispatcher.Invoke(method, env, jniEnv);
                    return;
                }

                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);

                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                EventFilter[] filters = GetEventFilters(EventKind.VirtualMachineDeath);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, default(ThreadId), default(TaggedReferenceTypeId), default(Location?)))
                    {
                        ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, default(ThreadId));
                        Callback.VirtualMachineDeath(filter.SuspendPolicy, filter.RequestId);
                    }
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleVMDeath(environment);
                //}

                //environment.VirtualMachine.ShutdownAgentDispatchers();
            }

            private void HandleThreadStart(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle)
            {
                /* This event is always sent on the thread that's starting. if it's an agent thread, just
                 * ignore the event to hide it from the IDE.
                 */
                if (VirtualMachine.IsAgentThread.Value)
                    return;

                // ignore events before VMInit
                if (AgentEventDispatcher == null)
                    return;

                // dispatch this call to an agent thread
                Action<jvmtiEnvHandle, JNIEnvHandle, jthread> method = HandleThreadStartImpl;
                AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle);
                return;
            }

            private void HandleThreadStartImpl(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle)
            {
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);
                EventFilter[] filters = GetEventFilters(EventKind.ThreadStart);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), default(Location?)))
                    {
                        ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                        Callback.ThreadStart(filter.SuspendPolicy, filter.RequestId, threadId);
                    }
                }
            }

            private void HandleThreadEnd(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle)
            {
                /* This event is always sent on the thread that's starting. if it's an agent thread, just
                 * ignore the event to hide it from the IDE.
                 */
                if (VirtualMachine.IsAgentThread.Value)
                    return;

                // ignore events before VMInit
                if (AgentEventDispatcher == null)
                    return;

                // dispatch this call to an agent thread
                Action<jvmtiEnvHandle, JNIEnvHandle, jthread> method = HandleThreadEndImpl;
                AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle);
                return;
            }

            private void HandleThreadEndImpl(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle)
            {
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));
                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);
                EventFilter[] filters = GetEventFilters(EventKind.ThreadEnd);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), default(Location?)))
                    {
                        ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                        Callback.ThreadDeath(filter.SuspendPolicy, filter.RequestId, threadId);
                    }
                }
            }

            private void HandleClassFileLoadHook(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jclass classBeingRedefinedHandle, jobject loaderHandle, IntPtr name, jobject protectionDomainHandle, int classDataLength, IntPtr classData, ref int newClassDataLength, ref IntPtr newClassData)
            {
                // need to extract the exception_table for each method
                ClassFile classFile = ClassFile.FromMemory(classData, 0);
                ConstantClass constantClass = classFile.ThisClass;
                ConstantUtf8 constantClassSignature = (ConstantUtf8)classFile.ConstantPool[constantClass.NameIndex - 1];
                string classSignature = constantClassSignature.Value;
                if (classSignature[0] != '[')
                    classSignature = 'L' + classSignature + ';';

                long classLoaderTag;
                JvmtiErrorHandler.ThrowOnFailure(Environment.TagClassLoader(loaderHandle, out classLoaderTag));

                foreach (var methodInfo in classFile.Methods)
                {
                    if ((methodInfo.Modifiers & (AccessModifiers.Abstract | AccessModifiers.Native)) != 0)
                        continue;

                    Code codeAttribute = methodInfo.Attributes.OfType<Code>().SingleOrDefault();
                    if (codeAttribute == null)
                        continue;

                    ConstantUtf8 constantMethodName = (ConstantUtf8)classFile.ConstantPool[methodInfo.NameIndex - 1];
                    string methodName = constantMethodName.Value;

                    ConstantUtf8 constantMethodSignature = (ConstantUtf8)classFile.ConstantPool[methodInfo.DescriptorIndex - 1];
                    string methodSignature = constantMethodSignature.Value;

                    VirtualMachine.SetExceptionTable(classLoaderTag, classSignature, methodName, methodSignature, codeAttribute.ExceptionTable);
                }

#if false
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    //Action<jvmtiEnvHandle, JNIEnvHandle, jclass, jobject, ModifiedUTF8StringData, jobject, int, IntPtr> method = HandleClassFileLoadHook;
                    //AgentEventDispatcher.Invoke(method, env, jniEnv);
                    //return;
                    throw new NotImplementedException();
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmClassReference classBeingRedefined = JvmClassReference.FromHandle(environment, jniEnv, classBeingRedefinedHandle, true);
                //JvmObjectReference loader = JvmObjectReference.FromHandle(environment, jniEnv, loaderHandle, true);
                //JvmObjectReference protectionDomain = JvmObjectReference.FromHandle(environment, jniEnv, protectionDomainHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleClassFileLoadHook(environment, classBeingRedefined, loader, name.GetString(), protectionDomain);
                //}
#endif
            }

            private void HandleClassLoad(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jclass classHandle)
            {
                //JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                //JniEnvironment nativeEnvironment;
                //JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                //VirtualMachine.HandleClassLoad(environment, nativeEnvironment, classHandle);
            }

            private void HandleClassPrepare(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jclass classHandle)
            {
                bool preventSuspend = VirtualMachine.IsAgentThread.Value;
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv);

                string signature;
                IntPtr signaturePtr;
                IntPtr genericPtr;
                JvmtiErrorHandler.ThrowOnFailure(RawInterface.GetClassSignature(env, classHandle, out signaturePtr, out genericPtr));
                try
                {
                    unsafe
                    {
                        signature = ModifiedUTF8Encoding.GetString((byte*)signaturePtr);
                    }
                }
                finally
                {
                    RawInterface.Deallocate(env, signaturePtr);
                    RawInterface.Deallocate(env, genericPtr);
                }

                ClassStatus classStatus = 0;
                jvmtiClassStatus internalClassStatus;
                JvmtiErrorHandler.ThrowOnFailure(RawInterface.GetClassStatus(env, classHandle, out internalClassStatus));
                if ((internalClassStatus & jvmtiClassStatus.Error) != 0)
                    classStatus |= ClassStatus.Error;
                if ((internalClassStatus & jvmtiClassStatus.Initialized) != 0)
                    classStatus |= ClassStatus.Initialized;
                if ((internalClassStatus & jvmtiClassStatus.Prepared) != 0)
                    classStatus |= ClassStatus.Prepared;
                if ((internalClassStatus & jvmtiClassStatus.Verified) != 0)
                    classStatus |= ClassStatus.Verified;

                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);
                TaggedReferenceTypeId classId = VirtualMachine.TrackLocalClassReference(classHandle, environment, nativeEnvironment, false);
                EventFilter[] filters = GetEventFilters(EventKind.ClassPrepare);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, classId, default(Location?)))
                    {
                        SendClassPrepareEvent(environment, filter, threadId, classId, signature, classStatus, preventSuspend);
                    }
                }
            }

            private void SendClassPrepareEvent(JvmtiEnvironment environment, EventFilter filter, ThreadId threadId, TaggedReferenceTypeId classId, string signature, ClassStatus classStatus, bool preventSuspend)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<JvmtiEnvironment, EventFilter, ThreadId, TaggedReferenceTypeId, string, ClassStatus, bool> method = SendClassPrepareEvent;
                    AgentEventDispatcher.Invoke(method, environment, filter, threadId, classId, signature, classStatus, preventSuspend);
                    return;
                }

                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, true));

                SuspendPolicy suspendPolicy = preventSuspend ? SuspendPolicy.None : filter.SuspendPolicy;
                if (!preventSuspend)
                    ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);

                Callback.ClassPrepare(suspendPolicy, filter.RequestId, threadId, classId.TypeTag, classId.TypeId, signature, classStatus);
            }

            private void HandleVMStart(jvmtiEnvHandle env, JNIEnvHandle jniEnv)
            {
                /**********************
                 * Note: there are no dispatchers available at this point.
                 */
            }

            private void HandleException(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation, jobject exceptionHandle, jmethodID catchMethodId, jlocation catchjLocation)
            {
                // don't send exception events from an agent thread
                if (VirtualMachine.IsAgentThread.Value)
                    return;

                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv);

                TaggedReferenceTypeId declaringClass;
                MethodId method = new MethodId(methodId.Handle);
                ulong index = (ulong)jlocation.Value;
                JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass));
                Location location = new Location(declaringClass, method, index);

                Location catchLocation;
                method = new MethodId(catchMethodId.Handle);
                index = (ulong)catchjLocation.Value;
                if (catchMethodId.Handle != IntPtr.Zero)
                {
                    JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass));
                    catchLocation = new Location(declaringClass, method, index);
                }
                else
                {
                    catchLocation = default(Location);
                }

                TaggedObjectId exceptionId = VirtualMachine.TrackLocalObjectReference(exceptionHandle, environment, nativeEnvironment, false);

                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);
                EventFilter[] filters = GetEventFilters(EventKind.Exception);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), location))
                    {
                        SendExceptionEvent(environment, filter, threadId, location, exceptionId, catchLocation);
                    }
                }

                ////Location location = new Location();
                ////Location catchLocation = new Location();
                //throw new NotImplementedException();

#if false
                ThreadId threadId = GetObjectId(ref threadHandle);
                TaggedObjectId exception = GetObjectId(ref exceptionHandle);
                EventFilter[] filters = GetEventFilters(EventKind.Exception);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(threadId, default(TaggedReferenceTypeId)))
                    {
                        ApplySuspendPolicy(environment, filter.SuspendPolicy, threadId);
                        Callback.HandleException(filter.SuspendPolicy, filter.RequestId, threadId, location, exception, catchLocation);
                    }
                }
#endif

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmLocation location = new JvmLocation(environment, method, jlocation);
                //JvmObjectReference exception = JvmObjectReference.FromHandle(environment, jniEnv, exceptionHandle, true);
                //JvmLocation catchLocation = new JvmLocation(environment, catchMethod, catchjLocation);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleException(environment, thread, location, exception, catchLocation);
                //}
            }

            private void SendExceptionEvent(JvmtiEnvironment environment, EventFilter filter, ThreadId threadId, Location location, TaggedObjectId exceptionId, Location catchLocation)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<JvmtiEnvironment, EventFilter, ThreadId, Location, TaggedObjectId, Location> invokeMethod = SendExceptionEvent;
                    AgentEventDispatcher.Invoke(invokeMethod, environment, filter, threadId, location, exceptionId, catchLocation);
                    return;
                }

                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                Callback.Exception(filter.SuspendPolicy, filter.RequestId, threadId, location, exceptionId, catchLocation);
            }

            private void HandleExceptionCatch(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation, jobject exceptionHandle)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, jlocation, jobject> method = HandleExceptionCatch;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, methodId, jlocation, exceptionHandle);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmLocation location = new JvmLocation(environment, method, jlocation);
                //JvmObjectReference exception = JvmObjectReference.FromHandle(environment, jniEnv, exceptionHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleExceptionCatch(environment, thread, location, exception);
                //}
            }

            private void HandleSingleStep(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation)
            {
                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv);

                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);

                TaggedReferenceTypeId declaringClass;
                MethodId method = new MethodId(methodId.Handle);
                ulong index = (ulong)jlocation.Value;
                JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass));
                Location location = new Location(declaringClass, method, index);

                EventFilter[] filters = GetEventFilters(EventKind.SingleStep);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), location))
                    {
                        SendSingleStepEvent(environment, filter, threadId, location);
                    }
                }
            }

            private void SendSingleStepEvent(JvmtiEnvironment environment, EventFilter filter, ThreadId threadId, Location location)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<JvmtiEnvironment, EventFilter, ThreadId, Location> invokeMethod = SendSingleStepEvent;
                    AgentEventDispatcher.Invoke(invokeMethod, environment, filter, threadId, location);
                    return;
                }

                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                Callback.SingleStep(filter.SuspendPolicy, filter.RequestId, threadId, location);
            }

            private void HandleFramePop(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, bool wasPoppedByException)
            {
                try
                {
                    JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                    JniEnvironment nativeEnvironment = JniEnvironment.GetOrCreateInstance(jniEnv);

                    TaggedReferenceTypeId declaringClass;
                    MethodId method = new MethodId(methodId.Handle);
                    jlocation jlocation;
                    JvmtiErrorHandler.ThrowOnFailure(environment.GetFrameLocation(threadHandle, 1, out methodId, out jlocation));
                    ulong index = (ulong)jlocation.Value;
                    JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass));
                    Location location = new Location(declaringClass, method, index);

                    ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);

                    EventFilter[] filters = GetEventFilters(EventKind.FramePop);
                    foreach (var filter in filters)
                    {
                        if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), location))
                        {
                            if (filter.InternalEventKind == EventKind.SingleStep)
                            {
                                // remove the frame pop event
                                JvmtiErrorHandler.ThrowOnFailure((jvmtiError)ClearEventInternal(EventKind.FramePop, filter.RequestId));
                                // set an actual step filter to respond when the thread arrives in the parent frame
                                JvmtiErrorHandler.ThrowOnFailure((jvmtiError)SetEventInternal(environment, nativeEnvironment, EventKind.SingleStep, filter));
                            }
                            else
                            {
                                SendFramePopEvent(environment, filter, threadId, location, wasPoppedByException);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    string caption = "Exception while handling a frame pop event";
                    System.Windows.Forms.MessageBox.Show(e.Message + System.Environment.NewLine + System.Environment.NewLine + e.StackTrace, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    throw;
                }
            }

            private void SendFramePopEvent(JvmtiEnvironment environment, EventFilter filter, ThreadId threadId, Location location, bool wasPoppedByException)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<JvmtiEnvironment, EventFilter, ThreadId, Location, bool> invokeMethod = SendFramePopEvent;
                    AgentEventDispatcher.Invoke(invokeMethod, environment, filter, threadId, location, wasPoppedByException);
                    return;
                }

                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                if (filter.InternalEventKind == EventKind.SingleStep)
                {
                    ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                    Callback.SingleStep(filter.SuspendPolicy, filter.RequestId, threadId, location);
                }
            }

            private void HandleBreakpoint(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, jlocation> invokeMethod = HandleBreakpoint;
                    AgentEventDispatcher.Invoke(invokeMethod, env, jniEnv, threadHandle, methodId, jlocation);
                    return;
                }

                JvmtiEnvironment environment = JvmtiEnvironment.GetOrCreateInstance(_service.VirtualMachine, env);
                JniEnvironment nativeEnvironment;
                JniErrorHandler.ThrowOnFailure(VirtualMachine.AttachCurrentThreadAsDaemon(environment, out nativeEnvironment, false));

                ThreadId threadId = VirtualMachine.TrackLocalThreadReference(threadHandle, environment, nativeEnvironment, false);

                TaggedReferenceTypeId declaringClass;
                MethodId method = new MethodId(methodId.Handle);
                ulong index = (ulong)jlocation.Value;
                JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, method, out declaringClass));
                Location location = new Location(declaringClass, method, index);

                EventFilter[] filters = GetEventFilters(EventKind.Breakpoint);
                foreach (var filter in filters)
                {
                    if (filter.ProcessEvent(environment, nativeEnvironment, this, threadId, default(TaggedReferenceTypeId), location))
                    {
                        ApplySuspendPolicy(environment, nativeEnvironment, filter.SuspendPolicy, threadId);
                        Callback.Breakpoint(filter.SuspendPolicy, filter.RequestId, threadId, location);
                    }
                }
            }

            private void HandleFieldAccess(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation, jclass fieldClassHandle, jobject objectHandle, jfieldID fieldId)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, jlocation, jclass, jobject, jfieldID> method = HandleFieldAccess;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, methodId, jlocation, fieldClassHandle, objectHandle, fieldId);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmLocation location = new JvmLocation(environment, method, jlocation);
                //JvmClassReference fieldClass = JvmClassReference.FromHandle(environment, jniEnv, fieldClassHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);
                //JvmField field = new JvmField(environment, fieldId);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleFieldAccess(environment, thread, location, fieldClass, @object, field);
                //}
            }

            private void HandleFieldModification(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, jlocation jlocation, jclass fieldClassHandle, jobject objectHandle, jfieldID fieldId, byte signatureType, jvalue newValue)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, jlocation, jclass, jobject, jfieldID, byte, jvalue> method = HandleFieldModification;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, methodId, jlocation, fieldClassHandle, objectHandle, fieldId, signatureType, newValue);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmLocation location = new JvmLocation(environment, method, jlocation);
                //JvmClassReference fieldClass = JvmClassReference.FromHandle(environment, jniEnv, fieldClassHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);
                //JvmField field = new JvmField(environment, fieldId);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleFieldModification(environment, thread, location, fieldClass, @object, field, signatureType, newValue);
                //}
            }

            private void HandleMethodEntry(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID> method = HandleMethodEntry;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, methodId);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmMethod method = new JvmMethod(environment, methodId);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMethodEntry(environment, thread, method);
                //}
            }

            private void HandleMethodExit(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, bool wasPoppedByException, jvalue returnValue)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, bool, jvalue> method = HandleMethodExit;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, methodId, wasPoppedByException, returnValue);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmMethod method = new JvmMethod(environment, methodId);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMethodExit(environment, thread, method, wasPoppedByException, returnValue);
                //}
            }

            private void HandleNativeMethodBind(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jmethodID methodId, IntPtr address, ref IntPtr newAddressPtr)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    //Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jmethodID, IntPtr, ReferenceTypeData intptr> method = HandleNativeMethodBind;
                    //AgentEventDispatcher.Invoke(method, env, jniEnv);
                    //return;
                    throw new NotImplementedException();
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmMethod method = new JvmMethod(environment, methodId);

                //foreach (var processor in _processors)
                //{
                //    IntPtr? newAddress = null;
                //    processor.HandleNativeMethodBind(environment, thread, method, address, ref newAddress);
                //}
            }

            private void HandleCompiledMethodLoad(jvmtiEnvHandle env, jmethodID methodId, int codeSize, IntPtr codeAddress, int mapLength, jvmtiAddressLocationMap[] map, IntPtr compileInfo)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, jmethodID, int, IntPtr, int, jvmtiAddressLocationMap[], IntPtr> method = HandleCompiledMethodLoad;
                    AgentEventDispatcher.Invoke(method, env, methodId, codeSize, codeAddress, mapLength, map, compileInfo);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    throw new NotImplementedException();
                //    //processor.HandleCompiledMethodLoad(environment, method, codeSize, codeAddress, map2, compileInfo);
                //}
            }

            private void HandleCompiledMethodUnload(jvmtiEnvHandle env, jmethodID methodId, IntPtr codeAddress)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, jmethodID, IntPtr> method = HandleCompiledMethodUnload;
                    AgentEventDispatcher.Invoke(method, env, methodId, codeAddress);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmMethod method = new JvmMethod(environment, methodId);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleCompiledMethodUnload(environment, method, codeAddress);
                //}
            }

            private void HandleDynamicCodeGenerated(jvmtiEnvHandle env, IntPtr name, IntPtr address, int length)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, IntPtr, IntPtr, int> method = HandleDynamicCodeGenerated;
                    AgentEventDispatcher.Invoke(method, env, name, address, length);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleDynamicCodeGenerated(environment, name.GetString(), address, length);
                //}
            }

            private void HandleDataDumpRequest(jvmtiEnvHandle env)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle> method = HandleDataDumpRequest;
                    AgentEventDispatcher.Invoke(method, env);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleDataDumpRequest(environment);
                //}
            }

            private void HandleMonitorWait(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jobject objectHandle, long millisecondsTimeout)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jobject, long> method = HandleMonitorWait;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, objectHandle, millisecondsTimeout);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMonitorWait(environment, thread, @object, millisecondsTimeout);
                //}
            }

            private void HandleMonitorWaited(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jobject objectHandle, bool timedOut)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jobject, bool> method = HandleMonitorWaited;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, objectHandle, timedOut);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMonitorWaited(environment, thread, @object, timedOut);
                //}
            }

            private void HandleMonitorContendedEnter(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jobject objectHandle)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jobject> method = HandleMonitorContendedEnter;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, objectHandle);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMonitorContendedEnter(environment, thread, @object);
                //}
            }

            private void HandleMonitorContendedEntered(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jobject objectHandle)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jobject> method = HandleMonitorContendedEntered;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, objectHandle);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleMonitorContendedEntered(environment, thread, @object);
                //}
            }

            private void HandleResourceExhausted(jvmtiEnvHandle env, JNIEnvHandle jniEnv, JvmResourceExhaustedFlags flags, IntPtr reserved, IntPtr description)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, JvmResourceExhaustedFlags, IntPtr, IntPtr> method = HandleResourceExhausted;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, flags, reserved, description);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleResourceExhausted(environment, flags, reserved, description.GetString());
                //}
            }

            private void HandleGarbageCollectionStart(jvmtiEnvHandle env)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle> method = HandleGarbageCollectionStart;
                    AgentEventDispatcher.Invoke(method, env);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleGarbageCollectionStart(environment);
                //}
            }

            private void HandleGarbageCollectionFinish(jvmtiEnvHandle env)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle> method = HandleGarbageCollectionFinish;
                    AgentEventDispatcher.Invoke(method, env);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleGarbageCollectionFinish(environment);
                //}
            }

            private void HandleObjectFree(jvmtiEnvHandle env, long tag)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, long> method = HandleObjectFree;
                    AgentEventDispatcher.Invoke(method, env, tag);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleObjectFree(environment, tag);
                //}
            }

            private void HandleVMObjectAlloc(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread threadHandle, jobject objectHandle, jclass objectClassHandle, long size)
            {
                if (!VirtualMachine.IsAgentThread.Value)
                {
                    // ignore events before VMInit
                    if (AgentEventDispatcher == null)
                        return;

                    // dispatch this call to an agent thread
                    Action<jvmtiEnvHandle, JNIEnvHandle, jthread, jobject, jclass, long> method = HandleVMObjectAlloc;
                    AgentEventDispatcher.Invoke(method, env, jniEnv, threadHandle, objectHandle, objectClassHandle, size);
                    return;
                }

                //JvmEnvironment environment = JvmEnvironment.GetEnvironment(env);
                //JvmThreadReference thread = JvmThreadReference.FromHandle(environment, jniEnv, threadHandle, true);
                //JvmObjectReference @object = JvmObjectReference.FromHandle(environment, jniEnv, objectHandle, true);
                //JvmClassReference objectClass = JvmClassReference.FromHandle(environment, jniEnv, objectClassHandle, true);

                //foreach (var processor in _processors)
                //{
                //    processor.HandleVMObjectAllocation(environment, thread, @object, objectClass, size);
                //}
            }
        }
    }
}
