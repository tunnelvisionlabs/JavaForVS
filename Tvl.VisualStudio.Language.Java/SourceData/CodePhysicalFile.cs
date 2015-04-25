namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Path = System.IO.Path;

    public class CodePhysicalFile : CodeElement
    {
        internal CodePhysicalFile(string fileName)
            : base(Path.GetFileName(fileName), fileName, new CodeLocation(fileName), null)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
        }

        public string PackageName
        {
            get
            {
                CodePackageStatement packageStatement = Children.OfType<CodePackageStatement>().FirstOrDefault();
                if (packageStatement == null)
                    return string.Empty;

                return packageStatement.FullName;
            }
        }

        public override void AugmentQuickInfoSession(IList<object> content)
        {
            content.Add("file " + FullName);
        }
    }
}
