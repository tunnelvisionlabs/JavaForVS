namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ILocatable))]
    internal abstract class ILocatableContracts : ILocatable
    {
        #region ILocatable Members

        public ILocation GetLocation()
        {
            Contract.Ensures(Contract.Result<ILocation>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
