#pragma warning disable 169 // The field 'fieldname' is never used

namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using Colors = System.Windows.Media.Colors;

    public static class JavaSymbolTaggerClassificationTypeNames
    {
        public const string Definition = "java.symboldefinition";
        public const string Reference = "java.symbolreference";
        public const string UnknownIdentifier = "java.unknownidentifier";

        [Export]
        [Name(JavaSymbolTaggerClassificationTypeNames.Definition)]
        private static readonly ClassificationTypeDefinition DefinitionClassification;

        [Export]
        [Name(JavaSymbolTaggerClassificationTypeNames.Reference)]
        private static readonly ClassificationTypeDefinition ReferenceClassification;

        [Export]
        [Name(JavaSymbolTaggerClassificationTypeNames.UnknownIdentifier)]
        private static readonly ClassificationTypeDefinition UnknownIdentifierClassification;

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaSymbolTaggerClassificationTypeNames.Definition + ".format")]
        [DisplayName("Java Symbol Tagger (definition)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaSymbolTaggerClassificationTypeNames.Definition)]
        [Order]
        internal class DefinitionTagFormatDefinition : ClassificationFormatDefinition
        {
            public DefinitionTagFormatDefinition()
            {
                this.BackgroundColor = Colors.LightBlue;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaSymbolTaggerClassificationTypeNames.Reference + ".format")]
        [DisplayName("Java Symbol Tagger (reference)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaSymbolTaggerClassificationTypeNames.Reference)]
        [Order]
        internal class ReferenceTagFormatDefinition : ClassificationFormatDefinition
        {
            public ReferenceTagFormatDefinition()
            {
                this.BackgroundColor = Colors.LightGreen;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaSymbolTaggerClassificationTypeNames.UnknownIdentifier + ".format")]
        [DisplayName("Java Symbol Tagger (unknown)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaSymbolTaggerClassificationTypeNames.UnknownIdentifier)]
        [Order]
        internal class UnknownIdentifierTagFormatDefinition : ClassificationFormatDefinition
        {
            public UnknownIdentifierTagFormatDefinition()
            {
                this.BackgroundColor = Colors.LightGray;
            }
        }
    }
}
