// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct jvmtiEnvHandle : IEquatable<jvmtiEnvHandle>
    {
        public static readonly jvmtiEnvHandle Null = default(jvmtiEnvHandle);

        private readonly IntPtr _handle;

        public jvmtiEnvHandle(IntPtr handle)
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

        public static bool operator ==(jvmtiEnvHandle x, jvmtiEnvHandle y)
        {
            return x._handle == y._handle;
        }

        public static bool operator !=(jvmtiEnvHandle x, jvmtiEnvHandle y)
        {
            return x._handle != y._handle;
        }

        public bool Equals(jvmtiEnvHandle other)
        {
            return this._handle == other._handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is jvmtiEnvHandle))
                return false;

            return this._handle == ((jvmtiEnvHandle)obj)._handle;
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}
