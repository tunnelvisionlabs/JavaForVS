namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;
    using Tvl.VisualStudio.Shell;

    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    [Guid(JavaProjectConstants.JavaProjectFactoryGuidString)]
    public class JavaProjectFactory : ProjectFactory
    {
        internal JavaProjectFactory(JavaProjectPackage package)
            : base(package)
        {
        }

        public new JavaProjectPackage Package
        {
            get
            {
                return (JavaProjectPackage)base.Package;
            }
        }

        protected override ProjectNode CreateProject()
        {
            JavaProjectNode node = new JavaProjectNode(Package);
            IOleServiceProvider serviceProvider = base.Package.GetService<IOleServiceProvider>();
            node.SetSite(serviceProvider);
            return node;
        }
    }
}
