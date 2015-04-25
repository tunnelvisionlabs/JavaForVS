namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum CommandSet : byte
    {
        VirtualMachine = 1,
        ReferenceType = 2,
        ClassType = 3,
        ArrayType = 4,
        InterfaceType = 5,
        Method = 6,
        Field = 8,
        ObjectReference = 9,
        StringReference = 10,
        ThreadReference = 11,
        ThreadGroupReference = 12,
        ArrayReference = 13,
        ClassLoaderReference = 14,
        EventRequest = 15,
        StackFrame = 16,
        ClassObjectReference = 17,
        Event = 64,
    }
}
