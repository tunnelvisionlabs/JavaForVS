namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;

    [ComVisible(true)]
    public class JavaFolderNodeProperties : FolderNodeProperties
    {
        public JavaFolderNodeProperties(JavaFolderNode node)
            : base(node)
        {
            Contract.Requires(node != null);
        }

        [Category("Advanced")]
        [DisplayName("Build Action")]
        [DefaultValue(FolderBuildAction.Folder)]
        public virtual FolderBuildAction BuildAction
        {
            get
            {
                string value = this.Node.ItemNode.ItemName;
                if (string.IsNullOrEmpty(value))
                    return FolderBuildAction.Folder;

                FolderBuildAction result;
                if (!Enum.TryParse(value, out result))
                    result = FolderBuildAction.Folder;

                return result;
            }

            set
            {
                JavaProjectNode projectNode = (JavaProjectNode)this.Node.ProjectManager;
                projectNode.UpdateFolderBuildAction((JavaFolderNode)this.Node, value);
            }
        }
    }
}
