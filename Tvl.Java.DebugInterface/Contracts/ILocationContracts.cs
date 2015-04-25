namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ILocation))]
    internal abstract class ILocationContracts : ILocation
    {
        #region ILocation Members

        public long GetCodeIndex()
        {
            Contract.Ensures(Contract.Result<long>() >= 0);

            throw new NotImplementedException();
        }

        public IReferenceType GetDeclaringType()
        {
            Contract.Ensures(Contract.Result<IReferenceType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IReferenceType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public int GetLineNumber()
        {
            Contract.Ensures(Contract.Result<int>() >= -1);

            throw new NotImplementedException();
        }

        public int GetLineNumber(string stratum)
        {
            Contract.Requires<ArgumentException>(stratum == null || stratum.Length > 0);
            Contract.Ensures(Contract.Result<int>() >= -1);

            throw new NotImplementedException();
        }

        public IMethod GetMethod()
        {
            Contract.Ensures(Contract.Result<IMethod>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMethod>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetSourceName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public string GetSourceName(string stratum)
        {
            Contract.Requires<ArgumentException>(stratum == null || stratum.Length > 0);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public string GetSourcePath()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public string GetSourcePath(string stratum)
        {
            Contract.Requires<ArgumentException>(stratum == null || stratum.Length > 0);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<ILocation> Members

        public int CompareTo(ILocation other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<ILocation> Members

        public bool Equals(ILocation other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
