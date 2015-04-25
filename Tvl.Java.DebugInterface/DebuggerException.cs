namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DebuggerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <c>DebuggerException</c> class.
        /// </summary>
        public DebuggerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerException</c> class
        /// with a specified error message.
        /// </summary>
        public DebuggerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public DebuggerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerException</c> class
        /// with serialized data.
        /// </summary>
        protected DebuggerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
