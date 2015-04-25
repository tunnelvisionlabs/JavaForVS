namespace Tvl.Java.DebugInterface.Connect.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ITransport))]
    internal abstract class ITransportContracts : ITransport
    {
        #region ITransport Members

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
