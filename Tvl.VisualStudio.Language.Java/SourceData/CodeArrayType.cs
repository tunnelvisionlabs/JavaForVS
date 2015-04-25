namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class CodeArrayType : CodeType
    {
        public CodeArrayType(CodeType elementType)
            : base(elementType.Name, string.Format("{0}[]", elementType.FullName), elementType.Location, elementType.Parent)
        {
            Contract.Requires(elementType != null);
        }

        public override void AugmentQuickInfoSession(IList<object> content)
        {
        }
    }
}
