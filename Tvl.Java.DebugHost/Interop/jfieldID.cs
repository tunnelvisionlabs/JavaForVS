// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using Tvl.Java.DebugInterface.Types;

    internal struct jfieldID : IEquatable<jfieldID>
    {
        public static readonly jfieldID Null = default(jfieldID);

        private readonly IntPtr _handle;

        public jfieldID(IntPtr handle)
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

        public static implicit operator jfieldID(FieldId fieldId)
        {
            return new jfieldID((IntPtr)fieldId.Handle);
        }

        public static implicit operator FieldId(jfieldID fieldId)
        {
            return new FieldId(fieldId.Handle.ToInt64());
        }

        public static bool operator ==(jfieldID x, jfieldID y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jfieldID x, jfieldID y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(jfieldID other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jfieldID))
                return false;

            return this._handle == ((jfieldID)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
