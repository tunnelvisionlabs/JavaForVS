namespace Tvl.Java.DebugInterface.Connect
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ConnectorArgumentException : DebuggerArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class.
        /// </summary>
        public ConnectorArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class
        /// with a specified error message.
        /// </summary>
        public ConnectorArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public ConnectorArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class
        /// with a specified error message and the name of the parameter that causes
        /// this exception.
        /// </summary>
        public ConnectorArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class
        /// with a specified error message, the parameter name, and a reference to the
        /// inner exception that is the cause of this exception.
        /// </summary>
        public ConnectorArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ConnectorArgumentException</c> class
        /// with serialized data.
        /// </summary>
        protected ConnectorArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
