namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
#if DEV10ONLY
    using Microsoft.VisualStudio.CallHierarchy.Package.Definitions;
#endif
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;
    using IGlyphService = Microsoft.VisualStudio.Language.Intellisense.IGlyphService;
    using IOleComponentManager = Microsoft.VisualStudio.OLE.Interop.IOleComponentManager;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
    using SOleComponentManager = Microsoft.VisualStudio.OLE.Interop.SOleComponentManager;

    public static class VsServiceProviderExtensions
    {
        public static IVsActivityLog GetActivityLog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsActivityLog, IVsActivityLog>();
        }

        public static IVsAddProjectItemDlg GetAddProjectItemDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsAddProjectItemDlg, IVsAddProjectItemDlg>();
        }

        public static IVsAddWebReferenceDlg GetAddWebReferenceDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsAddWebReferenceDlg, IVsAddWebReferenceDlg>();
        }

        public static IVsAppCommandLine GetAppCommandLine(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsAppCommandLine, IVsAppCommandLine>();
        }

        public static IVsAssemblyNameUnification GetAssemblyNameUnification(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsAssemblyNameUnification, IVsAssemblyNameUnification>();
        }

        public static IVsCallBrowser GetCallBrowser(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCallBrowser, IVsCallBrowser>();
        }

#if DEV10ONLY
        public static ICallHierarchy GetCallHierarchy(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SCallHierarchy, ICallHierarchy>();
        }
#endif

        public static IVsNavigationTool GetClassView(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsClassView, IVsNavigationTool>();
        }

        public static IVsCodeDefView GetCodeDefView(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCodeDefView, IVsCodeDefView>();
        }

        public static IVsCodeShareHandler GetCodeShareHandler(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCodeShareHandler, IVsCodeShareHandler>();
        }

        public static IVsCmdNameMapping GetCommandNameMapping(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCmdNameMapping, IVsCmdNameMapping>();
        }

        public static IVsCommandWindow GetCommandWindow(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCommandWindow, IVsCommandWindow>();
        }

        public static IVsCommandWindowsCollection GetCommandWindowsCollection(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCommandWindowsCollection, IVsCommandWindowsCollection>();
        }

        public static IComponentModel GetComponentModel(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SComponentModel, IComponentModel>();
        }

        public static IVsComponentSelectorDlg GetComponentSelectorDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsComponentSelectorDlg, IVsComponentSelectorDlg>();
        }

        public static IVsComponentSelectorDlg2 GetComponentSelectorDialog2(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsComponentSelectorDlg2, IVsComponentSelectorDlg2>();
        }

        public static IVsConfigurationManagerDlg GetConfigurationManagerDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsConfigurationManagerDlg, IVsConfigurationManagerDlg>();
        }

        public static IVsCreateAggregateProject GetCreateAggregateProject(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsCreateAggregateProject, IVsCreateAggregateProject>();
        }

        public static IVsDebuggableProtocol GetDebuggableProtocol(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsDebuggableProtocol, IVsDebuggableProtocol>();
        }

        public static IVsDebugLaunch GetDebugLaunch(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsDebugLaunch, IVsDebugLaunch>();
        }

        public static IVsDetermineWizardTrust GetDetermineWizardTrust(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsDetermineWizardTrust, IVsDetermineWizardTrust>();
        }

        public static IVsDiscoveryService GetDiscoveryService(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsDiscoveryService, IVsDiscoveryService>();
        }

#if false
        // DTE shouldn't be Dte
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        public static DTE GetDTE(this SVsServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider"); Contract.EndContractBlock();
            {
                return serviceProvider.GetService<EnvDTE._DTE, DTE>();
            }
        }

        // DTE2 shouldn't be Dte2
        [SuppressMessage("Microsoft.Naming", "CA1709")]
        public static DTE2 GetDTE2(this SVsServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider"); Contract.EndContractBlock();
            {
                return serviceProvider.GetService<EnvDTE._DTE, DTE2>();
            }
        }
