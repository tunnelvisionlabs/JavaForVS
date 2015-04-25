namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum ReferenceTypeCommand : byte
    {
        Invalid = 0,
        Signature = 1,
        ClassLoader = 2,
        Modifiers = 3,
        Fields = 4,
        Methods = 5,
        GetValues = 6,
        SourceFile = 7,
        NestedTypes = 8,
        Status = 9,
        Interfaces = 10,
        ClassObject = 11,
        SourceDebugExtension = 12,
        SignatureWithGeneric = 13,
        FieldsWithGeneric = 14,
        MethodsWithGeneric = 15,
        Instances = 16,
        ClassFileVersion = 17,
        ConstantPool = 18,
    }
}
