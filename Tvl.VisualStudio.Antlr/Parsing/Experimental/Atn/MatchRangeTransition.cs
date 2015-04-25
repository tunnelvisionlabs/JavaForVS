namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using Interval = Tvl.VisualStudio.Language.Parsing.Collections.Interval;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;

    public class MatchRangeTransition : Transition
    {
        private readonly Interval _range;

        public MatchRangeTransition(State targetState, Interval range)
            : base(targetState)
        {
            _range = range;
        }

        public Interval Range
        {
            get
            {
                return _range;
            }
        }

        public override bool IsEpsilon
        {
            get
            {
                return false;
            }
        }

        public override bool IsContext
        {
            get
            {
                return false;
            }
        }

        public override bool IsMatch
        {
            get
            {
                return true;
            }
        }

        public override IntervalSet MatchSet
        {
            get
            {
                return IntervalSet.Of(_range);
            }
        }

        public override bool MatchesSymbol(int symbol)
        {
            return _range.Contains(symbol);
        }

        public override bool Equals(object obj)
        {
            MatchRangeTransition other = obj as MatchRangeTransition;
            if (other == null)
                return false;

            return Range == other.Range
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Range.GetHashCode();
        }

        public override string ToString()
        {
            string source = SourceState != null ? SourceState.Id.ToString() + (SourceState.IsOptimized ? "!" : string.Empty) : "?";
            string target = TargetState != null ? TargetState.Id.ToString() + (TargetState.IsOptimized ? "!" : string.Empty) : "?";

            return string.Format("{0} -> match {1} -> {2}", source, _range, target);
        }
    }
}
