namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    public class BoundedEndInterpretTraceEqualityComparer : IEqualityComparer<InterpretTrace>
    {
        private static readonly BoundedEndInterpretTraceEqualityComparer _default = new BoundedEndInterpretTraceEqualityComparer();

        protected BoundedEndInterpretTraceEqualityComparer()
        {
        }

        public static BoundedEndInterpretTraceEqualityComparer Default
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

            // unique on the end context, the last transition's source state, and the end position
            for (ContextFrame xframe = x.EndContext, yframe = y.EndContext; true; xframe = xframe.Parent, yframe = yframe.Parent)
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

            InterpretTraceTransition lastx = x.Transitions.Last.Value;
            InterpretTraceTransition lasty = y.Transitions.Last.Value;
            if (!EqualityComparer<State>.Default.Equals(lastx.Transition.SourceState, lasty.Transition.SourceState))
                return false;

            InterpretTraceTransition lastxmatch = x.Transitions.LastOrDefault(i => i.Transition.IsMatch);
            InterpretTraceTransition lastymatch = y.Transitions.LastOrDefault(i => i.Transition.IsMatch);
            if (lastxmatch == null)
                return lastymatch == null;

            if (lastymatch == null)
                return false;

            if (lastxmatch.TokenIndex != lastymatch.TokenIndex)
                return false;

            return true;
        }

        public virtual int GetHashCode(InterpretTrace obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (obj.Transitions.Count == 0)
                return 0;

            InterpretTraceTransition last = obj.Transitions.Last.Value;
            int transitionCode = last.Transition.GetHashCode();

            InterpretTraceTransition lastmatch = obj.Transitions.LastOrDefault(i => i.Transition.IsMatch);
            if (lastmatch == null)
                return transitionCode;

            return transitionCode ^ lastmatch.TokenIndex.GetHashCode();
        }
    }
}
