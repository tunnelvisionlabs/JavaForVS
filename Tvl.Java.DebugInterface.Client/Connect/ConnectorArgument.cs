namespace Tvl.Java.DebugInterface.Client.Connect
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Connect;

    public abstract class ConnectorArgument : IConnectorArgument
    {
        private readonly string _name;
        private readonly string _label;
        private readonly string _description;
        private readonly bool _required;

        public ConnectorArgument(IConnectorArgument descriptor)
        {
            Contract.Requires<ArgumentNullException>(descriptor != null, "descriptor");

            _name = descriptor.Name;
            _label = descriptor.Label;
            _description = descriptor.Description;
            _required = descriptor.Required;
        }

        public ConnectorArgument(string name, string label, string description, bool required)
        {
            _name = name;
            _label = label;
            _description = description;
            _required = required;
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
        }

        public bool Required
        {
            get
            {
                return _required;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public abstract string StringValue
        {
            get;
            set;
        }

        public abstract bool IsValid(string value);
    }
}
