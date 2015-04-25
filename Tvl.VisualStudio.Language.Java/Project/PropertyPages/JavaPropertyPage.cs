namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Project;
    using Microsoft.VisualStudio.Shell.Interop;

    [ComVisible(true)]
    public abstract class JavaPropertyPage : IPropertyPage
    {
        private const int Win32SwHide = 0;

        private bool _active;
        private bool _isDirty;
        private JavaPropertyPagePanel _propertyPagePanel;
        private string _pageName;
        private IPropertyPageSite _pageSite;
        private JavaProjectNode _project;
        private ProjectConfig[] _projectConfigs;
        private bool _initializing;

        public JavaPropertyPage()
        {
        }

        [Browsable(false)]
        [AutomationBrowsable(false)]
        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }

            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    if (_pageSite != null)
                    {
                        _pageSite.OnStatusChange((uint)(_isDirty ? PropPageStatus.Dirty : PropPageStatus.Clean));
                    }
                }
            }
        }

        [Browsable(false)]
        [AutomationBrowsable(false)]
        public string PageName
        {
            get
            {
                return _pageName;
            }

            set
            {
                _pageName = value;
            }
        }

        public JavaProjectNode ProjectManager
        {
            get
            {
                return _project;
            }
        }

        internal System.IServiceProvider Site
        {
            get
            {
                return _project.Site;
            }
        }

        protected JavaPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return _propertyPagePanel;
            }

            private set
            {
                _propertyPagePanel = value;
            }
        }

        public ReadOnlyCollection<ProjectConfig> Configurations
        {
            get
            {
                return new ReadOnlyCollection<ProjectConfig>(_projectConfigs ?? new ProjectConfig[0]);
            }
        }

        void IPropertyPage.Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
            // create the panel control
            if (PropertyPagePanel == null)
                PropertyPagePanel = CreatePropertyPagePanel();

            // we need to create the control so the handle is valid
            PropertyPagePanel.CreateControl();

            // set our parent
            NativeMethods.SetParent(PropertyPagePanel.Handle, hWndParent);

            // set our initial size
            ResizeContents(pRect[0]);

            _active = true;
            _initializing = true;
            BindProperties();
            _initializing = false;
            IsDirty = false;
        }

        int IPropertyPage.Apply()
        {
            if (IsDirty)
            {
                // When a property page is being initialized, the corresponding controls will fire events for their values being changed.
                // We want to ignore them, since they don't represent any real changes done by the user.
                bool applied = true;
                if (!_initializing)
                {
                    applied = ApplyChanges();
                    IsDirty = !applied;
                }

                return (applied ? VSConstants.S_OK : VSConstants.S_FALSE);
            }

            return VSConstants.S_OK;
        }

        void IPropertyPage.Deactivate()
        {
            if (PropertyPagePanel != null)
            {
                PropertyPagePanel.Dispose();
                PropertyPagePanel = null;
            }

            _active = false;
        }

        void IPropertyPage.GetPageInfo(PROPPAGEINFO[] pPageInfo)
        {
            if (pPageInfo == null)
                throw new ArgumentNullException("pPageInfo");

            if (PropertyPagePanel == null)
                PropertyPagePanel = CreatePropertyPagePanel();

            PROPPAGEINFO info = new PROPPAGEINFO()
            {
                cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO)),
                dwHelpContext = 0,
                pszDocString = null,
                pszHelpFile = null,
                pszTitle = PageName,
                SIZE =
                {
                    cx = PropertyPagePanel.Width,
                    cy = PropertyPagePanel.Height
                }
            };

            pPageInfo[0] = info;
        }

        void IPropertyPage.Help(string pszHelpDir)
        {
        }

        int IPropertyPage.IsPageDirty()
        {
            return (IsDirty ? VSConstants.S_OK : VSConstants.S_FALSE);
        }

        void IPropertyPage.Move(RECT[] pRect)
        {
            ResizeContents(pRect[0]);
        }

        void IPropertyPage.SetObjects(uint cObjects, object[] ppunk)
        {
            if (cObjects == 0)
            {
                if (_project != null)
                {
                    //_project.CurrentOutputTypeChanging -= new PropertyChangingEventHandler( HandleOutputTypeChanging );
                    _project = null;
                }
                return;
            }

            if (ppunk[0] is ProjectConfig)
            {
                List<ProjectConfig> configs = new List<ProjectConfig>();

                for (int i = 0; i < cObjects; i++)
                {
                    ProjectConfig config = (ProjectConfig)ppunk[i];
                    if (_project == null)
                    {
                        _project = config.ProjectManager as JavaProjectNode;
                        //_project.CurrentOutputTypeChanging += new PropertyChangingEventHandler( HandleOutputTypeChanging );
                    }

                    configs.Add(config);
                }

                _projectConfigs = configs.ToArray();
            }
            else if (ppunk[0] is NodeProperties)
            {
                if (_project == null)
                {
                    _project = (ppunk[0] as NodeProperties).Node.ProjectManager as JavaProjectNode;
                    //_project.CurrentOutputTypeChanging += new PropertyChangingEventHandler( HandleOutputTypeChanging );
                }

                Dictionary<string, ProjectConfig> configsMap = new Dictionary<string, ProjectConfig>();

                for (int i = 0; i < cObjects; i++)
                {
                    NodeProperties property = (NodeProperties)ppunk[i];
                    IVsCfgProvider provider;
                    ErrorHandler.ThrowOnFailure(property.Node.ProjectManager.GetCfgProvider(out provider));
                    uint[] expected = new uint[1];
                    ErrorHandler.ThrowOnFailure(provider.GetCfgs(0, null, expected, null));
                    if (expected[0] > 0)
                    {
                        ProjectConfig[] configs = new ProjectConfig[expected[0]];
                        uint[] actual = new uint[1];
                        int hr = provider.GetCfgs(expected[0], configs, actual, null);
                        if (hr != VSConstants.S_OK)
                            Marshal.ThrowExceptionForHR(hr);

                        foreach (ProjectConfig config in configs)
                        {
                            string key = string.Format("{0}|{1}", config.ConfigName, config.Platform);
                            if (!configsMap.ContainsKey(key))
                                configsMap.Add(key, config);
                        }
                    }
                }

                if (configsMap.Count > 0)
                {
                    if (_projectConfigs == null)
                        _projectConfigs = new ProjectConfig[configsMap.Keys.Count];

                    configsMap.Values.CopyTo(_projectConfigs, 0);
                }
            }

            if (_active && _project != null)
            {
                BindProperties();
                IsDirty = false;
            }
        }

        void IPropertyPage.SetPageSite(IPropertyPageSite pPageSite)
        {
            _pageSite = pPageSite;
        }

        void IPropertyPage.Show(uint nCmdShow)
        {
            if (PropertyPagePanel != null)
            {
                if (nCmdShow == Win32SwHide)
                    PropertyPagePanel.Hide();
                else
                    PropertyPagePanel.Show();
            }
        }

        int IPropertyPage.TranslateAccelerator(MSG[] pMsg)
        {
            if (pMsg == null)
                throw new ArgumentNullException("pMsg");

            return VSConstants.S_FALSE;
        }

        public string GetConfigProperty(string propertyName, _PersistStorageType storageType)
        {
            string unifiedResult = string.Empty;

            if (ProjectManager != null)
            {
                bool cacheNeedReset = true;

                for (int i = 0; i < _projectConfigs.Length; i++)
                {
                    ProjectConfig config = _projectConfigs[i];
                    string property = config.GetConfigurationProperty(propertyName, storageType, cacheNeedReset);

                    cacheNeedReset = false;

                    if (property != null)
                    {
                        string text = property.Trim();

                        if (i == 0)
                        {
                            unifiedResult = text;
                        }
                        else if (unifiedResult != text)
                        {
                            unifiedResult = string.Empty;
                            break;
                        }
                    }
                }
            }

            return unifiedResult;
        }

        public bool GetConfigPropertyBoolean(string propertyName, _PersistStorageType storageType)
        {
            string value = GetConfigProperty(propertyName, storageType);

            bool converted;
            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out converted))
                return false;

            return converted;
        }

        public int GetConfigPropertyInt32(string propertyName, _PersistStorageType storageType)
        {
            string value = GetConfigProperty(propertyName, storageType);

            int converted;
            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out converted))
                return JavaProjectFileConstants.UnspecifiedValue;

            return converted;
        }

        public string GetProperty(string propertyName, _PersistStorageType storageType)
        {
            if (ProjectManager != null)
            {
                string property = ProjectManager.GetProjectProperty(propertyName, storageType, true);
                if (property != null)
                    return property;
            }

            return string.Empty;
        }

        public void SetConfigProperty(string propertyName, _PersistStorageType storageType, bool propertyValue)
        {
            SetConfigProperty(propertyName, storageType, propertyValue.ToString());
        }

        public void SetConfigProperty(string propertyName, _PersistStorageType storageType, int propertyValue)
        {
            SetConfigProperty(propertyName, storageType, propertyValue.ToString(CultureInfo.InvariantCulture));
        }

        public void SetConfigProperty(string propertyName, _PersistStorageType storageType, string propertyValue)
        {
            if (propertyValue == null)
            {
                propertyValue = string.Empty;
            }

            if (ProjectManager != null)
            {
                for (int i = 0, n = _projectConfigs.Length; i < n; i++)
                {
                    ProjectConfig config = _projectConfigs[i];
                    config.SetConfigurationProperty(propertyName, storageType, propertyValue);
                }

                ProjectManager.SetProjectFileDirty(true);
            }
        }

        public void SetProperty(string propertyName, _PersistStorageType storageType, string propertyValue)
        {
            if (ProjectManager != null)
                ProjectManager.SetProjectProperty(propertyName, storageType, propertyValue ?? string.Empty);
        }

        public void SetProperty(string propertyName, _PersistStorageType storageType, string propertyValue, string condition, bool treatPropertyValueAsLiteral)
        {
            if (ProjectManager != null)
                ProjectManager.SetProjectProperty(propertyName, storageType, propertyValue ?? string.Empty, condition, treatPropertyValueAsLiteral);
        }

        internal void UpdateStatus()
        {
            if (_pageSite != null)
            {
                _pageSite.OnStatusChange((uint)(IsDirty ? PropPageStatus.Dirty | PropPageStatus.Validate : PropPageStatus.Clean));
            }
        }

        protected abstract bool ApplyChanges();

        protected abstract void BindProperties();

        protected abstract JavaPropertyPagePanel CreatePropertyPagePanel();

        protected virtual void HandleOutputTypeChanging(object source, PropertyChangingEventArgs e)
        {
            // Do nothing here. Subclasses may optionally override.
        }

        private void ResizeContents(RECT newBounds)
        {
            if (PropertyPagePanel != null && PropertyPagePanel.IsHandleCreated)
            {
                // Visual Studio sends us the size of the area in which it wants us to size.
                // However, we don't want to size smaller than the property page's minimum
                // size, which scales according to the screen DPI.
                PropertyPagePanel.Bounds = new Rectangle(
                    newBounds.left,
                    newBounds.top,
                    Math.Max(newBounds.right - newBounds.left, PropertyPagePanel.MinimumSize.Width),
                    Math.Max(newBounds.bottom - newBounds.top, PropertyPagePanel.MinimumSize.Height));
            }
        }
    }
}
