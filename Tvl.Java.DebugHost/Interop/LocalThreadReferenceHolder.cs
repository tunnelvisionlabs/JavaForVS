namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Diagnostics.Contracts;

    public struct LocalThreadReferenceHolder : IDisposable
    {
        private readonly JniEnvironment _nativeEnvironment;
        private readonly jthread _reference;

        public LocalThreadReferenceHolder(JniEnvironment nativeEnvironment, jthread reference)
        {
            Contract.Requires<ArgumentNullException>(nativeEnvironment != null, "nativeEnvironment");

            _nativeEnvironment = nativeEnvironment;
            _reference = (jthread)_nativeEnvironment.NewLocalReference(reference);
        }

        public jthread Value
        {
            get
            {
                return _reference;
            }
        }

        public bool IsAlive
        {
            get
            {
                // per the spec of NewLocalReference
                return Value != jobject.Null;
            }
        }

        public void Dispose()
        {
            if (IsAlive)
                _nativeEnvironment.DeleteLocalReference(_reference);
        }
    }
}
