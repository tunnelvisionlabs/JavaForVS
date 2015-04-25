namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using Microsoft.VisualStudio.Debugger.Interop;

    public abstract class BreakpointLocation
    {
        public abstract enum_BP_LOCATION_TYPE LocationType
        {
            get;
        }

        public static BreakpointLocation FromNativeForm(BP_LOCATION location, bool releaseComObjects)
        {
            switch ((enum_BP_LOCATION_TYPE)location.bpLocationType)
            {
            case enum_BP_LOCATION_TYPE.BPLT_CODE_ADDRESS:
                return new BreakpointLocationCodeAddress(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_CODE_CONTEXT:
                return new BreakpointLocationCodeContext(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE:
                return new BreakpointLocationCodeFileLine(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_CODE_FUNC_OFFSET:
                return new BreakpointLocationCodeFunctionOffset(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_CODE_STRING:
                return new BreakpointLocationCodeString(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_DATA_STRING:
                return new BreakpointLocationDataString(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_RESOLUTION:
                return new BreakpointLocationResolution(location, releaseComObjects);

            case enum_BP_LOCATION_TYPE.BPLT_NONE:
            case enum_BP_LOCATION_TYPE.BPLT_STRING:
            case enum_BP_LOCATION_TYPE.BPLT_TYPE_MASK:
            case enum_BP_LOCATION_TYPE.BPLT_CONTEXT:
            case enum_BP_LOCATION_TYPE.BPLT_ADDRESS:
            case enum_BP_LOCATION_TYPE.BPLT_FILE_LINE:
            case enum_BP_LOCATION_TYPE.BPLT_FUNC_OFFSET:
            case enum_BP_LOCATION_TYPE.BPLT_LOCATION_TYPE_MASK:
            default:
                throw new ArgumentException();
            }
        }
    }
}
