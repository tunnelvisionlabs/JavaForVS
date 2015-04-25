namespace Tvl.VisualStudio.Language.Java
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Language.Intellisense;
    using Tvl.VisualStudio.Language.Java.SourceData;

    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("Java IntelliSense Controller")]
    [ContentType(Constants.JavaContentType)]
    [Order]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    public sealed class JavaIntellisenseControllerProvider : IntellisenseControllerProvider
    {
        [Import]
        internal IntelliSenseCache IntelliSenseCache
        {
            get;
            private set;
        }

        protected override IntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            var controller = new JavaIntellisenseController(textView, this);
            textView.Properties[typeof(JavaIntellisenseController)] = controller;
            return controller;
        }
    }
}
