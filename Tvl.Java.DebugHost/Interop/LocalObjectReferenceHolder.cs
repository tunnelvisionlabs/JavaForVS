namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Diagnostics.Contracts;

    public struct LocalObjectReferenceHolder : IDisposable
    {
        private readonly JniEnvironment _nativeEnvironment;
        private readonly jobject _reference;

        public LocalObjectReferenceHolder(JniEnvironment nativeEnvironment, jobject reference)
        {
            Contract.Requires<ArgumentNullException>(nativeEnvironment != null, "nativeEnvironment");

            _nativeEnvironment = nativeEnvironment;
            _reference = _nativeEnvironment.NewLocalReference(reference);
        }

        public jobject Value
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
