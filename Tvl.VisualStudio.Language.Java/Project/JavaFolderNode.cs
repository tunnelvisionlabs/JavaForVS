namespace Tvl.VisualStudio.Language.Java.Project
{
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;

    [ComVisible(true)]
    public class JavaFolderNode : FolderNode
    {
        public JavaFolderNode(ProjectNode root, string relativePath, ProjectElement element)
            : base(root, relativePath, element)
        {
            Contract.Requires(root != null);
            Contract.Requires(relativePath != null);
            Contract.Requires(element != null);

            if (element.IsVirtual)
            {
                string buildAction = element.GetMetadata(ProjectFileConstants.BuildAction);
                if (buildAction == ProjectFileConstants.Folder)
                    this.IsNonmemberItem = false;
            }
        }

        public new JavaProjectNode ProjectManager
        {
            get
            {
                Contract.Ensures(Contract.Result<JavaProjectNode>() != null);

                return (JavaProjectNode)base.ProjectManager;
            }
        }

        public override object GetIconHandle(bool open)
        {
            if (this.IsNonmemberItem)
                return base.GetIconHandle(open);

            if (string.Equals(ItemNode.ItemName, JavaProjectFileConstants.SourceFolder))
                return this.ProjectManager.ExtendedImageHandler.GetIconHandle(open ? (int)JavaProjectNode.ExtendedImageName.OpenSourceFolder : (int)JavaProjectNode.ExtendedImageName.SourceFolder);

            if (string.Equals(ItemNode.ItemName, JavaProjectFileConstants.TestSourceFolder))
                return this.ProjectManager.ExtendedImageHandler.GetIconHandle(open ? (int)JavaProjectNode.ExtendedImageName.OpenTestSourceFolder: (int)JavaProjectNode.ExtendedImageName.TestSourceFolder);

            return base.GetIconHandle(open);
        }

        protected override NodeProperties CreatePropertiesObject()
        {
            return new JavaFolderNodeProperties(this);
        }
    }
}
