namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IAttachingConnector))]
    internal abstract class IAttachingConnectorContracts : IAttachingConnector
    {
        #region IAttachingConnector Members

        public IVirtualMachine Attach(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments)
        {
            Contract.Requires<ArgumentNullException>(arguments != null, "arguments");
#if CONTRACTS_FORALL
            Contract.Requires<ArgumentException>(Contract.ForAll(arguments, pair => !string.IsNullOrEmpty(pair.Key) && pair.Value != null));
#endif
            Contract.Ensures(Contract.Result<IVirtualMachine>() != null);

            throw new NotImplementedException();
        }

        #endregion

        #region IConnector Members

        public IDictionary<string, IConnectorArgument> DefaultArguments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Description
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

        public ITransport Transport
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
