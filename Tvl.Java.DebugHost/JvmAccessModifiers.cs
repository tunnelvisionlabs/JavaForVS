namespace Tvl.Java.DebugHost
{
    using System;

    [Flags]
    public enum JvmAccessModifiers
    {
        None = 0,
        Public = 0x0001,
        Private = 0x0002,
        Protected = 0x0004,
        Static = 0x0008,
        Final = 0x0010,
        Super = 0x0020,
        Synchronized = 0x0020,
        Volatile = 0x0040,
        Transient = 0x0080,
        Native = 0x0100,
        Interface = 0x0200,
        Abstract = 0x0400,
        Strict = 0x0800,
    }
}
