namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class JavaDebugModule
        : IDebugModule3
        , IDebugModule2
        , IDebugQueryEngine2
    {
        #region IDebugModule2 Members

        public int GetInfo(enum_MODULE_INFO_FIELDS dwFields, MODULE_INFO[] pinfo)
        {
            throw new NotImplementedException();
        }

        public int ReloadSymbols_Deprecated(string pszUrlToSymbols, out string pbstrDebugMessage)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugModule3 Members


        public int GetSymbolInfo(enum_SYMBOL_SEARCH_INFO_FIELDS dwFields, MODULE_SYMBOL_SEARCH_INFO[] pinfo)
        {
            throw new NotImplementedException();
        }

        public int IsUserCode(out int pfUser)
        {
            throw new NotImplementedException();
        }

        public int LoadSymbols()
        {
            throw new NotImplementedException();
        }

        public int SetJustMyCodeState(int fIsUserCode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugQueryEngine2 Members

        public int GetEngineInterface(out object ppUnk)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
