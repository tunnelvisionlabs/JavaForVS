namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter;
    using Tvl.VisualStudio.OutputWindow.Interfaces;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;

    internal sealed class JavaSymbolTagger : BackgroundParser, ITagger<IClassificationTag>
    {
        private readonly IClassificationTypeRegistryService _classificationTypeRegistryService;

        private List<ITagSpan<IClassificationTag>> _tags = new List<ITagSpan<IClassificationTag>>();

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        private static object _lookupTablesLock = new object();
        private static IntervalSet _definitionSourceSet;
        private static IntervalSet _referenceSourceSet;
        private static IntervalSet _definitionFollowSet;
        private static IntervalSet _referenceFollowSet;
        private static IntervalSet _definitionOnlySourceSet;
        private static IntervalSet _referenceOnlySourceSet;
        private static IntervalSet _definitionOnlyFollowSet;
        private static IntervalSet _referenceOnlyFollowSet;
        // definitionContextSet1[IDENTIFIER] is the set of tokens that can follow the sequence IDENTIFIER SymbolDefinitionIdentifier
        private static Dictionary<int, IntervalSet> _definitionContextSet1;
        // referenceContextSet1[IDENTIFIER] is the set of tokens that can follow the sequence IDENTIFIER SymbolReferenceIdentifier
        private static Dictionary<int, IntervalSet> _referenceContextSet1;
        private static Dictionary<int, IntervalSet> _definitionSourceSet2;
        private static Dictionary<int, IntervalSet> _referenceSourceSet2;
        private static Dictionary<int, IntervalSet> _definitionFollowSet2;
        private static Dictionary<int, IntervalSet> _referenceFollowSet2;

        public JavaSymbolTagger(ITextBuffer textBuffer, IClassificationTypeRegistryService classificationTypeRegistryService, TaskScheduler taskScheduler, ITextDocumentFactoryService textDocumentFactoryService, IOutputWindowService outputWindowService)
            : base(textBuffer, taskScheduler, textDocumentFactoryService, outputWindowService)
        {
            _classificationTypeRegistryService = classificationTypeRegistryService;
            RequestParse(false);
        }

        public override string Name
        {
            get
            {
                return "Symbol Tagger";
            }
        }

        public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return _tags;
        }

        private void OnTagsChanged(SnapshotSpanEventArgs e)
        {
            var t = TagsChanged;
            if (t != null)
                t(this, e);
        }

        protected override void ReParseImpl()
        {
            Stopwatch timer = Stopwatch.StartNew();

            // lex the entire document to get the set of identifiers we'll need to classify
            ITextSnapshot snapshot = TextBuffer.CurrentSnapshot;
            var input = new SnapshotCharStream(snapshot, new Span(0, snapshot.Length));
            JavaUnicodeStream inputWrapper = new JavaUnicodeStream(input);
            var lexer = new Java2Lexer(inputWrapper);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            List<IToken> nameKeywords = new List<IToken>();
            List<IToken> declColons = new List<IToken>();

            List<IToken> identifiers = new List<IToken>();

            HashSet<IToken> definitions = new HashSet<IToken>(TokenIndexEqualityComparer.Default);
            HashSet<IToken> references = new HashSet<IToken>(TokenIndexEqualityComparer.Default);

            GetLl2SymbolSets();

            while (tokens.LA(1) != CharStreamConstants.EndOfFile)
            {
                // covered by the double-sided check
                if (_definitionOnlySourceSet.Contains(tokens.LA(1)))
                {
                    if (tokens.LA(2) == Java2Lexer.IDENTIFIER)
                        definitions.Add(tokens.LT(2));
                }
                else if (_referenceOnlySourceSet.Contains(tokens.LA(1)))
                {
                    if (tokens.LA(2) == Java2Lexer.IDENTIFIER)
                        references.Add(tokens.LT(2));
                }

                if (_definitionOnlyFollowSet.Contains(tokens.LA(1)))
                {
                    IToken previous = tokens.LT(-1);
                    if (previous != null && previous.Type == Java2Lexer.IDENTIFIER)
                        definitions.Add(previous);
                }
                else if (_referenceOnlyFollowSet.Contains(tokens.LA(1)))
                {
                    IToken previous = tokens.LT(-1);
                    if (previous != null && previous.Type == Java2Lexer.IDENTIFIER)
                        references.Add(previous);
                }

                if (tokens.LA(2) == Java2Lexer.IDENTIFIER)
                {
                    IntervalSet bothWaysFollowDefinition;
                    IntervalSet bothWaysFollowReference;
                    _definitionContextSet1.TryGetValue(tokens.LA(1), out bothWaysFollowDefinition);
                    _referenceContextSet1.TryGetValue(tokens.LA(1), out bothWaysFollowReference);
                    bool couldBeDef = bothWaysFollowDefinition != null && bothWaysFollowDefinition.Contains(tokens.LA(3));
                    bool couldBeRef = bothWaysFollowReference != null && bothWaysFollowReference.Contains(tokens.LA(3));

                    if (couldBeDef && !couldBeRef)
                        definitions.Add(tokens.LT(2));
                    else if (couldBeRef && !couldBeDef)
                        references.Add(tokens.LT(2));
                }

                if (tokens.LA(3) == Java2Lexer.IDENTIFIER && _definitionSourceSet.Contains(tokens.LA(2)))
                {
                    IntervalSet sourceDefinition2;
                    IntervalSet sourceReference2;
                    _definitionSourceSet2.TryGetValue(tokens.LA(2), out sourceDefinition2);
                    _referenceSourceSet2.TryGetValue(tokens.LA(2), out sourceReference2);
                    bool couldBeDef = sourceDefinition2 != null && sourceDefinition2.Contains(tokens.LA(1));
                    bool couldBeRef = sourceReference2 != null && sourceReference2.Contains(tokens.LA(1));

                    if (couldBeDef && !couldBeRef)
                        definitions.Add(tokens.LT(3));
                    else if (couldBeRef && !couldBeDef)
                        references.Add(tokens.LT(3));
                }

                if (_definitionFollowSet.Contains(tokens.LA(1)))
                    declColons.Add(tokens.LT(1));

                if (tokens.LA(1) == Java2Lexer.IDENTIFIER)
                    identifiers.Add(tokens.LT(1));

                tokens.Consume();
            }

            foreach (var token in declColons)
            {
                tokens.Seek(token.TokenIndex);
                tokens.Consume();

                IToken potentialDeclaration = tokens.LT(-2);
                if (potentialDeclaration.Type != Java2Lexer.IDENTIFIER || definitions.Contains(potentialDeclaration) || references.Contains(potentialDeclaration))
                    continue;

                bool agree = false;
                NetworkInterpreter interpreter = CreateVarDeclarationNetworkInterpreter(tokens, token.Type);
                while (interpreter.TryStepBackward())
                {
                    if (interpreter.Contexts.Count == 0 || interpreter.Contexts.Count > 400)
                        break;

                    if (interpreter.Contexts.All(context => context.BoundedStart))
                        break;

                    interpreter.Contexts.RemoveAll(i => !IsConsistentWithPreviousResult(i, true, definitions, references));

                    agree = AllAgree(interpreter.Contexts, potentialDeclaration.TokenIndex);
                    if (agree)
                        break;
                }

                interpreter.CombineBoundedStartContexts();

                if (!agree)
                {
                    while (interpreter.TryStepForward())
                    {
                        if (interpreter.Contexts.Count == 0 || interpreter.Contexts.Count > 400)
                            break;

                        if (interpreter.Contexts.All(context => context.BoundedEnd))
                            break;

                        interpreter.Contexts.RemoveAll(i => !IsConsistentWithPreviousResult(i, false, definitions, references));

                        agree = AllAgree(interpreter.Contexts, potentialDeclaration.TokenIndex);
                        if (agree)
                            break;
                    }

                    interpreter.CombineBoundedEndContexts();
                }

                foreach (var context in interpreter.Contexts)
                {
                    foreach (var transition in context.Transitions)
                    {
                        if (!transition.Symbol.HasValue)
                            continue;

                        switch (transition.Symbol)
                        {
                        case Java2Lexer.IDENTIFIER:
                            //case Java2Lexer.KW_THIS:
                            RuleBinding rule = interpreter.Network.StateRules[transition.Transition.TargetState.Id];
                            if (rule.Name == JavaAtnBuilder.RuleNames.SymbolReferenceIdentifier)
                                references.Add(tokens.Get(transition.TokenIndex.Value));
                            else if (rule.Name == JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier)
                                definitions.Add(tokens.Get(transition.TokenIndex.Value));
                            break;

                        default:
                            continue;
                        }
                    }
                }
            }

            // tokens which are in both the 'definitions' and 'references' sets are actually unknown.
            HashSet<IToken> unknownIdentifiers = new HashSet<IToken>(definitions, TokenIndexEqualityComparer.Default);
            unknownIdentifiers.IntersectWith(references);
            definitions.ExceptWith(unknownIdentifiers);

#if true // set to true to mark all unknown identifiers as references (requires complete analysis of definitions)
            references = new HashSet<IToken>(identifiers, TokenIndexEqualityComparer.Default);
            references.ExceptWith(definitions);
            references.ExceptWith(unknownIdentifiers);
#else
            references.ExceptWith(unknownIdentifiers);

            // the full set of unknown identifiers are any that aren't explicitly classified as a definition or a reference
            unknownIdentifiers = new HashSet<IToken>(identifiers, TokenIndexEqualityComparer.Default);
            unknownIdentifiers.ExceptWith(definitions);
            unknownIdentifiers.ExceptWith(references);
#endif

            List<ITagSpan<IClassificationTag>> tags = new List<ITagSpan<IClassificationTag>>();

            IClassificationType definitionClassificationType = _classificationTypeRegistryService.GetClassificationType(JavaSymbolTaggerClassificationTypeNames.Definition);
            tags.AddRange(ClassifyTokens(snapshot, definitions, new ClassificationTag(definitionClassificationType)));

            IClassificationType referenceClassificationType = _classificationTypeRegistryService.GetClassificationType(JavaSymbolTaggerClassificationTypeNames.Reference);
            tags.AddRange(ClassifyTokens(snapshot, references, new ClassificationTag(referenceClassificationType)));

            IClassificationType unknownClassificationType = _classificationTypeRegistryService.GetClassificationType(JavaSymbolTaggerClassificationTypeNames.UnknownIdentifier);
            tags.AddRange(ClassifyTokens(snapshot, unknownIdentifiers, new ClassificationTag(unknownClassificationType)));

            _tags = tags;

            timer.Stop();

            IOutputWindowPane pane = OutputWindowService.TryGetPane(PredefinedOutputWindowPanes.TvlIntellisense);
            if (pane != null)
            {
                pane.WriteLine(string.Format("Finished classifying {0} identifiers in {1}ms: {2} definitions, {3} references, {4} unknown", identifiers.Count, timer.ElapsedMilliseconds, definitions.Count, references.Count, unknownIdentifiers.Count));
            }

            OnTagsChanged(new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, new Span(0, snapshot.Length))));
        }

        private static void GetLl2SymbolSets()
        {
            lock (_lookupTablesLock)
            {
                if (_definitionContextSet1 != null)
                    return;

                Network setAnalysisNetwork = NetworkBuilder<JavaSimplifiedAtnBuilder>.GetOrBuildNetwork();
                JavaSimplifiedAtnBuilder setAnalysisBuilder = (JavaSimplifiedAtnBuilder)setAnalysisNetwork.Builder;

                _definitionSourceSet = setAnalysisBuilder.DefinitionSourceSet;
                _referenceSourceSet = setAnalysisBuilder.ReferenceSourceSet;
                _definitionFollowSet = setAnalysisBuilder.DefinitionFollowSet;
                _referenceFollowSet = setAnalysisBuilder.ReferenceFollowSet;

                _definitionOnlySourceSet = setAnalysisBuilder.DefinitionOnlySourceSet;
                _referenceOnlySourceSet = setAnalysisBuilder.ReferenceOnlySourceSet;
                _definitionOnlyFollowSet = setAnalysisBuilder.DefinitionOnlyFollowSet;
                _referenceOnlyFollowSet = setAnalysisBuilder.ReferenceOnlyFollowSet;

                _definitionContextSet1 = new Dictionary<int, IntervalSet>();
                _referenceContextSet1 = new Dictionary<int, IntervalSet>();
                _definitionSourceSet2 = new Dictionary<int, IntervalSet>();
                _referenceSourceSet2 = new Dictionary<int, IntervalSet>();
                _definitionFollowSet2 = new Dictionary<int, IntervalSet>();
                _referenceFollowSet2 = new Dictionary<int, IntervalSet>();

                var sharedSourceTokens = setAnalysisBuilder.DefinitionSourceSet.Intersect(setAnalysisBuilder.ReferenceSourceSet);
                foreach (var sharedSourceToken in sharedSourceTokens)
                {
                    CommonTokenStream analysisTokenStream = new CommonTokenStream(new ArrayTokenSource(sharedSourceToken, Java2Lexer.IDENTIFIER));
                    analysisTokenStream.Fill();
                    analysisTokenStream.Seek(1);

                    // definition context set
                    NetworkInterpreter ll2analyzer = new NetworkInterpreter(setAnalysisNetwork, analysisTokenStream);
                    ll2analyzer.ExcludedStartRules.Add(setAnalysisNetwork.GetRule(JavaAtnBuilder.RuleNames.SymbolReferenceIdentifier));
                    ll2analyzer.TryStepForward();
                    ll2analyzer.TryStepBackward();
                    _definitionContextSet1[sharedSourceToken] = ll2analyzer.GetFollowSet();
                    _definitionSourceSet2[sharedSourceToken] = ll2analyzer.GetSourceSet();

                    // reference context set
                    ll2analyzer = new NetworkInterpreter(setAnalysisNetwork, analysisTokenStream);
                    ll2analyzer.ExcludedStartRules.Add(setAnalysisNetwork.GetRule(JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier));
                    ll2analyzer.TryStepForward();
                    ll2analyzer.TryStepBackward();
                    _referenceContextSet1[sharedSourceToken] = ll2analyzer.GetFollowSet();
                    _referenceSourceSet2[sharedSourceToken] = ll2analyzer.GetSourceSet();
                }

                var sharedFollowTokens = setAnalysisBuilder.DefinitionFollowSet.Intersect(setAnalysisBuilder.ReferenceFollowSet);
                foreach (var sharedFollowToken in sharedFollowTokens)
                {
                    CommonTokenStream analysisTokenStream = new CommonTokenStream(new ArrayTokenSource(Java2Lexer.IDENTIFIER, sharedFollowToken));
                    analysisTokenStream.Fill();
                    analysisTokenStream.Seek(0);

                    // definition follow set
                    NetworkInterpreter ll2analyzer = new NetworkInterpreter(setAnalysisNetwork, analysisTokenStream);
                    ll2analyzer.ExcludedStartRules.Add(setAnalysisNetwork.GetRule(JavaAtnBuilder.RuleNames.SymbolReferenceIdentifier));
                    ll2analyzer.TryStepForward();
                    ll2analyzer.TryStepBackward();
                    _definitionFollowSet2[sharedFollowToken] = ll2analyzer.GetFollowSet();

                    // reference follow set
                    ll2analyzer = new NetworkInterpreter(setAnalysisNetwork, analysisTokenStream);
                    ll2analyzer.ExcludedStartRules.Add(setAnalysisNetwork.GetRule(JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier));
                    ll2analyzer.TryStepForward();
                    ll2analyzer.TryStepBackward();
                    _referenceFollowSet2[sharedFollowToken] = ll2analyzer.GetFollowSet();
                }
            }
        }

        private bool IsConsistentWithPreviousResult(InterpretTrace trace, bool checkStart, HashSet<IToken> definitions, HashSet<IToken> references)
        {
            Contract.Requires(trace != null);
            Contract.Requires(definitions != null);
            Contract.Requires(references != null);

            InterpretTraceTransition transition = checkStart ? trace.Transitions.First.Value : trace.Transitions.Last.Value;
            IToken token = transition.Token;
            if (definitions.Contains(token) && !references.Contains(token))
            {
                if (transition.Interpreter.Network.StateRules[transition.Transition.SourceState.Id].Name != JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier)
                    return false;
            }
            else if (references.Contains(token) && !definitions.Contains(token))
            {
                if (transition.Interpreter.Network.StateRules[transition.Transition.SourceState.Id].Name != JavaAtnBuilder.RuleNames.SymbolReferenceIdentifier)
                    return false;
            }

            return true;
        }

        private static bool AllAgree(IEnumerable<InterpretTrace> contexts, int? tokenIndex)
        {
            var symbolTransitions = contexts.SelectMany(i => i.Transitions).Where(i => i.Symbol != null && (tokenIndex == null || i.TokenIndex == tokenIndex));
            var grouped = symbolTransitions.GroupBy(i => i.TokenIndex);
            bool foundIndex = false;
            foreach (var group in grouped)
            {
                if (tokenIndex != null && group.First().Token.TokenIndex == tokenIndex)
                    foundIndex = true;

                if (group.First().Token.Type != Java2Lexer.IDENTIFIER)
                    continue;

                bool hasDefinition = false;
                bool hasReference = false;
                foreach (var item in group)
                {
                    string ruleName = item.Interpreter.Network.StateRules[item.Transition.SourceState.Id].Name;
                    if (ruleName == JavaAtnBuilder.RuleNames.SymbolDefinitionIdentifier)
                        hasDefinition = true;
                    else if (ruleName == JavaAtnBuilder.RuleNames.SymbolReferenceIdentifier)
                        hasReference = true;
                    else
                        return false;

                    if (hasDefinition && hasReference)
                        return false;
                }
            }

            return tokenIndex == null || foundIndex;
        }

        private NetworkInterpreter CreateVarDeclarationNetworkInterpreter(ITokenStream tokens, int startToken)
        {
            NetworkInterpreter interpreter = CreateFullNetworkInterpreter(tokens, startToken);

            return interpreter;
        }

        private NetworkInterpreter CreateFullNetworkInterpreter(ITokenStream tokens, int startToken)
        {
            Network network = NetworkBuilder<JavaSimplifiedAtnBuilder>.GetOrBuildNetwork();

            NetworkInterpreter interpreter = new NetworkInterpreter(network, tokens);

            switch (startToken)
            {
            case Java2Lexer.CLASS:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ClassHeader));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.IdentifierSuffix));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                break;

            case Java2Lexer.INTERFACE:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceHeader));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AnnotationInterfaceHeader));
                break;

            case Java2Lexer.ENUM:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.EnumHeader));
                break;

            case Java2Lexer.COMMA:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeParameters));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.EnumBody));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.EnumConstants));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeList));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FieldDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeArguments));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.QualifiedNameList));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FormalParameterDecls));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ElementValuePairs));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ElementValueArrayInitializer));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.LocalVariableDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ExpressionList));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ArrayInitializer));
                interpreter.ExcludedStartRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceFieldDeclaration));
                break;

            case Java2Lexer.COLON:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Statement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.SwitchLabel));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ForStatement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AssignmentOperator));
                break;

            case Java2Lexer.EQ:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.VariableDeclarator));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ElementValuePair));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AssignmentOperator));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.RelationalOp));
                break;

            case Java2Lexer.EXTENDS:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.NormalClassExtends));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ExtendsTypeList));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeParameter));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeArgument));
                break;

            case Java2Lexer.LPAREN:
            case Java2Lexer.RPAREN:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FormalParameters));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Annotation));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AnnotationMethodDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.CatchClause));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ForStatement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ParExpression));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.CastExpression));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Arguments));
                break;

            case Java2Lexer.GT:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeParameters));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeArguments));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AssignmentOperator));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.RelationalOp));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ShiftOp));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.NonWildcardTypeArguments));
                break;

            case Java2Lexer.LBRACKET:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.MethodDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.VariableDeclarator));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Type));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.NormalParameterDecl));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FormalParameter));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Primary));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.IdentifierSuffix));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Selector));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ArrayCreator));
                interpreter.ExcludedStartRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceMethodDeclaration));
                break;

            case Java2Lexer.SEMI:
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.PackageDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ImportDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.EnumBodyDeclarations));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ClassBodyDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.MethodDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FieldDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceBodyDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceMethodDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.InterfaceFieldDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ExplicitConstructorInvocation));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AnnotationTypeElementDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.AnnotationMethodDeclaration));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.LocalVariableDeclarationStatement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Statement));
                interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ForStatement));
                break;

            default:
                break;
            }

            //// make sure we can handle forward walking from 'package'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.PackageClause));
            //// make sure we can handle forward walking from 'import'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ImportDecl));
            //// make sure we can handle forward walking from 'type'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeDecl));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.TypeSwitchGuard));
            //// make sure we can handle forward walking from 'const'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ConstDecl));
            //// make sure we can handle forward walking from 'var'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.VarDecl));
            //// make sure we can handle forward walking from 'func'
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FunctionType));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.FunctionDeclHeader));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.MethodDeclHeader));

            //// make sure we can handle forward and backward walking from ':='
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.ShortVarDecl));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.SimpleStmt));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.RangeClause));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.CommCase));

            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.Expression));
            //interpreter.BoundaryRules.Add(network.GetRule(JavaAtnBuilder.RuleNames.PrimaryExpr));

            return interpreter;
        }

        private static IEnumerable<ITagSpan<IClassificationTag>> ClassifyTokens(ITextSnapshot snapshot, IEnumerable<IToken> tokens, IClassificationTag classificationTag)
        {
            foreach (var token in tokens)
            {
                SnapshotSpan span = new SnapshotSpan(snapshot, Span.FromBounds(token.StartIndex, token.StopIndex + 1));
                yield return new TagSpan<IClassificationTag>(span, classificationTag);
            }
        }

        private class TokenIndexEqualityComparer : IEqualityComparer<IToken>
        {
            private static readonly TokenIndexEqualityComparer _default = new TokenIndexEqualityComparer();

            public static TokenIndexEqualityComparer Default
            {
                get
                {
                    return _default;
                }
            }

            public bool Equals(IToken x, IToken y)
            {
                if (x == null)
                    return y == null;

                if (y == null)
                    return false;

                return x.TokenIndex == y.TokenIndex;
            }

            public int GetHashCode(IToken obj)
            {
                return obj.TokenIndex.GetHashCode();
            }
        }

        private class ArrayTokenSource : ITokenSource
        {
            private readonly IEnumerable<IToken> _tokens;
            private readonly IEnumerator<IToken> _tokenEnumerator;

            private int _count = 0;
            private IToken _eofToken;

            public ArrayTokenSource(IEnumerable<IToken> tokens)
            {
                Contract.Requires<ArgumentNullException>(tokens != null, "tokens");

                _tokens = tokens;
                _tokenEnumerator = tokens.GetEnumerator();
            }

            public ArrayTokenSource(params IToken[] tokens)
            {
                Contract.Requires<ArgumentNullException>(tokens != null, "tokens");

                _tokens = tokens;
                _tokenEnumerator = _tokens.GetEnumerator();
            }

            public ArrayTokenSource(params int[] tokens)
            {
                Contract.Requires<ArgumentNullException>(tokens != null, "tokens");

                _tokens = tokens.Select(
                    (tokenType, index) => new CommonToken(tokenType)
                    {
                        TokenIndex = index
                    });

                _tokenEnumerator = _tokens.GetEnumerator();
            }

            public IToken NextToken()
            {
                if (_eofToken != null)
                    return _eofToken;

                if (_tokenEnumerator.MoveNext())
                {
                    _count++;
                    return _tokenEnumerator.Current;
                }

                _eofToken = new CommonToken(CharStreamConstants.EndOfFile)
                {
                    TokenIndex = _count
                };

                return _eofToken;
            }

            public string SourceName
            {
                get
                {
                    return string.Empty;
                }
            }

            public string[] TokenNames
            {
                get
                {
                    return new string[0];
                }
            }
        }
    }
}
