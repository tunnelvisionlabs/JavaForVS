namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Collections;
    using Tvl.Java.DebugHost.Interop;
    using Tvl.Java.DebugInterface.Types;
    using Tvl.Java.DebugInterface.Types.Analysis;

    using ExceptionTableEntry = Tvl.Java.DebugInterface.Types.Loader.ExceptionTableEntry;

    partial class DebugProtocolService
    {
        internal abstract class EventFilter
        {
            private readonly EventKind _internalEventKind;
            private readonly RequestId _requestId;
            private readonly SuspendPolicy _suspendPolicy;
            private readonly ImmutableList<EventRequestModifier> _modifiers;

            public EventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers)
            {
                Contract.Requires<ArgumentNullException>(modifiers != null, "modifiers");

                _internalEventKind = internalEventKind;
                _requestId = requestId;
                _suspendPolicy = suspendPolicy;
                _modifiers = new ImmutableList<EventRequestModifier>(modifiers);
            }

            public EventKind InternalEventKind
            {
                get
                {
                    return _internalEventKind;
                }
            }

            public RequestId RequestId
            {
                get
                {
                    return _requestId;
                }
            }

            public SuspendPolicy SuspendPolicy
            {
                get
                {
                    return _suspendPolicy;
                }
            }

            public ImmutableList<EventRequestModifier> Modifiers
            {
                get
                {
                    return _modifiers;
                }
            }

            public abstract bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location);

            public static EventFilter CreateFilter(EventKind internalEventKind, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, RequestId requestId, SuspendPolicy suspendPolicy, ImmutableList<EventRequestModifier> modifiers)
            {
                if (modifiers.Count == 0)
                    return new PassThroughEventFilter(internalEventKind, requestId, suspendPolicy, modifiers);

                EventFilter[] elements = modifiers.Select(modifier => CreateFilter(internalEventKind, environment, nativeEnvironment, requestId, suspendPolicy, modifiers, modifier)).ToArray();
                if (elements.Length == 1)
                    return elements[0];

                return new AggregateEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, elements);
            }

            private static EventFilter CreateFilter(EventKind internalEventKind, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, RequestId requestId, SuspendPolicy suspendPolicy, ImmutableList<EventRequestModifier> modifiers, EventRequestModifier modifier)
            {
                switch (modifier.Kind)
                {
                case ModifierKind.Count:
                    return new CountEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, modifier.Count);

                case ModifierKind.ThreadFilter:
                    return new ThreadEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, modifier.Thread);

                case ModifierKind.ClassTypeFilter:
                    throw new NotImplementedException();

                case ModifierKind.ClassMatchFilter:
                    throw new NotImplementedException();

                case ModifierKind.ClassExcludeFilter:
                    throw new NotImplementedException();

                case ModifierKind.LocationFilter:
                    return new LocationEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, modifier.Location);

                case ModifierKind.ExceptionFilter:
                    return new ExceptionEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, modifier.ExceptionOrNull, modifier.Caught, modifier.Uncaught);

                case ModifierKind.FieldFilter:
                    throw new NotImplementedException();

                case ModifierKind.Step:
                    return new StepEventFilter(internalEventKind, requestId, suspendPolicy, modifiers, modifier.Thread, environment, nativeEnvironment, modifier.StepSize, modifier.StepDepth);

                case ModifierKind.InstanceFilter:
                    throw new NotImplementedException();

                case ModifierKind.SourceNameMatchFilter:
                    throw new NotImplementedException();

                case ModifierKind.Conditional:
                    throw new NotImplementedException();

                case ModifierKind.Invalid:
                default:
                    throw new ArgumentException();
                }
            }
        }

        internal sealed class PassThroughEventFilter : EventFilter
        {
            public PassThroughEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                return true;
            }
        }

        internal sealed class AggregateEventFilter : EventFilter
        {
            private readonly EventFilter[] _filters;

            public AggregateEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, IEnumerable<EventFilter> filters)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
                Contract.Requires<ArgumentNullException>(filters != null, "filters");
                _filters = filters.ToArray();
            }

            public ReadOnlyCollection<EventFilter> Filters
            {
                get
                {
                    return new ReadOnlyCollection<EventFilter>(_filters);
                }
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                foreach (EventFilter filter in _filters)
                {
                    if (!filter.ProcessEvent(environment, nativeEnvironment, processor, thread, @class, location))
                        return false;
                }

                return true;
            }
        }

        internal sealed class CountEventFilter : EventFilter
        {
            private readonly int _count;

            private int _current;

            public CountEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, int count)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
                _count = count;
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                _current++;
                if (_current == _count)
                {
                    _current = 0;
                    return true;
                }

                return false;
            }
        }

        internal class ThreadEventFilter : EventFilter
        {
            private readonly ThreadId _thread;

            public ThreadEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, ThreadId thread)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
                _thread = thread;
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                return _thread == default(ThreadId)
                    || _thread == thread;
            }
        }

        internal sealed class LocationEventFilter : EventFilter
        {
            private readonly Location _location;

            public LocationEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, Location location)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
                _location = location;
            }

            public Location Location
            {
                get
                {
                    return _location;
                }
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                if (!location.HasValue)
                    return false;

                return _location.Index == location.Value.Index
                    && _location.Class == location.Value.Class
                    && _location.Method == location.Value.Method;
            }
        }

        internal sealed class ExceptionEventFilter : EventFilter
        {
            private readonly ReferenceTypeId _exceptionType;
            private readonly bool _caught;
            private readonly bool _uncaught;

            public ExceptionEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, ReferenceTypeId exceptionType, bool caught, bool uncaught)
                : base(internalEventKind, requestId, suspendPolicy, modifiers)
            {
                _exceptionType = exceptionType;
                _caught = caught;
                _uncaught = uncaught;
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                return true;
            }
        }

        internal sealed class StepEventFilter : ThreadEventFilter
        {
            private readonly StepSize _size;
            private readonly StepDepth _depth;

            // used for step over
            private bool _hasMethodInfo;
            private jmethodID _lastMethod;
            private jlocation _lastLocation;
            private int? _lastLine;
            private int _stackDepth;
            private bool _convertedToFramePop;
            private jmethodID _framePopMethod;

            private DisassembledMethod _disassembledMethod;
            private ReadOnlyCollection<ConstantPoolEntry> _constantPool;
            private ImmutableList<int?> _evaluationStackDepths;

            public StepEventFilter(EventKind internalEventKind, RequestId requestId, SuspendPolicy suspendPolicy, IEnumerable<EventRequestModifier> modifiers, ThreadId thread, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, StepSize size, StepDepth depth)
                : base(internalEventKind, requestId, suspendPolicy, modifiers, thread)
            {
                if (size == StepSize.Statement && JavaVM.DisableStatementStepping)
                    size = StepSize.Line;

                _size = size;
                _depth = depth;

                // gather reference information for the thread
                using (var threadHandle = environment.VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, thread))
                {
                    if (threadHandle.IsAlive)
                    {
                        jvmtiError error = environment.GetFrameLocation(threadHandle.Value, 0, out _lastMethod, out _lastLocation);
                        if (error == jvmtiError.None)
                            error = environment.GetFrameCount(threadHandle.Value, out _stackDepth);

                        if (error == jvmtiError.None)
                            _hasMethodInfo = true;

                        UpdateLastLine(environment);

                        if (error == jvmtiError.None && size == StepSize.Statement && (depth == StepDepth.Over || depth == StepDepth.Into))
                        {
                            byte[] bytecode;
                            JvmtiErrorHandler.ThrowOnFailure(environment.GetBytecodes(_lastMethod, out bytecode));
                            _disassembledMethod = BytecodeDisassembler.Disassemble(bytecode);

                            TaggedReferenceTypeId declaringClass;
                            JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodDeclaringClass(nativeEnvironment, _lastMethod, out declaringClass));
                            using (var classHandle = environment.VirtualMachine.GetLocalReferenceForClass(nativeEnvironment, declaringClass.TypeId))
                            {
                                int constantPoolCount;
                                byte[] data;
                                JvmtiErrorHandler.ThrowOnFailure(environment.GetConstantPool(classHandle.Value, out constantPoolCount, out data));

                                List<ConstantPoolEntry> entryList = new List<ConstantPoolEntry>();
                                int currentPosition = 0;
                                for (int i = 0; i < constantPoolCount - 1; i++)
                                {
                                    entryList.Add(ConstantPoolEntry.FromBytes(data, ref currentPosition));
                                    switch (entryList.Last().Type)
                                    {
                                    case ConstantType.Double:
                                    case ConstantType.Long:
                                        // these entries take 2 slots
                                        entryList.Add(ConstantPoolEntry.Reserved);
                                        i++;
                                        break;

                                    default:
                                        break;
                                    }
                                }

                                _constantPool = entryList.AsReadOnly();

                                string classSignature;
                                string classGenericSignature;
                                JvmtiErrorHandler.ThrowOnFailure(environment.GetClassSignature(classHandle.Value, out classSignature, out classGenericSignature));
                                string methodName;
                                string methodSignature;
                                string methodGenericSignature;
                                JvmtiErrorHandler.ThrowOnFailure(environment.GetMethodName(_lastMethod, out methodName, out methodSignature, out methodGenericSignature));

                                jobject classLoader;
                                JvmtiErrorHandler.ThrowOnFailure(environment.GetClassLoader(classHandle.Value, out classLoader));
                                long classLoaderTag;
                                JvmtiErrorHandler.ThrowOnFailure(environment.TagClassLoader(classLoader, out classLoaderTag));

                                ReadOnlyCollection<ExceptionTableEntry> exceptionTable;
                                JvmtiErrorHandler.ThrowOnFailure(environment.VirtualMachine.GetExceptionTable(classLoaderTag, classSignature, methodName, methodSignature, out exceptionTable));

                                _evaluationStackDepths = BytecodeDisassembler.GetEvaluationStackDepths(_disassembledMethod, _constantPool, exceptionTable);
                            }
                        }
                    }
                }
            }

            private void UpdateLastLine(JvmtiEnvironment environment)
            {
                _lastLine = null;
                if (!_hasMethodInfo)
                    return;

                LineNumberData[] lines;
                jvmtiError error = environment.GetLineNumberTable(_lastMethod, out lines);
                if (error != jvmtiError.None)
                    return;

                LineNumberData entry = lines.LastOrDefault(i => i.LineCodeIndex <= _lastLocation.Value);
                if (entry.LineNumber != 0)
                    _lastLine = entry.LineNumber;
            }

            public override bool ProcessEvent(JvmtiEnvironment environment, JniEnvironment nativeEnvironment, EventProcessor processor, ThreadId thread, TaggedReferenceTypeId @class, Location? location)
            {
                if (!base.ProcessEvent(environment, nativeEnvironment, processor, thread, @class, location))
                    return false;

                // Step Out is implemented with Frame Pop events set at the correct depth
                if (_depth == StepDepth.Out)
                {
                    if (location.HasValue && !location.Value.Method.Equals(_lastMethod))
                    {
                        _lastLocation = new jlocation((long)location.Value.Index);
                        _lastMethod = location.Value.Method;
                        UpdateLastLine(environment);
                    }

                    return true;
                }

                using (var threadHandle = environment.VirtualMachine.GetLocalReferenceForThread(nativeEnvironment, thread))
                {
                    int stackDepth;
                    JvmtiErrorHandler.ThrowOnFailure(environment.GetFrameCount(threadHandle.Value, out stackDepth));

                    if (_hasMethodInfo && stackDepth > _stackDepth)
                    {
                        bool convertToFramePop;
                        if (location.HasValue && (!_convertedToFramePop || !_framePopMethod.Equals(location.Value.Method)) && ShouldSkipCurrentMethod(processor.VirtualMachine, environment, nativeEnvironment, threadHandle.Value, stackDepth, location.Value, out convertToFramePop))
                        {
                            if (convertToFramePop)
                            {
                                // remove the single step event
                                JvmtiErrorHandler.ThrowOnFailure((jvmtiError)processor.ClearEventInternal(EventKind.FramePop, this.RequestId));
                                JvmtiErrorHandler.ThrowOnFailure((jvmtiError)processor.ClearEventInternal(EventKind.SingleStep, this.RequestId));
                                // set an actual step filter to respond when the thread arrives back in this frame
                                JvmtiErrorHandler.ThrowOnFailure((jvmtiError)processor.SetEventInternal(environment, nativeEnvironment, EventKind.FramePop, this));
                                _convertedToFramePop = true;
                                _framePopMethod = location.Value.Method;
                            }

                            return false;
                        }
                        else
                        {
                            _convertedToFramePop = false;
                            return true;
                        }
                    }
                    else if (stackDepth == _stackDepth)
                    {
                        if (_size == StepSize.Statement && _disassembledMethod != null)
                        {
                            int instructionIndex = _disassembledMethod.Instructions.FindIndex(i => (uint)i.Offset == location.Value.Index);
                            if (instructionIndex >= 0 && _evaluationStackDepths != null && (_evaluationStackDepths[instructionIndex] ?? 0) != 0)
                            {
                                return false;
                            }
                            else if (instructionIndex >= 0 && _disassembledMethod.Instructions[instructionIndex].OpCode.FlowControl == JavaFlowControl.Branch)
                            {
                                // follow branch instructions before stopping
                                return false;
                            }
                        }
                        else if (_lastLine != null)
                        {
                            // see if we're on the same line
                            LineNumberData[] lines;
                            jvmtiError error = environment.GetLineNumberTable(location.Value.Method, out lines);
                            if (error == jvmtiError.None)
                            {
                                LineNumberData entry = lines.LastOrDefault(i => i.LineCodeIndex <= (long)location.Value.Index);
                                if (entry.LineNumber == _lastLine)
                                    return false;
                            }
                        }
                    }

                    if (location.HasValue)
                    {
                        _lastLocation = new jlocation((long)location.Value.Index);
                        _lastMethod = location.Value.Method;
                        UpdateLastLine(environment);
                    }

                    _stackDepth = stackDepth;
                    return true;
                }
            }

            private bool ShouldSkipCurrentMethod(JavaVM virtualMachine, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, jthread thread, int stackDepth, Location location, out bool convertToFramePop)
            {
                Contract.Assert(_depth == StepDepth.Into || _depth == StepDepth.Over);

                convertToFramePop = false;

                if (!_hasMethodInfo || stackDepth < _stackDepth || (stackDepth == _stackDepth && (location.Method.Equals(_lastMethod))))
                    return false;

                /*
                 * change this to a Frame Pop event if we're not in a native frame
                 */
                bool native;
                jvmtiError error = environment.IsMethodNative(location.Method, out native);
                if (error != jvmtiError.None)
                    return false;

                convertToFramePop = !native;

                if (_depth == StepDepth.Over || native)
                    return true;

                JvmAccessModifiers modifiers;
                error = environment.GetMethodModifiers(location.Method, out modifiers);
                if (error != jvmtiError.None || ((modifiers & JvmAccessModifiers.Static) != 0))
                    return false;

                jobject thisObject;
                error = environment.GetLocalObject(thread, 0, 0, out thisObject);
                if (error != jvmtiError.None)
                    return false;

                try
                {
                    bool classLoader = nativeEnvironment.IsInstanceOf(thisObject, virtualMachine.ClassLoaderClass);
                    if (!classLoader)
                        return false;

                    string name;
                    string signature;
                    string genericSignature;
                    error = environment.GetMethodName(location.Method, out name, out signature, out genericSignature);
                    if (error != jvmtiError.None)
                        return false;

                    if (name == "loadClass" && signature == "(Ljava/lang/String;)Ljava/lang/Class;")
                        return true;

                    if (name == "checkPackageAccess" && signature == "(Ljava/lang/Class;Ljava/security/ProtectionDomain;)V")
                        return true;

                    return false;
                }
                finally
                {
                    nativeEnvironment.DeleteLocalReference(thisObject);
                }
            }
        }
    }
}
