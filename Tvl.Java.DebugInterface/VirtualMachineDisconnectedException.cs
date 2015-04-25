namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested operation cannot be completed because there is no longer a connection to the target VM.
    /// </summary>
    [Serializable]
    public class VirtualMachineDisconnectedException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineDisconnectedException</c> class.
        /// </summary>
        public VirtualMachineDisconnectedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineDisconnectedException</c> class
        /// with a specified error message.
        /// </summary>
        public VirtualMachineDisconnectedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineDisconnectedException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public VirtualMachineDisconnectedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineDisconnectedException</c> class
        /// with serialized data.
        /// </summary>
        protected VirtualMachineDisconnectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
