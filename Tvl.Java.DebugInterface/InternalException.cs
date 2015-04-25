namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that an unexpected internal error has occurred.
    /// </summary>
    [Serializable]
    public class InternalException : DebuggerException
    {
        private readonly int _errorCode;

        /// <summary>
        /// Initializes a new instance of the <c>InternalException</c> class.
        /// </summary>
        public InternalException(int errorCode)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InternalException</c> class
        /// with a specified error message.
        /// </summary>
        public InternalException(int errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InternalException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public InternalException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <c>InternalException</c> class
        /// with serialized data.
        /// </summary>
        protected InternalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _errorCode = info.GetInt32("JvmErrorCode");
        }

        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("JvmErrorCode", _errorCode);
        }
    }
}
