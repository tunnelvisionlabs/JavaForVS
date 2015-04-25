namespace Tvl.Java.DebugInterface.Types
{
    using System;

    [Flags]
    public enum Capabilities : long
    {
        None = 0,

        CanTagObjects = 0x00000001,

        CanGenerateFieldModificationEvents = 0x00000002,

        CanGenerateFieldAccessEvents = 0x00000004,

        CanGetBytecodes = 0x00000008,

        CanGetSyntheticAttribute = 0x00000010,

        CanGetOwnedMonitorInfo = 0x00000020,

        CanGetCurrentContendedMonitor = 0x00000040,

        CanGetMonitorInfo = 0x00000080,

        CanPopFrame = 0x00000100,

        CanRedefineClasses = 0x00000200,

        CanSignalThread = 0x00000400,

        CanGetSourceFileName = 0x00000800,

        CanGetLineNumbers = 0x00001000,

        CanGetSourceDebugExtension = 0x00002000,

        CanAccessLocalVariables = 0x00004000,

        CanMaintainOriginalMethodOrder = 0x00008000,

        CanGenerateSingleStepEvents = 0x00010000,

        CanGenerateExceptionEvents = 0x00020000,

        CanGenerateFramePopEvents = 0x00040000,

        CanGenerateBreakpointEvents = 0x00080000,

        CanSuspend = 0x00100000,

        CanRedefineAnyClass = 0x00200000,

        CanGetCurrentThreadCpuTime = 0x00400000,

        CanGetThreadCpuTime = 0x00800000,

        CanGenerateMethodEntryEvents = 0x01000000,

        CanGenerateMethodExitEvents = 0x02000000,

        CanGenerateAllClassHookEvents = 0x04000000,

        CanGenerateCompiledMethodLoadEvents = 0x08000000,

        CanGenerateMonitorEvents = 0x10000000,

        CanGenerateVmObjectAllocEvents = 0x20000000,

        CanGenerateNativeMethodBindEvents = 0x40000000,

        CanGenerateGarbageCollectionEvents = 0x80000000,

        CanGenerateObjectFreeEvents = 0x0000000100000000,

        CanForceEarlyReturn = 0x0000000200000000,

        CanGetOwnedMonitorStackDepthInfo = 0x0000000400000000,

        CanGetConstantPool = 0x0000000800000000,

        CanSetNativeMethodPrefix = 0x0000001000000000,

        CanRetransformClasses = 0x0000002000000000,

        CanRetransformAnyClass = 0x0000004000000000,

        CanGenerateResourceExhaustionHeapEvents = 0x0000008000000000,

        CanGenerateResourceExhaustionThreadsEvents = 0x0000010000000000,

        CanStepByStatement = 0x0100000000000000,

        CanInvokeWithoutThread = 0x0200000000000000,
    }
}
