namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because the specified stack frame is no longer valid.
    /// </summary>
    [Serializable]
    public class InvalidStackFrameException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>InvalidStackFrameException</c> class.
        /// </summary>
        public InvalidStackFrameException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidStackFrameException</c> class
        /// with a specified error message.
        /// </summary>
        public InvalidStackFrameException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidStackFrameException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InvalidStackFrameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidStackFrameException</c> class
        /// with serialized data.
        /// </summary>
        protected InvalidStackFrameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
