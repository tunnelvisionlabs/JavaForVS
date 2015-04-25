namespace Tvl.Java.DebugInterface.Types
{
    public enum ConstantType
    {
        Invalid = 0,
        Utf8 = 1,
        Integer = 3,
        Float = 4,
        Long = 5,
        Double = 6,
        Class = 7,
        String = 8,
        FieldReference = 9,
        MethodReference = 10,
        InterfaceMethodReference = 11,
        NameAndType = 12,
        MethodHandle = 15,
        MethodType = 16,
        InvokeDynamic = 18,
    }
}
