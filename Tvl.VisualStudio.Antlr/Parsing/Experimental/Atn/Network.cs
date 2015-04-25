namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class Network
    {
        private readonly NetworkBuilder _builder;
        private readonly List<RuleBinding> _rules;
        private readonly Dictionary<int, State> _states;
        private readonly List<Transition> _transitions;
        private readonly Dictionary<int, RuleBinding> _stateRules;
        private readonly Dictionary<int, RuleBinding> _contextRules;
        private readonly StateOptimizer _optimizer;

        public Network(NetworkBuilder builder, StateOptimizer optimizer, IEnumerable<RuleBinding> rules, Dictionary<int, RuleBinding> stateRules, Dictionary<int, RuleBinding> contextRules)
        {
            Contract.Requires<ArgumentNullException>(builder != null, "builder");
            Contract.Requires<ArgumentNullException>(optimizer != null, "optimizer");
            Contract.Requires<ArgumentNullException>(rules != null, "rules");

            _builder = builder;
            _rules = new List<RuleBinding>(rules);

            //Dictionary<int, string> stateRules = new Dictionary<int, string>();
            //foreach (var rule in _rules)
            //{
            //    stateRules[rule.StartState.Id] = rule.Name;
            //    stateRules[rule.EndState.Id] = rule.Name;
            //}

            HashSet<State> states = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            HashSet<Transition> transitions = new HashSet<Transition>(ObjectReferenceEqualityComparer<Transition>.Default);

            foreach (var rule in _rules)
            {
                ExtractStatesAndTransitions(optimizer, rule, rule.StartState, states, transitions, stateRules, contextRules);
                //ExtractStatesAndTransitions(rule.Name, rule.EndState, states, transitions, stateRules, contextRules);
            }

            _states = states.ToDictionary(i => i.Id);
            _transitions = new List<Transition>(transitions);
            _stateRules = stateRules;
            _contextRules = contextRules;
            _optimizer = optimizer;
        }

        public NetworkBuilder Builder
        {
            get
            {
                return _builder;
            }
        }

        public StateOptimizer Optimizer
        {
            get
            {
                return _optimizer;
            }
        }

        private static void ExtractStatesAndTransitions(StateOptimizer optimizer, RuleBinding currentRule, State currentState, HashSet<State> states, HashSet<Transition> transitions, Dictionary<int, RuleBinding> stateRules, Dictionary<int, RuleBinding> contextRules)
        {
            if (!states.Add(currentState))
                return;

            currentRule = currentRule ?? stateRules[currentState.Id];

            foreach (var transition in currentState.OutgoingTransitions)
            {
                transitions.Add(transition);
                var nextRule = transition.IsContext ? null : currentRule;
                ExtractStatesAndTransitions(optimizer, nextRule, transition.TargetState, states, transitions, stateRules, contextRules);

//                if (transitions.Add(transition))
//                {
//                    if (transition.IsContext)
//                    {
//                        PushContextTransition pushContext = transition as PushContextTransition;
//                        if (pushContext != null)
//                        {
//                            foreach (var popTransition in optimizer.GetPopContextTransitions(pushContext))
//                            {
//                                // the matching pop transitions should always end in this rule
//                                Contract.Assert(popTransition.ContextIdentifiers.Last() == pushContext.ContextIdentifiers.First());
//#if ALL_CHECKS
//                                // matching is symmetric
//                                Contract.Assert(popTransition.PushTransitions.Contains(pushContext));
//#endif
//                                // make sure there are no "matching" transitions which were removed by a call to State.RemoveTransition
//                                Contract.Assert(popTransition.SourceState != null);

//                                ExtractStatesAndTransitions(optimizer, currentRule, popTransition.TargetState, states, transitions, stateRules, contextRules);
//                            }

//                            ExtractStatesAndTransitions(optimizer, null, transition.TargetState, states, transitions, stateRules, contextRules);
//                            continue;
//                        }

//                        PopContextTransition popContext = transition as PopContextTransition;
//                        if (popContext != null)
//                        {
//                            foreach (var pushTransition in optimizer.GetPushContextTransitions(popContext))
//                            {
//                                // the matching push transitions should always start in this rule
//                                Contract.Assert(pushTransition.ContextIdentifiers.First() == popContext.ContextIdentifiers.Last());
//#if ALL_CHECKS
//                                // matching is symmetric
//                                Contract.Assert(pushTransition.PopTransitions.Contains(popContext));
//#endif
//                                // make sure there are no "matching" transitions which were removed by a call to State.RemoveTransition
//                                Contract.Assert(pushTransition.SourceState != null);
//                            }

//                            ExtractStatesAndTransitions(optimizer, null, transition.TargetState, states, transitions, stateRules, contextRules);
//                            continue;
//                        }

//                        throw new InvalidOperationException("Unrecognized context transition.");
//                    }
//                    else
//                    {
//                        ExtractStatesAndTransitions(optimizer, currentRule, transition.TargetState, states, transitions, stateRules, contextRules);
//                    }
//                }
            }

            foreach (var transition in currentState.IncomingTransitions)
            {
                transitions.Add(transition);
                var nextRule = transition.IsContext ? null : currentRule;
                ExtractStatesAndTransitions(optimizer, nextRule, transition.SourceState, states, transitions, stateRules, contextRules);
            }
        }

        public IDictionary<int, State> States
        {
            get
            {
                return _states;
            }
        }

        public ReadOnlyCollection<Transition> Transitions
        {
            get
            {
                return _transitions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<RuleBinding> Rules
        {
            get
            {
                return _rules.AsReadOnly();
            }
        }

        public Dictionary<int, RuleBinding> StateRules
        {
            get
            {
                return _stateRules;
            }
        }

        public Dictionary<int, RuleBinding> ContextRules
        {
            get
            {
                return _contextRules;
            }
        }

        public RuleBinding GetRule(string name)
        {
            return _rules.Single(i => string.Equals(i.Name, name));
        }

#if false
        private readonly Dictionary<long, bool> _nestedContexts = new Dictionary<long, bool>();
        private readonly Dictionary<long, bool> _contextStates = new Dictionary<long, bool>();

        public bool CanNestContexts(int outerContext, int innerContext)
        {
            return true;

            long key = ((uint)outerContext << 32) | (uint)innerContext;
            bool result;
            if (_nestedContexts.TryGetValue(key, out result))
                return result;

            List<PushContextTransition> transitions =
                this.Transitions.OfType<PushContextTransition>()
                .Where(i => i.ContextIdentifiers.Contains(outerContext))
                .ToList();

            // first look for a directly nested transition
            foreach (PushContextTransition transition in transitions)
            {
                if (transition.ContextIdentifiers[transition.ContextIdentifiers.Count - 1] == outerContext)
                    continue;

                if (transition.ContextIdentifiers[transition.ContextIdentifiers.IndexOf(outerContext) + 1] == innerContext)
                {
                    result = true;
                    goto end;
                }
            }

            foreach (PushContextTransition transition in transitions)
            {
                if (transition.ContextIdentifiers[transition.ContextIdentifiers.Count - 1] == outerContext)
                {
                    foreach (var nested in GetEnclosedStates(transition).SelectMany(i => i.OutgoingTransitions).OfType<PushContextTransition>())
                    {
                        if (nested.ContextIdentifiers[0] == innerContext)
                        {
                            result = true;
                            goto end;
                        }
                    }
                }
            }

        end:

            _nestedContexts[key] = result;
            return result;
        }

        public bool IsStateInContext(int context, int state)
        {
            return true;

            long key = ((uint)context << 32) | (uint)state;
            bool result;
            if (_contextStates.TryGetValue(key, out result))
                return result;

            List<PushContextTransition> transitions =
                this.Transitions.OfType<PushContextTransition>()
                .Where(i => i.ContextIdentifiers[i.ContextIdentifiers.Count - 1] == context)
                .ToList();

            foreach (var enclosedState in transitions.SelectMany(GetEnclosedStates))
            {
                if (enclosedState.Id == state)
                {
                    result = true;
                    break;
                }
            }

            _contextStates[key] = result;
            return result;
        }

        private IEnumerable<State> GetEnclosedStates(PushContextTransition transition)
        {
            HashSet<State> states = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            states.Add(transition.TargetState);
            GetSiblingStates(transition.TargetState, states);
            return states;
        }

        private void GetSiblingStates(State currentState, HashSet<State> states)
        {
            foreach (var transition in currentState.OutgoingTransitions)
            {
                if (transition.IsContext)
                {
                    PushContextTransition pushContextTransition = transition as PushContextTransition;
                    if (pushContextTransition == null)
                        continue;

                    foreach (var popTransition in Optimizer.GetPopContextTransitions(pushContextTransition))
                    {
                        if (states.Add(popTransition.TargetState))
                            GetSiblingStates(popTransition.TargetState, states);
                    }
                }
                else if (states.Add(transition.TargetState))
                {
                    GetSiblingStates(transition.TargetState, states);
                }
            }
        }
#endif
    }
}
