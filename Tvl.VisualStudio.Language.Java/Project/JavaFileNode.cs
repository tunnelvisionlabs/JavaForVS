namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;
    using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;
    using vsCommandStatus = EnvDTE.vsCommandStatus;
    using VSConstants = Microsoft.VisualStudio.VSConstants;
    using VsMenus = Microsoft.VisualStudio.Shell.VsMenus;

    [ComVisible(true)]
    public class JavaFileNode : FileNode
    {
        public JavaFileNode(ProjectNode root, ProjectElement element)
            : base(root, element)
        {
        }

        protected override NodeProperties CreatePropertiesObject()
        {
            return new JavaFileNodeProperties(this);
        }

        protected override int QueryStatusOnNode(Guid cmdGroup, uint cmd, IntPtr pCmdText, ref vsCommandStatus result)
        {
            if (cmdGroup == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VsCommands2K)cmd)
                {
                case VsCommands2K.INCLUDEINPROJECT:
                case VsCommands2K.EXCLUDEFROMPROJECT:
                    result = vsCommandStatus.vsCommandStatusUnsupported;
                    return VSConstants.S_OK;

                default:
                    break;
                }
            }

            return base.QueryStatusOnNode(cmdGroup, cmd, pCmdText, ref result);
        }
    }
}
