namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public abstract class CodeMemberBuilder : CodeElementBuilder
    {
        public CodeMemberBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }
    }
}
