namespace Tvl.VisualStudio.Language.Java.OptionsPages
{
    using System;
    using System.Windows.Forms;

    public partial class JavaIntellisenseOptionsControl : UserControl
    {
        public JavaIntellisenseOptionsControl( JavaIntellisenseOptions optionsPage )
        {
            InitializeComponent();

            OptionsPage = optionsPage;
            ReloadOptions();
        }

        private JavaIntellisenseOptions OptionsPage
        {
            get;
            set;
        }

        public void ReloadOptions()
        {
            chkShowCompletionAfterTypedChar.Checked = OptionsPage.ShowCompletionAfterTypedChar;
            chkKeywordsInCompletion.Enabled = !OptionsPage.ShowCompletionAfterTypedChar;
            chkKeywordsInCompletion.Checked = OptionsPage.KeywordsInCompletionLists || OptionsPage.ShowCompletionAfterTypedChar;
            chkCodeSnippetsInCompletion.Enabled = !OptionsPage.ShowCompletionAfterTypedChar;
            chkCodeSnippetsInCompletion.Checked = OptionsPage.CodeSnippetsInCompletionLists || OptionsPage.ShowCompletionAfterTypedChar;

            txtCompletionChars.Text = OptionsPage.CommitCharacters;
            chkCommitOnSpace.Checked = OptionsPage.CommitOnSpace;
            chkNewLineAfterEnter.Checked = OptionsPage.NewLineAfterEnterCompletion;

            chkRecentCompletions.Checked = OptionsPage.RecentCompletions;

            chkSemanticSymbolHighlighting.Checked = OptionsPage.SemanticSymbolHighlighting;
            chkParseJreSource.Checked = OptionsPage.ParseJreSource;
            txtJreSourcePath.Text = OptionsPage.JreSourcePath;
            txtJreSourcePath.Enabled = chkParseJreSource.Checked;
        }

        public void ApplyChanges()
        {
            OptionsPage.ShowCompletionAfterTypedChar = chkShowCompletionAfterTypedChar.Checked;
            if ( !OptionsPage.ShowCompletionAfterTypedChar )
            {
                OptionsPage.KeywordsInCompletionLists = chkKeywordsInCompletion.Checked;
                OptionsPage.CodeSnippetsInCompletionLists = chkCodeSnippetsInCompletion.Checked;
            }

            OptionsPage.CommitCharacters = txtCompletionChars.Text;
            OptionsPage.CommitOnSpace = chkCommitOnSpace.Checked;
            OptionsPage.NewLineAfterEnterCompletion = chkNewLineAfterEnter.Checked;

            OptionsPage.RecentCompletions = chkRecentCompletions.Checked;

            OptionsPage.SemanticSymbolHighlighting = chkSemanticSymbolHighlighting.Checked;
            OptionsPage.ParseJreSource = chkParseJreSource.Checked;
            OptionsPage.JreSourcePath = txtJreSourcePath.Text;
        }

        private void chkShowCompletionAfterTypedChar_CheckedChanged( object sender, EventArgs e )
        {
            chkKeywordsInCompletion.Enabled = !chkShowCompletionAfterTypedChar.Checked;
            chkCodeSnippetsInCompletion.Enabled = !chkShowCompletionAfterTypedChar.Checked;
        }

        private void chkParseJreSource_CheckedChanged(object sender, EventArgs e)
        {
            txtJreSourcePath.Enabled = chkParseJreSource.Checked;
        }
    }
}
