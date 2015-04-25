namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeInterfaceBuilder : CodeTypeBuilder
    {
        public CodeInterfaceBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
            Contract.Requires(parent != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            CodeInterface element = new CodeInterface(name, fullName, location, parent);
            return element;
        }
    }
}
