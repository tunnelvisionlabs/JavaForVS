namespace Tvl.VisualStudio.Language.Java.OptionsPages
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using DialogPage = Microsoft.VisualStudio.Shell.DialogPage;
    using IWin32Window = System.Windows.Forms.IWin32Window;
    using Point = System.Drawing.Point;

    [Guid("91E9717F-8BB2-49AF-ABAE-7F7A30CAF34F")]
    public class JavaIntellisenseOptions : DialogPage
    {
        public static readonly string DefaultCommitCharacters = "{}[]()<>.,:;+-*/%&|^!~=?@#'\"\\";

        public JavaIntellisenseOptions()
        {
            // initialize to default values
            ShowCompletionAfterTypedChar = false;
            KeywordsInCompletionLists = true;
            CodeSnippetsInCompletionLists = true;

            CommitCharacters = DefaultCommitCharacters;
            CommitOnSpace = true;
            NewLineAfterEnterCompletion = false;

            RecentCompletions = true;

            SemanticSymbolHighlighting = false;
            ParseJreSource = false;
            JreSourcePath = string.Empty;
        }

        private JavaIntellisenseOptionsControl OptionsControl
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                OptionsControl = new JavaIntellisenseOptionsControl(this);
                OptionsControl.Location = new Point(0, 0);
                return OptionsControl;
            }
        }

        [DefaultValue(false)]
        public bool ShowCompletionAfterTypedChar
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool KeywordsInCompletionLists
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool CodeSnippetsInCompletionLists
        {
            get;
            set;
        }

        [DefaultValue("{}[]().,:;+-*/%&|^!~=<>?@#'\"\\")]
        public string CommitCharacters
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool CommitOnSpace
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool NewLineAfterEnterCompletion
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool RecentCompletions
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool SemanticSymbolHighlighting
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool ParseJreSource
        {
            get;
            set;
        }

        [DefaultValue("")]
        public string JreSourcePath
        {
            get;
            set;
        }

        public override void SaveSettingsToStorage()
        {
            if (OptionsControl != null)
                OptionsControl.ApplyChanges();

            base.SaveSettingsToStorage();
        }
    }
}
