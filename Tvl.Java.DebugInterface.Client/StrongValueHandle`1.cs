namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;

    internal class StrongValueHandle<T> : Mirror, IStrongValueHandle<T>
        where T : Value
    {
        private readonly T _value;

        private bool _disposed;

        public StrongValueHandle(T value)
            : base(value.VirtualMachine)
        {
            Contract.Requires<ArgumentNullException>(value != null, "value");

            _value = value;
        }

        #region IStrongValueHandle<T> Members

        public T Value
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().ToString());

                return _value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            IObjectReference objectReference = _value as IObjectReference;
            if (objectReference != null)
                objectReference.EnableCollection();

            _disposed = true;
        }

        #endregion
    }
}
