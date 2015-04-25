namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using System.Collections.Generic;
    using Antlr.Runtime;
    using Antlr.Runtime.Misc;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Language.Parsing;

    internal class ClassAnchorTracker : AntlrTaggerBase<ClassAnchorTracker.ClassAnchorState, ScopeAnchorTag>
    {
        private static readonly Dictionary<string, int> KeywordTypes =
            new Dictionary<string,int>()
            {
                { "class", JavaColorizerLexer.CLASS },
                { "interface", JavaColorizerLexer.INTERFACE },
                { "enum", JavaColorizerLexer.ENUM },
            };

        private readonly List<ITagSpan<ScopeAnchorTag>> _anchors;


        public ClassAnchorTracker(ITextBuffer textBuffer, JavaBackgroundParser backgroundParser)
            : base(textBuffer, options: ClassifierOptions.ManualUpdate)
        {
            _anchors = new List<ITagSpan<ScopeAnchorTag>>();

            if (backgroundParser != null)
                backgroundParser.ParseComplete += HandleBackgroundParseComplete;
        }

        protected override ClassAnchorState GetStartState()
        {
            return ClassAnchorState.Initial;
        }

        protected override ITokenSourceWithState<ClassAnchorState> CreateLexer(SnapshotSpan span, ClassAnchorState startState)
        {
            SnapshotCharStream input = new SnapshotCharStream(span);
            return new JavaClassifierLexerWrapper<ClassAnchorState>(input, startState);
        }

        private class ScopeDetails
        {
            public int ScopeLevel;
            public ScopeAnchorTag Tag;
            public ScopeAnchorTag OpenScopeTag;
            public ScopeAnchorTag CloseScopeTag;

            public ScopeDetails(int scopeLevel, ScopeAnchorTag scopeTag)
            {
                ScopeLevel = scopeLevel;
                Tag = scopeTag;
            }
        }

        private readonly ListStack<ScopeDetails> _currentScope = new ListStack<ScopeDetails>();
        private int _currentBraceLevel = 0;

        protected override bool TryClassifyToken(IToken token, out ScopeAnchorTag tag)
        {
            tag = null;

            int tokenType = token.Type;
            if (tokenType == JavaColorizerLexer.IDENTIFIER)
            {
                if (!KeywordTypes.TryGetValue(token.Text, out tokenType))
                    tokenType = JavaColorizerLexer.IDENTIFIER;
            }

            switch (tokenType)
            {
            case JavaColorizerLexer.CLASS:
            case JavaColorizerLexer.INTERFACE:
            case JavaColorizerLexer.ENUM:
                {
                    IToken previous = PeekToken(-1, true);
                    if (previous != null && previous.Type == JavaColorizerLexer.DOT)
                        break;

                    IToken next = PeekToken(1, true);
                    if (next == null || next.Type != JavaColorizerLexer.IDENTIFIER)
                        break;

                    string name = next.Text;
                    int id = 0;
                    int type = tokenType;
                    int order = 1;
                    tag = new ScopeAnchorTag(name, id, type, order);

                    _currentScope.Push(new ScopeDetails(_currentBraceLevel, tag));

                    return true;
                }

            case JavaColorizerLexer.LBRACE:
                tag = new ScopeAnchorTag(token.Text, 0, token.Type, 1);
                if (_currentScope.Count > 0 && _currentScope.Peek().OpenScopeTag == null && _currentBraceLevel == _currentScope.Peek().ScopeLevel)
                    _currentScope.Peek().OpenScopeTag = tag;

                _currentBraceLevel++;
                return true;

            case JavaColorizerLexer.RBRACE:
                tag = new ScopeAnchorTag(token.Text, 0, token.Type, 1);
                _currentBraceLevel--;
                if (_currentScope.Count > 0 && _currentScope.Peek().ScopeLevel == _currentBraceLevel)
                {
                    if (_currentScope.Peek().Tag.Type != Java2Parser.FIELD_DECLARATION)
                    {
                        _currentScope.Peek().CloseScopeTag = tag;
                        _currentScope.Pop();
                    }
                }

                return true;

            case JavaColorizerLexer.SEMI:
                // close out any fields on the scope stack
                while (_currentScope.Count > 0 && _currentScope.Peek().Tag.Type == JavaColorizerLexer.FIELD_DECLARATION && _currentScope.Peek().ScopeLevel == _currentBraceLevel)
                {
                    if (tag == null)
                        tag = new ScopeAnchorTag(token.Text, 0, token.Type, 1);

                    _currentScope.Peek().CloseScopeTag = tag;
                    _currentScope.Pop();
                }

                return tag != null;

            case JavaColorizerLexer.IDENTIFIER:
                {
                    IToken next = PeekToken(1, true);
                    if (next == null)
                        break;

                    bool couldBeMethod = next.Type == JavaColorizerLexer.LPAREN;
                    bool couldBeField = /*next.Type == JavaColorizerLexer.LBRACKET ||*/ next.Type == JavaColorizerLexer.EQ || next.Type == JavaColorizerLexer.COMMA || next.Type == JavaColorizerLexer.SEMI;
                    if (!couldBeMethod && !couldBeField)
                        break;

                    ScopeAnchorTag enclosingTag = null;
                    int enclosingAnchoredBraceLevel = -1;
                    int anchoredBraceLevel = 0;
                    if (_currentScope.Count > 0)
                    {
                        enclosingTag = _currentScope.Peek().Tag;
                        enclosingAnchoredBraceLevel = _currentScope.Peek().ScopeLevel;
                        anchoredBraceLevel = _currentBraceLevel;
                    }

                    if (anchoredBraceLevel == enclosingAnchoredBraceLevel + 1 && enclosingTag != null && (enclosingTag.Type == JavaColorizerLexer.CLASS || enclosingTag.Type == JavaColorizerLexer.INTERFACE || enclosingTag.Type == JavaColorizerLexer.ENUM))
                    {
                        if (couldBeField)
                            tag = new ScopeAnchorTag(token.Text, 0, Java2Parser.FIELD_DECLARATION, 1);
                        else if (couldBeMethod)
                            tag = new ScopeAnchorTag(token.Text, 0, Java2Parser.METHOD_IDENTIFIER, 1);

                        _currentScope.Push(new ScopeDetails(_currentBraceLevel, tag));
                        return true;
                    }

                    tag = null;
                    return false;
                }

            default:
                break;
            }

            tag = null;
            return false;
        }

        private void DeriveCurrentScope(IToken token)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void HandleBackgroundParseComplete(object sender, ParseResultEventArgs e)
        {
            List<ITagSpan<ScopeAnchorTag>> anchors = new List<ITagSpan<ScopeAnchorTag>>();
            JavaParseResultEventArgs javaEventArgs = e as JavaParseResultEventArgs;
            if (javaEventArgs != null && javaEventArgs.Errors.Count == 0)
            {
                foreach (var type in javaEventArgs.Types)
                {
                    CommonTree tagTree = null;
                    switch (type.Type)
                    {
                    case Java2Parser.CLASS_TYPE_IDENTIFIER:
                        tagTree = (CommonTree)type.GetFirstChildWithType(Java2Parser.CLASS);
                        break;

                    case Java2Parser.ENUM_TYPE_IDENTIFIER:
                        tagTree = (CommonTree)type.GetFirstChildWithType(Java2Parser.ENUM);
                        break;

                    case Java2Parser.ANNOTATION_TYPE_IDENTIFIER:
                    case Java2Parser.INTERFACE_TYPE_IDENTIFIER:
                        tagTree = (CommonTree)type.GetFirstChildWithType(Java2Parser.INTERFACE);
                        break;

                    default:
                        continue;
                    }

                    IToken tagToken = tagTree.Token;
                    SnapshotSpan span = new SnapshotSpan(e.Snapshot, Span.FromBounds(tagToken.StartIndex, tagToken.StopIndex + 1));

                    string name = type.Text;
                    int id = 0;
                    int order = 0;
                    ScopeAnchorTag tag = new ScopeAnchorTag(name, id, type.Type, order);

                    anchors.Add(new TagSpan<ScopeAnchorTag>(span, tag));

                    // TODO: add } anchors as well
                }
            }

            // TODO: Force reclassification of lines changed since this background parse started

            _anchors.Clear();
            _anchors.AddRange(anchors);
        }

        public struct ClassAnchorState : ITextLineState<ClassAnchorState>, IJavaClassifierLexerState<ClassAnchorState>
        {
            public static readonly ClassAnchorState Initial = new ClassAnchorState(JavaClassifierLexerState.Initial, false, false);

            private readonly JavaClassifierLexerState _lexerState;
            private readonly bool _isDirty;
            private readonly bool _isMultiline;

            public ClassAnchorState(JavaClassifierLexerState lexerState, bool isDirty, bool isMultiline)
            {
                _lexerState = lexerState;
                _isDirty = isDirty;
                _isMultiline = isMultiline;
            }

            public JavaClassifierLexerState LexerState
            {
                get
                {
                    return _lexerState;
                }
            }

            public bool IsDirty
            {
                get
                {
                    return _isDirty;
                }
            }

            public bool IsMultiline
            {
                get
                {
                    return _isMultiline;
                }
            }

            public ClassAnchorState CreateDirtyState()
            {
                return new ClassAnchorState(this.LexerState, true, this.IsMultiline);
            }

            public ClassAnchorState CreateMultilineState()
            {
                return new ClassAnchorState(this.LexerState, this.IsDirty, true);
            }

            public ClassAnchorState CreateFromLexerState(JavaClassifierLexerState lexerState)
            {
                return new ClassAnchorState(lexerState, false, false);
            }
        }
    }
}
