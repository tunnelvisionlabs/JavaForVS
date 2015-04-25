namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;

    public interface IVirtualMachineEvents
    {
        event EventHandler<ThreadEventArgs> VirtualMachineStart;

        event EventHandler<VirtualMachineEventArgs> VirtualMachineDeath;

        event EventHandler<ThreadLocationEventArgs> SingleStep;

        event EventHandler<ThreadLocationEventArgs> Breakpoint;

        event EventHandler<ThreadLocationEventArgs> MethodEntry;

        event EventHandler<MethodExitEventArgs> MethodExit;

        event EventHandler<MonitorEventArgs> MonitorContendedEnter;

        event EventHandler<MonitorEventArgs> MonitorContendedEntered;

        event EventHandler<MonitorWaitEventArgs> MonitorContendedWait;

        event EventHandler<MonitorWaitedEventArgs> MonitorContendedWaited;

        event EventHandler<ExceptionEventArgs> Exception;

        event EventHandler<ThreadEventArgs> ThreadStart;

        event EventHandler<ThreadEventArgs> ThreadDeath;

        event EventHandler<ClassPrepareEventArgs> ClassPrepare;

        event EventHandler<ClassUnloadEventArgs> ClassUnload;

        event EventHandler<FieldAccessEventArgs> FieldAccess;

        event EventHandler<FieldModificationEventArgs> FieldModification;
    }
}
