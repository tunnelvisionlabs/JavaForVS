namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when one of the arguments provided to a debugger interface method is not valid.
    /// </summary>
    [Serializable]
    public class DebuggerArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class.
        /// </summary>
        public DebuggerArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class
        /// with a specified error message.
        /// </summary>
        public DebuggerArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public DebuggerArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class
        /// with a specified error message and the name of the parameter that causes
        /// this exception.
        /// </summary>
        public DebuggerArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class
        /// with a specified error message, the parameter name, and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        public DebuggerArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>DebuggerArgumentException</c> class
        /// with serialized data.
        /// </summary>
        protected DebuggerArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
