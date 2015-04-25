namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IConnectorArgument))]
    internal abstract class IConnectorArgumentContracts : IConnectorArgument
    {
        #region IConnectorArgument Members

        public string Description
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                throw new NotImplementedException();
            }
        }

        public string Label
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

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
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

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
