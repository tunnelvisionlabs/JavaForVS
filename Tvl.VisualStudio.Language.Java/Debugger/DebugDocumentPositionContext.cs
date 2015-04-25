namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Path = System.IO.Path;

    internal class DebugDocumentPositionContext : IDebugDocumentContext2
    {
        private readonly IDebugDocumentPosition2 _documentPosition;

        public DebugDocumentPositionContext(IDebugDocumentPosition2 documentPosition)
        {
            Contract.Requires<ArgumentNullException>(documentPosition != null, "documentPosition");
            _documentPosition = documentPosition;
        }

        #region IDebugDocumentContext2 Members

        public int Compare(enum_DOCCONTEXT_COMPARE Compare, IDebugDocumentContext2[] rgpDocContextSet, uint dwDocContextSetLen, out uint pdwDocContext)
        {
            throw new NotImplementedException();
        }

        public int EnumCodeContexts(out IEnumDebugCodeContexts2 ppEnumCodeCxts)
        {
            ppEnumCodeCxts = null;
            return VSConstants.E_FAIL;
        }

        public int GetDocument(out IDebugDocument2 ppDocument)
        {
            return _documentPosition.GetDocument(out ppDocument);
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Constants.JavaLanguageName;
            pguidLanguage = Constants.JavaLanguageGuid;
            return VSConstants.S_OK;
        }

        public int GetName(enum_GETNAME_TYPE gnType, out string pbstrFileName)
        {
            ErrorHandler.ThrowOnFailure(_documentPosition.GetFileName(out pbstrFileName));

            switch (gnType)
            {
            case enum_GETNAME_TYPE.GN_BASENAME:
                pbstrFileName = Path.GetFileName(pbstrFileName);
                return VSConstants.S_OK;

            case enum_GETNAME_TYPE.GN_FILENAME:
            case enum_GETNAME_TYPE.GN_MONIKERNAME:
            case enum_GETNAME_TYPE.GN_NAME:
                return VSConstants.S_OK;

            case enum_GETNAME_TYPE.GN_STARTPAGEURL:
            case enum_GETNAME_TYPE.GN_TITLE:
            case enum_GETNAME_TYPE.GN_URL:
            default:
                pbstrFileName = null;
                return VSConstants.E_INVALIDARG;
            }
        }

        public int GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            return _documentPosition.GetRange(pBegPosition, pEndPosition);
        }

        public int GetStatementRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            return VSConstants.E_FAIL;
        }

        public int Seek(int nCount, out IDebugDocumentContext2 ppDocContext)
        {
            ppDocContext = null;
            return VSConstants.E_FAIL;
        }

        #endregion
    }
}
