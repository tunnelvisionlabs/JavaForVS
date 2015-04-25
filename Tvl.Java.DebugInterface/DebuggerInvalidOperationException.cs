namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DebuggerInvalidOperationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>DebuggerInvalidOperationException</c> class.
        /// </summary>
        public DebuggerInvalidOperationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerInvalidOperationException</c> class
        /// with a specified error message.
        /// </summary>
        public DebuggerInvalidOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerInvalidOperationException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public DebuggerInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerInvalidOperationException</c> class
        /// with serialized data.
        /// </summary>
        protected DebuggerInvalidOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
