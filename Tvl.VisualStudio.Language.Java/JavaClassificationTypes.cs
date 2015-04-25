#pragma warning disable 169 // The field 'fieldname' is never used

namespace Tvl.VisualStudio.Language.Java
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Language.StandardClassification;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using Colors = System.Windows.Media.Colors;

    public static class JavaClassificationTypes
    {
        [Export]
        [BaseDefinition(PredefinedClassificationTypeNames.Comment)]
        [Name(JavaClassificationTypeNames.DocCommentText)]
        private static readonly ClassificationTypeDefinition DocCommentText;

        [Export]
        [BaseDefinition(JavaClassificationTypeNames.DocCommentText)]
        [Name(JavaClassificationTypeNames.DocCommentTag)]
        private static readonly ClassificationTypeDefinition DocCommentTag;

        [Export]
        [BaseDefinition(JavaClassificationTypeNames.DocCommentTag)]
        [Name(JavaClassificationTypeNames.DocCommentInvalidTag)]
        private static readonly ClassificationTypeDefinition DocCommentInvalidTag;

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaClassificationTypeNames.DocCommentText + ".format")]
        [DisplayName("Java Doc Comment (text)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaClassificationTypeNames.DocCommentText)]
        [Order]
        internal class DocCommentTextFormatDefinition : ClassificationFormatDefinition
        {
            public DocCommentTextFormatDefinition()
            {
                this.ForegroundColor = Colors.Teal;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaClassificationTypeNames.DocCommentTag + ".format")]
        [DisplayName("Java Doc Comment (tag)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaClassificationTypeNames.DocCommentTag)]
        [Order]
        internal class DocCommentTagFormatDefinition : ClassificationFormatDefinition
        {
            public DocCommentTagFormatDefinition()
            {
                this.ForegroundColor = Colors.DarkGray;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [Name(JavaClassificationTypeNames.DocCommentInvalidTag + ".format")]
        [DisplayName("Java Doc Comment (invalid tag)")]
        [UserVisible(false)]
        [ClassificationType(ClassificationTypeNames = JavaClassificationTypeNames.DocCommentInvalidTag)]
        [Order]
        internal class DocCommentInvalidTagFormatDefinition : ClassificationFormatDefinition
        {
            public DocCommentInvalidTagFormatDefinition()
            {
                this.ForegroundColor = Colors.Maroon;
                this.IsBold = true;
            }
        }

    }
}
