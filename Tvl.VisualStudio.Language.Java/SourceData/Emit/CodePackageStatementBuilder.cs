namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodePackageStatementBuilder : CodeElementBuilder
    {
        public CodePackageStatementBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }

        protected override string GetFullName(CodeElement parent)
        {
            return Name;
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            return new CodePackageStatement(name, fullName, location, parent);
        }
    }
}
