// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using Tvl.Java.DebugHost.Services;
    using Tvl.Java.DebugInterface.Types;

    public struct jmethodID : IEquatable<jmethodID>
    {
        public static readonly jmethodID Null = default(jmethodID);

        private readonly IntPtr _handle;

        public jmethodID(IntPtr handle)
        {
            _handle = handle;
        }

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public static bool operator ==(jmethodID x, jmethodID y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jmethodID x, jmethodID y)
        {
            return x._handle != y._handle;
        }

        public static explicit operator jmethodID(JvmMethodRemoteHandle methodHandle)
        {
            return new jmethodID((IntPtr)methodHandle.Handle);
        }

        public static implicit operator jmethodID(MethodId methodHandle)
        {
            return new jmethodID((IntPtr)methodHandle.Handle);
        }

        public static implicit operator MethodId(jmethodID methodHandle)
        {
            return new MethodId(methodHandle.Handle.ToInt64());
        }

        public bool Equals(jmethodID other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jmethodID))
                return false;

            return this._handle == ((jmethodID)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
