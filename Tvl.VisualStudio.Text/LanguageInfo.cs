namespace Tvl.VisualStudio.Text
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.VisualStudio.Shell;

    using IConnectionPointContainer = Microsoft.VisualStudio.OLE.Interop.IConnectionPointContainer;

    public abstract class LanguageInfo : IVsLanguageInfo, IVsLanguageDebugInfo, IDisposable
    {
        private readonly SVsServiceProvider _serviceProvider;
        private readonly Guid _languageGuid;
        private LanguagePreferences _languagePreferences;
        private IDisposable _languagePreferencesCookie;

        public LanguageInfo(SVsServiceProvider serviceProvider, Guid languageGuid)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");

            _serviceProvider = serviceProvider;
            _languageGuid = languageGuid;

            IVsTextManager2 textManager = serviceProvider.GetTextManager2();
            LANGPREFERENCES2[] preferences = new LANGPREFERENCES2[1];
            preferences[0].guidLang = languageGuid;
            ErrorHandler.ThrowOnFailure(textManager.GetUserPreferences2(null, null, preferences, null));
            _languagePreferences = CreateLanguagePreferences(preferences[0]);
            _languagePreferencesCookie = ((IConnectionPointContainer)textManager).Advise<LanguagePreferences, IVsTextManagerEvents2>(_languagePreferences);
        }

        public abstract string LanguageName
        {
            get;
        }

        public abstract IEnumerable<string> FileExtensions
        {
            get;
        }

        public LanguagePreferences LanguagePreferences
        {
            get
            {
                return _languagePreferences;
            }
        }

        public Guid LanguageGuid
        {
            get
            {
                return _languageGuid;
            }
        }

        protected SVsServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }

        protected IComponentModel ComponentModel
        {
            get
            {
                return _serviceProvider.GetComponentModel();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual int GetColorizer(IVsTextLines buffer, out IVsColorizer colorizer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");

            colorizer = null;
            return VSConstants.E_FAIL;
        }

        /// <summary>
        /// Returns the corresponding debugger back-end "language ID".
        /// </summary>
        /// <remarks>
        /// Return the corresponding debugger back-end language identifier. This is not the debug engine
        /// identifier, which should be obtained by the current project or somewhere else that knows how
        /// the sources for this language are being built.
        /// </remarks>
        /// <param name="buffer">[in] The <see cref="IVsTextBuffer"/> interface for which the language identifier is required.</param>
        /// <param name="line">[in] Integer containing the line index.</param>
        /// <param name="col">[in] Integer containing the column index.</param>
        /// <param name="languageId">[out] Returns a GUID specifying the language identifier.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public virtual int GetLanguageID(IVsTextBuffer buffer, int line, int col, out Guid languageId)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");

            languageId = LanguageGuid;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Deprecated. Do not use.
        /// </summary>
        /// <param name="name">Do not use.</param>
        /// <param name="pbstrMkDoc">Do not use.</param>
        /// <param name="spans">Do not use.</param>
        /// <returns></returns>
        [Obsolete]
        public virtual int GetLocationOfName(string name, out string pbstrMkDoc, TextSpan[] spans)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            pbstrMkDoc = null;
            return VSConstants.E_NOTIMPL;
        }

        /// <summary>
        /// Generates a name for the given location in the file.
        /// </summary>
        /// <remarks>
        /// This method generates a name for the given location in the given file. This name represents
        /// the "innermost named entity" in the source. If non-null, the <paramref name="lineOffset"/>
        /// parameter is filled with the offset from the first line of the named entity. Returns S_FALSE
        /// if the position doesn't fall within anything interesting.
        /// </remarks>
        /// <param name="buffer">[in] Returns the text buffer (<see cref="IVsTextBuffer"/> object) that contains the location.</param>
        /// <param name="line">[in] Number of the line containing the location.</param>
        /// <param name="col">[in] Column containing the location in the line.</param>
        /// <param name="name">[out] Returns a string containing the name of the location.</param>
        /// <param name="lineOffset">[out] Returns an integer containing the line offset from <paramref name="line"/>.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public virtual int GetNameOfLocation(IVsTextBuffer buffer, int line, int col, out string name, out int lineOffset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");

            name = null;
            lineOffset = 0;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Generates proximity expressions.
        /// </summary>
        /// <remarks>
        /// This method is implemented by a language service to provide information needed to populate the
        /// <strong>Autos</strong> debugging window. When the debugger calls this method, the debugger is
        /// requesting the names of any parameters and variables in a span of lines beginning with the
        /// starting position identified by the <paramref name="line"/> and <paramref name="col"/> parameters
        /// in the specified text buffer. The extent of lines beyond this point is specified by the
        /// <paramref name="cLines"/> parameter.
        /// </remarks>
        /// <param name="buffer">[in] The <see cref="IVsTextBuffer"/> interface for the text buffer containing the expression.</param>
        /// <param name="line">[in] Number of the line containing the start of the expression.</param>
        /// <param name="col">[in] Column position within the line.</param>
        /// <param name="cLines">[in] Number of lines within the expression.</param>
        /// <param name="expressions">[out] Returns an IVsEnumBSTR object that is used to enumerate BSTRs.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public virtual int GetProximityExpressions(IVsTextBuffer buffer, int line, int col, int cLines, out IVsEnumBSTR expressions)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");

            expressions = null;
            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// Returns whether the location contains code that is mapped to another document, for example,
        /// client-side script code.
        /// </summary>
        /// <remarks>
        /// Return whether the location contains code that is mapped to another document, for example client-side script code.
        /// </remarks>
        /// <param name="buffer">[in] The IVsTextBuffer interface that contains the location in question.</param>
        /// <param name="line">[in] Integer containing the line index.</param>
        /// <param name="col">[in] Integer containing the column index.</param>
        /// <returns>If the method succeeds, returns S_OK indicating the location contains mapped code.
        /// If the location does not contain mapped code, returns S_FALSE. Otherwise, returns an error code.</returns>
        public virtual int IsMappedLocation(IVsTextBuffer buffer, int line, int col)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// Disambiguates the given name, providing non-ambiguous names for all entities that "match" the name.
        /// </summary>
        /// <remarks>
        /// This method disambiguates the given name, providing non-ambiguous names for all entities that "match" the name.
        /// </remarks>
        /// <param name="name">[in] String containing the name.</param>
        /// <param name="flags">[in] Flags. For more information, see RESOLVENAMEFLAGS.</param>
        /// <param name="names">[out] Returns an object containing a list of names. For more information, see IVsEnumDebugName.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public virtual int ResolveName(string name, RESOLVENAMEFLAGS flags, out IVsEnumDebugName names)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            names = null;
            return VSConstants.E_NOTIMPL;
        }

        /// <summary>
        /// Validates the given position as a place to set a breakpoint.
        /// </summary>
        /// <remarks>
        /// This method validates the given position as a place to set a breakpoint without having to load the
        /// debugger. If the location is valid, then the span is filled in with the extent of the statement at
        /// which execution would stop. If the position is known to not contain code, this method returns S_FALSE.
        /// If the method fails, the breakpoint is set, pending validation during the debugger startup.
        /// 
        /// Even if you do not intend to support the ValidateBreakpointLocation method but your language does
        /// support breakpoints, you must implement this method and return a span that contains the specified
        /// line and column; otherwise, breakpoints cannot be set anywhere except line 1. You can return E_NOTIMPL
        /// to indicate that you do not otherwise support this method but the span must always be set. The example
        /// shows how this can be done.
        /// </remarks>
        /// <param name="buffer">[in] The IVsTextBuffer interface for the text buffer containing the breakpoint.</param>
        /// <param name="line">[in] Number of the line containing the breakpoint.</param>
        /// <param name="col">[in] Number of the column containing the breakpoint.</param>
        /// <param name="pCodeSpan">[out] Returns a span of text containing the extent of the statement at which execution would stop if the breakpoint were set.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public virtual int ValidateBreakpointLocation(IVsTextBuffer buffer, int line, int col, TextSpan[] pCodeSpan)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentNullException>(pCodeSpan != null, "pCodeSpan");
            Contract.Requires<ArgumentException>(pCodeSpan.Length > 0);

            return VSConstants.E_NOTIMPL;
        }

        int IVsLanguageInfo.GetColorizer(IVsTextLines buffer, out IVsColorizer colorizer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            return GetColorizer(buffer, out colorizer);
        }

        int IVsLanguageInfo.GetCodeWindowManager(IVsCodeWindow pCodeWin, out IVsCodeWindowManager ppCodeWinMgr)
        {
            IVsEditorAdaptersFactoryService adaptersFactory = ComponentModel.GetService<IVsEditorAdaptersFactoryService>();

            IVsTextLines textLines;
            ErrorHandler.ThrowOnFailure(pCodeWin.GetBuffer(out textLines));
            ITextBuffer textBuffer = adaptersFactory.GetDataBuffer(textLines);
            if (textBuffer == null)
            {
                ppCodeWinMgr = null;
                return VSConstants.E_FAIL;
            }

            ppCodeWinMgr = GetCodeWindowManager(pCodeWin, textBuffer);
            return VSConstants.S_OK;
        }

        int IVsLanguageInfo.GetFileExtensions(out string pbstrExtensions)
        {
            pbstrExtensions = string.Join(";", FileExtensions);
            return VSConstants.S_OK;
        }

        int IVsLanguageInfo.GetLanguageName(out string bstrName)
        {
            bstrName = LanguageName;
            return string.IsNullOrEmpty(bstrName) ? VSConstants.E_FAIL : VSConstants.S_OK;
        }

        int IVsLanguageDebugInfo.GetLanguageID(IVsTextBuffer pBuffer, int iLine, int iCol, out Guid pguidLanguageID)
        {
            if (pBuffer == null)
                throw new ArgumentNullException("pBuffer");

            return GetLanguageID(pBuffer, iLine, iCol, out pguidLanguageID);
        }

        [Obsolete]
        int IVsLanguageDebugInfo.GetLocationOfName(string pszName, out string pbstrMkDoc, TextSpan[] pspanLocation)
        {
            if (pszName == null)
                throw new ArgumentNullException("pszName");
            if (pszName.Length == 0)
                throw new ArgumentException();

            return GetLocationOfName(pszName, out pbstrMkDoc, pspanLocation);
        }

        int IVsLanguageDebugInfo.GetNameOfLocation(IVsTextBuffer pBuffer, int iLine, int iCol, out string pbstrName, out int piLineOffset)
        {
            if (pBuffer == null)
                throw new ArgumentNullException("pBuffer");

            return GetNameOfLocation(pBuffer, iLine, iCol, out pbstrName, out piLineOffset);
        }

        int IVsLanguageDebugInfo.GetProximityExpressions(IVsTextBuffer pBuffer, int iLine, int iCol, int cLines, out IVsEnumBSTR ppEnum)
        {
            if (pBuffer == null)
                throw new ArgumentNullException("pBuffer");

            return GetProximityExpressions(pBuffer, iLine, iCol, cLines, out ppEnum);
        }

        int IVsLanguageDebugInfo.IsMappedLocation(IVsTextBuffer pBuffer, int iLine, int iCol)
        {
            if (pBuffer == null)
                throw new ArgumentNullException("pBuffer");

            return IsMappedLocation(pBuffer, iLine, iCol);
        }

        int IVsLanguageDebugInfo.ResolveName(string pszName, uint dwFlags, out IVsEnumDebugName ppNames)
        {
            if (pszName == null)
                throw new ArgumentNullException("pszName");
            if (pszName.Length == 0)
                throw new ArgumentException();

            return ResolveName(pszName, (RESOLVENAMEFLAGS)dwFlags, out ppNames);
        }

        int IVsLanguageDebugInfo.ValidateBreakpointLocation(IVsTextBuffer pBuffer, int iLine, int iCol, TextSpan[] pCodeSpan)
        {
            if (pBuffer == null)
                throw new ArgumentNullException("pBuffer");

            return ValidateBreakpointLocation(pBuffer, iLine, iCol, pCodeSpan);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_languagePreferencesCookie != null)
                {
                    _languagePreferencesCookie.Dispose();
                    _languagePreferencesCookie = null;
                }
            }
        }

        protected virtual LanguagePreferences CreateLanguagePreferences(LANGPREFERENCES2 preferences)
        {
            Contract.Ensures(Contract.Result<LanguagePreferences>() != null);

            return new LanguagePreferences(preferences);
        }

        protected virtual IVsCodeWindowManager GetCodeWindowManager(IVsCodeWindow codeWindow, ITextBuffer textBuffer)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Ensures(Contract.Result<IVsCodeWindowManager>() != null);

            return textBuffer.Properties.GetOrCreateSingletonProperty<CodeWindowManager>(() => new CodeWindowManager(codeWindow, ServiceProvider, LanguagePreferences));
        }
    }
}
