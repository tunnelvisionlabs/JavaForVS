namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;
    using Contract = System.Diagnostics.Contracts.Contract;

    public class ContextFrame : IEquatable<ContextFrame>
    {
        public readonly State State;
        public readonly int? Context;
        public readonly ContextFrame Parent;
        public readonly NetworkInterpreter Interpreter;

        private readonly int? _headContext;
        private readonly int _hashCode;

        public ContextFrame(State state, int? context, ContextFrame parent, NetworkInterpreter interpreter)
        {
            Contract.Requires(parent == null || parent.Context.HasValue);

            State = state;
            Context = context;
            Parent = parent;
            Interpreter = interpreter;
            _headContext = parent != null ? parent.HeadContext : context;

            int stateCode = (State != null) ? EqualityComparer<State>.Default.GetHashCode(State) : 0;
            long parentCode = (Parent != null) ? Parent.GetHashCode() * 104729 : 0;
            _hashCode = stateCode ^ (Context ?? 0) ^ parentCode.GetHashCode();
        }

        public Network Network
        {
            get
            {
                return Interpreter.Network;
            }
        }

        public int? HeadContext
        {
            get
            {
                return _headContext;
            }
        }

        internal ContextFrame AddHeadContext(ContextFrame context)
        {
            ContextFrame parent = (Parent != null) ? Parent.AddHeadContext(context) : context;
            return new ContextFrame(State, Context, parent, context.Interpreter);
        }

        public virtual bool Equals(ContextFrame other)
        {
            if (other == null)
                return false;

            return EqualityComparer<State>.Default.Equals(State, other.State)
                && Context == other.Context
                && EqualityComparer<ContextFrame>.Default.Equals(Parent, other.Parent);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as ContextFrame);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override string ToString()
        {
            string current = "?";
            if (State != null)
                current = string.Format("{0}({1})", State.Id, Network.StateRules[State.Id].Name);

            List<string> parentContexts = new List<string>();
            for (ContextFrame frame = Parent; frame != null; frame = frame.Parent)
            {
                string contextName = frame.Context != null ? frame.Context.ToString() : "<null>";

                RuleBinding parentRule;
                if (frame.Context != null && Network.ContextRules.TryGetValue(frame.Context.Value, out parentRule))
                    contextName = string.Format("{0}({1})", contextName, parentRule.Name);

                parentContexts.Add(contextName);
            }

            return string.Format("{0} : [{1}]", current, string.Join(",", parentContexts));
        }
    }
}
