namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Language.Java.Experimental;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    using CancellationToken = System.Threading.CancellationToken;
    using Contract = System.Diagnostics.Contracts.Contract;
    using IOutputWindowPane = Tvl.VisualStudio.OutputWindow.Interfaces.IOutputWindowPane;
    using IQuickInfoSession = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoSession;
    using IQuickInfoSource = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoSource;
    using ITextBuffer = Microsoft.VisualStudio.Text.ITextBuffer;
    using ITextSnapshot = Microsoft.VisualStudio.Text.ITextSnapshot;
    using ITrackingSpan = Microsoft.VisualStudio.Text.ITrackingSpan;
    using IWpfTextView = Microsoft.VisualStudio.Text.Editor.IWpfTextView;
    using JavaAtnBuilder = Tvl.VisualStudio.Language.Java.Experimental.JavaAtnBuilder;
    using JavaSimplifiedAtnBuilder = Tvl.VisualStudio.Language.Java.Experimental.JavaSimplifiedAtnBuilder;
    using NetworkInterpreter = Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter.NetworkInterpreter;
    using PredefinedOutputWindowPanes = Tvl.VisualStudio.OutputWindow.Interfaces.PredefinedOutputWindowPanes;
    using SnapshotCharStream = Tvl.VisualStudio.Language.Parsing.SnapshotCharStream;
    using Stopwatch = System.Diagnostics.Stopwatch;

    internal class JavaQuickInfoSource : IQuickInfoSource
    {
        private readonly ITextBuffer _textBuffer;
        private readonly JavaQuickInfoSourceProvider _provider;

        private readonly object _contentUpdateLock = new object();
        private SnapshotPoint? _triggerPoint;
        private ITrackingSpan _applicableToSpan;
        private IList<object> _quickInfoContent = new object[0];

        public JavaQuickInfoSource(ITextBuffer textBuffer, JavaQuickInfoSourceProvider provider)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Requires<ArgumentNullException>(provider != null, "provider");

            _textBuffer = textBuffer;
            _provider = provider;
        }

        public ITextBuffer TextBuffer
        {
            get
            {
                return _textBuffer;
            }
        }

        public JavaQuickInfoSourceProvider Provider
        {
            get
            {
                return _provider;
            }
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
        {
            applicableToSpan = null;
            if (session == null || quickInfoContent == null)
                return;

            Provider.IntelliSenseCache.BeginReferenceSourceParsing();

            if (session.TextView.TextBuffer == this.TextBuffer)
            {
                ITextSnapshot currentSnapshot = this.TextBuffer.CurrentSnapshot;
                SnapshotPoint? triggerPoint = session.GetTriggerPoint(currentSnapshot);
                if (!triggerPoint.HasValue)
                    return;

                lock (_contentUpdateLock)
                {
                    if (triggerPoint == this._triggerPoint)
                    {
                        foreach (var content in _quickInfoContent)
                            quickInfoContent.Add(content);

                        applicableToSpan = _applicableToSpan;
                        return;
                    }
                }

                BeginUpdateQuickInfoContent(session, triggerPoint.Value);
            }
        }

        private void BeginUpdateQuickInfoContent(IQuickInfoSession session, SnapshotPoint triggerPoint)
        {
            Contract.Requires(session != null);

            Action updateAction = () => UpdateQuickInfoContent(session, triggerPoint);
            Task.Factory.StartNew(updateAction, CancellationToken.None, TaskCreationOptions.None, Provider.PriorityIntelliSenseTaskScheduler).HandleNonCriticalExceptions();
        }

        private void UpdateQuickInfoContent(IQuickInfoSession session, SnapshotPoint triggerPoint)
        {
            /* use the experimental model to locate and process the expression */
            Stopwatch stopwatch = Stopwatch.StartNew();

            // lex the entire document
            var currentSnapshot = triggerPoint.Snapshot;
            var input = new SnapshotCharStream(currentSnapshot, new Span(0, currentSnapshot.Length));
            var unicodeInput = new JavaUnicodeStream(input);
            var lexer = new Java2Lexer(unicodeInput);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            // locate the last token before the trigger point
            while (true)
            {
                IToken nextToken = tokens.LT(1);
                if (nextToken.Type == CharStreamConstants.EndOfFile)
                    break;

                if (nextToken.StartIndex > triggerPoint.Position)
                    break;

                tokens.Consume();
            }

            IToken triggerToken = tokens.LT(-1);
            if (triggerToken == null)
                return;

            switch (triggerToken.Type)
            {
            // symbol references
            case Java2Lexer.IDENTIFIER:
            case Java2Lexer.THIS:
            case Java2Lexer.SUPER:
            // primitive types
            case Java2Lexer.BOOLEAN:
            case Java2Lexer.CHAR:
            case Java2Lexer.BYTE:
            case Java2Lexer.SHORT:
            case Java2Lexer.INT:
            case Java2Lexer.LONG:
            case Java2Lexer.FLOAT:
            case Java2Lexer.DOUBLE:
            // literals
            case Java2Lexer.INTLITERAL:
            case Java2Lexer.LONGLITERAL:
            case Java2Lexer.FLOATLITERAL:
            case Java2Lexer.DOUBLELITERAL:
            case Java2Lexer.CHARLITERAL:
            case Java2Lexer.STRINGLITERAL:
            case Java2Lexer.TRUE:
            case Java2Lexer.FALSE:
            case Java2Lexer.NULL:
                break;

            default:
                return;
            }

            NetworkInterpreter interpreter = CreateNetworkInterpreter(tokens);

            while (interpreter.TryStepBackward())
            {
                if (interpreter.Contexts.Count == 0 || interpreter.Contexts.Count > 400)
                    break;

                if (interpreter.Contexts.All(context => context.BoundedStart))
                    break;
            }

            interpreter.Contexts.RemoveAll(i => !i.BoundedStart);
            interpreter.CombineBoundedStartContexts();

            IOutputWindowPane pane = Provider.OutputWindowService.TryGetPane(PredefinedOutputWindowPanes.TvlIntellisense);
            if (pane != null)
            {
                pane.WriteLine(string.Format("Located {0} QuickInfo expression(s) in {1}ms.", interpreter.Contexts.Count, stopwatch.ElapsedMilliseconds));
            }

            HashSet<string> intermediateResult = new HashSet<string>();
            HashSet<string> finalResult = new HashSet<string>();
            List<object> quickInfoContent = new List<object>();
            foreach (var context in interpreter.Contexts)
            {
                Span? span = null;

                foreach (var transition in context.Transitions)
                {
                    if (!transition.Transition.IsMatch)
                        continue;

                    IToken token = transition.Token;
                    Span tokenSpan = new Span(token.StartIndex, token.StopIndex - token.StartIndex + 1);
                    if (span == null)
                        span = tokenSpan;
                    else
                        span = Span.FromBounds(Math.Min(span.Value.Start, tokenSpan.Start), Math.Max(span.Value.End, tokenSpan.End));
                }

                if (span.HasValue && !span.Value.IsEmpty)
                {
                    string text = currentSnapshot.GetText(span.Value);
                    if (!intermediateResult.Add(text))
                        continue;

                    AstParserRuleReturnScope<CommonTree, CommonToken> result = null;

                    try
                    {
                        var expressionInput = new ANTLRStringStream(text);
                        var expressionUnicodeInput = new JavaUnicodeStream(expressionInput);
                        var expressionLexer = new Java2Lexer(expressionUnicodeInput);
                        var expressionTokens = new CommonTokenStream(expressionLexer);
                        var expressionParser = new Java2Parser(expressionTokens);
                        result = expressionParser.primary();

                        // anchors experiment
                        Contract.Assert(TextBuffer.CurrentSnapshot == triggerPoint.Snapshot);
                        ClassAnchorTracker tracker = new ClassAnchorTracker(TextBuffer, null);
                        SnapshotSpan trackedSpan = new SnapshotSpan(triggerPoint.Snapshot, 0, triggerPoint.Position);
                        ITagSpan<ScopeAnchorTag>[] tags = tracker.GetTags(new NormalizedSnapshotSpanCollection(trackedSpan)).ToArray();

                        text = result.Tree.ToStringTree();
                    }
                    catch (RecognitionException)
                    {
                        text = "Could not parse: " + text;
                    }

                    text = text.Replace("\n", "\\n").Replace("\r", "\\r");
                    finalResult.Add(text);

                    //if (Regex.IsMatch(text, @"^[A-Za-z_]+(?:\.\w+)*$"))
                    //{
                    //    NameResolutionContext resolutionContext = NameResolutionContext.Global(Provider.IntelliSenseCache);
                    //    resolutionContext = resolutionContext.Filter(text, null, true);
                    //    CodeElement[] matching = resolutionContext.GetMatchingElements();
                    //    if (matching.Length > 0)
                    //    {
                    //        foreach (var element in matching)
                    //        {
                    //            element.AugmentQuickInfoSession(quickInfoContent);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        // check if this is a package
                    //        CodePhysicalFile[] files = Provider.IntelliSenseCache.GetPackageFiles(text, true);
                    //        if (files.Length > 0)
                    //        {
                    //            finalResult.Add(string.Format("package {0}", text));
                    //        }
                    //        else
                    //        {
                    //            // check if this is a type
                    //            string typeName = text.Substring(text.LastIndexOf('.') + 1);
                    //            CodeType[] types = Provider.IntelliSenseCache.GetTypes(typeName, true);
                    //            foreach (var type in types)
                    //            {
                    //                if (type.FullName == text)
                    //                    finalResult.Add(string.Format("{0}: {1}", type.GetType().Name, type.FullName));
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    finalResult.Add(text);
                    //}
                }
            }

            ITrackingSpan applicableToSpan = null;

            foreach (var result in finalResult)
            {
                quickInfoContent.Add(result);
            }

            applicableToSpan = currentSnapshot.CreateTrackingSpan(new Span(triggerToken.StartIndex, triggerToken.StopIndex - triggerToken.StartIndex + 1), SpanTrackingMode.EdgeExclusive);

            //try
            //{
            //    Expression currentExpression = Provider.IntellisenseCache.ParseExpression(selection);
            //    if (currentExpression != null)
            //    {
            //        SnapshotSpan? span = currentExpression.Span;
            //        if (span.HasValue)
            //            applicableToSpan = span.Value.Snapshot.CreateTrackingSpan(span.Value, SpanTrackingMode.EdgeExclusive);

            //        quickInfoContent.Add(currentExpression.ToString());
            //    }
            //    else
            //    {
            //        quickInfoContent.Add("Could not parse expression.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ErrorHandler.IsCriticalException(ex))
            //        throw;

            //    quickInfoContent.Add(ex.Message);
            //}

            lock (_contentUpdateLock)
            {
                _triggerPoint = triggerPoint;
                _applicableToSpan = applicableToSpan;
                _quickInfoContent = quickInfoContent;
            }

            IWpfTextView wpfTextView = session.TextView as IWpfTextView;
            if (wpfTextView != null && wpfTextView.VisualElement != null)
            {
                ITrackingPoint trackingTriggerPoint = triggerPoint.Snapshot.CreateTrackingPoint(triggerPoint.Position, PointTrackingMode.Negative);
                wpfTextView.VisualElement.Dispatcher.BeginInvoke((Action<IQuickInfoSession, ITrackingPoint, bool>)RetriggerQuickInfo, session, trackingTriggerPoint, true);
            }
        }

        private void RetriggerQuickInfo(IQuickInfoSession session, ITrackingPoint triggerPoint, bool trackMouse)
        {
            Provider.QuickInfoBroker.TriggerQuickInfo(session.TextView, triggerPoint, true);
        }

        private NetworkInterpreter CreateNetworkInterpreter(CommonTokenStream tokens)
        {
            Network network = NetworkBuilder<JavaSimplifiedAtnBuilder>.GetOrBuildNetwork();
            NetworkInterpreter interpreter = new NetworkInterpreter(network, tokens);

            IToken previousToken = tokens.LT(-1);
            if (previousToken == null)
                return new NetworkInterpreter(network, new CommonTokenStream());

            switch (previousToken.Type)
            {
            case Java2Lexer.IDENTIFIER:
                // definitions always appear as a single identifier (at least the part of them we care about for Quick Info)
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier));

                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ClassOrInterfaceType));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.QualifiedName));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ElementValuePair));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Statement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InnerCreator));

                break;

            case Java2Lexer.SUPER:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeArgument));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ExplicitConstructorInvocation));
                break;

            case Java2Lexer.THIS:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ExplicitConstructorInvocation));
                break;

            case Java2Lexer.CLASS:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                interpreter.ExcludedStartRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ClassHeader));
                break;

            case Java2Lexer.BOOLEAN:
            case Java2Lexer.CHAR:
            case Java2Lexer.BYTE:
            case Java2Lexer.SHORT:
            case Java2Lexer.INT:
            case Java2Lexer.LONG:
            case Java2Lexer.FLOAT:
            case Java2Lexer.DOUBLE:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.PrimitiveType));
                break;

            case Java2Lexer.INTLITERAL:
            case Java2Lexer.LONGLITERAL:
            case Java2Lexer.FLOATLITERAL:
            case Java2Lexer.DOUBLELITERAL:
            case Java2Lexer.CHARLITERAL:
            case Java2Lexer.STRINGLITERAL:
            case Java2Lexer.TRUE:
            case Java2Lexer.FALSE:
            case Java2Lexer.NULL:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Literal));
                break;

            default:
                return new NetworkInterpreter(network, new CommonTokenStream());
            }

            return interpreter;
        }

        public void Dispose()
        {
        }
    }
}
