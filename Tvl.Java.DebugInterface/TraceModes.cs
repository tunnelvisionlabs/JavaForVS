namespace Tvl.Java.DebugInterface
{
    using System;

    [Flags]
    public enum TraceModes
    {
        /// <summary>
        /// All tracing is disabled.
        /// </summary>
        None = 0,

        /// <summary>
        /// Tracing enabled for JDWP packets sent to target VM.
        /// </summary>
        Sends = 0x000001,

        /// <summary>
        /// Tracing enabled for JDWP packets received from target VM.
        /// </summary>
        Receives = 0x000002,

        /// <summary>
        /// Tracing enabled for internal event handling.
        /// </summary>
        Events = 0x000004,

        /// <summary>
        /// Tracing enabled for internal managment of reference types.
        /// </summary>
        ReferenceTypes = 0x000008,

        /// <summary>
        /// Tracing enabled for internal management of object references.
        /// </summary>
        ObjectReferences = 0x000010,

        /// <summary>
        /// All tracing is enabled.
        /// </summary>
        All = 0xFFFFFF,
    }
}
