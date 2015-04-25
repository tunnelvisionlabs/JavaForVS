namespace Tvl.VisualStudio.Language.Java.Text.Language
{
    using System;
    using Microsoft.VisualStudio.Text;

    public class LanguageElementsChangedEventArgs : EventArgs
    {
        public LanguageElementsChangedEventArgs(SnapshotSpan affectedSpan)
        {
            AffectedSpan = affectedSpan;
        }

        public SnapshotSpan AffectedSpan
        {
            get;
            private set;
        }
    }
}
