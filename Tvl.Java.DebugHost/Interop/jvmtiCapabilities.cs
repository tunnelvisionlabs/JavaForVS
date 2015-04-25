// The field 'field_name' is never used
#pragma warning disable 169
// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jvmtiCapabilities
    {
        private readonly CapabilityFlags1 _flags1;
        private readonly CapabilityFlags2 _flags2;
        private readonly CapabilityFlags3 _flags3;
        private readonly CapabilityFlags4 _flags4;

        public jvmtiCapabilities(CapabilityFlags1 flags1 = 0, CapabilityFlags2 flags2 = 0, CapabilityFlags3 flags3 = 0, CapabilityFlags4 flags4 = 0)
        {
            _flags1 = flags1;
            _flags2 = flags2;
            _flags3 = flags3;
            _flags4 = flags4;
        }

        public CapabilityFlags1 Capabilities1
        {
            get
            {
                return _flags1;
            }
        }

        public CapabilityFlags2 Capabilities2
        {
            get
            {
                return _flags2;
            }
        }

        public CapabilityFlags3 Capabilities3
        {
            get
            {
                return _flags3;
            }
        }

        public CapabilityFlags4 Capabilities4
        {
            get
            {
                return _flags4;
            }
        }

        public bool CanTagObjects
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanTagObjects) != 0;
            }
        }

        public bool CanGenerateFieldModificationEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateFieldModificationEvents) != 0;
            }
        }

        public bool CanGenerateFieldAccessEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateFieldAccessEvents) != 0;
            }
        }

        public bool CanGetBytecodes
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetBytecodes) != 0;
            }
        }

        public bool CanGetSyntheticAttribute
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetSyntheticAttribute) != 0;
            }
        }

        public bool CanGetOwnedMonitorInfo
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetOwnedMonitorInfo) != 0;
            }
        }

        public bool CanGetCurrentContendedMonitor
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetCurrentContendedMonitor) != 0;
            }
        }

        public bool CanGetMonitorInfo
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetMonitorInfo) != 0;
            }
        }

        public bool CanPopFrame
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanPopFrame) != 0;
            }
        }

        public bool CanRedefineClasses
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanRedefineClasses) != 0;
            }
        }

        public bool CanSignalThread
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanSignalThread) != 0;
            }
        }

        public bool CanGetSourceFileName
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetSourceFileName) != 0;
            }
        }

        public bool CanGetLineNumbers
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetLineNumbers) != 0;
            }
        }

        public bool CanGetSourceDebugExtension
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetSourceDebugExtension) != 0;
            }
        }

        public bool CanAccessLocalVariables
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanAccessLocalVariables) != 0;
            }
        }

        public bool CanMaintainOriginalMethodOrder
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanMaintainOriginalMethodOrder) != 0;
            }
        }

        public bool CanGenerateSingleStepEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateSingleStepEvents) != 0;
            }
        }

        public bool CanGenerateExceptionEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateExceptionEvents) != 0;
            }
        }

        public bool CanGenerateFramePopEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateFramePopEvents) != 0;
            }
        }

        public bool CanGenerateBreakpointEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateBreakpointEvents) != 0;
            }
        }

        public bool CanSuspend
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanSuspend) != 0;
            }
        }

        public bool CanRedefineAnyClass
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanRedefineAnyClass) != 0;
            }
        }

        public bool CanGetCurrentThreadCpuTime
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetCurrentThreadCpuTime) != 0;
            }
        }

        public bool CanGetThreadCpuTime
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGetThreadCpuTime) != 0;
            }
        }

        public bool CanGenerateMethodEntryEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateMethodEntryEvents) != 0;
            }
        }

        public bool CanGenerateMethodExitEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateMethodExitEvents) != 0;
            }
        }

        public bool CanGenerateAllClassHookEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateAllClassHookEvents) != 0;
            }
        }

        public bool CanGenerateCompiledMethodLoadEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateCompiledMethodLoadEvents) != 0;
            }
        }

        public bool CanGenerateMonitorEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateMonitorEvents) != 0;
            }
        }

        public bool CanGenerateVmObjectAllocEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateVmObjectAllocEvents) != 0;
            }
        }

        public bool CanGenerateNativeMethodBindEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateNativeMethodBindEvents) != 0;
            }
        }

        public bool CanGenerateGarbageCollectionEvents
        {
            get
            {
                return (_flags1 & CapabilityFlags1.CanGenerateGarbageCollectionEvents) != 0;
            }
        }

        public bool CanGenerateObjectFreeEvents
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanGenerateObjectFreeEvents) != 0;
            }
        }

        public bool CanForceEarlyReturn
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanForceEarlyReturn) != 0;
            }
        }

        public bool CanGetOwnedMonitorStackDepthInfo
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanGetOwnedMonitorStackDepthInfo) != 0;
            }
        }

        public bool CanGetConstantPool
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanGetConstantPool) != 0;
            }
        }

        public bool CanSetNativeMethodPrefix
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanSetNativeMethodPrefix) != 0;
            }
        }

        public bool CanRetransformClasses
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanRetransformClasses) != 0;
            }
        }

        public bool CanRetransformAnyClass
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanRetransformAnyClass) != 0;
            }
        }

        public bool CanGenerateResourceExhaustionHeapEvents
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanGenerateResourceExhaustionHeapEvents) != 0;
            }
        }

        public bool CanGenerateResourceExhaustionThreadsEvents
        {
            get
            {
                return (_flags2 & CapabilityFlags2.CanGenerateResourceExhaustionThreadsEvents) != 0;
            }
        }

        [Flags]
        public enum CapabilityFlags1 : uint
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
        }

        [Flags]
        public enum CapabilityFlags2 : uint
        {
            None = 0,
            CanGenerateObjectFreeEvents = 0x00000001,
            CanForceEarlyReturn = 0x00000002,
            CanGetOwnedMonitorStackDepthInfo = 0x00000004,
            CanGetConstantPool = 0x00000008,
            CanSetNativeMethodPrefix = 0x00000010,
            CanRetransformClasses = 0x00000020,
            CanRetransformAnyClass = 0x00000040,
            CanGenerateResourceExhaustionHeapEvents = 0x00000080,
            CanGenerateResourceExhaustionThreadsEvents = 0x00000100,
        }

        [Flags]
        public enum CapabilityFlags3 : uint
        {
            None = 0,
        }

        [Flags]
        public enum CapabilityFlags4 : uint
        {
            None = 0,
        }
    }
}
