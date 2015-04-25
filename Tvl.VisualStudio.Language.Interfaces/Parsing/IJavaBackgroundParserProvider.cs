namespace Tvl.VisualStudio.Language.Parsing
{
    using Microsoft.VisualStudio.Text;

    public interface IJavaBackgroundParserProvider
    {
        IBackgroundParser CreateParser(ITextBuffer buffer);
    }
}
