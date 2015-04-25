namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed while the specified thread is in its current state.
    /// </summary>
    [Serializable]
    public class IncompatibleThreadStateException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>IncompatibleThreadStateException</c> class.
        /// </summary>
        public IncompatibleThreadStateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>IncompatibleThreadStateException</c> class
        /// with a specified error message.
        /// </summary>
        public IncompatibleThreadStateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>IncompatibleThreadStateException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public IncompatibleThreadStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>IncompatibleThreadStateException</c> class
        /// with serialized data.
        /// </summary>
        protected IncompatibleThreadStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
