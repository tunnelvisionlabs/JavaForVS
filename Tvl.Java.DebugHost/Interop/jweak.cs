// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct jweak : IEquatable<jweak>
    {
        public static readonly jweak Null = default(jweak);

        private readonly IntPtr _handle;

        public jweak(IntPtr handle)
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

        public static bool operator ==(jweak x, jweak y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jweak x, jweak y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(jweak other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jweak))
                return false;

            return this._handle == ((jweak)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
