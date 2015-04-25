namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Antlr.Runtime;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;
    using Stopwatch = System.Diagnostics.Stopwatch;

    public class NetworkInterpreter
    {
        public const int UnknownSymbol = -3;

        private readonly Network _network;
        private readonly ITokenStream _input;

        private readonly List<InterpretTrace> _contexts = new List<InterpretTrace>();

        private bool _trackContextTransitions = false;
        private bool _trackBoundedContexts = false;
        private readonly HashSet<InterpretTrace> _boundedStartContexts = new HashSet<InterpretTrace>(BoundedStartInterpretTraceEqualityComparer.Default);
        private readonly HashSet<InterpretTrace> _boundedEndContexts = new HashSet<InterpretTrace>(BoundedEndInterpretTraceEqualityComparer.Default);
#if DFA
        private DeterministicTrace _deterministicTrace;
#endif

        private readonly HashSet<RuleBinding> _boundaryRules = new HashSet<RuleBinding>(ObjectReferenceEqualityComparer<RuleBinding>.Default);
        private readonly HashSet<RuleBinding> _excludedStartRules = new HashSet<RuleBinding>(ObjectReferenceEqualityComparer<RuleBinding>.Default);

        private int _lookBehindPosition = 0;
        private int _lookAheadPosition = 0;

        private bool _beginningOfFile;
        private bool _endOfFile;
        private bool _failedBackward;
        private bool _failedForward;

        public NetworkInterpreter(Network network, ITokenStream input)
        {
            Contract.Requires<ArgumentNullException>(network != null, "network");
            Contract.Requires<ArgumentNullException>(input != null, "input");

            _network = network;
            _input = input;
        }

        public Network Network
        {
            get
            {
                return _network;
            }
        }

        public ITokenStream Input
        {
            get
            {
                return _input;
            }
        }

        public List<InterpretTrace> Contexts
        {
            get
            {
                return _contexts;
            }
        }

        public bool BeginningOfFile
        {
            get
            {
                return _beginningOfFile;
            }
        }

        public bool EndOfFile
        {
            get
            {
                return _endOfFile;
            }
        }

        public bool FailedBackward
        {
            get
            {
                return _failedBackward;
            }
        }

        public bool FailedForward
        {
            get
            {
                return _failedForward;
            }
        }

        public bool Failed
        {
            get
            {
                return FailedBackward || FailedForward;
            }
        }

        public bool TrackContextTransitions
        {
            get
            {
                return _trackContextTransitions;
            }

            set
            {
                _trackContextTransitions = value;
            }
        }

        public bool TrackBoundedContexts
        {
            get
            {
                return _trackBoundedContexts;
            }

            set
            {
                _trackBoundedContexts = value;
            }
        }

        public ICollection<InterpretTrace> BoundedStartContexts
        {
            get
            {
                return _boundedStartContexts;
            }
        }

        public ICollection<InterpretTrace> BoundedEndContexts
        {
            get
            {
                return _boundedEndContexts;
            }
        }

        public ICollection<RuleBinding> BoundaryRules
        {
            get
            {
                return _boundaryRules;
            }
        }

        public ICollection<RuleBinding> ExcludedStartRules
        {
            get
            {
                return _excludedStartRules;
            }
        }

        public IntervalSet GetSourceSet()
        {
            int symbol = UnknownSymbol;
            int symbolPosition = _input.Index - _lookBehindPosition;

            if (_lookAheadPosition == 0 && _lookBehindPosition == 0 && _contexts.Count == 0)
            {
                IntervalSet allTokens = new IntervalSet();
                foreach (var transition in Network.Transitions.Where(i => i.IsMatch))
                    allTokens.UnionWith(transition.MatchSet);

                return allTokens;
            }

            Stopwatch updateTimer = Stopwatch.StartNew();

            List<InterpretTrace> existing = new List<InterpretTrace>(_contexts);
            SortedSet<int> states = new SortedSet<int>();
            HashSet<InterpretTrace> contexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);

            foreach (var context in existing)
            {
                states.Add(context.StartContext.State.Id);
                StepBackward(contexts, states, context, symbol, symbolPosition, PreventContextType.None);
                states.Clear();
            }

            IntervalSet result = new IntervalSet();
            if (contexts.Count > 0)
            {
                foreach (var context in contexts)
                {
                    var firstMatch = context.Transitions.First.Value;
                    if (firstMatch.Transition.IsMatch)
                        result.UnionWith(firstMatch.Transition.MatchSet);
                }
            }

            long nfaUpdateTime = updateTimer.ElapsedMilliseconds;

            return result;
        }

        public IntervalSet GetFollowSet()
        {
            int symbol = UnknownSymbol;
            int symbolPosition = _input.Index + _lookAheadPosition - 1;

            if (_lookAheadPosition == 0 && _lookBehindPosition == 0 && _contexts.Count == 0)
            {
                IntervalSet allTokens = new IntervalSet();
                foreach (var transition in Network.Transitions.Where(i => i.IsMatch))
                    allTokens.UnionWith(transition.MatchSet);

                return allTokens;
            }

            Stopwatch updateTimer = Stopwatch.StartNew();

            List<InterpretTrace> existing = new List<InterpretTrace>(_contexts);
            SortedSet<int> states = new SortedSet<int>();
            HashSet<InterpretTrace> contexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);

            foreach (var context in existing)
            {
                states.Add(context.EndContext.State.Id);
                StepForward(contexts, states, context, symbol, symbolPosition, PreventContextType.None);
                states.Clear();
            }

            IntervalSet result = new IntervalSet();
            if (contexts.Count > 0)
            {
                foreach (var context in contexts)
                {
                    var lastMatch = context.Transitions.Last.Value;
                    if (lastMatch.Transition.IsMatch)
                        result.UnionWith(lastMatch.Transition.MatchSet);
                }
            }

            long nfaUpdateTime = updateTimer.ElapsedMilliseconds;

            return result;
        }

        public void CombineBoundedStartContexts()
        {
            IList<InterpretTrace> contexts = _contexts.Distinct(BoundedStartInterpretTraceEqualityComparer.Default).ToList();
            if (contexts.Count != _contexts.Count)
            {
                _contexts.Clear();
                _contexts.AddRange(contexts);
            }
        }

        public void CombineBoundedEndContexts()
        {
            IList<InterpretTrace> contexts = _contexts.Distinct(BoundedEndInterpretTraceEqualityComparer.Default).ToList();
            if (contexts.Count != _contexts.Count)
            {
                _contexts.Clear();
                _contexts.AddRange(contexts);
            }
        }

        public bool TryStepBackward()
        {
            if (_failedBackward || _beginningOfFile)
                return false;

            if (_input.Index - _lookBehindPosition <= 0)
            {
                _beginningOfFile = true;
                return false;
            }

            IToken token = _input.LT(-1 - _lookBehindPosition);
            if (token == null)
            {
                _beginningOfFile = true;
                return false;
            }

            int symbol = token.Type;
            int symbolPosition = token.TokenIndex;

            /*
             * Update the non-deterministic trace
             */

            Stopwatch updateTimer = Stopwatch.StartNew();

            if (_lookAheadPosition == 0 && _lookBehindPosition == 0 && _contexts.Count == 0)
            {
                HashSet<InterpretTrace> initialContexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);

                /* create our initial set of states as the ones at the target end of a match transition
                 * that contains 'symbol' in the match set.
                 */
                List<Transition> transitions = new List<Transition>(_network.Transitions.Where(i => i.MatchesSymbol(symbol)));
                foreach (var transition in transitions)
                {
                    if (ExcludedStartRules.Contains(Network.StateRules[transition.SourceState.Id]))
                        continue;

                    if (ExcludedStartRules.Contains(Network.StateRules[transition.TargetState.Id]))
                        continue;

                    ContextFrame startContext = new ContextFrame(transition.TargetState, null, null, this);
                    ContextFrame endContext = new ContextFrame(transition.TargetState, null, null, this);
                    initialContexts.Add(new InterpretTrace(startContext, endContext));
                }

                _contexts.AddRange(initialContexts);

#if DFA
                DeterministicState deterministicState = new DeterministicState(_contexts.Select(i => i.StartContext));
                _deterministicTrace = new DeterministicTrace(deterministicState, deterministicState);
#endif
            }

            List<InterpretTrace> existing = new List<InterpretTrace>(_contexts);
            _contexts.Clear();
            SortedSet<int> states = new SortedSet<int>();
            HashSet<InterpretTrace> contexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);
