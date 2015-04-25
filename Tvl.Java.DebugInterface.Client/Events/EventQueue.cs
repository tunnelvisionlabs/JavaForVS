namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Events;

    internal sealed class EventQueue : Mirror, IEventQueue, IVirtualMachineEvents
    {
        public EventQueue(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public event EventHandler<ThreadEventArgs> VirtualMachineStart;

        public event EventHandler<VirtualMachineEventArgs> VirtualMachineDeath;

        public event EventHandler<ThreadLocationEventArgs> SingleStep;

        public event EventHandler<ThreadLocationEventArgs> Breakpoint;

        public event EventHandler<ThreadLocationEventArgs> MethodEntry;

        public event EventHandler<MethodExitEventArgs> MethodExit;

        public event EventHandler<MonitorEventArgs> MonitorContendedEnter;

        public event EventHandler<MonitorEventArgs> MonitorContendedEntered;

        public event EventHandler<MonitorWaitEventArgs> MonitorContendedWait;

        public event EventHandler<MonitorWaitedEventArgs> MonitorContendedWaited;

        public event EventHandler<ExceptionEventArgs> Exception;

        public event EventHandler<ThreadEventArgs> ThreadStart;

        public event EventHandler<ThreadEventArgs> ThreadDeath;

        public event EventHandler<ClassPrepareEventArgs> ClassPrepare;

        public event EventHandler<ClassUnloadEventArgs> ClassUnload;

        public event EventHandler<FieldAccessEventArgs> FieldAccess;

        public event EventHandler<FieldModificationEventArgs> FieldModification;

        public IEventSet Remove()
        {
            throw new NotImplementedException();
        }

        public IEventSet Remove(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        internal void OnVirtualMachineStart(ThreadEventArgs e)
        {
            var t = VirtualMachineStart;
            if (t != null)
                t(this, e);
        }

        internal void OnVirtualMachineDeath(VirtualMachineEventArgs e)
        {
            var t = VirtualMachineDeath;
            if (t != null)
                t(this, e);
        }

        internal void OnSingleStep(ThreadLocationEventArgs e)
        {
            var t = SingleStep;
            if (t != null)
                t(this, e);
        }

        internal void OnBreakpoint(ThreadLocationEventArgs e)
        {
            var t = Breakpoint;
            if (t != null)
                t(this, e);
        }

        internal void OnMethodEntry(ThreadLocationEventArgs e)
        {
            var t = MethodEntry;
            if (t != null)
                t(this, e);
        }

        internal void OnMethodExit(MethodExitEventArgs e)
        {
            var t = MethodExit;
            if (t != null)
                t(this, e);
        }

        internal void OnMonitorContendedEnter(MonitorEventArgs e)
        {
            var t = MonitorContendedEnter;
            if (t != null)
                t(this, e);
        }

        internal void OnMonitorContendedEntered(MonitorEventArgs e)
        {
            var t = MonitorContendedEntered;
            if (t != null)
                t(this, e);
        }

        internal void OnMonitorContendedWait(MonitorWaitEventArgs e)
        {
            var t = MonitorContendedWait;
            if (t != null)
                t(this, e);
        }

        internal void OnMonitorContendedWaited(MonitorWaitedEventArgs e)
        {
            var t = MonitorContendedWaited;
            if (t != null)
                t(this, e);
        }

        internal void OnException(ExceptionEventArgs e)
        {
            var t = Exception;
            if (t != null)
                t(this, e);
        }

        internal void OnThreadStart(ThreadEventArgs e)
        {
            var t = ThreadStart;
            if (t != null)
                t(this, e);
        }

        internal void OnThreadDeath(ThreadEventArgs e)
        {
            var t = ThreadDeath;
            if (t != null)
                t(this, e);
        }

        internal void OnClassPrepare(ClassPrepareEventArgs e)
        {
            var t = ClassPrepare;
            if (t != null)
                t(this, e);
        }

        internal void OnClassUnload(ClassUnloadEventArgs e)
        {
            var t = ClassUnload;
            if (t != null)
                t(this, e);
        }

        internal void OnFieldAccess(FieldAccessEventArgs e)
        {
            var t = FieldAccess;
            if (t != null)
                t(this, e);
        }

        internal void OnFieldModification(FieldModificationEventArgs e)
        {
            var t = FieldModification;
            if (t != null)
                t(this, e);
        }
    }
}
