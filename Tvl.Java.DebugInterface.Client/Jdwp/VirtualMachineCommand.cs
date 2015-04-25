namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    public enum VirtualMachineCommand : byte
    {
        Invalid = 0,
        Version = 1,
        ClassesBySignature = 2,
        AllClasses = 3,
        AllThreads = 4,
        TopLevelThreadGroups = 5,
        Dispose = 6,
        IDSizes = 7,
        Suspend = 8,
        Resume = 9,
        Exit = 10,
        CreateString = 11,
        Capabilities = 12,
        ClassPaths = 13,
        DisposeObjects = 14,
        HoldEvents = 15,
        ReleaseEvents = 16,
        CapabilitiesNew = 17,
        RedefineClasses = 18,
        SetDefaultStratum = 19,
        AllClassesWithGeneric = 20,
        InstanceCounts = 21,
    }
}
