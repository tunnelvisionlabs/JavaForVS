namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using Microsoft.VisualStudio.Debugger.Interop;

    public abstract class BreakpointResolutionLocation
    {
        public abstract enum_BP_TYPE Type
        {
            get;
        }

        public static BreakpointResolutionLocation FromNativeForm(BP_RESOLUTION_LOCATION location, bool releaseComObjects)
        {
            switch ((enum_BP_TYPE)location.bpType)
            {
            case enum_BP_TYPE.BPT_CODE:
                return new BreakpointResolutionLocationCode(location, releaseComObjects);

            case enum_BP_TYPE.BPT_DATA:
                return new BreakpointResolutionLocationData(location, releaseComObjects);

            case enum_BP_TYPE.BPT_NONE:
            case enum_BP_TYPE.BPT_SPECIAL:
            default:
                throw new NotSupportedException();
            }
        }

        public abstract void ToNativeForm(out BP_RESOLUTION_LOCATION location);
    }
}
