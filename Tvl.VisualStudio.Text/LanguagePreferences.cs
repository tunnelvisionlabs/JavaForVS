namespace Tvl.VisualStudio.Text
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio;
    using FONTCOLORPREFERENCES2 = Microsoft.VisualStudio.TextManager.Interop.FONTCOLORPREFERENCES2;
    using FRAMEPREFERENCES2 = Microsoft.VisualStudio.TextManager.Interop.FRAMEPREFERENCES2;
    using IVsTextManagerEvents2 = Microsoft.VisualStudio.TextManager.Interop.IVsTextManagerEvents2;
    using IVsTextView = Microsoft.VisualStudio.TextManager.Interop.IVsTextView;
    using LANGPREFERENCES2 = Microsoft.VisualStudio.TextManager.Interop.LANGPREFERENCES2;
    using VIEWPREFERENCES2 = Microsoft.VisualStudio.TextManager.Interop.VIEWPREFERENCES2;

    public class LanguagePreferences : IVsTextManagerEvents2
    {
        private LANGPREFERENCES2 _preferences;

        public LanguagePreferences(LANGPREFERENCES2 preferences)
        {
            _preferences = preferences;
        }

        public event EventHandler PreferencesChanged;

        public LANGPREFERENCES2 RawPreferences
        {
            get
            {
                return _preferences;
            }
        }

        public bool ShowDropdownBar
        {
            get
            {
                return _preferences.fDropdownBar != 0;
            }
        }

        int IVsTextManagerEvents2.OnRegisterMarkerType(int iMarkerType)
        {
            return VSConstants.S_OK;
        }

        int IVsTextManagerEvents2.OnRegisterView(IVsTextView pView)
        {
            return VSConstants.S_OK;
        }

        int IVsTextManagerEvents2.OnReplaceAllInFilesBegin()
        {
            return VSConstants.S_OK;
        }

        int IVsTextManagerEvents2.OnReplaceAllInFilesEnd()
        {
            return VSConstants.S_OK;
        }

        int IVsTextManagerEvents2.OnUnregisterView(IVsTextView pView)
        {
            return VSConstants.S_OK;
        }

        int IVsTextManagerEvents2.OnUserPreferencesChanged2(VIEWPREFERENCES2[] pViewPrefs, FRAMEPREFERENCES2[] pFramePrefs, LANGPREFERENCES2[] pLangPrefs, FONTCOLORPREFERENCES2[] pColorPrefs)
        {
            if (pLangPrefs != null)
            {
                LANGPREFERENCES2[] preferences = pLangPrefs.Where(i => i.guidLang == _preferences.guidLang).ToArray();
                if (preferences.Length > 0)
                    _preferences = preferences[0];
            }

            return VSConstants.S_OK;
        }

        protected virtual void OnPreferencesChanged(EventArgs e)
        {
            var t = PreferencesChanged;
            if (t != null)
                t(this, e);
        }
    }
}
