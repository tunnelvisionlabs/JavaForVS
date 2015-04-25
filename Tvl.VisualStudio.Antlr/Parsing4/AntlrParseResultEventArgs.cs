namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Antlr4.Runtime;
    using Microsoft.VisualStudio.Text;
    using ParseErrorEventArgs = Tvl.VisualStudio.Language.Parsing.ParseErrorEventArgs;
    using ParseResultEventArgs = Tvl.VisualStudio.Language.Parsing.ParseResultEventArgs;

    public class AntlrParseResultEventArgs : ParseResultEventArgs
    {
        public AntlrParseResultEventArgs(ITextSnapshot snapshot, IList<ParseErrorEventArgs> errors, TimeSpan elapsedTime, IList<IToken> tokens, ParserRuleContext result)
            : base(snapshot, errors, elapsedTime)
        {
            Tokens = tokens as ReadOnlyCollection<IToken>;
            if (Tokens == null)
                Tokens = new ReadOnlyCollection<IToken>(tokens ?? new IToken[0]);

            Result = result;
        }

        public ReadOnlyCollection<IToken> Tokens
        {
            get;
            private set;
        }

        public ParserRuleContext Result
        {
            get;
            private set;
        }
    }
}
