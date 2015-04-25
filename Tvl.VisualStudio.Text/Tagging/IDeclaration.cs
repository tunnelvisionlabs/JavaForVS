namespace Tvl.VisualStudio.Text.Tagging
{
    using System.Windows.Media;

    public interface IDeclaration
    {
        ImageSource Glyph
        {
            get;
        }

        string Name
        {
            get;
        }

        string ShortSignature
        {
            get;
        }

        string LongSignature
        {
            get;
        }
    }
}
