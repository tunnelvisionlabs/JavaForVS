namespace Tvl.VisualStudio.Language.Parsing
{
    using System;
    using Microsoft.VisualStudio.Text;

    public class ParseErrorEventArgs : EventArgs
    {
        public ParseErrorEventArgs(string message, Span span)
        {
            this.Message = message;
            this.Span = span;
        }

        public string Message
        {
            get;
            private set;
        }

        public Span Span
        {
            get;
            private set;
        }
    }
}
