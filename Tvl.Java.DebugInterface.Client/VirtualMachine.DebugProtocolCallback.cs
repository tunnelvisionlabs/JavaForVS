namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Tvl.Java.DebugInterface.Client.DebugProtocol;
    using Tvl.Java.DebugInterface.Client.Events;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Types;
    using SuspendPolicy = Tvl.Java.DebugInterface.Request.SuspendPolicy;

    partial class VirtualMachine
    {
        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true)]
        internal class DebugProtocolCallback : IDebugProtocolServiceCallback
        {
            private readonly VirtualMachine _virtualMachine;

            public DebugProtocolCallback(VirtualMachine virtualMachine)
            {
                Contract.Requires(virtualMachine != null);
                _virtualMachine = virtualMachine;
            }

            public VirtualMachine VirtualMachine
            {
                get
                {
                    return _virtualMachine;
                }
            }

            public void VirtualMachineStart(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.VirtualMachineStart, requestId);
                ThreadEventArgs e = new ThreadEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread);
                VirtualMachine.EventQueue.OnVirtualMachineStart(e);
            }

            public void SingleStep(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId, Types.Location location)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.SingleStep, requestId);
                Location loc = VirtualMachine.GetMirrorOf(location);

                ThreadLocationEventArgs e = new ThreadLocationEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread, loc);
                VirtualMachine.EventQueue.OnSingleStep(e);
            }

            public void Breakpoint(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId, Types.Location location)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.Breakpoint, requestId);
                Location loc = VirtualMachine.GetMirrorOf(location);

                ThreadLocationEventArgs e = new ThreadLocationEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread, loc);
                VirtualMachine.EventQueue.OnBreakpoint(e);
            }

            public void MethodEntry(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Types.Location location)
            {
                throw new NotImplementedException();
            }

            public void MethodExit(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Types.Location location, Types.Value returnValue)
            {
                throw new NotImplementedException();
            }

            public void MonitorContendedEnter(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Types.Location location)
            {
                throw new NotImplementedException();
            }

            public void MonitorContendedEntered(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Types.Location location)
            {
                throw new NotImplementedException();
            }

            public void MonitorContendedWait(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Types.Location location, TimeSpan timeout)
            {
                throw new NotImplementedException();
            }

            public void MonitorContendedWaited(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Types.Location location, bool timedOut)
            {
                throw new NotImplementedException();
            }

            public void Exception(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId, Types.Location location, TaggedObjectId exception, Types.Location catchLocation)
            {
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.Exception, requestId);
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                Location loc = VirtualMachine.GetMirrorOf(location);
                ObjectReference exceptionReference = VirtualMachine.GetMirrorOf(exception);
                Location catchLoc = VirtualMachine.GetMirrorOf(catchLocation);
                ExceptionEventArgs e = new ExceptionEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread, loc, exceptionReference, catchLoc);
                VirtualMachine.EventQueue.OnException(e);
            }

            public void ThreadStart(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.ThreadStart, requestId);
                ThreadEventArgs e = new ThreadEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread);
                VirtualMachine.EventQueue.OnThreadStart(e);
            }

            public void ThreadDeath(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.ThreadDeath, requestId);
                ThreadEventArgs e = new ThreadEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread);
                VirtualMachine.EventQueue.OnThreadDeath(e);
            }

            public void ClassPrepare(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId threadId, TypeTag typeTag, ReferenceTypeId typeId, string signature, ClassStatus status)
            {
                ThreadReference thread = VirtualMachine.GetMirrorOf(threadId);
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.ClassPrepare, requestId);
                ReferenceType type = VirtualMachine.GetMirrorOf(typeTag, typeId);
                ClassPrepareEventArgs e = new ClassPrepareEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request, thread, signature, type);
                VirtualMachine.EventQueue.OnClassPrepare(e);
            }

            public void ClassUnload(Types.SuspendPolicy suspendPolicy, RequestId requestId, string signature)
            {
                throw new NotImplementedException();
            }

            public void FieldAccess(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Types.Location location, TypeTag typeTag, ReferenceTypeId typeId, FieldId field, TaggedObjectId @object)
            {
                throw new NotImplementedException();
            }

            public void FieldModification(Types.SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Types.Location location, TypeTag typeTag, ReferenceTypeId typeId, FieldId field, TaggedObjectId @object, Types.Value newValue)
            {
                throw new NotImplementedException();
            }

            public void VirtualMachineDeath(Types.SuspendPolicy suspendPolicy, RequestId requestId)
            {
                EventRequest request = VirtualMachine.EventRequestManager.GetEventRequest(EventKind.ClassPrepare, requestId);
                VirtualMachineEventArgs e = new VirtualMachineEventArgs(VirtualMachine, (SuspendPolicy)suspendPolicy, request);
                VirtualMachine.EventQueue.OnVirtualMachineDeath(e);
            }
        }
    }
}
