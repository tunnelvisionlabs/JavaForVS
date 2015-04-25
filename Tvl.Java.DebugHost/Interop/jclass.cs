// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jclass : IEquatable<jclass>
    {
        public static readonly jclass Null = default(jclass);

        private readonly IntPtr _handle;

        public jclass(IntPtr handle)
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

        public static bool operator ==(jclass x, jclass y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jclass x, jclass y)
        {
            return x._handle != y._handle;
        }

        public static explicit operator jclass(jobject @object)
        {
            return new jclass(@object.Handle);
        }

        public static implicit operator jobject(jclass @class)
        {
            return new jobject(@class.Handle);
        }

        public bool Equals(jclass other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jclass))
                return false;

            return this._handle == ((jclass)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
