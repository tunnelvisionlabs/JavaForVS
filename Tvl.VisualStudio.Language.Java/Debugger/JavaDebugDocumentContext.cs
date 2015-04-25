namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Atn;
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Collections;
    using Tvl.Java.DebugInterface;
    using Tvl.Java.DebugInterface.Types.Analysis;
    using Tvl.Java.DebugInterface.Types.Loader;
    using File = System.IO.File;

    [ComVisible(true)]
    public class JavaDebugDocumentContext : IDebugDocumentContext2, IDebugDocumentChecksum2
    {
        private readonly ILocation _location;

        private TEXT_POSITION? _beginStatement;
        private TEXT_POSITION? _endStatement;

        public JavaDebugDocumentContext(ILocation location)
        {
            Contract.Requires<ArgumentNullException>(location != null, "location");
            _location = location;
        }

        #region IDebugDocumentContext2 Members

        public int Compare(enum_DOCCONTEXT_COMPARE Compare, IDebugDocumentContext2[] rgpDocContextSet, uint dwDocContextSetLen, out uint pdwDocContext)
        {
            throw new NotImplementedException();
        }

        public int EnumCodeContexts(out IEnumDebugCodeContexts2 ppEnumCodeCxts)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the document that contains this document context.
        /// </summary>
        /// <param name="ppDocument">Returns an IDebugDocument2 object that represents the document that contains this document context.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// This method is for those debug engines that supply documents directly to the IDE. Otherwise, this method should return E_NOTIMPL.
        /// </remarks>
        public int GetDocument(out IDebugDocument2 ppDocument)
        {
            // this might be implemented if we support download-on-demand for the jre source in the future
            ppDocument = null;
            return VSConstants.E_NOTIMPL;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Constants.JavaLanguageName;
            pguidLanguage = Constants.JavaLanguageGuid;
            return VSConstants.S_OK;
        }

        public int GetName(enum_GETNAME_TYPE gnType, out string pbstrFileName)
        {
            pbstrFileName = null;

            switch (gnType)
            {
            case enum_GETNAME_TYPE.GN_BASENAME:
                // Specifies a base file name instead of a full path of the document or context
                pbstrFileName = _location.GetSourceName();
                return VSConstants.S_OK;

            case enum_GETNAME_TYPE.GN_FILENAME:
                // Specifies the full path of the document or context
                pbstrFileName = _location.GetSourcePath();
                return VSConstants.S_OK;

            case enum_GETNAME_TYPE.GN_NAME:
                // Specifies a friendly name of the document or context
                pbstrFileName = _location.GetSourceName();
                return VSConstants.S_OK;

            case enum_GETNAME_TYPE.GN_MONIKERNAME:
                // Specifies a unique name of the document or context in the form of a moniker
                return VSConstants.E_INVALIDARG;

            case enum_GETNAME_TYPE.GN_STARTPAGEURL:
                // Gets the starting page URL for processes
                return VSConstants.E_INVALIDARG;

            case enum_GETNAME_TYPE.GN_TITLE:
                // Specifies a title of the document, if one exists
                return VSConstants.E_INVALIDARG;

            case enum_GETNAME_TYPE.GN_URL:
                // Specifies a URL name of the document or context
                return VSConstants.E_INVALIDARG;

            default:
                return VSConstants.E_INVALIDARG;
            }
        }

        /// <summary>
        /// Gets the source code range of this document context.
        /// </summary>
        /// <param name="pBegPosition">[in, out] A TEXT_POSITION structure that is filled in with the starting position. Set this argument to a null value if this information is not needed.</param>
        /// <param name="pEndPosition">[in, out] A TEXT_POSITION structure that is filled in with the ending position. Set this argument to a null value if this information is not needed.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// A source range is the entire range of source code, from the current statement back to just
        /// after the previous statement that contributed code. The source range is typically used for
        /// mixing source statements, including comments, with code in the disassembly window.
        /// 
        /// To get the range for just the code statements contained within this document context, call
        /// the IDebugDocumentContext2.GetStatementRange method.
        /// </remarks>
        public int GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            // TODO: also includes lines leading up to this one which do not contain executable code
            if (pBegPosition != null && pBegPosition.Length == 0)
                throw new ArgumentException("pBegPosition");
            if (pEndPosition != null && pEndPosition.Length == 0)
                throw new ArgumentException("pEndPosition");

            TEXT_POSITION begin = new TEXT_POSITION();
            TEXT_POSITION end = new TEXT_POSITION();

            begin.dwLine = (uint)_location.GetLineNumber() - 1;
            begin.dwColumn = 0;
            end = begin;

            if (pBegPosition != null)
                pBegPosition[0] = begin;

            if (pEndPosition != null)
                pEndPosition[0] = end;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the file statement range of the document context.
        /// </summary>
        /// <param name="pBegPosition">[in, out] A TEXT_POSITION structure that is filled in with the starting position. Set this argument to a null value if this information is not needed.</param>
        /// <param name="pEndPosition">[in, out] A TEXT_POSITION structure that is filled in with the ending position. Set this argument to a null value if this information is not needed.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// A statement range is the range of the lines that contributed the code to which this document context refers.
        /// 
        /// To obtain the range of source code (including comments) within this document context, call the
        /// IDebugDocumentContext2.GetSourceRange method.
        /// </remarks>
        public int GetStatementRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            if (pBegPosition != null && pBegPosition.Length == 0)
                throw new ArgumentException("pBegPosition");
            if (pEndPosition != null && pEndPosition.Length == 0)
                throw new ArgumentException("pEndPosition");

            if (!_beginStatement.HasValue)
            {
                IList<IToken> tokens = null;
                IParseTree associatedTree = null;
                if (TryGetAssociatedTree(out associatedTree, out tokens))
                {
                    IToken startToken = tokens[associatedTree.SourceInterval.a];
                    IToken endToken = tokens[associatedTree.SourceInterval.b];
                    _beginStatement = new TEXT_POSITION()
                    {
                        dwLine = (uint)startToken.Line - 1,
                        dwColumn = (uint)startToken.Column
                    };
                    _endStatement = new TEXT_POSITION()
                    {
                        dwLine = (uint)endToken.Line - 1,
                        dwColumn = (uint)(endToken.Column + (endToken.StopIndex - endToken.StartIndex + 1))
                    };
                }
                else
                {
                    _beginStatement = new TEXT_POSITION()
                    {
                        dwLine = (uint)_location.GetLineNumber() - 1,
                        dwColumn = 0
                    };
                    _endStatement = _beginStatement;
                }
            }

            if (pBegPosition != null)
                pBegPosition[0] = _beginStatement ?? new TEXT_POSITION();

            if (pEndPosition != null)
                pEndPosition[0] = _endStatement ?? new TEXT_POSITION();

            return VSConstants.S_OK;
        }

        public int Seek(int nCount, out IDebugDocumentContext2 ppDocContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugDocumentChecksum2 Members

        public int GetChecksumAndAlgorithmId(out Guid pRetVal, uint cMaxBytes, byte[] pChecksum, out uint pcNumBytes)
        {
            pRetVal = default(Guid);
            pcNumBytes = default(uint);
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        [RuleDependency(typeof(JavaParser), JavaParser.RULE_compilationUnit, 0, Dependents.Ancestors)]
        private bool TryGetAssociatedTree(out IParseTree associatedTree, out IList<IToken> tokens)
        {
            try
            {
                string sourcePath = _location.GetSourcePath();
                if (!File.Exists(sourcePath))
                {
                    associatedTree = null;
                    tokens = null;
                    return false;
                }

                string text = File.ReadAllText(sourcePath);
                AntlrInputStream input = new AntlrInputStream(text);
                JavaLexer lexer = new JavaLexer(new JavaUnicodeStreamV4(input));
                CommonTokenStream tokenStream = new CommonTokenStream(lexer);
                JavaParser parser = new JavaParser(tokenStream);

                parser.Interpreter.PredictionMode = PredictionMode.Sll;
                parser.BuildParseTree = true;
                JavaParser.CompilationUnitContext result = parser.compilationUnit();

                associatedTree = null;
                tokens = tokenStream.GetTokens();

                AssociatedTreeListener listener = new AssociatedTreeListener(_location, tokens);
                ParseTreeWalker.Default.Walk(listener, result);
                List<IParseTree> potentialTrees = listener.AssociatedTree;

                if (potentialTrees.Count == 1)
                {
                    associatedTree = potentialTrees[0];
                }
                else if (potentialTrees.Count > 1)
                {
                    byte[] bytecode = _location.GetMethod().GetBytecodes();
                    DisassembledMethod disassembledMethod = BytecodeDisassembler.Disassemble(bytecode);

                    var constantPool = _location.GetDeclaringType().GetConstantPool();
                    ReadOnlyCollection<ExceptionTableEntry> exceptionTable;
                    try
                    {
                        exceptionTable = _location.GetMethod().GetExceptionTable();
                    }
                    catch (DebuggerException)
                    {
                        exceptionTable = new ReadOnlyCollection<ExceptionTableEntry>(new ExceptionTableEntry[0]);
                    }

                    ImmutableList<int?> evaluationStackDepths = BytecodeDisassembler.GetEvaluationStackDepths(disassembledMethod, constantPool, exceptionTable);
                    ReadOnlyCollection<ILocation> locations = _location.GetMethod().GetLineLocations();

                    // find all bytecode offsets with evaluation stack depth 0 on the current line
                    List<int> relevantOffsets = new List<int>();
                    for (int i = 0; i < locations.Count; i++)
                    {
                        if (locations[i].GetLineNumber() != _location.GetLineNumber())
                            continue;

                        long offsetLimit = i < locations.Count - 1 ? locations[i + 1].GetCodeIndex() : bytecode.Length;
                        // start with the instruction for this bytecode offset
                        for (int j = GetInstructionAtOffset(disassembledMethod, locations[i].GetCodeIndex());
                            j >= 0 && j < disassembledMethod.Instructions.Count && disassembledMethod.Instructions[j].Offset < offsetLimit;
                            j++)
                        {
                            if (evaluationStackDepths[j] == 0)
                            {
                                // ignore unconditional branches
                                if (disassembledMethod.Instructions[j].OpCode.FlowControl == JavaFlowControl.Branch)
                                    continue;

                                relevantOffsets.Add(disassembledMethod.Instructions[j].Offset);
                            }
                        }
                    }

                    if (relevantOffsets.Count == potentialTrees.Count)
                    {
                        // heuristic: assume they appear in the same order as the source code on this line
                        int treeIndex = relevantOffsets.IndexOf((int)_location.GetCodeIndex());
                        if (treeIndex >= 0)
                            associatedTree = potentialTrees[treeIndex];
                    }
                }

                if (associatedTree == null)
                {
                    tokens = null;
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                if (ErrorHandler.IsCriticalException(e))
                    throw;

                associatedTree = null;
                tokens = null;
                return false;
            }
        }

        private static int GetInstructionAtOffset(DisassembledMethod disassembledMethod, long codeIndex)
        {
            return disassembledMethod.Instructions.FindIndex(i => i.Offset == codeIndex);
        }

        private class AssociatedTreeListener : JavaBaseListener
        {
            private readonly ILocation _location;
            private readonly IList<IToken> _tokens;
            private readonly List<IParseTree> _associatedTrees = new List<IParseTree>();

            public AssociatedTreeListener(ILocation location, IList<IToken> tokens)
            {
                _location = location;
                _tokens = tokens;
            }

            public List<IParseTree> AssociatedTree
            {
                get
                {
                    return _associatedTrees;
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
                        _associatedTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_forInit, 0, Dependents.Parents)]
            public override void EnterForInit(JavaParser.ForInitContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _associatedTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_forUpdate, 0, Dependents.Parents)]
            public override void EnterForUpdate(JavaParser.ForUpdateContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _associatedTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_enhancedForControl, 0, Dependents.Parents)]
            public override void EnterEnhancedForControl(JavaParser.EnhancedForControlContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _associatedTrees.Add(context);
                }
            }

            [RuleDependency(typeof(JavaParser), JavaParser.RULE_localVariableDeclarationStatement, 0, Dependents.Parents)]
            public override void EnterLocalVariableDeclarationStatement(JavaParser.LocalVariableDeclarationStatementContext context)
            {
                if (IsInCurrentContext(context))
                {
                    _associatedTrees.Add(context);
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
                    _associatedTrees.Add(context.expression());
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
                        _associatedTrees.Add(context);
                    }
                }
            }

            private bool IsInCurrentContext(IParseTree tree)
            {
                int currentLine = _location.GetLineNumber();

                IToken startToken = _tokens[tree.SourceInterval.a];
                IToken stopToken = _tokens[tree.SourceInterval.b];
                if (stopToken.Line < currentLine || startToken.Line > currentLine)
                    return false;

                return true;
            }
        }
    }
}
