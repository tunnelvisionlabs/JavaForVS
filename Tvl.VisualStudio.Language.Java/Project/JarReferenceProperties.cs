namespace Tvl.VisualStudio.Language.Java.Project
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;

    [ComVisible(true)]
    public class JarReferenceProperties : ReferenceNodeProperties
    {
        public JarReferenceProperties(JarReferenceNode node)
            : base(node)
        {
        }
    }
}
