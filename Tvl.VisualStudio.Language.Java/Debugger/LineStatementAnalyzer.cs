namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Atn;
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Text;

    public static class LineStatementAnalyzer
    {
        [RuleDependency(typeof(JavaParser), JavaParser.RULE_compilationUnit, 0, Dependents.Ancestors)]
        public static bool TryGetLineStatements(ITextBuffer textBuffer, int lineNumber, out IList<IParseTree> statementTrees, out IList<IToken> tokens)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Requires<ArgumentOutOfRangeException>(lineNumber >= 0);

            string text = textBuffer.CurrentSnapshot.GetText();
            return TryGetLineStatements(text, lineNumber, out statementTrees, out tokens);
        }

        [RuleDependency(typeof(JavaParser), JavaParser.RULE_compilationUnit, 0, Dependents.Ancestors)]
        public static bool TryGetLineStatements(string text, int lineNumber, out IList<IParseTree> statementTrees, out IList<IToken> tokens)
        {
            Contract.Requires<ArgumentNullException>(text != null, "text");
            Contract.Requires<ArgumentOutOfRangeException>(lineNumber >= 0);

            try
            {
                AntlrInputStream input = new AntlrInputStream(text);
                JavaLexer lexer = new JavaLexer(new JavaUnicodeStreamV4(input));
                CommonTokenStream tokenStream = new CommonTokenStream(lexer);
                JavaParser parser = new JavaParser(tokenStream);

                parser.Interpreter.PredictionMode = PredictionMode.Sll;
                parser.BuildParseTree = true;
                JavaParser.CompilationUnitContext result = parser.compilationUnit();

                statementTrees = null;
                tokens = tokenStream.GetTokens();

                AssociatedTreeListener listener = new AssociatedTreeListener(lineNumber, tokens);
                ParseTreeWalker.Default.Walk(listener, result);
                statementTrees = listener.StatementTrees;

                return true;
            }
            catch (Exception e)
            {
                if (ErrorHandler.IsCriticalException(e))
                    throw;

                statementTrees = null;
                tokens = null;
                return false;
            }
        }

        private class AssociatedTreeListener : JavaBaseListener
        {
            private readonly int _lineNumber;
            private readonly IList<IToken> _tokens;
            private readonly List<IParseTree> _statementTrees = new List<IParseTree>();

            public AssociatedTreeListener(int lineNumber, IList<IToken> tokens)
            {
                _lineNumber = lineNumber;
                _tokens = tokens;
            }

            public List<IParseTree> StatementTrees
            {
                get
                {
                    return _statementTrees;
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_statement, 0, Dependents.Parents)]
            [RuleDependency(typeof(JavaParser), JavaParser.RULE_statementExpression, 0, Dependents.Self)]
            public override void EnterStatement(JavaParser.StatementContext context)
            {
                if (context.ASSERT() != null
                    || context.RETURN() != null
                    || context.THROW() != null
                    || context.BREAK() != null
                    || context.CONTINUE() != null
                    || (context.SEMI() != null && context.ChildCount == 1)
                    || context.statementExpression() != null)
                {
                    if (IsInCurrentContext(context))
                        _statementTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_forInit, 0, Dependents.Parents)]
            public override void EnterForInit(JavaParser.ForInitContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _statementTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_forUpdate, 0, Dependents.Parents)]
            public override void EnterForUpdate(JavaParser.ForUpdateContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _statementTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_enhancedForControl, 0, Dependents.Parents)]
            public override void EnterEnhancedForControl(JavaParser.EnhancedForControlContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _statementTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_localVariableDeclarationStatement, 0, Dependents.Parents)]
            public override void EnterLocalVariableDeclarationStatement(JavaParser.LocalVariableDeclarationStatementContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _statementTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_parExpression, 0, Dependents.Parents)]
            [RuleDependency(typeof(JavaParser), JavaParser.RULE_expression, 0, Dependents.Self)]
            public override void EnterParExpression(JavaParser.ParExpressionContext context)
            {
                if (context.expression() == null)
                    return;

                if (IsInCurrentContext(context.expression()))
                {
                    _statementTrees.Add(context.expression());
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_expression, 0, Dependents.Parents)]
            [RuleDependency(typeof(JavaParser), JavaParser.RULE_forControl, 0, Dependents.Self)]
            public override void EnterExpression(JavaParser.ExpressionContext context)
            {
                if (context.Parent is JavaParser.ForControlContext)
                {
                    if (IsInCurrentContext(context))
                    {
                        _statementTrees.Add(context);
                    }
                }
            }

            private bool IsInCurrentContext(IParseTree tree)
            {
                IToken startToken = _tokens[tree.SourceInterval.a];
                IToken stopToken = _tokens[tree.SourceInterval.b];
                if (stopToken.Line - 1 < _lineNumber || startToken.Line - 1 > _lineNumber)
                    return false;

                return true;
            }
        }
    }
}
