namespace Tvl.Java.DebugInterface.Types
{
    using System;

    [Flags]
    public enum InvokeOptions
    {
        None = 0,
        SingleThreaded = 0x0001,
        NonVirtual = 0x0002,
    }
}
