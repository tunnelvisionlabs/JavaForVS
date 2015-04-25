namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointResolutionLocationData : BreakpointResolutionLocation
    {
        private readonly string _dataExpression;
        private readonly string _function;
        private readonly string _image;
        private readonly enum_BP_RES_DATA_FLAGS _flags;

        public BreakpointResolutionLocationData(string dataExpression, string function, string image, enum_BP_RES_DATA_FLAGS flags)
        {
            _dataExpression = dataExpression;
            _function = function;
            _image = image;
            _flags = flags;
        }

        public BreakpointResolutionLocationData(BP_RESOLUTION_LOCATION location, bool releaseComObjects)
        {
            if (location.bpType != (uint)enum_BP_TYPE.BPT_DATA)
                throw new ArgumentException();

            if (location.unionmember1 != IntPtr.Zero)
            {
                _dataExpression = Marshal.PtrToStringBSTR(location.unionmember1);
                if (releaseComObjects)
                    Marshal.FreeBSTR(location.unionmember1);
            }

            if (location.unionmember2 != IntPtr.Zero)
            {
                _function = Marshal.PtrToStringBSTR(location.unionmember2);
                if (releaseComObjects)
                    Marshal.FreeBSTR(location.unionmember2);
            }

            if (location.unionmember3 != IntPtr.Zero)
            {
                _image = Marshal.PtrToStringBSTR(location.unionmember3);
                if (releaseComObjects)
                    Marshal.FreeBSTR(location.unionmember3);
            }

            _flags = (enum_BP_RES_DATA_FLAGS)location.unionmember4;
        }

        public override enum_BP_TYPE Type
        {
            get
            {
                return enum_BP_TYPE.BPT_DATA;
            }
        }

        public override void ToNativeForm(out BP_RESOLUTION_LOCATION location)
        {
            location.bpType = (uint)Type;
            location.unionmember1 = _function != null ? Marshal.StringToBSTR(_dataExpression) : IntPtr.Zero;
            location.unionmember2 = _function != null ? Marshal.StringToBSTR(_function) : IntPtr.Zero;
            location.unionmember3 = _function != null ? Marshal.StringToBSTR(_image) : IntPtr.Zero;
            location.unionmember4 = (uint)_flags;
        }
    }
}