#if false
            HashSet<ContextFrame> existingUnique = new HashSet<ContextFrame>(existing.Select(i => i.StartContext), EqualityComparer<ContextFrame>.Default);
            Contract.Assert(existingUnique.Count == existing.Count);
#endif

            foreach (var context in existing)
            {
                states.Add(context.StartContext.State.Id);
                StepBackward(contexts, states, context, symbol, symbolPosition, PreventContextType.None);
                states.Clear();
            }

            bool success = false;
            if (contexts.Count > 0)
            {
                _contexts.AddRange(contexts);
                if (TrackBoundedContexts)
                    _boundedStartContexts.UnionWith(_contexts.Where(i => i.BoundedStart));
                success = true;
            }
            else
            {
                _contexts.AddRange(existing);
            }

            long nfaUpdateTime = updateTimer.ElapsedMilliseconds;

#if DFA
            /*
             * Update the deterministic trace
             */

            updateTimer.Restart();

            DeterministicTransition deterministicTransition = _deterministicTrace.StartState.IncomingTransitions.SingleOrDefault(i => i.MatchSet.Contains(symbol));
            if (deterministicTransition == null)
            {
                DeterministicState sourceState = new DeterministicState(contexts.Select(i => i.StartContext));
                DeterministicState targetState = _deterministicTrace.StartState;
                deterministicTransition = targetState.IncomingTransitions.SingleOrDefault(i => i.SourceState.Equals(sourceState));
                if (deterministicTransition == null)
                {
                    deterministicTransition = new DeterministicTransition(targetState);
                    sourceState.AddTransition(deterministicTransition);
                }

                deterministicTransition.MatchSet.Add(symbol);
            }

            IEnumerable<DeterministicTraceTransition> deterministicTransitions = Enumerable.Repeat(new DeterministicTraceTransition(deterministicTransition, symbol, symbolPosition, this), 1);
            deterministicTransitions = deterministicTransitions.Concat(_deterministicTrace.Transitions);
            _deterministicTrace = new DeterministicTrace(deterministicTransition.SourceState, _deterministicTrace.EndState, deterministicTransitions);

            long dfaUpdateTime = updateTimer.ElapsedMilliseconds;
