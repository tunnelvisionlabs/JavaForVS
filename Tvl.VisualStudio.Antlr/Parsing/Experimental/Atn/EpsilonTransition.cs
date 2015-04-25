namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;

    public sealed class EpsilonTransition : Transition
    {
        public EpsilonTransition(State targetState)
            : base(targetState)
        {
        }

        public override sealed bool IsEpsilon
        {
            get
            {
                return true;
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
                return false;
            }
        }

        public override IntervalSet MatchSet
        {
            get
            {
                return new IntervalSet();
            }
        }

        public override string ToString()
        {
            string source = SourceState != null ? SourceState.Id.ToString() + (SourceState.IsOptimized ? "!" : string.Empty) : "?";
            string target = TargetState != null ? TargetState.Id.ToString() + (TargetState.IsOptimized ? "!" : string.Empty) : "?";

            return string.Format("{0} -> {1}", source, target);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as EpsilonTransition);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
