namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Tvl.VisualStudio.Shell;

    using IServiceContainer = System.ComponentModel.Design.IServiceContainer;
    using JavaIntellisenseOptions = Tvl.VisualStudio.Language.Java.OptionsPages.JavaIntellisenseOptions;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(Constants.JavaLanguagePackageNameResourceString, Constants.JavaLanguagePackageDetailsResourceString, Constants.JavaLanguagePackageProductVersionString/*, IconResourceID = 400*/)]
    //[ProvideAutoLoad(Constants.UIContextNoSolution)]
    //[ProvideAutoLoad(Constants.UIContextSolutionExists)]
    [Guid(Constants.JavaLanguagePackageGuidString)]
    [ProvideLanguageService(typeof(JavaLanguageInfo), Constants.JavaLanguageName, Constants.JavaLanguageResourceId,
        //AutoOutlining = true,
        //QuickInfo = true,
        ShowCompletion = true,
        ShowDropDownOptions = true,
        //ShowHotURLs = true,
        //ShowMatchingBrace = true,
        EnableAdvancedMembersOption = false,
        DefaultToInsertSpaces = false,
        //EnableCommenting = true,
        //HideAdvancedMembersByDefault = false,
        EnableLineNumbers = true,
        //CodeSense = true,
        RequestStockColors = true)]
    [ProvideLanguageExtension(typeof(JavaLanguageInfo), Constants.JavaFileExtension)]

    [ProvideLanguageEditorOptionPage(typeof(JavaIntellisenseOptions), Constants.JavaLanguageName, "", "IntelliSense", "#410")]
    public class JavaLanguagePackage : Package
    {
        private static JavaLanguagePackage _instance;
        private JavaLanguageInfo _languageInfo;

        public JavaLanguagePackage()
        {
            _instance = this;
        }

        public static JavaLanguagePackage Instance
        {
            get
            {
                return _instance;
            }
        }

        public JavaIntellisenseOptions IntellisenseOptions
        {
            get
            {
                return GetDialogPage<JavaIntellisenseOptions>();
            }
        }

        protected T GetDialogPage<T>()
            where T : DialogPage
        {
            return (T)base.GetDialogPage(typeof(T));
        }

        protected override void Initialize()
        {
            base.Initialize();

            // register the language service
            _languageInfo = new JavaLanguageInfo(this.AsVsServiceProvider());
            ((IServiceContainer)this).AddService(typeof(JavaLanguageInfo), _languageInfo, true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_languageInfo != null)
                {
                    _languageInfo.Dispose();
                    _languageInfo = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
