namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeEnumBuilder : CodeClassBuilder
    {
        public CodeEnumBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
            Contract.Requires(parent != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
        }
    }
}
