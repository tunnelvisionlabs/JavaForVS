namespace Tvl.VisualStudio.Text.Tagging
{
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;

    public interface ILanguageElementTag : ITag
    {
        string Name
        {
            get;
        }

        string Category
        {
            get;
        }

        ImageSource Glyph
        {
            get;
        }

        SnapshotSpan Target
        {
            get;
        }
    }
}
