namespace Tvl.Java.DebugInterface.Client.Connect
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Connect;

    public sealed class ConnectorIntegerArgument : ConnectorArgument, IConnectorIntegerArgument
    {
        private readonly int _minimumValue;
        private readonly int _maximumValue;

        private int _value;

        public ConnectorIntegerArgument(IConnectorIntegerArgument descriptor, int value)
            : base(descriptor)
        {
            Contract.Requires(descriptor != null);

            _minimumValue = descriptor.MinimumValue;
            _maximumValue = descriptor.MaximumValue;
            _value = descriptor.Value;

            Value = value;
        }

        internal ConnectorIntegerArgument(string name, string label, string description, bool required, int value, int minimumValue, int maximumValue)
            : base(name, label, description, required)
        {
            Contract.Requires<ArgumentException>(minimumValue <= maximumValue);
            Contract.Requires<ArgumentException>(value >= minimumValue && value <= maximumValue);

            _minimumValue = minimumValue;
            _maximumValue = maximumValue;
            _value = value;
        }

        public override string StringValue
        {
            get
            {
                return _value.ToString();
            }

            set
            {
                Value = int.Parse(value);
            }
        }

        public override bool IsValid(string value)
        {
            int intValue;
            if (!int.TryParse(value, out intValue))
                return false;

            return IsValid(intValue);
        }

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value > MaximumValue)
                    throw new ArgumentException();
                if (value < MinimumValue)
                    throw new ArgumentException();

                _value = value;
            }
        }

        public int MinimumValue
        {
            get
            {
                return _minimumValue;
            }
        }

        public int MaximumValue
        {
            get
            {
                return _maximumValue;
            }
        }

        public bool IsValid(int value)
        {
            return value >= MinimumValue && value <= MaximumValue;
        }

        public string GetStringValueOf(int value)
        {
            return value.ToString();
        }
    }
}
