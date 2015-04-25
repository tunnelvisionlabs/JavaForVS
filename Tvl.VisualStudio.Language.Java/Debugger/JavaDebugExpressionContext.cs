namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class JavaDebugExpressionContext : IDebugExpressionContext2
    {
        private readonly JavaDebugStackFrame _stackFrame;

        public JavaDebugExpressionContext(JavaDebugStackFrame stackFrame)
        {
            Contract.Requires<ArgumentNullException>(stackFrame != null, "stackFrame");
            _stackFrame = stackFrame;
        }

        public JavaDebugStackFrame StackFrame
        {
            get
            {
                return _stackFrame;
            }
        }

        public int GetName(out string pbstrName)
        {
            return _stackFrame.GetName(out pbstrName);
        }

        /// <summary>
        /// Parses an expression in text form for later evaluation.
        /// </summary>
        /// <param name="pszCode">The expression to be parsed.</param>
        /// <param name="dwFlags">A combination of flags from the PARSEFLAGS enumeration that controls parsing.</param>
        /// <param name="nRadix">The radix to be used in parsing any numerical information in pszCode.</param>
        /// <param name="ppExpr">Returns the IDebugExpression2 object that represents the parsed expression, which is ready for binding and evaluation.</param>
        /// <param name="pbstrError">Returns the error message if the expression contains an error.</param>
        /// <param name="pichError">Returns the character index of the error in pszCode if the expression contains an error.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// When this method is called, a debug engine (DE) should parse the expression and validate it for correctness.
        /// The pbstrError and pichError parameters may be filled in if the expression is invalid.
        /// 
        /// Note that the expression is not evaluated, only parsed. A later call to the IDebugExpression2.EvaluateSync
        /// or IDebugExpression2.EvaluateAsync methods evaluates the parsed expression.
        /// </remarks>
        public int ParseText(string pszCode, enum_PARSEFLAGS dwFlags, uint nRadix, out IDebugExpression2 ppExpr, out string pbstrError, out uint pichError)
        {
            if (pszCode == null)
                throw new ArgumentNullException("pszCode");
            if (pszCode.Length == 0)
                throw new ArgumentException();
            // dwFlags=0 in the Immediate window
            if (dwFlags != enum_PARSEFLAGS.PARSE_EXPRESSION && dwFlags != 0)
                throw new NotImplementedException();

            try
            {
                var expressionInput = new ANTLRStringStream(pszCode);
                var expressionUnicodeInput = new JavaUnicodeStream(expressionInput);
                var expressionLexer = new Java2Lexer(expressionUnicodeInput);
                var expressionTokens = new CommonTokenStream(expressionLexer);
                var expressionParser = new Java2Parser(expressionTokens);
                IAstRuleReturnScope<CommonTree> result = expressionParser.standaloneExpression();

                ppExpr = new JavaDebugExpression(this, result.Tree, pszCode);
                pbstrError = null;
                pichError = 0;
                return VSConstants.S_OK;
            }
            catch (RecognitionException e)
            {
                ppExpr = null;
                pbstrError = e.Message;
                pichError = (uint)Math.Max(0, e.Index);
                return VSConstants.E_FAIL;
            }
        }
    }
}
