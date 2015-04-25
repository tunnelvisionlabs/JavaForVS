// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using IntPtr = System.IntPtr;
    using jboolean = System.Byte;

    internal struct jvmtiThreadGroupInfo
    {
        public readonly jthreadGroup _parent;
        public readonly IntPtr _name;
        public readonly int _maxPriority;
        public readonly jboolean _isDaemon;

        public string Name
        {
            get
            {
                if (_name == IntPtr.Zero)
                    return null;

                unsafe
                {
                    return ModifiedUTF8Encoding.GetString((byte*)_name);
                }
            }
        }
    }
}
