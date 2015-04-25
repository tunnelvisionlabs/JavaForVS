// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jthreadGroup : IEquatable<jthreadGroup>
    {
        public static readonly jthreadGroup Null = default(jthreadGroup);

        private readonly IntPtr _handle;

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public static bool operator ==(jthreadGroup x, jthreadGroup y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jthreadGroup x, jthreadGroup y)
        {
            return x._handle != y._handle;
        }

        public static implicit operator jobject(jthreadGroup threadGroup)
        {
            return new jobject(threadGroup.Handle);
        }

        public bool Equals(jthreadGroup other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jthreadGroup))
                return false;

            return this._handle == ((jthreadGroup)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
