namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeImportBuilder : CodeElementBuilder
    {
        public CodeImportBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }

        public bool IsStatic
        {
            get;
            set;
        }

        public bool IsWildcard
        {
            get;
            set;
        }

        protected override string GetFullName(CodeElement parent)
        {
            return Name;
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            CodeImportStatement import = new CodeImportStatement(name, fullName, location, parent);
            return import;
        }
    }
}