#endif

            if (success)
                _lookBehindPosition++;

            if (!success)
                _failedBackward = true;

            return success;
        }

        public bool TryStepForward()
        {
            if (_failedForward || _endOfFile)
                return false;

            if (_input.Index + _lookAheadPosition >= _input.Count)
            {
                _endOfFile = true;
                return false;
            }

            IToken token = _input.LT(-1 - _lookBehindPosition);
            if (token == null)
            {
                _endOfFile = true;
                return false;
            }

            int symbol = token.Type;
            int symbolPosition = token.TokenIndex;

            Stopwatch updateTimer = Stopwatch.StartNew();

            if (_lookAheadPosition == 0 && _lookBehindPosition == 0 && _contexts.Count == 0)
            {
                HashSet<InterpretTrace> initialContexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);

                /* create our initial set of states as the ones at the target end of a match transition
                 * that contains 'symbol' in the match set.
                 */
                List<Transition> transitions = new List<Transition>(_network.Transitions.Where(i => i.MatchesSymbol(symbol)));
                foreach (var transition in transitions)
                {
                    if (ExcludedStartRules.Contains(Network.StateRules[transition.SourceState.Id]))
                        continue;

                    if (ExcludedStartRules.Contains(Network.StateRules[transition.TargetState.Id]))
                        continue;

                    ContextFrame startContext = new ContextFrame(transition.SourceState, null, null, this);
                    ContextFrame endContext = new ContextFrame(transition.SourceState, null, null, this);
                    initialContexts.Add(new InterpretTrace(startContext, endContext));
                }

                _contexts.AddRange(initialContexts);
            }

            List<InterpretTrace> existing = new List<InterpretTrace>(_contexts);
            _contexts.Clear();
            SortedSet<int> states = new SortedSet<int>();
            HashSet<InterpretTrace> contexts = new HashSet<InterpretTrace>(EqualityComparer<InterpretTrace>.Default);
#if false
            HashSet<ContextFrame> existingUnique = new HashSet<ContextFrame>(existing.Select(i => i.StartContext), EqualityComparer<ContextFrame>.Default);
            Contract.Assert(existingUnique.Count == existing.Count);
