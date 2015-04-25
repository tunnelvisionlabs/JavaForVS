namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because the a mirror from
    /// one target VM is being combined with a mirror from another target VM.
    /// </summary>
    [Serializable]
    public class VirtualMachineMismatchException : DebuggerArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class.
        /// </summary>
        public VirtualMachineMismatchException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class
        /// with a specified error message.
        /// </summary>
        public VirtualMachineMismatchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public VirtualMachineMismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class
        /// with a specified error message and the name of the parameter that causes
        /// this exception.
        /// </summary>
        public VirtualMachineMismatchException(string message, string paramName)
            : base(message, paramName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class
        /// with a specified error message, the parameter name, and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        public VirtualMachineMismatchException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineMismatchException</c> class
        /// with serialized data.
        /// </summary>
        protected VirtualMachineMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
