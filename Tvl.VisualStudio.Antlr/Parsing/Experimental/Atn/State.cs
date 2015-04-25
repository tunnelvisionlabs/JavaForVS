namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;
    using PreventContextType = Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter.PreventContextType;

    public class State
    {
        private static int _nextState = 0;

        private ICollection<Transition> _outgoingTransitions = new List<Transition>();
        private ICollection<Transition> _incomingTransitions = new List<Transition>();
        private int _id = _nextState++;

        private IntervalSet[] _sourceSet;
        private IntervalSet[] _followSet;
        private bool? _isForwardRecursive;
        private bool? _isBackwardRecursive;

        private bool? _recursiveTransitions;
        private bool _isOptimized;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public ICollection<Transition> OutgoingTransitions
        {
            get
            {
                return _outgoingTransitions;
            }
        }

        public ICollection<Transition> IncomingTransitions
        {
            get
            {
                return _incomingTransitions;
            }
        }

        public bool IsRecursiveAnalysisComplete
        {
            get
            {
                return _recursiveTransitions != null;
            }
        }

        public bool? HasRecursiveTransitions
        {
            get
            {
                return _recursiveTransitions;
            }
        }

        public bool IsOptimized
        {
            get
            {
                return _isOptimized;
            }

            internal set
            {
                _isOptimized = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsBackwardRecursive
        {
            get
            {
                if (_isBackwardRecursive == null)
                {
                    _isBackwardRecursive = false;

                    HashSet<Transition> visited = new HashSet<Transition>(ObjectReferenceEqualityComparer<Transition>.Default);
                    Queue<Transition> queue = new Queue<Transition>(_incomingTransitions);

                    while (queue.Count > 0)
                    {
                        Transition transition = queue.Dequeue();
                        if (!visited.Add(transition))
                            continue;

                        if (transition.IsEpsilon || transition.IsContext)
                        {
                            if (transition.SourceState == this)
                            {
                                _isBackwardRecursive = true;
                                break;
                            }

                            foreach (var incoming in transition.SourceState.IncomingTransitions)
                                queue.Enqueue(incoming);
                        }
                    }
                }

                return _isBackwardRecursive.Value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsForwardRecursive
        {
            get
            {
                if (_isForwardRecursive == null)
                {
                    _isForwardRecursive = false;

                    HashSet<Transition> visited = new HashSet<Transition>(ObjectReferenceEqualityComparer<Transition>.Default);
                    Queue<Transition> queue = new Queue<Transition>(_outgoingTransitions);

                    while (queue.Count > 0)
                    {
                        Transition transition = queue.Dequeue();
                        if (!visited.Add(transition))
                            continue;

                        if (transition.IsEpsilon || transition.IsContext)
                        {
                            if (transition.TargetState == this)
                            {
                                _isForwardRecursive = true;
                                break;
                            }

                            foreach (var outgoing in transition.TargetState.OutgoingTransitions)
                                queue.Enqueue(outgoing);
                        }
                    }
                }

                return _isForwardRecursive.Value;
            }
        }

        public void RemoveExtraEpsilonTransitions(StateOptimizer optimizer, bool isStartState)
        {
            if (OutgoingTransitions.Count == 1 && OutgoingTransitions.First().IsEpsilon)
            {
                Transition epsilon = OutgoingTransitions.First();
                foreach (var incoming in IncomingTransitions.ToArray())
                {
                    State sourceState = incoming.SourceState;
                    sourceState.RemoveTransitionInternal(incoming, optimizer);
                    sourceState.AddTransitionInternal(MergeTransitions(incoming, epsilon), optimizer);
                }

                if (!isStartState)
                    RemoveTransitionInternal(epsilon, optimizer);
            }

            if (IncomingTransitions.Count == 1 && IncomingTransitions.First().IsEpsilon)
            {
                Transition epsilon = IncomingTransitions.First();
                State sourceState = epsilon.SourceState;

                foreach (var outgoing in OutgoingTransitions.ToArray())
                {
                    if (!isStartState)
                        this.RemoveTransitionInternal(outgoing, optimizer);

                    sourceState.AddTransitionInternal(MergeTransitions(epsilon, outgoing), optimizer);
                }

                epsilon.SourceState.RemoveTransitionInternal(epsilon, optimizer);
            }
        }

        public void AddRecursiveTransitions(StateOptimizer optimizer)
        {
            if (_recursiveTransitions != null)
                return;

            int originalTransitionCount = OutgoingTransitions.Count;

            HashSet<State> visited = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            List<int> trace = new List<int>();
            AddRecursivePushTransitions(visited, this, new EpsilonTransition(this), trace, optimizer);
            AddRecursivePopTransitions(visited, this, new EpsilonTransition(this), trace, optimizer);

            _recursiveTransitions = OutgoingTransitions.Count != originalTransitionCount;
        }

        private bool AddRecursivePushTransitions(HashSet<State> visited, State currentState, Transition effectiveTransition, List<int> contexts, StateOptimizer optimizer)
        {
            Contract.Requires(visited != null);
            Contract.Requires(currentState != null);
            Contract.Requires(contexts != null);
            Contract.Ensures(visited.Count == Contract.OldValue(visited.Count));
            Contract.Ensures(contexts.Count == Contract.OldValue(contexts.Count));

            bool foundRecursive = false;

            foreach (var transition in currentState.OutgoingTransitions.Where(i => !i.IsRecursive && (i.IsEpsilon || (i is PushContextTransition))).ToArray())
            {
                ContextTransition contextTransition = transition as ContextTransition;
                try
                {
                    if (contextTransition != null)
                        contexts.AddRange(contextTransition.ContextIdentifiers);

                    if (transition.TargetState == this)
                    {
                        foundRecursive = true;

                        if (contexts.Count == 0)
                        {
                            Trace.WriteLine(string.Format("State {0} is self-recursive.", this.Id));
                            continue;
                        }

                        PushContextTransition recursive = new PushContextTransition(this, contexts);
                        recursive.IsRecursive = true;
                        AddTransitionInternal(recursive, optimizer);
                        continue;
                    }

                    if (!visited.Add(transition.TargetState))
                        continue;

                    try
                    {
                        AddRecursivePushTransitions(visited, transition.TargetState, MergeTransitions(effectiveTransition, transition), contexts, optimizer);
                    }
                    finally
                    {
                        visited.Remove(transition.TargetState);
                    }
                }
                finally
                {
                    if (contextTransition != null)
                        contexts.RemoveRange(contexts.Count - contextTransition.ContextIdentifiers.Count, contextTransition.ContextIdentifiers.Count);
                }
            }

            return foundRecursive;
        }

        private bool AddRecursivePopTransitions(HashSet<State> visited, State currentState, Transition effectiveTransition, List<int> contexts, StateOptimizer optimizer)
        {
            Contract.Requires(visited != null);
            Contract.Requires(currentState != null);
            Contract.Requires(contexts != null);
            Contract.Ensures(visited.Count == Contract.OldValue(visited.Count));
            Contract.Ensures(contexts.Count == Contract.OldValue(contexts.Count));

            bool foundRecursive = false;

            foreach (var transition in currentState.OutgoingTransitions.Where(i => !i.IsRecursive && (i.IsEpsilon || (i is PopContextTransition))).ToArray())
            {
                ContextTransition contextTransition = transition as ContextTransition;
                try
                {
                    if (contextTransition != null)
                        contexts.AddRange(contextTransition.ContextIdentifiers);

                    if (transition.TargetState == this)
                    {
                        foundRecursive = true;

                        if (contexts.Count == 0)
                        {
                            Trace.WriteLine(string.Format("State {0} is self-recursive.", this.Id));
                            continue;
                        }

                        PopContextTransition recursive = new PopContextTransition(this, contexts);
                        recursive.IsRecursive = true;
                        AddTransitionInternal(recursive, optimizer);
                        continue;
                    }

                    if (!visited.Add(transition.TargetState))
                        continue;

                    try
                    {
                        AddRecursivePopTransitions(visited, transition.TargetState, MergeTransitions(effectiveTransition, transition), contexts, optimizer);
                    }
                    finally
                    {
                        visited.Remove(transition.TargetState);
                    }
                }
                finally
                {
                    if (contextTransition != null)
                        contexts.RemoveRange(contexts.Count - contextTransition.ContextIdentifiers.Count, contextTransition.ContextIdentifiers.Count);
                }
            }

            return foundRecursive;
        }

        internal IEnumerable<Transition> GetOptimizedTransitions()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Transition>>() != null);

            if (IsOptimized)
                return OutgoingTransitions;

            HashSet<State> visited = new HashSet<State>(ObjectReferenceEqualityComparer<State>.Default);
            if (HasRecursiveTransitions ?? true)
                visited.Add(this);

            return OutgoingTransitions.SelectMany(i => FollowOptimizedTransitions(visited, i));
        }

        private IEnumerable<Transition> FollowOptimizedTransitions(HashSet<State> visited, Transition currentTransition)
        {
            Contract.Requires(visited != null);
            Contract.Requires(currentTransition != null);

            Contract.Ensures(visited.Count == Contract.OldValue(visited.Count));

            if (currentTransition.TargetState == this)
            {
                if (currentTransition.IsRecursive || currentTransition.IsMatch)
                    return new Transition[] { currentTransition };

                return Enumerable.Empty<Transition>();
            }

            if (currentTransition.IsMatch)
                return new Transition[] { currentTransition };

            List<Transition> result = new List<Transition>();
            bool emitCurrent = false;
            foreach (var transition in currentTransition.TargetState.OutgoingTransitions)
            {
                if (transition.IsMatch && !currentTransition.IsEpsilon)
                {
                    emitCurrent = true;
                    continue;
                }

                if (transition.IsRecursive)
                {
                    // should probably try to detect overlapping paths
                    emitCurrent = true;
                    continue;
                }

                if (transition.TargetState.HasRecursiveTransitions ?? true)
                {
                    if (!visited.Add(transition.TargetState))
                        continue; // is this correct?
                }

                try
                {
                    bool canMerge = false;
                    if (currentTransition.IsEpsilon || transition.IsEpsilon)
                    {
                        canMerge = true;
                    }
                    else if (currentTransition.IsContext && transition.IsContext)
                    {
                        canMerge = (currentTransition is PushContextTransition && transition is PushContextTransition)
                            || (currentTransition is PopContextTransition && transition is PopContextTransition);
                    }

                    if (!canMerge)
                    {
                        emitCurrent = true;
                    }
                    else
                    {
                        Transition merged = MergeTransitions(currentTransition, transition);
                        result.AddRange(FollowOptimizedTransitions(visited, merged));
                    }
                }
                finally
                {
                    if (transition.TargetState.HasRecursiveTransitions ?? true)
                        visited.Remove(transition.TargetState);
                }
            }

            if (emitCurrent)
                result.Add(currentTransition);

            return result;
        }

        public void Optimize(StateOptimizer optimizer)
        {
            Contract.Requires<ArgumentNullException>(optimizer != null, "optimizer");
            Contract.Requires<InvalidOperationException>(IsRecursiveAnalysisComplete);

            if (IsOptimized)
                return;

            OptimizeOutgoingTransitions(optimizer);
            _isOptimized = true;
        }

        private void OptimizeOutgoingTransitions(StateOptimizer optimizer)
        {
            List<Transition> oldTransitions = new List<Transition>(OutgoingTransitions.Where(i => !i.IsRecursive));
            foreach (var transition in oldTransitions)
                RemoveTransitionInternal(transition, optimizer);

            Contract.Assert(Contract.ForAll(OutgoingTransitions, i => i.IsRecursive));

            foreach (var transition in oldTransitions)
            {
                HashSet<Transition> visited = new HashSet<Transition>(ObjectReferenceEqualityComparer<Transition>.Default);
                visited.Add(transition);
                AddOptimizedTransitions(optimizer, visited, transition, PreventContextType.None);
            }

            Contract.Assert(oldTransitions.Count == 0 || OutgoingTransitions.Count > 0);
        }

        private void AddOptimizedTransitions(StateOptimizer optimizer, HashSet<Transition> visited, Transition transition, PreventContextType preventContextType)
        {
            Contract.Requires(optimizer != null);
            Contract.Requires(visited != null);
            Contract.Requires(transition != null);
            Contract.Requires(transition.SourceState == null);
            Contract.Requires(preventContextType != PreventContextType.PushRecursive && preventContextType != PreventContextType.PopRecursive);

            Contract.Ensures(visited.Count == Contract.OldValue(visited.Count));

            List<Transition> addedTransitions = null;

            if (transition.TargetState == this)
                return;

            try
            {
                while (true)
                {
                    /* Done when we find:
                     *  - a match transition
                     *  - a recursive transition (which should already be analyzed)
                     */

                    if (transition.IsMatch || transition.TargetState.OutgoingTransitions.Count == 0 || transition.IsRecursive)
                    {
                        //if (!transition.IsMatch && transition.TargetState.OutgoingTransitions.Count > 0)
                        //{
                        //    // must be here because it's a recursive state
                        //    transition.IsRecursive = true;
                        //}

                        AddTransitionInternal(transition, optimizer);
                        return;
                    }

                    // inline merge of single epsilon transitions
                    if (transition.TargetState.OutgoingTransitions.Count == 1 && transition.TargetState.OutgoingTransitions.First().IsEpsilon)
                    {
                        if (!visited.Add(transition.TargetState.OutgoingTransitions.First()))
                            throw new InvalidOperationException();

                        addedTransitions = addedTransitions ?? new List<Transition>();
                        addedTransitions.Add(transition.TargetState.OutgoingTransitions.First());
                        transition = MergeTransitions(transition, transition.TargetState.OutgoingTransitions.First());
                        continue;
                    }

                    break;
                }

                bool added = false;

                foreach (var nextTransition in transition.TargetState.OutgoingTransitions.ToArray())
                {
                    bool preventMerge = nextTransition.IsRecursive;

                    if (!nextTransition.IsRecursive)
                    {
                        switch (preventContextType)
                        {
                        case PreventContextType.Pop:
                            if (transition is PopContextTransition)
                                preventMerge = true;

                            break;

                        case PreventContextType.Push:
                            if (transition is PushContextTransition)
                                preventMerge = true;

                            break;

                        default:
                            break;
                        }
                    }

                    if (transition.IsEpsilon)
                    {
                        if (preventMerge && !added)
                        {
                            AddTransitionInternal(transition, optimizer);
                            added = true;
                            continue;
                        }

                        if (!visited.Add(nextTransition))
                            continue;

                        try
                        {
                            Contract.Assert(!preventMerge);
                            AddOptimizedTransitions(optimizer, visited, MergeTransitions(transition, nextTransition), preventContextType);
                        }
                        finally
                        {
                            visited.Remove(nextTransition);
                        }
                    }
                    else if (transition.IsContext)
                    {
                        PreventContextType nextPreventContextType = PreventContextType.None;
                        if (!preventMerge && transition.TargetState.IsOptimized)
                        {
                            if (nextTransition is PushContextTransition)
                                nextPreventContextType = PreventContextType.Push;
                            else if (nextTransition is PopContextTransition)
                                nextPreventContextType = PreventContextType.Pop;
                        }

                        bool canMerge = !preventMerge && !nextTransition.IsMatch;

                        if (canMerge && nextTransition.IsContext)
                        {
                            canMerge = (transition is PopContextTransition && nextTransition is PopContextTransition)
                                || (transition is PushContextTransition && nextTransition is PushContextTransition);

#if false
                            if (canMerge)
                            {
                                bool recursive = ((ContextTransition)transition).ContextIdentifiers.Any(((ContextTransition)nextTransition).ContextIdentifiers.Contains);
                                if (recursive)
                                {
                                    transition.IsRecursive = true;
                                    canMerge = false;
                                }
                            }
#endif
                        }

                        if (canMerge)
                        {
                            if (!visited.Add(nextTransition))
                                continue;

                            try
                            {
                                AddOptimizedTransitions(optimizer, visited, MergeTransitions(transition, nextTransition), nextPreventContextType);
                            }
                            finally
                            {
                                visited.Remove(nextTransition);
                            }
                        }
                        else if (!added)
                        {
                            AddTransitionInternal(transition, optimizer);
                            added = true;
                        }
                    }
                }
            }
            finally
            {
                if (addedTransitions != null)
                    visited.ExceptWith(addedTransitions);
            }
        }

        private Transition MergeTransitions(Transition first, Transition second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

            Contract.Ensures(Contract.Result<Transition>().SourceState == null);
            Contract.Ensures(Contract.Result<Transition>().IsRecursive == second.IsRecursive);

            Contract.Assert(!first.IsRecursive);
            Contract.Assert(first.IsEpsilon || !second.IsRecursive);

            if (first.IsMatch)
            {
                if (!second.IsEpsilon)
                    throw new InvalidOperationException();

                MatchRangeTransition matchRangeTransition = first as MatchRangeTransition;
                if (matchRangeTransition != null)
                    return new MatchRangeTransition(second.TargetState, matchRangeTransition.Range);

                throw new NotImplementedException("Unknown match transition type.");
            }

            if (first.IsEpsilon)
            {
                if (second.IsEpsilon)
                    return new EpsilonTransition(second.TargetState);

                MatchRangeTransition matchRangeTransition = second as MatchRangeTransition;
                if (matchRangeTransition != null)
                    return new MatchRangeTransition(second.TargetState, matchRangeTransition.Range);

                PopContextTransition popContextTransition = second as PopContextTransition;
                if (popContextTransition != null)
                {
                    var transition = new PopContextTransition(second.TargetState, popContextTransition.ContextIdentifiers);
                    //transition.PushTransitions.UnionWith(popContextTransition.PushTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PushTransitions, i => i.SourceState != null));
                    return transition;
                }

                PushContextTransition pushContextTransition = second as PushContextTransition;
                if (pushContextTransition != null)
                {
                    var transition = new PushContextTransition(second.TargetState, pushContextTransition.ContextIdentifiers);
                    //transition.PopTransitions.UnionWith(pushContextTransition.PopTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PopTransitions, i => i.SourceState != null));
                    return transition;
                }

                throw new NotSupportedException();
            }

            PopContextTransition popFirst = first as PopContextTransition;
            if (popFirst != null)
            {
                if (second.IsEpsilon)
                {
                    var transition = new PopContextTransition(second.TargetState, popFirst.ContextIdentifiers);
                    //transition.PushTransitions.UnionWith(popFirst.PushTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PushTransitions, i => i.SourceState != null));
                    return transition;
                }

                PopContextTransition popSecond = second as PopContextTransition;
                if (popSecond != null)
                {
                    var transition = new PopContextTransition(popSecond.TargetState, popFirst.ContextIdentifiers.Concat(popSecond.ContextIdentifiers));
                    ////transition.PushTransitions.UnionWith(popFirst.PushTransitions);
                    //transition.PushTransitions.UnionWith(popSecond.PushTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PushTransitions, i => i.SourceState != null));
                    return transition;
                }

                if (second is PushContextTransition)
                    throw new InvalidOperationException();

                if (second.IsMatch)
                    throw new NotSupportedException();

                throw new NotImplementedException();
            }

            PushContextTransition pushFirst = first as PushContextTransition;
            if (pushFirst != null)
            {
                if (second.IsEpsilon)
                {
                    var transition = new PushContextTransition(second.TargetState, pushFirst.ContextIdentifiers);
                    //transition.PopTransitions.UnionWith(pushFirst.PopTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PopTransitions, i => i.SourceState != null));
                    return transition;
                }

                PushContextTransition pushSecond = second as PushContextTransition;
                if (pushSecond != null)
                {
                    var transition = new PushContextTransition(pushSecond.TargetState, pushFirst.ContextIdentifiers.Concat(pushSecond.ContextIdentifiers));
                    //transition.PopTransitions.UnionWith(pushFirst.PopTransitions);
                    ////transition.PopTransitions.UnionWith(pushSecond.PopTransitions);
                    //Contract.Assert(Contract.ForAll(transition.PopTransitions, i => i.SourceState != null));
                    return transition;
                }

                if (second is PopContextTransition)
                    throw new InvalidOperationException();

                if (second.IsMatch)
                    throw new NotSupportedException();

                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        public void AddTransition(Transition transition, StateOptimizer optimizer = null)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");
            Contract.Requires<InvalidOperationException>(transition.SourceState == null);
            Contract.Requires<InvalidOperationException>(!OutgoingTransitions.Contains(transition));
            Contract.Requires<InvalidOperationException>(!transition.TargetState.IncomingTransitions.Contains(transition));

            AddTransitionInternal(transition, optimizer);
        }

        public void RemoveTransition(Transition transition, StateOptimizer optimizer = null)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");
            Contract.Requires<ArgumentException>(transition.SourceState == this);
            Contract.Requires<InvalidOperationException>(OutgoingTransitions.Contains(transition));

            Contract.Ensures(transition.SourceState == null);
            Contract.Ensures(!Contract.OldValue(transition.SourceState).OutgoingTransitions.Contains(transition));
            Contract.Ensures(!transition.TargetState.IncomingTransitions.Contains(transition));

            RemoveTransitionInternal(transition, optimizer);
        }

        public IntervalSet GetSourceSet()
        {
            return GetSourceSet(PreventContextType.None);
        }

        public IntervalSet GetSourceSet(PreventContextType preventContextType)
        {
            if (_sourceSet != null && _sourceSet[(int)preventContextType] != null)
                return _sourceSet[(int)preventContextType];

            IntervalSet[] sets = _sourceSet ?? new IntervalSet[Enum.GetValues(typeof(PreventContextType)).Cast<int>().Max() + 1];
            IntervalSet set = new IntervalSet();
            var queue = new Queue<Tuple<Transition, PreventContextType>>(IncomingTransitions.Select(i => Tuple.Create(i, preventContextType)));
            var comparer = new TupleEqualityComparer<Transition, PreventContextType>(ObjectReferenceEqualityComparer<Transition>.Default, null);
            var visited = new HashSet<Tuple<Transition, PreventContextType>>(queue, comparer);

            while (queue.Count > 0)
            {
                var pair = queue.Dequeue();
                Transition transition = pair.Item1;
                PreventContextType nextPreventContextType = pair.Item2;

                if (transition.SourceState.IsOptimized)
                {
                    switch (nextPreventContextType)
                    {
                    case PreventContextType.Pop:
                        if (!transition.IsRecursive && transition is PopContextTransition)
                            continue;

                        break;

                    case PreventContextType.PopRecursive:
                        if (transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.Push:
                        if (!transition.IsRecursive && transition is PushContextTransition)
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

                if (transition.IsEpsilon || transition.IsContext)
                {
                    if (transition.IsContext)
                    {
                        nextPreventContextType = PreventContextType.None;
                        if (transition is PushContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                        else if (transition is PopContextTransition)
                            nextPreventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
                    }

                    if (transition.SourceState._sourceSet != null && transition.SourceState._sourceSet[(int)nextPreventContextType] != null)
                    {
                        set.UnionWith(transition.SourceState._sourceSet[(int)nextPreventContextType]);
                    }
                    else
                    {
                        foreach (var incoming in transition.SourceState.IncomingTransitions)
                        {
                            var nextPair = Tuple.Create(incoming, nextPreventContextType);
                            if (visited.Add(nextPair))
                                queue.Enqueue(Tuple.Create(incoming, nextPreventContextType));
                        }
                    }
                }
                else
                {
                    set.UnionWith(transition.MatchSet);
                }
            }

            _sourceSet = sets;
            _sourceSet[(int)preventContextType] = set;

            return set;
        }

        public IntervalSet GetFollowSet()
        {
            return GetFollowSet(PreventContextType.None);
        }

        public IntervalSet GetFollowSet(PreventContextType preventContextType)
        {
            if (_followSet != null && _followSet[(int)preventContextType] != null)
                return _followSet[(int)preventContextType];

            IntervalSet[] sets = _followSet ?? new IntervalSet[Enum.GetValues(typeof(PreventContextType)).Cast<int>().Max() + 1];
            IntervalSet set = new IntervalSet();
            var queue = new Queue<Tuple<Transition, PreventContextType>>(OutgoingTransitions.Select(i => Tuple.Create(i, preventContextType)));
            var comparer = new TupleEqualityComparer<Transition, PreventContextType>(ObjectReferenceEqualityComparer<Transition>.Default, null);
            var visited = new HashSet<Tuple<Transition, PreventContextType>>(queue, comparer);

            while (queue.Count > 0)
            {
                var pair = queue.Dequeue();
                Transition transition = pair.Item1;
                PreventContextType nextPreventContextType = pair.Item2;

                if (transition.IsContext)
                {
                    switch (nextPreventContextType)
                    {
                    case PreventContextType.Pop:
                        if (!transition.IsRecursive && transition is PopContextTransition)
                            continue;

                        break;

                    case PreventContextType.PopRecursive:
                        if (transition.IsRecursive && (transition is PopContextTransition))
                            continue;

                        break;

                    case PreventContextType.Push:
                        if (!transition.IsRecursive && transition is PushContextTransition)
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

                if (transition.IsEpsilon || transition.IsContext)
                {
                    // the preventContextType can only change if we're following a another context transition
                    if (transition.IsContext)
                    {
                        nextPreventContextType = PreventContextType.None;
                        if (transition.SourceState.IsOptimized)
                        {
                            if (transition is PushContextTransition)
                                nextPreventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                            else if (transition is PopContextTransition)
                                nextPreventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
                        }
                    }

                    if (transition.TargetState._followSet != null && transition.TargetState._followSet[(int)nextPreventContextType] != null)
                    {
                        set.UnionWith(transition.TargetState._followSet[(int)nextPreventContextType]);
                    }
                    else
                    {
                        foreach (var outgoing in transition.TargetState.OutgoingTransitions)
                        {
                            var nextPair = Tuple.Create(outgoing, nextPreventContextType);
                            if (visited.Add(nextPair))
                                queue.Enqueue(Tuple.Create(outgoing, nextPreventContextType));
                        }
                    }
                }
                else
                {
                    set.UnionWith(transition.MatchSet);
                }
            }

            _followSet = sets;
            _followSet[(int)preventContextType] = set;

            return set;
        }

        public override string ToString()
        {
            return string.Format(
                "State {0}{1}{2} {3}/{4}",
                Id,
                IsOptimized ? "!" : string.Empty,
                (_recursiveTransitions ?? false) ? "*" : string.Empty,
                IncomingTransitions.Count,
                OutgoingTransitions.Count);
        }

        internal void AddTransitionInternal(Transition transition, StateOptimizer optimizer)
        {
            Contract.Requires(transition != null);
            Contract.Requires(transition.SourceState == null);
#if ALL_CHECKS
            Contract.Requires(!OutgoingTransitions.Contains(transition));
            Contract.Requires(!transition.TargetState.IncomingTransitions.Contains(transition));
#endif

            if (IsRecursiveAnalysisComplete && !transition.IsMatch && transition.TargetState == this && !transition.IsRecursive)
                throw new InvalidOperationException();

            PopContextTransition popContextTransition = transition as PopContextTransition;
            PushContextTransition pushContextTransition = transition as PushContextTransition;

#if false
            if (popContextTransition != null && !transition.IsRecursive)
            {
                foreach (var recursive in OutgoingTransitions.OfType<PopContextTransition>().Where(i => i.IsRecursive))
                {
                    if (popContextTransition.ContextIdentifiers.Take(recursive.ContextIdentifiers.Count).SequenceEqual(recursive.ContextIdentifiers))
                    {
                        if (popContextTransition.ContextIdentifiers.Count > recursive.ContextIdentifiers.Count)
                            throw new InvalidOperationException();
                    }
                }
            }
#endif

            if (_outgoingTransitions.Count > 10 && !(_outgoingTransitions is ISet<Transition>))
            {
                _outgoingTransitions = new HashSet<Transition>(_outgoingTransitions, ObjectReferenceEqualityComparer<Transition>.Default);
            }

#if false
            if (transition.IsContext && transition.IsRecursive)
            {
                PopContextTransition first = transition as PopContextTransition;
                if (first != null)
                {
                    foreach (var existing in OutgoingTransitions.OfType<PopContextTransition>().ToArray())
                    {
                        if (existing.TargetState != transition.TargetState)
                            continue;

                        if (first.ContextIdentifiers.Take(existing.ContextIdentifiers.Count).SequenceEqual(existing.ContextIdentifiers))
                            RemoveTransitionInternal(existing, optimizer);
                    }
                }

                PushContextTransition second = transition as PushContextTransition;
                if (second != null)
                {
                    foreach (var existing in OutgoingTransitions.OfType<PushContextTransition>().ToArray())
                    {
                        if (existing.TargetState != transition.TargetState)
                            continue;

                        if (second.ContextIdentifiers.Take(existing.ContextIdentifiers.Count).SequenceEqual(existing.ContextIdentifiers))
                            RemoveTransitionInternal(existing, optimizer);
                    }
                }
            }
#endif

            OutgoingTransitions.Add(transition);

            if (transition.TargetState.IncomingTransitions.Count > 10 && !(transition.TargetState.IncomingTransitions is ISet<Transition>))
            {
                transition.TargetState._incomingTransitions = new HashSet<Transition>(transition.TargetState._incomingTransitions, ObjectReferenceEqualityComparer<Transition>.Default);
            }

            transition.TargetState.IncomingTransitions.Add(transition);
            transition.SourceState = this;

            if (optimizer != null)
                optimizer.AddTransition(transition);

            if (popContextTransition != null)
            {
                //if (optimizer != null)
                //    popContextTransition.PushTransitions.UnionWith(optimizer.GetPushContextTransitions(popContextTransition.ContextIdentifiers.Last()));

                //foreach (var pushTransition in popContextTransition.PushTransitions)
                //{
                //    Contract.Assert(pushTransition.ContextIdentifiers.First() == popContextTransition.ContextIdentifiers.Last());
                //    Contract.Assert(pushTransition.SourceState != null);
                //    pushTransition.PopTransitions.Add(popContextTransition);
                //}

#if ALL_CHECKS
                Contract.Assert(Contract.ForAll(OutgoingTransitions.OfType<PushContextTransition>(), i => i.ContextIdentifiers.Last() != popContextTransition.ContextIdentifiers.First() || popContextTransition.PushTransitions.Contains(i)));
#endif
            }
            else if (pushContextTransition != null)
            {
                //if (optimizer != null)
                //    pushContextTransition.PopTransitions.UnionWith(optimizer.GetPopContextTransitions(pushContextTransition.ContextIdentifiers[0]));

                //foreach (var popTransition in pushContextTransition.PopTransitions)
                //{
                //    Contract.Assert(popTransition.ContextIdentifiers.Last() == pushContextTransition.ContextIdentifiers.First());
                //    Contract.Assert(popTransition.SourceState != null);
                //    popTransition.PushTransitions.Add(pushContextTransition);
                //}

#if ALL_CHECKS
                Contract.Assert(Contract.ForAll(OutgoingTransitions.OfType<PopContextTransition>(), i => i.ContextIdentifiers.Last() != pushContextTransition.ContextIdentifiers.First() || pushContextTransition.PopTransitions.Contains(i)));
#endif
            }

            _followSet = null;
            _isForwardRecursive = null;
            transition.TargetState._sourceSet = null;
            transition.TargetState._isBackwardRecursive = null;
        }

        internal void RemoveTransitionInternal(Transition transition, StateOptimizer optimizer)
        {
            Contract.Requires(transition != null, "transition");
            Contract.Requires(transition.SourceState == this);
#if ALL_CHECKS
            Contract.Requires(OutgoingTransitions.Contains(transition));
#endif

            Contract.Ensures(transition.SourceState == null);
#if ALL_CHECKS
            Contract.Ensures(!Contract.OldValue(transition.SourceState).OutgoingTransitions.Contains(transition));
            Contract.Ensures(!transition.TargetState.IncomingTransitions.Contains(transition));

            Contract.Assert(transition.TargetState.IncomingTransitions.Contains(transition));
#endif

            //PopContextTransition popContextTransition = transition as PopContextTransition;
            //if (popContextTransition != null)
            //{
            //    foreach (var pushTransition in popContextTransition.PushTransitions)
            //    {
            //        Contract.Assert(pushTransition.PopTransitions.Contains(transition));
            //        pushTransition.PopTransitions.Remove(popContextTransition);
            //    }
            //}

            //PushContextTransition pushContextTransition = transition as PushContextTransition;
            //if (pushContextTransition != null)
            //{
            //    foreach (var popTransition in pushContextTransition.PopTransitions)
            //    {
            //        Contract.Assert(popTransition.PushTransitions.Contains(transition));
            //        popTransition.PushTransitions.Remove(pushContextTransition);
            //    }
            //}

            if (optimizer != null)
                optimizer.RemoveTransition(transition);

            OutgoingTransitions.Remove(transition);
            transition.TargetState.IncomingTransitions.Remove(transition);

            _followSet = null;
            _isForwardRecursive = null;
            transition.TargetState._sourceSet = null;
            transition.TargetState._isBackwardRecursive = null;
            transition.SourceState = null;
        }

        private class TupleEqualityComparer<T1, T2> : EqualityComparer<Tuple<T1, T2>>
        {
            private readonly IEqualityComparer<T1> _item1Comparer;
            private readonly IEqualityComparer<T2> _item2Comparer;

            public TupleEqualityComparer(IEqualityComparer<T1> item1Comparer, IEqualityComparer<T2> item2Comparer)
            {
                _item1Comparer = item1Comparer ?? EqualityComparer<T1>.Default;
                _item2Comparer = item2Comparer ?? EqualityComparer<T2>.Default;
            }

            public override bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y)
            {
                if (object.ReferenceEquals(x, y))
                    return true;

                if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                    return false;

                return _item1Comparer.Equals(x.Item1, y.Item1) && _item2Comparer.Equals(x.Item2, y.Item2);
            }

            public override int GetHashCode(Tuple<T1, T2> obj)
            {
                return _item1Comparer.GetHashCode(obj.Item1) ^ _item2Comparer.GetHashCode(obj.Item2);
            }
        }
    }
}
