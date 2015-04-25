namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum ArrayReferenceCommand : byte
    {
        Invalid = 0,
        Length = 1,
        GetValues = 2,
        SetValues = 3,
    }
}
