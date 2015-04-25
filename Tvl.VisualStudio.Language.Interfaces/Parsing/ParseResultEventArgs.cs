namespace Tvl.VisualStudio.Language.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.VisualStudio.Text;

    public class ParseResultEventArgs : EventArgs
    {
        public ParseResultEventArgs(ITextSnapshot snapshot)
            : this(snapshot, new ParseErrorEventArgs[0])
        {
        }

        public ParseResultEventArgs(ITextSnapshot snapshot, IList<ParseErrorEventArgs> errors)
        {
            this.Snapshot = snapshot;
            this.Errors = new ReadOnlyCollection<ParseErrorEventArgs>(errors);
        }

        public ParseResultEventArgs(ITextSnapshot snapshot, IList<ParseErrorEventArgs> errors, TimeSpan elapsedTime)
        {
            this.Snapshot = snapshot;
            this.Errors = new ReadOnlyCollection<ParseErrorEventArgs>(errors);
            this.ElapsedTime = elapsedTime;
        }

        public ITextSnapshot Snapshot
        {
            get;
            private set;
        }

        public ReadOnlyCollection<ParseErrorEventArgs> Errors
        {
            get;
            private set;
        }

        public TimeSpan? ElapsedTime
        {
            get;
            private set;
        }
    }
}
