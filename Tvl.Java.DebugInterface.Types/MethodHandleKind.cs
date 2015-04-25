namespace Tvl.Java.DebugInterface.Types
{
    public enum MethodHandleKind : byte
    {
        Invalid = 0,
        GetField = 1,
        GetStatic = 2,
        PutField = 3,
        PutStatic = 4,
        InvokeVirtual = 5,
        InvokeStatic = 6,
        InvokeSpecial = 7,
        NewInvokeSpecial = 8,
        InvokeInterface = 9,
    }
}
