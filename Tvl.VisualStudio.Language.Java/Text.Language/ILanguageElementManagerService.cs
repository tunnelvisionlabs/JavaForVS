namespace Tvl.VisualStudio.Language.Java.Text.Language
{
    using Microsoft.VisualStudio.Text.Editor;

    public interface ILanguageElementManagerService
    {
        ILanguageElementManager GetLanguageElementManager(ITextView textView);
    }
}
