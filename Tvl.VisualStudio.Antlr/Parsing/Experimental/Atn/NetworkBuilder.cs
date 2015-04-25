namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Contract = System.Diagnostics.Contracts.Contract;
    using StringBuilder = System.Text.StringBuilder;

    public abstract class NetworkBuilder
    {
        protected abstract IList<RuleBinding> Rules
        {
            get;
        }

        public Network BuildNetwork()
        {
            return BuildNetworkImpl();
        }

        protected abstract void BindRules();

        protected virtual Network BuildNetworkImpl()
        {
            BindRules();

            HashSet<State> states = GetAllStates(Rules);
            HashSet<State> reachableStates = GetReachableStates(Rules);
            HashSet<State> ruleStartStates = new HashSet<State>(Rules.Where(i => i.IsStartRule).Select(i => i.StartState), ObjectReferenceEqualityComparer<State>.Default);

            StateOptimizer optimizer = new StateOptimizer(states);

            Dictionary<int, RuleBinding> stateRules = new Dictionary<int, RuleBinding>();
            foreach (var rule in Rules)
                GetRuleStates(optimizer, rule, rule.StartState, stateRules);

            Dictionary<int, RuleBinding> contextRules = new Dictionary<int, RuleBinding>();
            foreach (var state in reachableStates)
                GetContextRules(state, stateRules, contextRules);

#if !OLD_OPTIMIZER
            optimizer.Optimize(ruleStartStates);
            RemoveUnreachableStates(optimizer, Rules, states, ruleStartStates);
#else
            foreach (var state in states)
                state.RemoveExtraEpsilonTransitions(optimizer, ruleStartStates.Contains(state));

            foreach (var state in states)
                state.AddRecursiveTransitions(optimizer);

            RemoveUnreachableStates(optimizer, Rules, states, ruleStartStates);

            //ExportDot(AlloyParser.tokenNames, ruleBindings, reachableStates, stateRules, @"C:\dev\SimpleC\TestGenerated\AlloySimplified.dot");
            //ExportDgml(AlloyParser.tokenNames, ruleBindings, reachableStates, stateRules, @"C:\dev\SimpleC\TestGenerated\AlloySimplified.dgml");


            int skippedCount = 0;
            int optimizedCount = 0;
            foreach (var state in reachableStates)
            {
                bool skip = false;

                /* if there are no incoming transitions and it's not a rule start state,
                 * then the state is unreachable and will be removed so there's no need to
                 * optimize it.
                 */
                if (!ruleStartStates.Contains(state) && state.OutgoingTransitions.Count > 0)
                {
                    if (state.IncomingTransitions.Count == 0)
                        skip = true;

                    if (!skip && state.IncomingTransitions.All(i => i.IsEpsilon))
                        skip = true;

                    if (!skip && !state.IncomingTransitions.Any(i => i.IsMatch) && !state.OutgoingTransitions.Any(i => i.IsMatch))
                    {
                        bool incomingPush = state.IncomingTransitions.Any(i => i is PushContextTransition);
                        bool incomingPop = state.IncomingTransitions.Any(i => i is PopContextTransition);
                        bool outgoingPush = state.OutgoingTransitions.Any(i => i is PushContextTransition);
                        bool outgoingPop = state.OutgoingTransitions.Any(i => i is PopContextTransition);
                        if ((incomingPop && !outgoingPush) || (incomingPush && !outgoingPop))
                            skip = true;
                    }
                }

                if (skip)
                {
                    skippedCount++;
                    continue;
                }

                state.Optimize(optimizer);
                optimizedCount++;
            }

            HashSet<State> reachableOptimizedStates = GetReachableStates(Rules);

            int removed = RemoveUnreachableStates(optimizer, Rules, reachableStates, ruleStartStates);

            foreach (var state in reachableOptimizedStates)
            {
                if (!state.IsOptimized && (state.OutgoingTransitions.Count == 0 || state.IncomingTransitions.Any(i => i.IsRecursive)))
                {
                    bool hadTransitions = state.OutgoingTransitions.Count > 0;
                    state.Optimize(optimizer);
                    if (hadTransitions)
                        removed = RemoveUnreachableStates(optimizer, Rules, reachableStates, ruleStartStates);
                }
            }

            //RemoveUnreachableStates(Rules, reachableStates, ruleStartStates);

            //ExportDot(AlloyParser.tokenNames, ruleBindings, reachableOptimizedStates, stateRules, @"C:\dev\SimpleC\TestGenerated\AlloySimplifiedOptimized.dot");
            //ExportDgml(AlloyParser.tokenNames, ruleBindings, reachableOptimizedStates, stateRules, @"C:\dev\SimpleC\TestGenerated\AlloySimplifiedOptimized.dgml");

#if false
            foreach (var rule in ruleBindings)
                OptimizeRule(rule, ruleStartStates);
#endif

            reachableOptimizedStates = GetReachableStates(Rules);
            foreach (var state in reachableOptimizedStates)
                state.Optimize(optimizer);

            RemoveUnreachableStates(optimizer, Rules, reachableStates, ruleStartStates);
            reachableOptimizedStates = GetReachableStates(Rules);

            //stateRules = RenumberStates(reachableOptimizedStates, reachableStates, stateRules);
#endif

            return new Network(this, optimizer, Rules, stateRules, contextRules);
        }

        protected virtual void TryBindRule(RuleBinding ruleBinding, Nfa nfa)
        {
            Contract.Requires(ruleBinding != null);
            if (nfa == null)
                return;

            Nfa.BindRule(ruleBinding, nfa);
        }

        protected virtual void GetContextRules(State state, Dictionary<int, RuleBinding> stateRules, Dictionary<int, RuleBinding> contextRules)
        {
            foreach (var transition in state.OutgoingTransitions.OfType<PushContextTransition>())
                contextRules[transition.ContextIdentifiers[0]] = stateRules[state.Id];

            foreach (var transition in state.IncomingTransitions.OfType<PopContextTransition>())
                contextRules[transition.ContextIdentifiers.Last()] = stateRules[state.Id];
        }

        protected virtual void GetRuleStates(StateOptimizer optimizer, RuleBinding ruleName, State state, Dictionary<int, RuleBinding> stateRules)
        {
            if (stateRules.ContainsKey(state.Id))
                return;

            stateRules[state.Id] = ruleName;

            foreach (var transition in state.OutgoingTransitions)
            {
                if (transition is PopContextTransition)
                    continue;

                PushContextTransition contextTransition = transition as PushContextTransition;
                if (contextTransition != null)
                {
                    foreach (var popTransition in optimizer.GetPopContextTransitions(contextTransition))
                        GetRuleStates(optimizer, ruleName, popTransition.TargetState, stateRules);
                }
                else
                {
                    GetRuleStates(optimizer, ruleName, transition.TargetState, stateRules);
                }
            }
        }

        protected virtual HashSet<State> GetAllStates(IEnumerable<RuleBinding> rules)
        {
            HashSet<State> states = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            foreach (var rule in rules)
                GetReachableStates(rule, states, true);

            return states;
        }

        protected virtual HashSet<State> GetReachableStates(IEnumerable<RuleBinding> rules)
        {
            HashSet<State> reachableStates = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            foreach (var rule in rules.Where(i => i.IsStartRule))
                GetReachableStates(rule, reachableStates, false);

            return reachableStates;
        }

        protected virtual void GetReachableStates(RuleBinding rule, HashSet<State> states, bool trackIncoming)
        {
            if (states.Add(rule.StartState))
                GetReachableStates(rule.StartState, states, trackIncoming);
        }

        protected virtual void GetReachableStates(State state, HashSet<State> states, bool trackIncoming)
        {
            foreach (var transition in state.OutgoingTransitions)
            {
                if (states.Add(transition.TargetState))
                    GetReachableStates(transition.TargetState, states, trackIncoming);
            }

            if (trackIncoming)
            {
                foreach (var transition in state.IncomingTransitions)
                {
                    if (states.Add(transition.SourceState))
                        GetReachableStates(transition.SourceState, states, trackIncoming);
                }
            }
        }

        protected virtual int RemoveUnreachableStates(StateOptimizer optimizer, IEnumerable<RuleBinding> rules, HashSet<State> states, HashSet<State> ruleStartStates)
        {
            int removedCount = 0;

            while (true)
            {
                HashSet<State> reachableStates = GetReachableStates(rules);

                bool removed = false;

                foreach (var state in states)
                {
                    // already removed (or a terminal state)
                    if (state.OutgoingTransitions.Count == 0)
                        continue;

                    /* if there are no incoming transitions and it's not a rule start state,
                     * then the state is unreachable so we remove it.
                     */
                    if (!reachableStates.Contains(state))
                    {
                        removedCount++;
                        removed = true;
                        foreach (var transition in state.OutgoingTransitions.ToArray())
                            state.RemoveTransition(transition, optimizer);
                    }
                }

                if (!removed)
                    break;
            }

#if DEBUG
            int recursiveStates = GetReachableStates(rules).Count(i => i.HasRecursiveTransitions ?? true);
#endif

            return removedCount;
        }

        protected virtual Dictionary<int, RuleBinding> RenumberStates(HashSet<State> reachableStates, HashSet<State> allStates, Dictionary<int, RuleBinding> stateRules)
        {
            HashSet<State> unreachableStates = new HashSet<State>(allStates, allStates.Comparer);
            unreachableStates.ExceptWith(reachableStates);

            Dictionary<int, int> remapping = new Dictionary<int, int>();
            int currentState = 0;
            // favor the reachable states for low numbers since we may use a bitarray to track visited states
            foreach (var state in reachableStates)
            {
                remapping[state.Id] = currentState;
                state.Id = currentState;
                currentState++;
            }

            foreach (var state in unreachableStates)
            {
                remapping[state.Id] = currentState;
                state.Id = currentState;
                currentState++;
            }

            /* no more need to renumber contexts because the contextRules mapping is independent of the stateRules mapping */
            //foreach (var state in allStates)
            //{
            //    foreach (var transition in state.OutgoingTransitions.OfType<ContextTransition>())
            //    {
            //        for (int i = 0; i < transition.ContextIdentifiers.Count; i++)
            //            transition.ContextIdentifiers[i] = remapping[transition.ContextIdentifiers[i]];
            //    }
            //}

            Dictionary<int, RuleBinding> updatedStateRules = new Dictionary<int, RuleBinding>(stateRules.Comparer);
            foreach (var pair in stateRules)
            {
                updatedStateRules.Add(remapping[pair.Key], pair.Value);
            }

            return updatedStateRules;
        }

        #region graph output

        private static void ExportDot(IList<string> tokenNames, List<RuleBinding> rules, HashSet<State> reachableStates, Dictionary<int, RuleBinding> stateRules, string path)
        {
            Contract.Requires(rules != null);
            Contract.Requires(reachableStates != null);
            Contract.Requires(stateRules != null);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("digraph G {");

            foreach (var rule in rules)
            {
                builder.AppendLine(string.Format("subgraph rule_{0} {{", rule.Name));

                foreach (var state in reachableStates.Where(i => stateRules[i.Id] == rule))
                {
                    builder.AppendLine(string.Format("state_{0}[label=\"{1}\"]", state.Id, GetStateLabel(state)));
                }

                builder.AppendLine(string.Format("label = \"{0}\"", rule.Name));
                builder.AppendLine("}");
            }

            foreach (var state in reachableStates)
            {
                // now define the transitions
                foreach (var transition in state.OutgoingTransitions)
                {
                    builder.AppendLine(string.Format("state_{0} -> state_{1}[label=\"{2}\"]", transition.SourceState.Id, transition.TargetState.Id, GetTransitionLabel(transition, tokenNames)));
                }
            }

            builder.AppendLine("}");
            System.IO.File.WriteAllText(path, builder.ToString());
        }

        private static void ExportDgml(IList<string> tokenNames, List<RuleBinding> rules, HashSet<State> reachableStates, Dictionary<int, RuleBinding> stateRules, string path)
        {
            Contract.Requires(rules != null);
            Contract.Requires(reachableStates != null);
            Contract.Requires(stateRules != null);

            List<XElement> extraLinks = new List<XElement>();
            XElement nodes = GetNodes(rules, reachableStates, stateRules, extraLinks);
            XElement links = GetLinks(tokenNames, reachableStates, stateRules, extraLinks);

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(Elements.DirectedGraph,
                    new XAttribute(Attributes.GraphDirection, GraphDirection.TopToBottom),
                    new XAttribute(Attributes.Layout, Layout.Sugiyama),
                    nodes,
                    links,
                    GetCategories(),
                    GetProperties(),
                    GetStyles()));

            document.Save(path);
        }

        private static XElement GetNodes(List<RuleBinding> rules, HashSet<State> reachableStates, Dictionary<int, RuleBinding> stateRules, List<XElement> extraLinks)
        {
            Contract.Requires(rules != null);
            Contract.Requires(reachableStates != null);
            Contract.Requires(stateRules != null);
            Contract.Requires(extraLinks != null);

            Contract.Ensures(Contract.Result<XElement>() != null);

            Dictionary<State, XElement> nodes = new Dictionary<State, XElement>();
            List<XElement> extraNodes = new List<XElement>();

            foreach (var rule in rules)
            {
                extraNodes.Add(new XElement(Elements.Node,
                    new XAttribute(Attributes.Id, "rule_" + rule.Name),
                    new XAttribute(Attributes.Label, rule.Name),
                    new XAttribute(Attributes.Group, "Collapsed")));
            }

            foreach (var state in reachableStates)
            {
                string nodeCategory;
                if (state.OutgoingTransitions.Count == 0)
                    nodeCategory = Categories.StopState;
                else
                    nodeCategory = Categories.State;

                XElement node = new XElement(Elements.Node,
                    new XAttribute(Attributes.Id, "state_" + state.Id),
                    new XAttribute(Attributes.Label, GetStateLabel(state)),
                    new XAttribute(Attributes.Category, nodeCategory));

                nodes.Add(state, node);
                extraLinks.Add(CreateContainmentLink("rule_" + stateRules[state.Id].Name, "state_" + state.Id));
            }

            return new XElement(Elements.Nodes, nodes.Values.Concat(extraNodes));
        }

        private static XElement GetLinks(IList<string> tokenNames, HashSet<State> reachableStates, Dictionary<int, RuleBinding> stateRules, List<XElement> extraLinks)
        {
            Contract.Requires(reachableStates != null);
            Contract.Requires(stateRules != null);
            Contract.Requires(extraLinks != null);
            Contract.Ensures(Contract.Result<XElement>() != null);

            List<XElement> links = new List<XElement>();

            foreach (var state in reachableStates)
            {
                foreach (var transition in state.OutgoingTransitions)
                {
                    string transitionCategory;
                    if (transition.IsEpsilon)
                        transitionCategory = Categories.EpsilonEdge;
                    else if (transition.IsMatch)
                        transitionCategory = Categories.AtomEdge;
                    else if (transition is PushContextTransition)
                        transitionCategory = Categories.PushContextEdge;
                    else if (transition is PopContextTransition)
                        transitionCategory = Categories.PopContextEdge;
                    else
                        transitionCategory = Categories.Edge;

                    XElement link = new XElement(Elements.Link,
                        new XAttribute(Attributes.Source, "state_" + state.Id),
                        new XAttribute(Attributes.Target, "state_" + transition.TargetState.Id),
                        new XAttribute(Attributes.Category, transitionCategory),
                        new XAttribute(Attributes.Label, GetTransitionLabel(transition, tokenNames)));

                    links.Add(link);
                }
            }

            return new XElement(Elements.Links, links.Concat(extraLinks));
        }

        private static XElement CreateContainmentLink(string source, string target)
        {
            return new XElement(Elements.Link,
                new XAttribute(Attributes.Source, source),
                new XAttribute(Attributes.Target, target),
                new XAttribute(Attributes.Category, Categories.Contains));
        }

        private static string GetStateLabel(State state)
        {
            if (state == null)
                return "null";

            if (state.IsOptimized)
                return state.Id + "!";

            return state.Id.ToString();

            //string stateLabel = state.StateNumber.ToString();
            //DFAState dfaState = state as DFAState;
            //NFAState nfaState = state as NFAState;
            //if (dfaState != null)
            //{
            //    StringBuilder builder = new StringBuilder(250);
            //    builder.Append('s');
            //    builder.Append(state.StateNumber);
            //    if (AntlrTool.internalOption_ShowNFAConfigsInDFA)
            //    {
            //        if (dfaState.AbortedDueToRecursionOverflow)
            //        {
            //            builder.AppendLine();
            //            builder.AppendLine("AbortedDueToRecursionOverflow");
            //        }

            //        var alts = dfaState.AltSet;
            //        if (alts != null)
            //        {
            //            builder.AppendLine();
            //            List<int> altList = alts.OrderBy(i => i).ToList();
            //            ICollection<NFAConfiguration> configurations = dfaState.NfaConfigurations;
            //            for (int i = 0; i < altList.Count; i++)
            //            {
            //                int alt = altList[i];
            //                if (i > 0)
            //                    builder.AppendLine();
            //                builder.AppendFormat("alt{0}:", alt);
            //                // get a list of configs for just this alt
            //                // it will help us print better later
            //                List<NFAConfiguration> configsInAlt = new List<NFAConfiguration>();
            //                foreach (NFAConfiguration c in configurations)
            //                {
            //                    if (c.Alt != alt)
            //                        continue;

            //                    configsInAlt.Add(c);
            //                }

            //                int n = 0;
            //                for (int cIndex = 0; cIndex < configsInAlt.Count; cIndex++)
            //                {
            //                    NFAConfiguration c = configsInAlt[cIndex];
            //                    n++;
            //                    builder.Append(c.ToString(false));
            //                    if ((cIndex + 1) < configsInAlt.Count)
            //                    {
            //                        builder.Append(", ");
            //                    }
            //                    if (n % 5 == 0 && (configsInAlt.Count - cIndex) > 3)
            //                    {
            //                        builder.Append("\\n");
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    if (dfaState.IsAcceptState)
            //    {
            //        builder.Append("⇒" + dfaState.GetUniquelyPredictedAlt());
            //    }

            //    stateLabel = builder.ToString();
            //}
            //else if (nfaState != null)
            //{
            //    if (nfaState.IsDecisionState)
            //        stateLabel += ",d=" + nfaState.DecisionNumber;

            //    if (nfaState.endOfBlockStateNumber != State.INVALID_STATE_NUMBER)
            //        stateLabel += ",eob=" + nfaState.endOfBlockStateNumber;
            //}

            //return stateLabel;
        }

        private static string GetTransitionLabel(Transition transition, IList<string> tokenNames)
        {
            if (transition.IsEpsilon)
                return string.Empty;

            ContextTransition contextTransition = transition as ContextTransition;
            if (contextTransition != null)
            {
                string type = transition is PushContextTransition ? "push" : "pop";
                string context = string.Join(" ", contextTransition.ContextIdentifiers);

                //string sourceSet = transition.SourceState.IsOptimized ? transition.SourceState.GetSourceSet(preventContextType) : transition.SourceState.GetSourceSet();

                return string.Format("{0} {1}", type, context);
            }

            return transition.MatchSet.ToString(tokenNames);
        }

        private static XElement GetCategories()
        {
            return new XElement(Elements.Categories,
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.GrammarRule),
                    new XAttribute(Attributes.FontFamily, "Consolas")),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.LexerIdentifier),
                    new XAttribute(Attributes.BasedOn, Categories.LexerRule),
                    new XAttribute(Attributes.Foreground, Colors.Blue)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.LexerLiteral),
                    new XAttribute(Attributes.BasedOn, Categories.LexerRule),
                    new XAttribute(Attributes.Foreground, Colors.DarkGreen)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.LexerRule),
                    new XAttribute(Attributes.BasedOn, Categories.GrammarRule),
                    new XAttribute(Attributes.NodeRadius, 0)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.ParserRule),
                    new XAttribute(Attributes.BasedOn, Categories.GrammarRule),
                    new XAttribute(Attributes.Foreground, Colors.Purple)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.State)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.DecisionState),
                    new XAttribute(Attributes.BasedOn, Categories.State)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.StopState),
                    new XAttribute(Attributes.BasedOn, Categories.State)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.Edge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.ActionEdge),
                    new XAttribute(Attributes.BasedOn, Categories.Edge),
                    new XAttribute(Attributes.FontFamily, "Consolas")),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.EpsilonEdge),
                    new XAttribute(Attributes.FontFamily, "Times New Roman"),
                    new XAttribute(Attributes.FontStyle, "Italic")),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.RuleClosureEdge),
                    new XAttribute(Attributes.BasedOn, Categories.Edge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.AtomEdge),
                    new XAttribute(Attributes.BasedOn, Categories.Edge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.ContextEdge),
                    new XAttribute(Attributes.BasedOn, Categories.Edge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.PushContextEdge),
                    new XAttribute(Attributes.BasedOn, Categories.ContextEdge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.PopContextEdge),
                    new XAttribute(Attributes.BasedOn, Categories.ContextEdge)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.OptimizedEdge),
                    new XAttribute(Attributes.Stroke, Colors.Red),
                    new XAttribute(Attributes.Visibility, "Collapsed")),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.VerboseNode)),
                new XElement(Elements.Category,
                    new XAttribute(Attributes.Id, Categories.Contains),
                    new XAttribute(Attributes.IsContainment, true),
                    new XAttribute(Attributes.Label, Categories.Contains),
                    new XAttribute(Attributes.CanBeDataDriven, false),
                    new XAttribute(Attributes.CanLinkedNodesBeDataDriven, true),
                    new XAttribute(Attributes.IncomingActionLabel, "Contained By"),
                    new XAttribute(Attributes.OutgoingActionLabel, "Contains")));
        }

        private static XElement GetProperties()
        {
            return new XElement(Elements.Properties,
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.FontFamily),
                    new XAttribute(Attributes.DataType, "System.Windows.Media.FontFamily")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.Foreground),
                    new XAttribute(Attributes.Label, "Foreground"),
                    new XAttribute(Attributes.Description, "The foreground color"),
                    new XAttribute(Attributes.DataType, "System.Windows.Media.Brush")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.GraphDirection),
                    new XAttribute(Attributes.DataType, "Microsoft.VisualStudio.Progression.Layout.GraphDirection")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.Label),
                    new XAttribute(Attributes.Label, "Label"),
                    new XAttribute(Attributes.Description, "Displayable label of an Annotatable object"),
                    new XAttribute(Attributes.DataType, "System.String")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.Layout),
                    new XAttribute(Attributes.DataType, "System.String")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.NodeRadius),
                    new XAttribute(Attributes.DataType, "System.Double")),
                new XElement(Elements.Property,
                    new XAttribute(Attributes.Id, Attributes.Shape),
                    new XAttribute(Attributes.DataType, "System.String")));
        }

        private static XElement GetStyles()
        {
            return new XElement(Elements.Styles,
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Node"),
                    new XAttribute(Attributes.GroupLabel, "Verbose State"),
                    new XAttribute(Attributes.ValueLabel, "Verbose State"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.VerboseNode))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Background),
                        new XAttribute(Attributes.Value, Colors.LightYellow))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Node"),
                    new XAttribute(Attributes.GroupLabel, "Stop State"),
                    new XAttribute(Attributes.ValueLabel, "Stop State"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.StopState))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.Black)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.StrokeThickness),
                        new XAttribute(Attributes.Value, 2))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Node"),
                    new XAttribute(Attributes.GroupLabel, "Decision State"),
                    new XAttribute(Attributes.ValueLabel, "Decision State"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.DecisionState))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.Black)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.StrokeThickness),
                        new XAttribute(Attributes.Value, 1)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.StrokeDashArray),
                        new XAttribute(Attributes.Value, "2,2"))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Link"),
                    new XAttribute(Attributes.GroupLabel, "Epsilon Edge"),
                    new XAttribute(Attributes.ValueLabel, "Epsilon Edge"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.EpsilonEdge))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.StrokeDashArray),
                        new XAttribute(Attributes.Value, "2,2"))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Link"),
                    new XAttribute(Attributes.GroupLabel, "Rule Closure Edge"),
                    new XAttribute(Attributes.ValueLabel, "Rule Closure Edge"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.RuleClosureEdge))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.Purple)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.FontFamily),
                        new XAttribute(Attributes.Value, "Consolas"))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Link"),
                    new XAttribute(Attributes.GroupLabel, "Atom Edge"),
                    new XAttribute(Attributes.ValueLabel, "Atom Edge"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.AtomEdge))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.DarkBlue)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.FontFamily),
                        new XAttribute(Attributes.Value, "Consolas"))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Link"),
                    new XAttribute(Attributes.GroupLabel, "Push Context Edge"),
                    new XAttribute(Attributes.ValueLabel, "Push Context Edge"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.PushContextEdge))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.DarkGreen)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.FontFamily),
                        new XAttribute(Attributes.Value, "Consolas"))),
                new XElement(Elements.Style,
                    new XAttribute(Attributes.TargetType, "Link"),
                    new XAttribute(Attributes.GroupLabel, "Pop Context Edge"),
                    new XAttribute(Attributes.ValueLabel, "Pop Context Edge"),
                    new XElement(Elements.Condition,
                        new XAttribute(Attributes.Expression, string.Format("HasCategory('{0}')", Categories.PopContextEdge))),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.Stroke),
                        new XAttribute(Attributes.Value, Colors.DarkRed)),
                    new XElement(Elements.Setter,
                        new XAttribute(Attributes.Property, Attributes.FontFamily),
                        new XAttribute(Attributes.Value, "Consolas"))));
        }

        private static class GraphDirection
        {
            public const string LeftToRight = "LeftToRight";
            public const string TopToBottom = "TopToBottom";
            public const string RightToLeft = "TopToBottom";
            public const string BottomToTop = "BottomToTop";
        }

        private static class Layout
        {
            public const string None = "None";
            public const string Sugiyama = "Sugiyama";
            public const string ForceDirected = "ForceDirected";
            public const string DependencyMatrix = "DependencyMatrix";
        }

        private static class Elements
        {
            private static readonly XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";
            public static readonly XName DirectedGraph = ns + "DirectedGraph";
            public static readonly XName Nodes = ns + "Nodes";
            public static readonly XName Node = ns + "Node";
            public static readonly XName Links = ns + "Links";
            public static readonly XName Link = ns + "Link";
            public static readonly XName Categories = ns + "Categories";
            public static readonly XName Category = ns + "Category";
            public static readonly XName Properties = ns + "Properties";
            public static readonly XName Property = ns + "Property";
            public static readonly XName Styles = ns + "Styles";
            public static readonly XName Style = ns + "Style";
            public static readonly XName Condition = ns + "Condition";
            public static readonly XName Setter = ns + "Setter";
        }

        private static class Attributes
        {
            public const string Id = "Id";
            public const string GraphDirection = "GraphDirection";
            public const string Layout = "Layout";
            public const string FontFamily = "FontFamily";
            public const string BasedOn = "BasedOn";
            public const string Background = "Background";
            public const string Foreground = "Foreground";
            public const string NodeRadius = "NodeRadius";
            public const string DataType = "DataType";
            public const string Label = "Label";
            public const string Source = "Source";
            public const string Target = "Target";
            public const string Category = "Category";
            public const string Shape = "Shape";
            public const string Description = "Description";
            public const string FontStyle = "FontStyle";
            public const string Ref = "Ref";
            public const string Stroke = "Stroke";
            public const string StrokeThickness = "StrokeThickness";
            public const string StrokeDashArray = "StrokeDashArray";
            public const string Visibility = "Visibility";
            public const string Expression = "Expression";
            public const string Property = "Property";
            public const string Value = "Value";
            public const string TargetType = "TargetType";
            public const string GroupLabel = "GroupLabel";
            public const string ValueLabel = "ValueLabel";
            public const string Reference = "Reference";
            public const string IsContainment = "IsContainment";
            public const string CanBeDataDriven = "CanBeDataDriven";
            public const string CanLinkedNodesBeDataDriven = "CanLinkedNodesBeDataDriven";
            public const string IncomingActionLabel = "IncomingActionLabel";
            public const string OutgoingActionLabel = "OutgoingActionLabel";
            public const string Group = "Group";
        }

        private static class Categories
        {
            public const string GrammarRule = "GrammarRule";
            public const string LexerIdentifier = "LexerIdentifier";
            public const string LexerLiteral = "LexerLiteral";
            public const string LexerRule = "LexerRule";
            public const string ParserRule = "ParserRule";
            public const string OptimizedEdge = "OptimizedEdge";
            public const string VerboseNode = "VerboseNode";
            public const string EpsilonEdge = "EpsilonEdge";
            public const string ActionEdge = "ActionEdge";
            public const string RuleClosureEdge = "RuleClosureEdge";
            public const string AtomEdge = "AtomEdge";
            public const string ContextEdge = "ContextEdge";
            public const string PopContextEdge = "PopContextEdge";
            public const string PushContextEdge = "PushContextEdge";
            public const string Edge = "Edge";
            public const string State = "State";
            public const string DecisionState = "DecisionState";
            public const string StopState = "StopState";
            public const string Contains = "Contains";
        }

        private static class Colors
        {
            public const string DarkBlue = "DarkBlue";
            public const string DarkGreen = "#FF008000";
            public const string DarkRed = "DarkRed";
            public const string Purple = "#FF800080";
            public const string Blue = "#FF00008B";
            public const string Red = "#FFFF0000";
            public const string Black = "#FF000000";
            public const string LightYellow = "LightYellow";
        }

        private static class Shapes
        {
            public const string None = "None";
            public const string Rectangle = "Rectangle";
        }
        #endregion
    }
}
