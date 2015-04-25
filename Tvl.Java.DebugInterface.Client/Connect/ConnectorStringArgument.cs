namespace Tvl.Java.DebugInterface.Client.Connect
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Connect;

    public sealed class ConnectorStringArgument : ConnectorArgument, IConnectorStringArgument
    {
        private string _value;

        public ConnectorStringArgument(IConnectorStringArgument descriptor, string value)
            : base(descriptor)
        {
            Contract.Requires(descriptor != null);
            _value = value;
        }
        
        internal ConnectorStringArgument(string name, string label, string description, bool required, string value)
            : base(name, label, description, required)
        {
            _value = value;
        }

        public override string StringValue
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public override bool IsValid(string value)
        {
            return true;
        }
    }
}
