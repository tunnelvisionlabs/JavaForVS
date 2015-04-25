namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the operation is invalid because it would modify the VM and the VM is read-only.
    /// </summary>
    [Serializable]
    public class VirtualMachineCannotBeModifiedException : DebuggerInvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineCannotBeModifiedException</c> class.
        /// </summary>
        public VirtualMachineCannotBeModifiedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineCannotBeModifiedException</c> class
        /// with a specified error message.
        /// </summary>
        public VirtualMachineCannotBeModifiedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineCannotBeModifiedException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public VirtualMachineCannotBeModifiedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineCannotBeModifiedException</c> class
        /// with serialized data.
        /// </summary>
        protected VirtualMachineCannotBeModifiedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
