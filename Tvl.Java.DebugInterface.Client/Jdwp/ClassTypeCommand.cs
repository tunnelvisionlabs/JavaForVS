namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum ClassTypeCommand : byte
    {
        Invalid = 0,
        Superclass = 1,
        SetValues = 2,
        InvokeMethod = 3,
        NewInstance = 4
    }
}
