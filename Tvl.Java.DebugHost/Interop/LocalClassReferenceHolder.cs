namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Diagnostics.Contracts;

    public struct LocalClassReferenceHolder : IDisposable
    {
        private readonly JniEnvironment _nativeEnvironment;
        private readonly jclass _reference;

        public LocalClassReferenceHolder(JniEnvironment nativeEnvironment, jclass reference)
        {
            Contract.Requires<ArgumentNullException>(nativeEnvironment != null, "nativeEnvironment");

            _nativeEnvironment = nativeEnvironment;
            _reference = (jclass)_nativeEnvironment.NewLocalReference(reference);
        }

        public jclass Value
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
