namespace Tvl.VisualStudio.Language.Java.Project.Automation
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project.Automation;

    [ComVisible(true)]
    public class OAJarReference : OAReferenceBase<JarReferenceNode>
    {
        public OAJarReference(JarReferenceNode jarReference)
            : base(jarReference)
        {
        }

        public override string Name
        {
            get
            {
                return BaseReferenceNode.Caption;
            }
        }

        public override string Culture
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
