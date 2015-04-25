namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using System;
    using Contract = System.Diagnostics.Contracts.Contract;
    using RuntimeHelpers = System.Runtime.CompilerServices.RuntimeHelpers;

    public class RuleBinding : IEquatable<RuleBinding>
    {
        private readonly string _name;
        private readonly State _startState;
        private readonly State _endState;

        private bool _isStartRule;

        public RuleBinding(string name)
            : this(name, new State(), new State())
        {
            Contract.Requires(!String.IsNullOrEmpty(name));

            Contract.Ensures(!string.IsNullOrEmpty(this.Name));
            Contract.Ensures(this.StartState != null);
            Contract.Ensures(this.EndState != null);
        }

        public RuleBinding(string name, State startState, State endState)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentNullException>(startState != null, "startState");
            Contract.Requires<ArgumentNullException>(endState != null, "endState");

            Contract.Ensures(!string.IsNullOrEmpty(this.Name));
            Contract.Ensures(this.StartState != null);
            Contract.Ensures(this.EndState != null);

            _name = name;
            _startState = startState;
            _endState = endState;
        }

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return _name;
            }
        }

        public State StartState
        {
            get
            {
                Contract.Ensures(Contract.Result<State>() != null);
                return _startState;
            }
        }

        public State EndState
        {
            get
            {
                Contract.Ensures(Contract.Result<State>() != null);
                return _endState;
            }
        }

        public bool IsStartRule
        {
            get
            {
                return _isStartRule;
            }

            set
            {
                _isStartRule = value;
            }
        }

        public bool Equals(RuleBinding other)
        {
            return object.ReferenceEquals(this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RuleBinding);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public override string ToString()
        {
            return string.Format("Rule '{0}': {1}", Name, StartState);
        }
    }
}
