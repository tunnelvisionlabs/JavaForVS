namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that the requested class has not yet been loaded through the appropriate class loader.
    /// </summary>
    [Serializable]
    public class ClassNotLoadedException : DebuggerInvalidOperationException
    {
        private readonly string _className;

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotLoadedException</c> class.
        /// </summary>
        public ClassNotLoadedException(string className)
        {
            _className = className;
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotLoadedException</c> class
        /// with a specified error message.
        /// </summary>
        public ClassNotLoadedException(string className, string message)
            : base(message)
        {
            _className = className;
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotLoadedException</c> class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        public ClassNotLoadedException(string className, string message, Exception innerException)
            : base(message, innerException)
        {
            _className = className;
        }

        /// <summary>
        /// Initializes a new instance of the <c>ClassNotLoadedException</c> class
        /// with serialized data.
        /// </summary>
        protected ClassNotLoadedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _className = info.GetString("JvmClassName");
        }

        public string ClassName
        {
            get
            {
                return _className;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("JvmClassName", _className);
        }
    }
}
