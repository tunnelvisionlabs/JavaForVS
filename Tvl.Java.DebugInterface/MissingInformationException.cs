namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate line number or variable information is not available.
    /// </summary>
    [Serializable]
    public class MissingInformationException : DebuggerException
    {
        /// <summary>
        /// Initializes a new instance of the <c>MissingInformationException</c> class.
        /// </summary>
        public MissingInformationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>MissingInformationException</c> class
        /// with a specified error message.
        /// </summary>
        public MissingInformationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>MissingInformationException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public MissingInformationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>MissingInformationException</c> class
        /// with serialized data.
        /// </summary>
        protected MissingInformationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
