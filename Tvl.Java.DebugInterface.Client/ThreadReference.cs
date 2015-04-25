namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;
    using ThreadStatus = Tvl.Java.DebugInterface.ThreadStatus;

    internal sealed class ThreadReference : ObjectReference, IThreadReference
    {
        internal ThreadReference(VirtualMachine virtualMachine, ThreadId threadId)
            : base(virtualMachine, threadId, null)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal ThreadReference(VirtualMachine virtualMachine, ThreadId threadId, IReferenceType threadType)
            : base(virtualMachine, threadId, threadType)
        {
            Contract.Requires(virtualMachine != null);
        }

        public ThreadId ThreadId
        {
            get
            {
                return (ThreadId)base.ObjectId;
            }
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.Thread, ObjectId.Handle);
        }

        public IObjectReference GetCurrentContendedMonitor()
        {
            throw new NotImplementedException();
        }

        public void ForceEarlyReturn(IValue returnValue)
        {
            throw new NotImplementedException();
        }

        public IStackFrame GetFrame(int index)
        {
            return GetFrames(index, 1)[0];
        }

        public int GetFrameCount()
        {
            int frameCount;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadFrameCount(out frameCount, this.ThreadId));
            return frameCount;
        }

        public ReadOnlyCollection<IStackFrame> GetFrames()
        {
            return GetFrames(0, GetFrameCount());
        }

        public ReadOnlyCollection<IStackFrame> GetFrames(int startFrame, int length)
        {
            FrameLocationData[] framesData;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadFrames(out framesData, ThreadId, startFrame, length));
            return new ReadOnlyCollection<IStackFrame>(Array.ConvertAll(framesData, frameData => VirtualMachine.GetMirrorOf(this, frameData)));
        }

        public void Interrupt()
        {
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.InterruptThread(ThreadId));
        }

        public bool GetIsAtBreakpoint()
        {
            throw new NotImplementedException();
        }

        public bool GetIsSuspended()
        {
            Types.ThreadStatus threadStatus;
            SuspendStatus suspendStatus;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadStatus(out threadStatus, out suspendStatus, ThreadId));
            return suspendStatus == SuspendStatus.Suspended;
        }

        public string GetName()
        {
            string name;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadName(out name, ThreadId));
            return name;
        }

        public ReadOnlyCollection<IObjectReference> GetOwnedMonitors()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorInfo> GetOwnedMonitorsAndFrames()
        {
            throw new NotImplementedException();
        }

        public void PopFrames(IStackFrame frame)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.ResumeThread(this.ThreadId));
        }

        public ThreadStatus GetStatus()
        {
            Types.ThreadStatus threadStatus;
            SuspendStatus suspendStatus;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadStatus(out threadStatus, out suspendStatus, ThreadId));
            return (ThreadStatus)threadStatus;
        }

        public void Stop(IObjectReference throwable)
        {
            ObjectReference objectReference = throwable as ObjectReference;
            if (objectReference == null || !VirtualMachine.Equals(objectReference.VirtualMachine))
                throw new VirtualMachineMismatchException();

            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.StopThread(this.ThreadId, objectReference.ObjectId));
        }

        public void Suspend()
        {
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SuspendThread(this.ThreadId));
        }

        public int GetSuspendCount()
        {
            int suspendCount;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadSuspendCount(out suspendCount, ThreadId));
            return suspendCount;
        }

        public IThreadGroupReference GetThreadGroup()
        {
            ThreadGroupId threadGroup;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadGroup(out threadGroup, ThreadId));
            return VirtualMachine.GetMirrorOf(threadGroup);
        }
    }
}
