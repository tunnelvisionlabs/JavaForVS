namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate a type mismatch in setting the value of a field or variable, or in specifying the return value of a method.
    /// </summary>
    [Serializable]
    public class InvalidTypeException : DebuggerArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class.
        /// </summary>
        public InvalidTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class
        /// with a specified error message.
        /// </summary>
        public InvalidTypeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InvalidTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class
        /// with a specified error message and the name of the parameter that causes
        /// this exception.
        /// </summary>
        public InvalidTypeException(string message, string paramName)
            : base(message, paramName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class
        /// with a specified error message, the parameter name, and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        public InvalidTypeException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>InvalidTypeException</c> class
        /// with serialized data.
        /// </summary>
        protected InvalidTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
