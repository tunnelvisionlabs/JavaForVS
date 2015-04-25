namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate an exception occurred in an invoked method within the target VM.
    /// </summary>
    [Serializable]
    public class InvocationException : DebuggerException
    {
        private readonly IObjectReference _exception;

        /// <summary>
        /// Initializes a new instance of the <c>InvocationException</c> class.
        /// </summary>
        public InvocationException(IObjectReference exception)
        {
            _exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvocationException</c> class
        /// with a specified error message.
        /// </summary>
        public InvocationException(IObjectReference exception, string message)
            : base(message)
        {
            _exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvocationException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InvocationException(IObjectReference exception, string message, Exception innerException)
            : base(message, innerException)
        {
            _exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvocationException</c> class
        /// with serialized data.
        /// </summary>
        protected InvocationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IObjectReference Exception
        {
            get
            {
                return _exception;
            }
        }
    }
}
