namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum StackFrameCommand : byte
    {
        Invalid = 0,
        GetValues = 1,
        SetValues = 2,
        ThisObject = 3,
        PopFrames = 4,
    }
}
