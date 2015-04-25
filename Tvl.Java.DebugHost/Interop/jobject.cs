// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jobject : IEquatable<jobject>
    {
        public static readonly jobject Null = default(jobject);

        private readonly IntPtr _handle;

        public jobject(IntPtr handle)
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

        public static bool operator ==(jobject x, jobject y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jobject x, jobject y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(jobject other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jobject))
                return false;

            return this._handle == ((jobject)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
