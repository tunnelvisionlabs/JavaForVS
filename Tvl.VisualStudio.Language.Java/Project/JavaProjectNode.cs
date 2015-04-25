namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.VisualStudio.Project;

    using __VSHPROPID = Microsoft.VisualStudio.Shell.Interop.__VSHPROPID;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;
    using ComponentSelectorGuids80 = Microsoft.VisualStudio.Shell.Interop.ComponentSelectorGuids80;
    using CultureInfo = System.Globalization.CultureInfo;
    using MSBuild = Microsoft.Build.Evaluation;
    using OAVSProject = Microsoft.VisualStudio.Project.Automation.OAVSProject;
    using Path = System.IO.Path;
    using PrjKind = VSLangProj.PrjKind;
    using StackTrace = System.Diagnostics.StackTrace;
    using VSCOMPONENTSELECTORTABINIT = Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORTABINIT;
    using VSConstants = Microsoft.VisualStudio.VSConstants;

    [ComVisible(true)]
    public class JavaProjectNode : ProjectNode
    {
        private static readonly char[] charsToEscape = new char[] { '%', '*', '?', '@', '$', '(', ')', ';', '\'' };

        private readonly JavaBuildOptions _sharedBuildOptions;
        private VSLangProj.VSProject _vsProject;
        private ImageHandler _extendedImageHandler;

        public JavaProjectNode(ProjectPackage package)
            : base(package)
        {
            _sharedBuildOptions = new JavaBuildOptions();
            CanProjectDeleteItems = true;
            OleServiceProvider.AddService(typeof(VSLangProj.VSProject), HandleCreateService, false);

            AddCatIdMapping(typeof(JavaFileNodeProperties), typeof(FileNodeProperties).GUID);
        }

        public JavaBuildOptions SharedBuildOptions
        {
            get
            {
                return _sharedBuildOptions;
            }
        }

        public override int ImageIndex
        {
            get
            {
                return HierarchyNode.NoImage;
            }
        }

        public override Guid ProjectGuid
        {
            get
            {
                StackTrace trace = new StackTrace();
                if (trace.GetFrames().Any(i => i.GetMethod().Name == "IsSupported" && i.GetMethod().DeclaringType.Name == "VsUtility" && i.GetMethod().DeclaringType.Namespace == "NuGet.VisualStudio"))
                {
                    // NuGet only operates with a select group of project kinds. This hook tricks it into working with Java projects.
                    return new Guid(PrjKind.prjKindCSharpProject);
                }

                return typeof(JavaProjectFactory).GUID;
            }
        }

        public override string ProjectType
        {
            get
            {
                return Constants.JavaLanguageName;
            }
        }

        protected override bool SupportsProjectDesigner
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets an ImageHandler for the project node.
        /// </summary>
        public ImageHandler ExtendedImageHandler
        {
            get
            {
                if (_extendedImageHandler == null)
                {
                    _extendedImageHandler = new ImageHandler(typeof(JavaProjectNode).Assembly.GetManifestResourceStream("Tvl.VisualStudio.Language.Java.Project.Resources.imagelis.bmp"));
                }

                return _extendedImageHandler;
            }
        }

        protected internal VSLangProj.VSProject VSProject
        {
            get
            {
                if (_vsProject == null)
                    _vsProject = new OAVSProject(this);

                return _vsProject;
            }
        }

        internal void UpdateFolderBuildAction(JavaFolderNode folderNode, FolderBuildAction buildAction)
        {
            string existingBuildAction = folderNode.ItemNode.ItemName;
            if (string.IsNullOrEmpty(existingBuildAction))
                existingBuildAction = FolderBuildAction.Folder.ToString();

            if (string.Equals(existingBuildAction, buildAction.ToString()))
                return;

            if (buildAction == FolderBuildAction.Folder && !folderNode.ItemNode.IsVirtual && folderNode.ItemNode.Item.DirectMetadataCount == 0)
            {
                // remove <Folder /> elements from the project as long as they don't have any direct metadata (xml child elements)
                ProjectElement updatedElement = new ProjectElement(this, null, true);
                updatedElement.Rename(folderNode.ItemNode.Item.EvaluatedInclude);
                updatedElement.SetMetadata(ProjectFileConstants.Name, folderNode.ItemNode.Item.EvaluatedInclude);

                ProjectElement oldElement = folderNode.ItemNode;
                folderNode.ItemNode = updatedElement;

                oldElement.RemoveFromProjectFile();
            }
            else if (!folderNode.ItemNode.IsVirtual)
            {
                folderNode.ItemNode.ItemName = buildAction.ToString();
            }
            else
            {
                ProjectElement updatedElement = AddFolderToMSBuild(folderNode.VirtualNodeName, buildAction.ToString());
                folderNode.ItemNode = updatedElement;
            }

            folderNode.Redraw(UIHierarchyElements.Icon);
        }

        protected override ProjectElement AddFolderToMSBuild(string folder, string itemType)
        {
            if (itemType == ProjectFileConstants.Folder)
            {
                ProjectElement folderElement = new ProjectElement(this, null, true);
                folderElement.Rename(folder);
                folderElement.SetMetadata(ProjectFileConstants.Name, folder);
                folderElement.SetMetadata(ProjectFileConstants.BuildAction, itemType);
                return folderElement;
            }
            else
            {
                return base.AddFolderToMSBuild(folder, itemType);
            }
        }

        protected override string GetComponentSelectorBrowseFilters()
        {
            return "Java Archive Files (*.jar)\0*.jar\0";
        }

        protected override ReadOnlyCollection<VSCOMPONENTSELECTORTABINIT> GetComponentSelectorTabList()
        {
            // no .NET or COM assemblies
            return new List<VSCOMPONENTSELECTORTABINIT>()
                {
                    //new VSCOMPONENTSELECTORTABINIT {
                    //    guidTab = VSConstants.GUID_COMPlusPage,
                    //    varTabInitInfo = GetComponentPickerDirectories(),
                    //},
                    //new VSCOMPONENTSELECTORTABINIT {
                    //    guidTab = VSConstants.GUID_COMClassicPage,
                    //},
                    new VSCOMPONENTSELECTORTABINIT {
                        // Tell the Add Reference dialog to call hierarchies GetProperty with the following
                        // propID to enablefiltering out ourself from the Project to Project reference
                        varTabInitInfo = (int)__VSHPROPID.VSHPROPID_ShowProjInSolutionPage,
                        guidTab = VSConstants.GUID_SolutionPage,
                    },
                    // Add the Browse for file page            
                    new VSCOMPONENTSELECTORTABINIT {
                        varTabInitInfo = 0,
                        guidTab = VSConstants.GUID_BrowseFilePage,
                    },
                    // Add the Maven packages page
                    new VSCOMPONENTSELECTORTABINIT {
                        varTabInitInfo = 0,
                        guidTab = JavaProjectConstants.MavenComponentSelectorGuid,
                    },
                    new VSCOMPONENTSELECTORTABINIT {
                        guidTab = new Guid(ComponentSelectorGuids80.MRUPage),
                    },
                }.AsReadOnly();
        }

        private static string Escape(string unescapedString)
        {
            if (unescapedString == null)
                throw new ArgumentNullException("unescapedString", "Null strings not allowed.");

            if (!ContainsReservedCharacters(unescapedString))
                return unescapedString;

            StringBuilder builder = new StringBuilder(unescapedString);
            foreach (char ch in charsToEscape)
            {
                int num = Convert.ToInt32(ch);
                string newValue = string.Format(CultureInfo.InvariantCulture, "%{0:x00}", new object[] { num });
                builder.Replace(ch.ToString(CultureInfo.InvariantCulture), newValue);
            }
            return builder.ToString();
        }

        private static bool ContainsReservedCharacters(string unescapedString)
        {
            return unescapedString.IndexOfAny(charsToEscape) != -1;
        }

#if false
        protected override void AddNonMemberFileItems(IList<string> fileList)
        {
            for (int i = fileList.Count - 1; i >= 0; i--)
            {
                if ((new FileInfo(Path.Combine(ProjectFolder, fileList[i])).Attributes & FileAttributes.Hidden) != 0)
                    fileList.RemoveAt(i);
            }

            base.AddNonMemberFileItems(fileList);
        }

        protected override void AddNonMemberFolderItems(IList<string> folderList)
        {
            for (int i = folderList.Count - 1; i >= 0; i--)
            {
                if ((new DirectoryInfo(Path.Combine(ProjectFolder, folderList[i])).Attributes & FileAttributes.Hidden) != 0)
                    folderList.RemoveAt(i);
            }

            base.AddNonMemberFolderItems(folderList);
        }
#endif

        protected override bool FilterItemTypeToBeAddedToHierarchy(string itemType)
        {
            return string.Equals(itemType, JavaProjectFileConstants.SourceFolder, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemType, JavaProjectFileConstants.TestSourceFolder, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemType, JavaProjectFileConstants.JarReference, StringComparison.OrdinalIgnoreCase)
                || string.Equals(itemType, JavaProjectFileConstants.MavenReference, StringComparison.OrdinalIgnoreCase)
                || base.FilterItemTypeToBeAddedToHierarchy(itemType);
        }

        protected override ReferenceContainerNode CreateReferenceContainerNode()
        {
            return new JavaReferenceContainerNode(this);
        }

        public override FileNode CreateFileNode(ProjectElement item)
        {
            return new JavaFileNode(this, item);
        }

        public override FolderNode CreateFolderNode(string path, ProjectElement element)
        {
            return new JavaFolderNode(this, path, element);
        }

        protected override ConfigProvider CreateConfigProvider()
        {
            return new JavaConfigProvider(this);
        }

        protected override bool PerformTargetFrameworkCheck()
        {
            return true;
        }

#if false
        protected override int QueryStatusOnNode(Guid cmdGroup, uint cmd, IntPtr pCmdText, ref QueryStatusResult result)
        {
            if (cmdGroup == VsMenus.guidStandardCommandSet2K)
            {
                if ((VsCommands2K)cmd == VsCommands2K.SHOWALLFILES)
                {
                    result |= QueryStatusResult.NOTSUPPORTED;
                    return VSConstants.S_OK;
                }
            }

            return base.QueryStatusOnNode(cmdGroup, cmd, pCmdText, ref result);
        }
#endif

#if false
        protected override void ProcessFolders()
        {
            base.ProcessFolders();

            this.AddNonMemberItems();
        }
#endif

        public override bool IsCodeFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            string extension = Path.GetExtension(fileName);
            if (extension.Equals(Constants.JavaFileExtension, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        public override bool IsFolderItem(MSBuild.ProjectItem buildItem)
        {
            if (string.Equals(buildItem.ItemType, JavaProjectFileConstants.SourceFolder, StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(buildItem.ItemType, JavaProjectFileConstants.TestSourceFolder, StringComparison.OrdinalIgnoreCase))
                return true;

            return base.IsFolderItem(buildItem);
        }

        public void SetProjectProperty(string propertyName, _PersistStorageType storageType, string propertyValue, string condition)
        {
            SetProjectProperty(propertyName, storageType, propertyValue, condition, false);
        }

        public void SetProjectProperty(string propertyName, _PersistStorageType storageType, string propertyValue, string condition, bool treatPropertyValueAsLiteral)
        {
            if (propertyValue == null)
                propertyValue = string.Empty;

            // see if the value is the same as what's already in the project so we
            // know whether to actually mark the project file dirty or not
            string oldValue = GetProjectProperty(propertyName, storageType, true);

            if (!String.Equals(oldValue, propertyValue, StringComparison.Ordinal))
            {
                if (treatPropertyValueAsLiteral)
                    propertyValue = Escape(propertyValue);

                SetPropertyUnderCondition(propertyName, storageType, condition, propertyValue);
            }
        }

        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            return new Guid[]
                {
                    typeof(PropertyPages.JavaBuildEventsPropertyPage).GUID,
                };
        }

        protected override Guid[] GetConfigurationDependentPropertyPages()
        {
            return new Guid[]
                {
                    typeof(PropertyPages.JavaApplicationPropertyPage).GUID,
                    typeof(PropertyPages.JavaBuildPropertyPage).GUID,
                    typeof(PropertyPages.JavaDebugPropertyPage).GUID,
                };
        }

        internal object HandleCreateService(Type serviceType)
        {
            object service = null;

            if (serviceType == typeof(VSLangProj.VSProject))
            {
                service = this.VSProject;
            }
            else if (serviceType == typeof(EnvDTE.Project))
            {
                service = this.GetAutomationObject();
            }

            return service;
        }

        public enum ExtendedImageName
        {
            SourceFolder = 0,
            OpenSourceFolder = 1,
            TestSourceFolder = 2,
            OpenTestSourceFolder = 3,
        }
    }
}
