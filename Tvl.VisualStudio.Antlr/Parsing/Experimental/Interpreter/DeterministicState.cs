namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class DeterministicState : IEquatable<DeterministicState>
    {
        private readonly HashSet<ContextFrame> _contexts;

        private readonly List<DeterministicTransition> _incomingTransitions = new List<DeterministicTransition>();
        private readonly List<DeterministicTransition> _outgoingTransitions = new List<DeterministicTransition>();

        public DeterministicState(IEnumerable<ContextFrame> contexts)
        {
            Contract.Requires<ArgumentNullException>(contexts != null, "contexts");

            _contexts = new HashSet<ContextFrame>(contexts);
        }

        public ICollection<ContextFrame> Contexts
        {
            get
            {
                return _contexts;
            }
        }

        public ReadOnlyCollection<DeterministicTransition> IncomingTransitions
        {
            get
            {
                return _incomingTransitions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<DeterministicTransition> OutgoingTransitions
        {
            get
            {
                return _outgoingTransitions.AsReadOnly();
            }
        }

        public void AddTransition(DeterministicTransition transition)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");

            _outgoingTransitions.Add(transition);
            transition.TargetState._incomingTransitions.Add(transition);
            transition.SourceState = this;
        }

        public bool Equals(DeterministicState other)
        {
            if (other == null)
                return false;

            return _contexts.SetEquals(other._contexts);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as DeterministicState);
        }

        public override int GetHashCode()
        {
            return _contexts.Aggregate(0, (hashCode, context) => hashCode ^ context.GetHashCode());
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
