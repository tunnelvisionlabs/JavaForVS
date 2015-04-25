namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public class DebugDocumentCodeContext : IDebugMemoryContext2, IDebugCodeContext2, IDebugCodeContext3
    {
        private readonly IDebugDocumentPosition2 _documentPosition;

        public DebugDocumentCodeContext(IDebugDocumentPosition2 documentPosition)
        {
            Contract.Requires<ArgumentNullException>(documentPosition != null, "documentPosition");
            _documentPosition = documentPosition;
        }

        #region IDebugMemoryContext2 Members

        public int Add(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = null;
            return VSConstants.E_FAIL;
        }

        public int Compare(enum_CONTEXT_COMPARE Compare, IDebugMemoryContext2[] rgpMemoryContextSet, uint dwMemoryContextSetLen, out uint pdwMemoryContext)
        {
            pdwMemoryContext = 0;
            return VSConstants.E_FAIL;
        }

        public int GetInfo(enum_CONTEXT_INFO_FIELDS dwFields, CONTEXT_INFO[] pinfo)
        {
            return VSConstants.E_FAIL;
        }

        public int GetName(out string pbstrName)
        {
            return _documentPosition.GetFileName(out pbstrName);
        }

        public int Subtract(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            ppMemCxt = null;
            return VSConstants.E_FAIL;
        }

        #endregion

        #region IDebugCodeContext2 Members

        public int GetDocumentContext(out IDebugDocumentContext2 ppSrcCxt)
        {
            ppSrcCxt = new DebugDocumentPositionContext(_documentPosition);
            return VSConstants.S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Constants.JavaLanguageName;
            pguidLanguage = Constants.JavaLanguageGuid;
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugCodeContext3 Members

        public int GetModule(out IDebugModule2 ppModule)
        {
            ppModule = null;
            return VSConstants.E_FAIL;
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = null;
            return VSConstants.E_FAIL;
        }

        #endregion
    }
}
