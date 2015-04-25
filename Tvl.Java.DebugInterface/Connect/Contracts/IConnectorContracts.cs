namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IConnector))]
    internal abstract class IConnectorContracts : IConnector
    {
        #region IConnector Members

        public IDictionary<string, IConnectorArgument> DefaultArguments
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, IConnectorArgument>>() != null);
#if CONTRACTS_FORALL
                Contract.Ensures(Contract.ForAll(Contract.Result<IDictionary<string, IConnectorArgument>>(), pair => !string.IsNullOrEmpty(pair.Key) && pair.Value != null));
#endif

                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

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

        public ITransport Transport
        {
            get
            {
                Contract.Ensures(Contract.Result<ITransport>() != null);

                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
