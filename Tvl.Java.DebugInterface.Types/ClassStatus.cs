namespace Tvl.Java.DebugInterface.Types
{
    using System;

    [Flags]
    public enum ClassStatus
    {
        None = 0,
        Verified = 0x0001,
        Prepared = 0x0002,
        Initialized = 0x0004,
        Error = 0x0008,
    }
}
