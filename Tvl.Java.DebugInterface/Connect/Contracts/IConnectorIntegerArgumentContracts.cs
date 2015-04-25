namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IConnectorIntegerArgument))]
    internal abstract class IConnectorIntegerArgumentContracts : IConnectorIntegerArgument
    {
        #region IConnectorIntegerArgument Members

        public int Value
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

        public int MinimumValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int MaximumValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValid(int value)
        {
            throw new NotImplementedException();
        }

        public string GetStringValueOf(int value)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        #endregion

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
