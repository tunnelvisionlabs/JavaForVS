// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jthrowable : IEquatable<jthrowable>
    {
        public static readonly jthrowable Null = default(jthrowable);

        private readonly IntPtr _handle;

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public static implicit operator jobject(jthrowable throwable)
        {
            return new jobject(throwable.Handle);
        }

        public static bool operator ==(jthrowable x, jthrowable y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jthrowable x, jthrowable y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(jthrowable other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jthrowable))
                return false;

            return this._handle == ((jthrowable)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
