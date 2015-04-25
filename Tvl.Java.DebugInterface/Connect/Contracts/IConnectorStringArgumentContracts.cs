namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IConnectorStringArgument))]
    internal abstract class IConnectorStringArgumentContracts : IConnectorStringArgument
    {
        #region IConnectorArgument Members

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Label
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Required
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string StringValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValid(string value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
