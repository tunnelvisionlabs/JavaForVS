namespace Tvl.Java.DebugInterface.Request
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidRequestStateException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>InvalidRequestStateException</c> class.
        /// </summary>
        public InvalidRequestStateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidRequestStateException</c> class
        /// with a specified error message.
        /// </summary>
        public InvalidRequestStateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidRequestStateException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InvalidRequestStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidRequestStateException</c> class
        /// with serialized data.
        /// </summary>
        protected InvalidRequestStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
