namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum ThreadReferenceCommand : byte
    {
        Invalid = 0,
        Name = 1,
        Suspend = 2,
        Resume = 3,
        Status = 4,
        ThreadGroup = 5,
        Frames = 6,
        FrameCount = 7,
        OwnedMonitors = 8,
        CurrentContendedMonitor = 9,
        Stop = 10,
        Interrupt = 11,
        SuspendCount = 12,
        OwnedMonitorsStackDepthInfo = 13,
        ForceEarlyReturn = 14,
    }
}
