namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class DeterministicTransition
    {
        private DeterministicState _sourceState;
        private DeterministicState _targetState;
        private IntervalSet _matchSet = new IntervalSet();

        public DeterministicTransition(DeterministicState targetState)
        {
            Contract.Requires<ArgumentNullException>(targetState != null, "targetState");

            _targetState = targetState;
        }

        public DeterministicState SourceState
        {
            get
            {
                return _sourceState;
            }

            set
            {
                _sourceState = value;
            }
        }

        public DeterministicState TargetState
        {
            get
            {
                return _targetState;
            }
        }

        public IntervalSet MatchSet
        {
            get
            {
                return _matchSet;
            }
        }
    }
}
