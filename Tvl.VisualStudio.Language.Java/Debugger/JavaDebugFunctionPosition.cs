namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class JavaDebugFunctionPosition : IDebugFunctionPosition2
    {
        #region IDebugFunctionPosition2 Members

        public int GetFunctionName(out string pbstrFunctionName)
        {
            throw new NotImplementedException();
        }

        public int GetOffset(TEXT_POSITION[] pPosition)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
