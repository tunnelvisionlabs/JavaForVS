namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that there is an inconistency in the debug information provided by the target VM.
    /// </summary>
    [Serializable]
    public class InconsistentDebugInfoException : DebuggerException
    {
        /// <summary>
        /// Initializes a new instance of the <c>InconsistentDebugInfoException</c> class.
        /// </summary>
        public InconsistentDebugInfoException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InconsistentDebugInfoException</c> class
        /// with a specified error message.
        /// </summary>
        public InconsistentDebugInfoException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InconsistentDebugInfoException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InconsistentDebugInfoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InconsistentDebugInfoException</c> class
        /// with serialized data.
        /// </summary>
        protected InconsistentDebugInfoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
