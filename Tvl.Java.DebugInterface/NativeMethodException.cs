namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate an operation cannot be completed because it is not valid for a native method.
    /// </summary>
    [Serializable]
    public class NativeMethodException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>NativeMethodException</c> class.
        /// </summary>
        public NativeMethodException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>NativeMethodException</c> class
        /// with a specified error message.
        /// </summary>
        public NativeMethodException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>NativeMethodException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public NativeMethodException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>NativeMethodException</c> class
        /// with serialized data.
        /// </summary>
        protected NativeMethodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