#endif

        public static IVsEnumHierarchyItemsFactory GetEnumHierarchyItemsFactory(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsEnumHierarchyItemsFactory, IVsEnumHierarchyItemsFactory>();
        }

        public static IVsErrorList GetErrorList(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsErrorList, IVsErrorList>();
        }

        public static IVsExpansionManager GetExpansionManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");

            {
                IVsExpansionManager expMgr;
                var tmgr = serviceProvider.GetTextManager() as IVsTextManager2;
                if (tmgr != null && ErrorHandler.Succeeded(tmgr.GetExpansionManager(out expMgr)))
                    return expMgr;

                return null;
            }
        }

        public static IVsExternalFilesManager GetExternalFilesManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsExternalFilesManager, IVsExternalFilesManager>();
        }

        public static IVsFileChangeEx GetFileChange(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsFileChangeEx, IVsFileChangeEx>();
        }

        public static IVsFilterAddProjectItemDlg GetFilterAddProjectItemDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsFilterAddProjectItemDlg, IVsFilterAddProjectItemDlg>();
        }

        public static IVsFilterKeys GetFilterKeys(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsFilterKeys, IVsFilterKeys>();
        }

        public static IVsFindSymbol GetFindSymbol(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsObjectSearch, IVsFindSymbol>();
        }

        public static IVsFontAndColorCacheManager GetFontAndColorCacheManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsFontAndColorCacheManager, IVsFontAndColorCacheManager>();
        }

        public static IVsFontAndColorStorage GetFontAndColorStorage(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsFontAndColorStorage, IVsFontAndColorStorage>();
        }

        public static IVsFontAndColorUtilities GetFontAndColorUtilities(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return (IVsFontAndColorUtilities)serviceProvider.GetFontAndColorStorage();
        }

        public static IGlyphService GetGlyphService(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetComponentModel().GetService<IGlyphService>();
        }

        public static IVsHelpSystem GetHelpSystem(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsHelpService, IVsHelpSystem>();
        }

        public static IVsHTMLConverter GetHtmlConverter(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsHTMLConverter, IVsHTMLConverter>();
        }

        public static IVsIME GetIme(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsIME, IVsIME>();
        }

        // spelling as intended
        [SuppressMessage("Microsoft.Naming", "CA1704")]
        public static IVsIntelliMouseHandler GetIntelliMouseHandler(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsIntelliMouseHandler, IVsIntelliMouseHandler>();
        }

        public static IVsIntellisenseEngine GetIntellisenseEngine(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsIntellisenseEngine, IVsIntellisenseEngine>();
        }

        public static IVsIntellisenseProjectHost GetIntellisenseProjectHost(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsIntellisenseProjectHost, IVsIntellisenseProjectHost>();
        }

        public static IVsIntellisenseProjectManager GetIntellisenseProjectManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsIntellisenseProjectManager, IVsIntellisenseProjectManager>();
        }

        public static IVsInvisibleEditorManager GetInvisibleEditorManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsInvisibleEditorManager, IVsInvisibleEditorManager>();
        }

        public static IVsLaunchPad GetLaunchPad(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsLaunchPad, IVsLaunchPad>();
        }

        public static IVsLaunchPadFactory GetLaunchPadFactory(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsLaunchPadFactory, IVsLaunchPadFactory>();
        }

        public static IVSMDTypeResolutionService GetMDTypeResolutionService(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVSMDTypeResolutionService, IVSMDTypeResolutionService>();
        }

        public static IVsMenuEditor GetMenuEditor(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsMenuEditor, IVsMenuEditor>();
        }

        public static IVsMonitorUserContext GetMonitorUserContext(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsMonitorUserContext, IVsMonitorUserContext>();
        }

        public static IVsMonitorSelection GetMonitorSelection(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<IVsMonitorSelection, IVsMonitorSelection>();
        }

        public static IVsNavigationTool GetObjectBrowser(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsObjBrowser, IVsNavigationTool>();
        }

        public static IOleComponentManager GetOleComponentManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SOleComponentManager, IOleComponentManager>();
        }

        public static IVsObjectManager GetObjectManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsObjectManager, IVsObjectManager>();
        }

        public static IVsObjectSearch GetObjectSearch(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsObjectSearch, IVsObjectSearch>();
        }

        public static IVsObjectSearchPane GetObjectSearchPane(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetObjectSearch() as IVsObjectSearchPane;
        }

        public static IOleServiceProvider GetOleServiceProvider(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<IOleServiceProvider, IOleServiceProvider>();
        }

        // We don't mean 'Projector'
        [SuppressMessage("Microsoft.Naming", "CA1702")]
        public static IVsOpenProjectOrSolutionDlg GetOpenProjectOrSolutionDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsOpenProjectOrSolutionDlg, IVsOpenProjectOrSolutionDlg>();
        }

        public static IVsOutputWindow GetOutputWindow(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsOutputWindow, IVsOutputWindow>();
        }

        public static IVsParseCommandLine GetParseCommandLine(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsParseCommandLine, IVsParseCommandLine>();
        }

        public static IVsPathVariableResolver GetPathVariableResolver(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsPathVariableResolver, IVsPathVariableResolver>();
        }

        public static IVsPreviewChangesService GetPreviewChangesService(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsPreviewChangesService, IVsPreviewChangesService>();
        }

        public static IVsProfileDataManager GetProfileDataManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsProfileDataManager, IVsProfileDataManager>();
        }

        public static IVsProfilesManagerUI GetProfilesManagerUI(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsProfilesManagerUI, IVsProfilesManagerUI>();
        }

        public static IVsPropertyPageFrame GetPropertyPageFrame(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsPropertyPageFrame, IVsPropertyPageFrame>();
        }

        public static IVsQueryEditQuerySave2 GetQueryEditQuerySave(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsQueryEditQuerySave, IVsQueryEditQuerySave2>();
        }

        public static IVsRegisterProjectDebugTargetProvider GetRegisterProjectDebugTargetProvider(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRegisterDebugTargetProvider, IVsRegisterProjectDebugTargetProvider>();
        }

        public static IVsRegisterEditors GetRegisterEditors(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRegisterEditors, IVsRegisterEditors>();
        }

        public static IVsRegisterNewDialogFilters GetRegisterNewDialogFilters(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRegisterNewDialogFilters, IVsRegisterNewDialogFilters>();
        }

        public static IVsRegisterPriorityCommandTarget GetRegisterPriorityCommandTarget(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRegisterPriorityCommandTarget, IVsRegisterPriorityCommandTarget>();
        }

        public static IVsRegisterProjectTypes GetRegisterProjectTypes(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRegisterProjectTypes, IVsRegisterProjectTypes>();
        }

        public static IVsResourceManager GetResourceManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsResourceManager, IVsResourceManager>();
        }

        public static IVsResourceView GetResourceView(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsResourceView, IVsResourceView>();
        }

        public static IVsRunningDocumentTable GetRunningDocumentTable(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsRunningDocumentTable, IVsRunningDocumentTable>();
        }

        public static IVsSccManager2 GetSourceControlManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSccManager, IVsSccManager2>();
        }

        public static IVsSccToolsOptions GetSourceControlToolsOptions(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSccToolsOptions, IVsSccToolsOptions>();
        }

        public static IVsSettingsReader GetSettingsReader(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSettingsReader, IVsSettingsReader>();
        }

        public static IVsShell GetShell(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsShell, IVsShell>();
        }

        public static IVsDebugger2 GetShellDebugger(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsShellDebugger, IVsDebugger2>();
        }

        public static IVsMonitorSelection GetShellMonitorSelection(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsShellMonitorSelection, IVsMonitorSelection>();
        }

        public static IVsSmartOpenScope GetSmartOpenScope(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSmartOpenScope, IVsSmartOpenScope>();
        }

        public static IVsSolution GetSolution(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSolution, IVsSolution>();
        }

        public static IVsSolutionBuildManager GetSolutionBuildManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSolutionBuildManager, IVsSolutionBuildManager>();
        }

        [Obsolete("Use VSServices.Solution instead.")]
        public static IVsSolution GetSolutionObject(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSolutionObject, IVsSolution>();
        }

        public static IVsSolutionPersistence GetSolutionPersistence(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSolutionPersistence, IVsSolutionPersistence>();
        }

        public static IVsSQLCLRReferences GetSqlClrReferences(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSQLCLRReferences, IVsSQLCLRReferences>();
        }

        public static IVsStartPageDownload GetStartPageDownload(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsStartPageDownload, IVsStartPageDownload>();
        }

        public static IVsStatusbar GetStatusBar(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsStatusbar, IVsStatusbar>();
        }

        public static IVsStrongNameKeys GetStrongNameKeys(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsStrongNameKeys, IVsStrongNameKeys>();
        }

        public static IVsStructuredFileIO GetStructuredFileIO(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsStructuredFileIO, IVsStructuredFileIO>();
        }

        public static IVsSymbolicNavigationManager GetSymbolicNavigationManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsSymbolicNavigationManager, IVsSymbolicNavigationManager>();
        }

        public static IVsTargetFrameworkAssemblies GetTargetFrameworkAssemblies(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTargetFrameworkAssemblies, IVsTargetFrameworkAssemblies>();
        }

        public static IVsTaskList GetTaskList(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTaskList, IVsTaskList>();
        }

        public static IVsTextOut GetTextOut(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTextOut, IVsTextOut>();
        }

        public static IVsTextManager GetTextManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<VsTextManagerClass, IVsTextManager>();
        }

        public static IVsTextManager2 GetTextManager2(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");


            return serviceProvider.GetService<VsTextManagerClass, IVsTextManager2>();
        }

        public static IVsThreadedWaitDialog GetThreadedWaitDialog(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsThreadedWaitDialog, IVsThreadedWaitDialog>();
        }

        public static IVsThreadPool GetThreadPool(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsThreadPool, IVsThreadPool>();
        }

        public static IVsToolbox GetToolbox(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsToolbox, IVsToolbox>();
        }

        public static IVsToolboxDataProvider GetToolboxActiveXDataProvider(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsToolboxActiveXDataProvider, IVsToolboxDataProvider>();
        }

        public static IVsToolboxDataProviderRegistry GetToolboxDataProviderRegistry(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsToolboxDataProviderRegistry, IVsToolboxDataProviderRegistry>();
        }

        public static IVsToolsOptions GetToolsOptions(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsToolsOptions, IVsToolsOptions>();
        }

        public static IVsTrackProjectDocuments2 GetTrackProjectDocuments2(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTrackProjectDocuments, IVsTrackProjectDocuments2>();
        }

        public static IVsTrackProjectDocuments3 GetTrackProjectDocuments3(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTrackProjectDocuments, IVsTrackProjectDocuments3>();
        }

        public static IVsTrackSelectionEx GetTrackSelection(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsTrackSelectionEx, IVsTrackSelectionEx>();
        }

        public static IVsUIHierWinClipboardHelper GetUIHierarchyWindowClipboardHelper(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsUIHierWinClipboardHelper, IVsUIHierWinClipboardHelper>();
        }

        public static IVsUIShell GetUIShell(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsUIShell, IVsUIShell>();
        }

        public static IVsUIShellDocumentWindowMgr GetUIShellDocumentWindowManager(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsUIShellDocumentWindowMgr, IVsUIShellDocumentWindowMgr>();
        }

        public static IVsUIShellOpenDocument GetUIShellOpenDocument(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsUIShellOpenDocument, IVsUIShellOpenDocument>();
        }

        public static IVsUpgradeLogger GetUpgradeLogger(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsUpgradeLogger, IVsUpgradeLogger>();
        }

        public static IVsWebBrowsingService GetWebBrowsingService(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsWebBrowsingService, IVsWebBrowsingService>();
        }

        public static IVsWebFavorites GetWebFavorites(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsWebFavorites, IVsWebFavorites>();
        }

        public static IVsWebPreview GetWebPreview(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsWebPreview, IVsWebPreview>();
        }

        public static IVsWebProxy GetWebProxy(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsWebProxy, IVsWebProxy>();
        }

        public static IVsWebURLMRU GetWebUrlMru(this SVsServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null, "serviceProvider");
            return serviceProvider.GetService<SVsWebURLMRU, IVsWebURLMRU>();
        }
    }
}
