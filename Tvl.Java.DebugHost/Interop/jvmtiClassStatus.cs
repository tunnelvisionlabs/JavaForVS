namespace Tvl.Java.DebugHost.Interop
{
    using System;

    [Flags]
    public enum jvmtiClassStatus
    {
        None = 0,

        /// <summary>
        /// Class bytecodes have been verified.
        /// </summary>
        Verified = 1,

        /// <summary>
        /// Class preparation is complete.
        /// </summary>
        Prepared = 2,

        /// <summary>
        /// Class initialization is complete. Static initializer has been run.
        /// </summary>
        Initialized = 4,

        /// <summary>
        /// Error during initialization makes class unusable.
        /// </summary>
        Error = 8,

        /// <summary>
        /// Class is an array. If set, all other bits are zero.
        /// </summary>
        Array = 16,

        /// <summary>
        /// Class is a primitive class (for example, java.lang.Integer.TYPE). If set, all other bits are zero.
        /// </summary>
        Primitive = 32,
    }
}
