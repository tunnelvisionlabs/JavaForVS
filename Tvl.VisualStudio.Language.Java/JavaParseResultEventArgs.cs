namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Parsing;

    internal class JavaParseResultEventArgs : AntlrParseResultEventArgs
    {
        private readonly ReadOnlyCollection<CommonTree> _types;

        public JavaParseResultEventArgs(ITextSnapshot snapshot, IList<ParseErrorEventArgs> errors, TimeSpan elapsedTime, IList<IToken> tokens, IRuleReturnScope result, IList<CommonTree> types)
            : base(snapshot, errors, elapsedTime, tokens, result)
        {
            _types = types as ReadOnlyCollection<CommonTree>;
            if (_types == null)
                _types = new ReadOnlyCollection<CommonTree>(types ?? new CommonTree[0]);
        }

        public ReadOnlyCollection<CommonTree> Types
        {
            get
            {
                return _types;
            }
        }
    }
}
