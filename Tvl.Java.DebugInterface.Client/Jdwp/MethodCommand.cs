namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum MethodCommand : byte
    {
        Invalid = 0,
        LineTable = 1,
        VariableTable = 2,
        Bytecodes = 3,
        IsObsolete = 4,
        VariableTableWithGeneric = 5,
    }
}
