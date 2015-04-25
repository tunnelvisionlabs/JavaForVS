namespace Tvl.Java.DebugInterface.Types
{
    using System;

    [Flags]
    public enum AccessModifiers
    {
        None = 0,

        /// <summary>
        /// Declared public; may be accessed from outside its package.
        /// </summary>
        Public = 0x0001,
        Private = 0x0002,
        Protected = 0x0004,
        Static = 0x0008,

        /// <summary>
        /// Declared final; no subclasses allowed.
        /// </summary>
        Final = 0x0010,

        /// <summary>
        /// Treat superclass methods specially when invoked by the <c>invokespecial</c> instruction.
        /// </summary>
        Super = 0x0020,
        Synchronized = 0x0020,
        Volatile = 0x0040,
        Bridge = 0x0040,
        Transient = 0x0080,
        VarArgs = 0x0080,
        Native = 0x0100,

        /// <summary>
        /// Is an interface, not a class.
        /// </summary>
        /// <remarks>
        /// An interface is distinguished by its <see cref="Interface"/> flag being set. If its
        /// <see cref="Interface"/> flag is not set, this class file defines a class, not an interface.
        /// 
        /// If the <see cref="Interface"/> flag of this class file is set, its <see cref="Abstract"/>
        /// flag must also be set (§2.13.1) and its <see cref="Public"/> flag may be set. Such a class
        /// file may not have any of the other flags in Table 4.1 set.
        /// 
        /// If the <see cref="Interface"/> flag of this class file is not set, it may have any of the
        /// other flags in Table 4.1 set. However, such a class file cannot have both its <see cref="Final"/>
        /// and <see cref="Abstract"/> flags set (§2.8.2). 
        /// </remarks>
        Interface = 0x0200,

        /// <summary>
        /// Declared abstract; may not be instantiated.
        /// </summary>
        Abstract = 0x0400,
        Strict = 0x0800,

        Synthetic = 0x1000,

        Annotation = 0x2000,

        Enum = 0x4000,
    }
}
