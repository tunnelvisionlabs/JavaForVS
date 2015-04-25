namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum EventRequestCommand : byte
    {
        Invalid = 0,
        Set = 1,
        Clear = 2,
        ClearAllBreakpoints = 3,
    }
}
