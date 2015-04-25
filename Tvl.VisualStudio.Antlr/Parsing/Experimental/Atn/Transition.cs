namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;

    public abstract class Transition
    {
        private readonly State _targetState;
        private State _sourceState;
        private bool _isRecursive;

        public Transition(State targetState)
        {
            Contract.Requires<ArgumentNullException>(targetState != null, "targetState");

            _targetState = targetState;
        }

        public State SourceState
        {
            get
            {
                return _sourceState;
            }

            internal set
            {
                _sourceState = value;
            }
        }

        public State TargetState
        {
            get
            {
                return _targetState;
            }
        }

        public bool IsRecursive
        {
            get
            {
                return _isRecursive;
            }

            set
            {
                _isRecursive = value;
            }
        }

        public abstract bool IsEpsilon
        {
            get;
        }

        public abstract bool IsContext
        {
            get;
        }

        public abstract bool IsMatch
        {
            get;
        }

        public abstract IntervalSet MatchSet
        {
            get;
        }

        public virtual bool MatchesSymbol(int symbol)
        {
            return IsMatch && MatchSet.Contains(symbol);
        }

        public override bool Equals(object obj)
        {
            Transition other = obj as Transition;
            if (other == null)
                return false;

            return EqualityComparer<State>.Default.Equals(SourceState, other.SourceState)
                && EqualityComparer<State>.Default.Equals(TargetState, other.TargetState)
                && IsRecursive == other.IsRecursive;
        }

        public override int GetHashCode()
        {
            // this is the only value ensured to not change
            return _targetState.GetHashCode();
            //int source = _sourceState != null ? _sourceState.GetHashCode() : 0;
            //int target = _targetState != null ? _targetState.GetHashCode() : 0;
            //return source ^ target ^ (_isRecursive ? 1 : 0);
        }
    }
}
