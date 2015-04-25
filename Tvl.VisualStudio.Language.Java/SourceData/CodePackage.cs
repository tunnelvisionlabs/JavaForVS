namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class CodePackage : CodeElement
    {
        private readonly IntelliSenseCache _cache;

        public CodePackage(IntelliSenseCache cache, string name, string fullName, CodeElement parent)
            : base(name, fullName, CodeLocation.Abstract, parent)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(fullName));
            Contract.Requires(parent != null);

            _cache = cache;
        }

        public override IEnumerable<CodeElement> Children
        {
            get
            {
                // child packages
                string prefix = this.FullName + ".";
                IEnumerable<string> descendantPackageNames = _cache.GetPackageNames(true).Where(i => i.StartsWith(prefix));
                HashSet<string> childPackageNames = new HashSet<string>();
                foreach (var packageName in descendantPackageNames)
                {
                    string name = packageName.Substring(prefix.Length);
                    int firstDot = name.IndexOf('.');
                    if (firstDot >= 0)
                        name = name.Substring(0, firstDot);

                    childPackageNames.Add(name);
                }

                IEnumerable<CodeElement> childPackages = childPackageNames.Select(name => new CodePackage(_cache, name, prefix + name, this));

                // child types
                IEnumerable<CodeElement> types = _cache.GetPackageFiles(this.FullName, true).SelectMany(i => i.Children.OfType<CodeType>());

                return childPackages.Concat(types);
            }
        }

        public override void AugmentQuickInfoSession(IList<object> content)
        {
            content.Add("package " + FullName);
        }
    }
}
