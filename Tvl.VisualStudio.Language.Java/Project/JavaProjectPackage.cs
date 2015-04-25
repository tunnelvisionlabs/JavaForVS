namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;
    using Microsoft.VisualStudio.Shell;
    using Tvl.VisualStudio.Shell;

    using IVsComponentSelectorProvider = Microsoft.VisualStudio.Shell.Interop.IVsComponentSelectorProvider;
    using VSConstants = Microsoft.VisualStudio.VSConstants;
    using VSPROPSHEETPAGE = Microsoft.VisualStudio.Shell.Interop.VSPROPSHEETPAGE;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(JavaProjectConstants.JavaProjectPackageNameResourceString, JavaProjectConstants.JavaProjectPackageDetailsResourceString, JavaProjectConstants.JavaProjectPackageProductVersionString/*, IconResourceID = 400*/)]
    [Guid(JavaProjectConstants.JavaProjectPackageGuidString)]
    [ProvideProjectFactory(
        typeof(JavaProjectFactory),
        "Java",
        "Java Project Files (*.javaproj);*.javaproj",
        "javaproj",
        "javaproj",
        "ProjectTemplates",
        LanguageVsTemplate = Constants.JavaLanguageName,
        NewProjectRequireNewFolderVsTemplate = false)]

    [ProvideObject(typeof(PropertyPages.JavaApplicationPropertyPage))]
    [ProvideObject(typeof(PropertyPages.JavaBuildEventsPropertyPage))]
    [ProvideObject(typeof(PropertyPages.JavaBuildPropertyPage))]
    [ProvideObject(typeof(PropertyPages.JavaDebugPropertyPage))]
    [ProvideObject(typeof(PropertyPages.JavaGeneralPropertyPage))]

    [ProvideComponentSelectorTab(typeof(PropertyPages.MavenComponentSelector), typeof(JavaProjectPackage), "Maven")]
    public class JavaProjectPackage : ProjectPackage, IVsComponentSelectorProvider
    {
        private PropertyPages.MavenComponentSelector _mavenComponentSelector;

        public override string ProductUserContext
        {
            get
            {
                return "Java";
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            RegisterProjectFactory(new JavaProjectFactory(this));
        }

        #region IVsComponentSelectorProvider Members

        public int GetComponentSelectorPage(ref Guid rguidPage, VSPROPSHEETPAGE[] ppage)
        {
            if (ppage == null)
                throw new ArgumentNullException("ppage");
            if (ppage.Length == 0)
                throw new ArgumentException();

            if (rguidPage == JavaProjectConstants.MavenComponentSelectorGuid)
            {
                _mavenComponentSelector = _mavenComponentSelector ?? new PropertyPages.MavenComponentSelector();

                ppage[0] = new VSPROPSHEETPAGE()
                    {
                        dwFlags = (uint)default(PropertySheetPageFlags),
                        dwReserved = 0,
                        dwSize = (uint)Marshal.SizeOf(typeof(VSPROPSHEETPAGE)),
                        dwTemplateSize = 0,
                        HINSTANCE = 0,
                        hwndDlg = _mavenComponentSelector.Handle,
                        lParam = IntPtr.Zero,
                        pcRefParent = IntPtr.Zero,
                        pfnCallback = IntPtr.Zero,
                        pfnDlgProc = IntPtr.Zero,
                        pTemplate = IntPtr.Zero,
                        wTemplateId = 0,
                    };

                return VSConstants.S_OK;
            }

            return VSConstants.E_INVALIDARG;
        }

        #endregion
    }
}
