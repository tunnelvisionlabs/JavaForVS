namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because the specified object has been garbage collected.
    /// </summary>
    [Serializable]
    public class ObjectCollectionException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>ObjectCollectionException</c> class.
        /// </summary>
        public ObjectCollectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ObjectCollectionException</c> class
        /// with a specified error message.
        /// </summary>
        public ObjectCollectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ObjectCollectionException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public ObjectCollectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ObjectCollectionException</c> class
        /// with serialized data.
        /// </summary>
        protected ObjectCollectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
