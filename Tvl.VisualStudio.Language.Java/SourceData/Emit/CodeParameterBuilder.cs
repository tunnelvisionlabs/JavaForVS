namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeParameterBuilder : CodeElementBuilder
    {
        public CodeParameterBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            CodeParameter parameter = new CodeParameter(name, fullName, location, parent);
            return parameter;
        }
    }
}
