namespace Tvl.Java.DebugInterface.Request
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateRequestException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>DuplicateRequestException</c> class.
        /// </summary>
        public DuplicateRequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DuplicateRequestException</c> class
        /// with a specified error message.
        /// </summary>
        public DuplicateRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DuplicateRequestException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public DuplicateRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DuplicateRequestException</c> class
        /// with serialized data.
        /// </summary>
        protected DuplicateRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
