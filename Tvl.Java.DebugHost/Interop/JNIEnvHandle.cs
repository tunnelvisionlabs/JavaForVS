// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct JNIEnvHandle : IEquatable<JNIEnvHandle>
    {
        public static readonly JNIEnvHandle Null = default(JNIEnvHandle);

        private readonly IntPtr _handle;

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public static bool operator ==(JNIEnvHandle x, JNIEnvHandle y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(JNIEnvHandle x, JNIEnvHandle y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(JNIEnvHandle other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JNIEnvHandle))
                return false;

            return this._handle == ((JNIEnvHandle)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
