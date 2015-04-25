namespace Tvl.Java.DebugInterface.Connect
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class VirtualMachineStartException : DebuggerException
    {
        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineStartException</c> class.
        /// </summary>
        public VirtualMachineStartException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineStartException</c> class
        /// with a specified error message.
        /// </summary>
        public VirtualMachineStartException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineStartException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public VirtualMachineStartException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>VirtualMachineStartException</c> class
        /// with serialized data.
        /// </summary>
        protected VirtualMachineStartException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
