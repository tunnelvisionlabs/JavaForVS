namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using IntervalSet = Tvl.VisualStudio.Language.Parsing.Collections.IntervalSet;

    public abstract class ContextTransition : Transition
    {
        private readonly ReadOnlyCollection<int> _contextIdentifiers;

        public ContextTransition(State targetState, IEnumerable<int> contextIdentifiers)
            : base(targetState)
        {
            Contract.Requires<ArgumentNullException>(contextIdentifiers != null, "contextIdentifiers");
            Contract.Requires<ArgumentException>(contextIdentifiers.Any());

            _contextIdentifiers = new ReadOnlyCollection<int>(contextIdentifiers.ToArray());
        }

        public ContextTransition(State targetState, params int[] contextIdentifiers)
            : base(targetState)
        {
            Contract.Requires<ArgumentNullException>(contextIdentifiers != null, "contextIdentifiers");
            Contract.Requires<ArgumentException>(contextIdentifiers.Length > 0);

            _contextIdentifiers = new ReadOnlyCollection<int>(contextIdentifiers.CloneArray());
        }

        public ReadOnlyCollection<int> ContextIdentifiers
        {
            get
            {
                return _contextIdentifiers;
            }
        }

        public override sealed bool IsEpsilon
        {
            get
            {
                return false;
            }
        }

        public override sealed bool IsContext
        {
            get
            {
                return true;
            }
        }

        public sealed override bool IsMatch
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

        public override bool Equals(object obj)
        {
            ContextTransition other = obj as ContextTransition;
            if (other == null)
                return false;

            if (object.ReferenceEquals(other, this))
                return true;

            return base.Equals(other)
                && ContextIdentifiers.SequenceEqual(other.ContextIdentifiers);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ContextIdentifiers.Count.GetHashCode();
        }
    }
}
