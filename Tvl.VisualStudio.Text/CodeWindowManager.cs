namespace Tvl.VisualStudio.Text
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.Events;
    using Tvl.VisualStudio.Shell;
    using Tvl.VisualStudio.Text.Navigation;

    public class CodeWindowManager : IVsCodeWindowManager
    {
        private readonly IVsCodeWindow _codeWindow;
        private readonly SVsServiceProvider _serviceProvider;
        private readonly LanguagePreferences _languagePreferences;
        private IVsDropdownBarClient _dropdownBarClient;

        public CodeWindowManager(IVsCodeWindow codeWindow, SVsServiceProvider serviceProvider, LanguagePreferences languagePreferences)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            Contract.Requires<ArgumentNullException>(languagePreferences != null, "languagePreferences");

            _codeWindow = codeWindow;
            _serviceProvider = serviceProvider;
            _languagePreferences = languagePreferences;
            _languagePreferences.PreferencesChanged += WeakEvents.AsWeak(HandleLanguagePreferencesChanged, handler => _languagePreferences.PreferencesChanged -= handler);
        }

        public IVsCodeWindow CodeWindow
        {
            get
            {
                return _codeWindow;
            }
        }

        public SVsServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }

        public LanguagePreferences LanguagePreferences
        {
            get
            {
                return _languagePreferences;
            }
        }

        public virtual int AddAdornments()
        {
            IVsTextView textView;
            if (ErrorHandler.Succeeded(CodeWindow.GetPrimaryView(out textView)) && textView != null)
                ErrorHandler.ThrowOnFailure(OnNewView(textView));
            if (ErrorHandler.Succeeded(CodeWindow.GetSecondaryView(out textView)) && textView != null)
                ErrorHandler.ThrowOnFailure(OnNewView(textView));

            int comboBoxCount;
            IVsDropdownBarClient client;
            if (LanguagePreferences.ShowDropdownBar && TryCreateDropdownBarClient(out comboBoxCount, out client))
            {
                ErrorHandler.ThrowOnFailure(AddDropdownBar(comboBoxCount, client));
                _dropdownBarClient = client;
            }

            return VSConstants.S_OK;
        }

        public virtual int OnNewView(IVsTextView view)
        {
            Contract.Requires<ArgumentNullException>(view != null, "view");

            return VSConstants.S_OK;
        }

        int IVsCodeWindowManager.OnNewView(IVsTextView pView)
        {
            if (pView == null)
                throw new ArgumentNullException("pView");

            return OnNewView(pView);
        }

        public virtual int RemoveAdornments()
        {
            return RemoveDropdownBar();
        }

        protected virtual bool TryCreateDropdownBarClient(out int comboBoxCount, out IVsDropdownBarClient client)
        {
            var componentModel = _serviceProvider.GetComponentModel();
            var dropdownBarFactory = componentModel.DefaultExportProvider.GetExportedValueOrDefault<IJavaEditorNavigationDropdownBarFactoryService>();
            var editorAdaptersFactory = componentModel.DefaultExportProvider.GetExportedValueOrDefault<IVsEditorAdaptersFactoryService>();

            editorAdaptersFactory.GetWpfTextView(_codeWindow.GetPrimaryView());
            var dropdownBarClient = dropdownBarFactory.CreateEditorNavigationDropdownBar(CodeWindow, editorAdaptersFactory);

            if (dropdownBarClient == null || dropdownBarClient.DropdownCount == 0)
            {
                comboBoxCount = 0;
                client = null;
                return false;
            }

            comboBoxCount = dropdownBarClient.DropdownCount;
            client = dropdownBarClient;
            return true;
        }

        protected virtual int AddDropdownBar(int comboBoxCount, IVsDropdownBarClient client)
        {
            Contract.Requires<ArgumentNullException>(client != null, "client");
            Contract.Requires<ArgumentOutOfRangeException>(comboBoxCount > 0);

            IVsDropdownBarManager manager = CodeWindow as IVsDropdownBarManager;
            if (manager == null)
                throw new NotSupportedException();

            IVsDropdownBar dropdownBar;
            int hr = manager.GetDropdownBar(out dropdownBar);
            if (ErrorHandler.Succeeded(hr) && dropdownBar != null)
            {
                hr = manager.RemoveDropdownBar();
                if (ErrorHandler.Failed(hr))
                    return hr;
            }

            return manager.AddDropdownBar(comboBoxCount, client);
        }

        protected virtual int RemoveDropdownBar()
        {
            IVsDropdownBarManager manager = CodeWindow as IVsDropdownBarManager;
            if (manager == null)
                return VSConstants.E_FAIL;

            IVsDropdownBar dropdownBar;
            int hr = manager.GetDropdownBar(out dropdownBar);
            if (ErrorHandler.Succeeded(hr) && dropdownBar != null)
            {
                IVsDropdownBarClient client;
                hr = dropdownBar.GetClient(out client);
                if (ErrorHandler.Succeeded(hr) && client == _dropdownBarClient)
                {
                    _dropdownBarClient = null;
                    return manager.RemoveDropdownBar();
                }
            }

            _dropdownBarClient = null;
            return VSConstants.S_OK;
        }

        protected virtual void HandleLanguagePreferencesChanged(object sender, EventArgs e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

            int comboBoxCount;
            IVsDropdownBarClient client;
            if (_dropdownBarClient == null && LanguagePreferences.ShowDropdownBar && TryCreateDropdownBarClient(out comboBoxCount, out client))
            {
                ErrorHandler.ThrowOnFailure(AddDropdownBar(comboBoxCount, client));
                _dropdownBarClient = client;
            }
            else if (_dropdownBarClient != null && !LanguagePreferences.ShowDropdownBar)
            {
                ErrorHandler.ThrowOnFailure(RemoveDropdownBar());
            }
        }
    }
}
