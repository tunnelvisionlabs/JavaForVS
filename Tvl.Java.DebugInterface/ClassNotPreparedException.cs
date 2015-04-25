namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because the specified class has not yet been prepared.
    /// </summary>
    [Serializable]
    public class ClassNotPreparedException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>ClassNotPreparedException</c> class.
        /// </summary>
        public ClassNotPreparedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotPreparedException</c> class
        /// with a specified error message.
        /// </summary>
        public ClassNotPreparedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotPreparedException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public ClassNotPreparedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotPreparedException</c> class
        /// with serialized data.
        /// </summary>
        protected ClassNotPreparedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