#endif

            foreach (var context in existing)
            {
                states.Add(context.EndContext.State.Id);
                StepForward(contexts, states, context, symbol, symbolPosition, PreventContextType.None);
                states.Clear();
            }

            bool success = false;
            if (contexts.Count > 0)
            {
                _contexts.AddRange(contexts);
                if (TrackBoundedContexts)
                    _boundedEndContexts.UnionWith(_contexts.Where(i => i.BoundedEnd));
                success = true;
            }
            else
            {
                _contexts.AddRange(existing);
            }

            long nfaUpdateTime = updateTimer.ElapsedMilliseconds;

            if (success)
                _lookAheadPosition++;

            if (!success)
                _failedForward = true;

            return success;
        }

        private void StepBackward(ICollection<InterpretTrace> result, ICollection<int> states, InterpretTrace context, int symbol, int symbolPosition, PreventContextType preventContextType)
        {
            foreach (var transition in context.StartContext.State.IncomingTransitions)
            {
                if (transition.SourceState.IsOptimized)
                {
                    switch (preventContextType)
                    {
                    case PreventContextType.Pop:
                        if (!transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.PopRecursive:
                        if (transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.Push:
                        if (!transition.IsRecursive && (transition is PushContextTransition))
                            continue;

                        break;

                    case PreventContextType.PushRecursive:
                        if (transition.IsRecursive && (transition is PushContextTransition))
                            continue;

                        break;

                    default:
                        break;
                    }
                }

                InterpretTrace step;
                if (context.TryStepBackward(transition, symbol, symbolPosition, out step))
                {
                    if (transition.IsMatch)
                    {
                        result.Add(step);
                        continue;
                    }

                    bool recursive = transition.SourceState.IsBackwardRecursive;
                    if (recursive && states.Contains(transition.SourceState.Id))
                    {
                        // TODO: check postfix rule
                        continue;
                    }

                    if (recursive)
                        states.Add(transition.SourceState.Id);

                    PreventContextType nextPreventContextType = PreventContextType.None;
                    if (context.StartContext.State.IsOptimized)
                    {
                        if (transition is PushContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                        else if (transition is PopContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
                    }

                    StepBackward(result, states, step, symbol, symbolPosition, nextPreventContextType);

                    if (recursive)
                        states.Remove(transition.SourceState.Id);
                }
            }
        }

        private void StepForward(ICollection<InterpretTrace> result, ICollection<int> states, InterpretTrace context, int symbol, int symbolPosition, PreventContextType preventContextType)
        {
            foreach (var transition in context.EndContext.State.OutgoingTransitions)
            {
                if (transition.IsContext)
                {
                    PopContextTransition popContextTransition = transition as PopContextTransition;
                    if (popContextTransition != null && context.EndContext.Parent != null)
                    {
                        if (popContextTransition.ContextIdentifiers[0] != context.EndContext.Parent.Context)
                            continue;
                    }

                    switch (preventContextType)
                    {
                    case PreventContextType.Pop:
                        if (!transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.PopRecursive:
                        if (transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.Push:
                        if (!transition.IsRecursive && (transition is PushContextTransition))
                            continue;

                        break;

                    case PreventContextType.PushRecursive:
                        if (transition.IsRecursive && (transition is PushContextTransition))
                            continue;

                        break;

                    default:
                        break;
                    }
                }

                InterpretTrace step;
                if (context.TryStepForward(transition, symbol, symbolPosition, out step))
                {
                    if (transition.IsMatch)
                    {
                        result.Add(step);
                        continue;
                    }

                    bool recursive = transition.TargetState.IsForwardRecursive;
                    bool addRecursive = false;
                    if (recursive)
                    {
                        addRecursive = !states.Contains(transition.TargetState.Id);
                        if (!addRecursive && (!(transition is PopContextTransition) || context.EndContext.Parent == null))
                            continue;
                    }

                    if (addRecursive)
                        states.Add(transition.TargetState.Id);

                    PreventContextType nextPreventContextType = PreventContextType.None;
                    if (context.EndContext.State.IsOptimized)
                    {
                        if (transition is PushContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                        else if (transition is PopContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
                    }

                    StepForward(result, states, step, symbol, symbolPosition, nextPreventContextType);

                    if (addRecursive)
                        states.Remove(transition.TargetState.Id);
                }
            }
        }
    }
}
