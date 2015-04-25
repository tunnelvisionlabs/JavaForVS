namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    public class BoundedStartInterpretTraceEqualityComparer : IEqualityComparer<InterpretTrace>
    {
        private static readonly BoundedStartInterpretTraceEqualityComparer _default = new BoundedStartInterpretTraceEqualityComparer();

        protected BoundedStartInterpretTraceEqualityComparer()
        {
        }

        public static BoundedStartInterpretTraceEqualityComparer Default
        {
            get
            {
                return _default;
            }
        }

        public virtual bool Equals(InterpretTrace x, InterpretTrace y)
        {
            if (object.ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (object.ReferenceEquals(x.Transitions, y.Transitions))
                return true;

            if (x.Transitions.Count == 0)
                return y.Transitions.Count == 0;

            if (y.Transitions.Count == 0)
                return false;

            // unique on the start context, the first transition's target state, and the start position
            for (ContextFrame xframe = x.StartContext, yframe = y.StartContext; true; xframe = xframe.Parent, yframe = yframe.Parent)
            {
                if (xframe == null || yframe == null)
                {
                    if (xframe != null || yframe != null)
                        return false;

                    break;
                }

                if (xframe.Context != yframe.Context)
                    return false;
            }

            InterpretTraceTransition firstx = x.Transitions.First.Value;
            InterpretTraceTransition firsty = y.Transitions.First.Value;
            if (!EqualityComparer<State>.Default.Equals(firstx.Transition.TargetState, firsty.Transition.TargetState))
                return false;

            InterpretTraceTransition firstxmatch = x.Transitions.FirstOrDefault(i => i.Transition.IsMatch);
            InterpretTraceTransition firstymatch = y.Transitions.FirstOrDefault(i => i.Transition.IsMatch);
            if (firstxmatch == null)
                return firstymatch == null;

            if (firstymatch == null)
                return false;

            if (firstxmatch.TokenIndex != firstymatch.TokenIndex)
                return false;

            return true;
        }

        public virtual int GetHashCode(InterpretTrace obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (obj.Transitions.Count == 0)
                return 0;

            InterpretTraceTransition first = obj.Transitions.First.Value;
            int transitionCode = first.Transition.GetHashCode();

            InterpretTraceTransition firstmatch = obj.Transitions.FirstOrDefault(i => i.Transition.IsMatch);
            if (firstmatch == null)
                return transitionCode;

            return transitionCode ^ firstmatch.TokenIndex.GetHashCode();
        }
    }
}
