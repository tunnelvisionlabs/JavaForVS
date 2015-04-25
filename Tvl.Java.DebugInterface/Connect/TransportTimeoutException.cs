namespace Tvl.Java.DebugInterface.Connect
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TransportTimeoutException : DebuggerTimeoutException
    {
        /// <summary>
        /// Initializes a new instance of the <c>TransportTimeoutException</c> class.
        /// </summary>
        public TransportTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>TransportTimeoutException</c> class
        /// with a specified error message.
        /// </summary>
        public TransportTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>TransportTimeoutException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public TransportTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>TransportTimeoutException</c> class
        /// with serialized data.
        /// </summary>
        protected TransportTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
