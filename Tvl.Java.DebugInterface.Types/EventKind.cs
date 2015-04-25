namespace Tvl.Java.DebugInterface.Types
{
    public enum EventKind
    {
        Invalid = 0,
        SingleStep = 1,
        Breakpoint = 2,
        FramePop = 3,
        Exception = 4,
        UserDefined = 5,
        ThreadStart = 6,
        ThreadEnd = 7,
        ClassPrepare = 8,
        ClassUnload = 9,
        ClassLoad = 10,
        FieldAccess = 20,
        FieldModification = 21,
        ExceptionCatch = 30,
        MethodEntry = 40,
        MethodExit = 41,
        MethodExitWithReturnValue = 42,
        MonitorContendedEnter = 43,
        MonitorContendedEntered = 44,
        MonitorWait = 45,
        MonitorWaited = 46,
        VirtualMachineInit = 90,
        VirtualMachineDeath = 99,
        VirtualMachineDisconnected = 100,
        VirtualMachineStart = VirtualMachineInit,
        ThreadDeath = ThreadEnd,
    }
}
