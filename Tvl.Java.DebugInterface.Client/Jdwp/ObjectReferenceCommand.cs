namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum ObjectReferenceCommand : byte
    {
        Invalid = 0,
        ReferenceType = 1,
        GetValues = 2,
        SetValues = 3,
        MonitorInfo = 5,
        InvokeMethod = 6,
        DisableCollection = 7,
        EnableCollection = 8,
        IsCollected = 9,
        ReferringObjects = 10,
    }
}
