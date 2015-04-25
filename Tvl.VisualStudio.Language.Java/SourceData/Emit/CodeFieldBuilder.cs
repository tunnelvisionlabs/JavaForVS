namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeFieldBuilder : CodeMemberBuilder
    {
        public CodeFieldBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            CodeField field = new CodeField(name, fullName, location, parent);
            return field;
        }
    }
}
