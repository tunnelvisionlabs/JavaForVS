namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;

    public class ModifiedUTF8StringData : CriticalHandle
    {
        public readonly IntPtr _data;

        public ModifiedUTF8StringData(string value)
            : base(IntPtr.Zero)
        {
            byte[] data = ModifiedUTF8Encoding.GetBytes(value);
            _data = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, _data, data.Length);
        }

        public string GetString()
        {
            unsafe
            {
                return ModifiedUTF8Encoding.GetString((byte*)_data);
            }
        }

        public override bool IsInvalid
        {
            get
            {
                return _data == IntPtr.Zero;
            }
        }

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(_data);
            return true;
        }
    }
}
