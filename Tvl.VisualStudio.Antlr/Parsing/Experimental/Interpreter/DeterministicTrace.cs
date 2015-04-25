namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class DeterministicTrace
    {
        private readonly DeterministicState _startState;
        private readonly DeterministicState _endState;
        private readonly List<DeterministicTraceTransition> _transitions;

        public DeterministicTrace(DeterministicState startState, DeterministicState endState)
            : this(startState, endState, Enumerable.Empty<DeterministicTraceTransition>())
        {
        }

        public DeterministicTrace(DeterministicState startState, DeterministicState endState, IEnumerable<DeterministicTraceTransition> transitions)
        {
            Contract.Requires<ArgumentNullException>(startState != null, "startState");
            Contract.Requires<ArgumentNullException>(endState != null, "endState");
            Contract.Requires<ArgumentNullException>(transitions != null, "transitions");

            _startState = startState;
            _endState = endState;
            _transitions = new List<DeterministicTraceTransition>(transitions);
        }

        public DeterministicState StartState
        {
            get
            {
                return _startState;
            }
        }

        public DeterministicState EndState
        {
            get
            {
                return _endState;
            }
        }

        public ReadOnlyCollection<DeterministicTraceTransition> Transitions
        {
            get
            {
                return _transitions.AsReadOnly();
            }
        }

        //public int TraceCount
        //{
        //    get
        //    {
        //        int total = 1;

        //        ICollection<ContextFrame> sourceFrames = _transitions[0].Transition.SourceState.Contexts;

        //        for (int i = 0; total > 0 && i < _transitions.Count; i++)
        //        {
        //            ICollection<ContextFrame> targetFrames = _transitions[i].Transition.TargetState.Contexts;

        //            IEnumerable<ContextFrame> potentialContexts = sourceFrames.SelectMany(GetReachableContexts);

        //            sourceFrames = targetFrames;
        //        }

        //        return total;
        //    }
        //}
    }
}
