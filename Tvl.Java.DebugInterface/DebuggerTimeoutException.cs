namespace Tvl.Java.DebugInterface.Connect
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DebuggerTimeoutException : TimeoutException
    {
        /// <summary>
        /// Initializes a new instance of the <c>DebuggerTimeoutException</c> class.
        /// </summary>
        public DebuggerTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerTimeoutException</c> class
        /// with a specified error message.
        /// </summary>
        public DebuggerTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerTimeoutException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public DebuggerTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerTimeoutException</c> class
        /// with serialized data.
        /// </summary>
        protected DebuggerTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
