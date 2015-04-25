// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jthread : IEquatable<jthread>
    {
        public static readonly jthread Null = default(jthread);

        private readonly IntPtr _handle;

        public jthread(IntPtr handle)
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

        public static bool operator ==(jthread x, jthread y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jthread x, jthread y)
        {
            return x._handle != y._handle;
        }

        public static explicit operator jthread(jobject @object)
        {
            return new jthread(@object.Handle);
        }

        public static implicit operator jobject(jthread thread)
        {
            return new jobject(thread.Handle);
        }

        public bool Equals(jthread other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jthread))
                return false;

            return this._handle == ((jthread)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
