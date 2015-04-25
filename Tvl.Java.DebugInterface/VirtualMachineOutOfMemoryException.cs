namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because the target VM has run out of memory.
    /// </summary>
    [Serializable]
    public class VirtualMachineOutOfMemoryException : DebuggerException
    {
        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineOutOfMemoryException</c> class.
        /// </summary>
        public VirtualMachineOutOfMemoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineOutOfMemoryException</c> class
        /// with a specified error message.
        /// </summary>
        public VirtualMachineOutOfMemoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineOutOfMemoryException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public VirtualMachineOutOfMemoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineOutOfMemoryException</c> class
        /// with serialized data.
        /// </summary>
        protected VirtualMachineOutOfMemoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
