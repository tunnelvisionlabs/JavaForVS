namespace Tvl.VisualStudio.Language.Java
{
    using Antlr4.Runtime;
    using Microsoft.VisualStudio.Language.StandardClassification;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Tvl.VisualStudio.Language.Parsing4;

    internal sealed class JavaClassifier : AntlrClassifierBase<SimpleLexerState>
    {
        private readonly IStandardClassificationService _standardClassificationService;
        private readonly IClassificationTypeRegistryService _classificationTypeRegistryService;

        private readonly IClassificationType _docCommentText;
        private readonly IClassificationType _docCommentTag;
        private readonly IClassificationType _docCommentInvalidTag;

        public JavaClassifier(ITextBuffer textBuffer, IStandardClassificationService standardClassificationService, IClassificationTypeRegistryService classificationTypeRegistryService)
            : base(textBuffer)
        {
            this._standardClassificationService = standardClassificationService;
            this._classificationTypeRegistryService = classificationTypeRegistryService;

            this._docCommentText = this._classificationTypeRegistryService.GetClassificationType(JavaClassificationTypeNames.DocCommentText);
            this._docCommentTag = this._classificationTypeRegistryService.GetClassificationType(JavaClassificationTypeNames.DocCommentTag);
            this._docCommentInvalidTag = this._classificationTypeRegistryService.GetClassificationType(JavaClassificationTypeNames.DocCommentInvalidTag);
        }

        protected override SimpleLexerState GetStartState()
        {
            return SimpleLexerState.Initial;
        }

        protected override ITokenSourceWithState<SimpleLexerState> CreateLexer(ICharStream input, int startLine, SimpleLexerState state)
        {
            var lexer = new JavaColorizerLexer2(new JavaUnicodeStreamV4(input));
            lexer.Line = startLine;
            lexer.Column = 0;
            state.Apply(lexer);
            return lexer;
        }

        protected override IClassificationType ClassifyToken(IToken token)
        {
            switch (token.Type)
            {
#if false // operator coloring was just annoying
            case JavaColorizerLexer2.EQ:
            case JavaColorizerLexer2.NEQ:
            case JavaColorizerLexer2.EQEQ:
            case JavaColorizerLexer2.PLUS:
            case JavaColorizerLexer2.PLUSEQ:
            case JavaColorizerLexer2.MINUS:
            case JavaColorizerLexer2.MINUSEQ:
            case JavaColorizerLexer2.TIMES:
            case JavaColorizerLexer2.TIMESEQ:
            case JavaColorizerLexer2.DIV:
            case JavaColorizerLexer2.DIVEQ:
            case JavaColorizerLexer2.LT:
            case JavaColorizerLexer2.GT:
            case JavaColorizerLexer2.LE:
            case JavaColorizerLexer2.GE:
            case JavaColorizerLexer2.NOT:
            case JavaColorizerLexer2.BITNOT:
            case JavaColorizerLexer2.AND:
            case JavaColorizerLexer2.BITAND:
            case JavaColorizerLexer2.ANDEQ:
            case JavaColorizerLexer2.QUES:
            case JavaColorizerLexer2.OR:
            case JavaColorizerLexer2.BITOR:
            case JavaColorizerLexer2.OREQ:
            case JavaColorizerLexer2.COLON:
            case JavaColorizerLexer2.INC:
            case JavaColorizerLexer2.DEC:
            case JavaColorizerLexer2.XOR:
            case JavaColorizerLexer2.XOREQ:
            case JavaColorizerLexer2.MOD:
            case JavaColorizerLexer2.MODEQ:
            case JavaColorizerLexer2.LSHIFT:
            case JavaColorizerLexer2.RSHIFT:
            case JavaColorizerLexer2.LSHIFTEQ:
            case JavaColorizerLexer2.RSHIFTEQ:
            case JavaColorizerLexer2.ROR:
            case JavaColorizerLexer2.ROREQ:
                return _standardClassificationService.Operator;
#endif

            case JavaColorizerLexer2.CHAR_LITERAL:
                //return _standardClassificationService.CharacterLiteral;
                return _standardClassificationService.StringLiteral;

            case JavaColorizerLexer2.STRING_LITERAL:
                return _standardClassificationService.StringLiteral;

            case JavaColorizerLexer2.NUMBER:
                return _standardClassificationService.NumberLiteral;

            case JavaColorizerLexer2.ABSTRACT:
            case JavaColorizerLexer2.ASSERT:
            case JavaColorizerLexer2.BOOLEAN:
            case JavaColorizerLexer2.BREAK:
            case JavaColorizerLexer2.BYTE:
            case JavaColorizerLexer2.CASE:
            case JavaColorizerLexer2.CATCH:
            case JavaColorizerLexer2.CHAR:
            case JavaColorizerLexer2.CLASS:
            case JavaColorizerLexer2.CONST:
            case JavaColorizerLexer2.CONTINUE:
            case JavaColorizerLexer2.DEFAULT:
            case JavaColorizerLexer2.DO:
            case JavaColorizerLexer2.DOUBLE:
            case JavaColorizerLexer2.ELSE:
            case JavaColorizerLexer2.ENUM:
            case JavaColorizerLexer2.EXTENDS:
            case JavaColorizerLexer2.FINAL:
            case JavaColorizerLexer2.FINALLY:
            case JavaColorizerLexer2.FLOAT:
            case JavaColorizerLexer2.FOR:
            case JavaColorizerLexer2.IF:
            case JavaColorizerLexer2.GOTO:
            case JavaColorizerLexer2.IMPLEMENTS:
            case JavaColorizerLexer2.IMPORT:
            case JavaColorizerLexer2.INSTANCEOF:
            case JavaColorizerLexer2.INT:
            case JavaColorizerLexer2.INTERFACE:
            case JavaColorizerLexer2.LONG:
            case JavaColorizerLexer2.NATIVE:
            case JavaColorizerLexer2.NEW:
            case JavaColorizerLexer2.PACKAGE:
            case JavaColorizerLexer2.PRIVATE:
            case JavaColorizerLexer2.PROTECTED:
            case JavaColorizerLexer2.PUBLIC:
            case JavaColorizerLexer2.RETURN:
            case JavaColorizerLexer2.SHORT:
            case JavaColorizerLexer2.STATIC:
            case JavaColorizerLexer2.STRICTFP:
            case JavaColorizerLexer2.SUPER:
            case JavaColorizerLexer2.SWITCH:
            case JavaColorizerLexer2.SYNCHRONIZED:
            case JavaColorizerLexer2.THIS:
            case JavaColorizerLexer2.THROW:
            case JavaColorizerLexer2.THROWS:
            case JavaColorizerLexer2.TRANSIENT:
            case JavaColorizerLexer2.TRY:
            case JavaColorizerLexer2.VOID:
            case JavaColorizerLexer2.VOLATILE:
            case JavaColorizerLexer2.WHILE:
            case JavaColorizerLexer2.TRUE:
            case JavaColorizerLexer2.FALSE:
            case JavaColorizerLexer2.NULL:
                return _standardClassificationService.Keyword;

            case JavaColorizerLexer2.IDENTIFIER:
                return _standardClassificationService.Identifier;

            case JavaColorizerLexer2.COMMENT:
            case JavaColorizerLexer2.ML_COMMENT:
                return _standardClassificationService.Comment;

            case JavaColorizerLexer2.DOC_COMMENT_TEXT:
                return _docCommentText;

            case JavaColorizerLexer2.DOC_COMMENT_TAG:
            case JavaColorizerLexer2.DOC_COMMENT_INVALID_TAG:
                return _docCommentTag;

            default:
                return null;
            }
        }
    }
}
