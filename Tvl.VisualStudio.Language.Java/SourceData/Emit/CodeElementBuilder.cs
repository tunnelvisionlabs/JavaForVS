namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public abstract class CodeElementBuilder
    {
        private readonly CodeElementBuilder _parent;
        private readonly string _name;
        private readonly Interval _span;
        private readonly Interval _seek;

        private readonly List<CodeElementBuilder> _children = new List<CodeElementBuilder>();

        public CodeElementBuilder(string name)
        {
            Contract.Assert(this is CodeFileBuilder);
            _name = name;
        }

        public CodeElementBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
        {
            Contract.Requires(parent != null);
            Contract.Requires(!string.IsNullOrEmpty(name));

            _parent = parent;
            _name = name;
            _span = span;
            _seek = seek;
        }

        public CodeElementBuilder Parent
        {
            get
            {
                return _parent;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Interval Span
        {
            get
            {
                return _span;
            }
        }

        public Interval Seek
        {
            get
            {
                return _seek;
            }
        }

        protected internal List<CodeElementBuilder> Children
        {
            get
            {
                return _children;
            }
        }

        protected virtual string NestedNameSeparator
        {
            get
            {
                return ".";
            }
        }

        protected virtual string GetFullName(CodeElement parent)
        {
            // full name of the enclosing element if any, otherwise the package name
            string nameQualifier = string.Empty;

            /* '+' if this is a nested type, otherwise '.' if the qualifier is a
             * package or this is a member
             */
            string nameSeparator = string.Empty;

            string name = Name;

            // java doesn't use a type arguments suffix (such as "`n" in .NET) so this will stay empty
            string typeArgumentsSuffix = string.Empty;

            CodePhysicalFile file = parent as CodePhysicalFile;
            if (file != null)
            {
                var packageStatement = file.Children.OfType<CodePackageStatement>().FirstOrDefault();
                if (packageStatement != null)
                {
                    nameQualifier = packageStatement.FullName;
                    if (!string.IsNullOrEmpty(nameQualifier))
                        nameSeparator = ".";
                }
            }
            else
            {
                nameQualifier = parent.FullName;
                if (!string.IsNullOrEmpty(nameQualifier))
                    nameSeparator = NestedNameSeparator;
            }

            string fullName = nameQualifier + nameSeparator + name + typeArgumentsSuffix;
            return fullName;
        }

        protected virtual CodeLocation GetLocation(CodeElement parent)
        {
            CodeLocation location = new CodeLocation(parent.Location.FileName, Span, Seek);
            return location;
        }

        internal virtual CodeElement BuildElement(CodeElement parent)
        {
            string fullName = GetFullName(parent);
            CodeLocation location = GetLocation(parent);
            CodeElement element = CreateCodeElement(Name, fullName, location, parent);

            foreach (var child in Children)
            {
                var childElement = child.BuildElement(element);
                element.AddChild(childElement);
            }

            return element;
        }

        protected abstract CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent);
    }
}
