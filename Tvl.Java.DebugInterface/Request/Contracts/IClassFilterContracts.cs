namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IClassFilter))]
    internal abstract class IClassFilterContracts : IClassFilter
    {
        #region IClassFilter Members

        public void AddClassFilter(IReferenceType referenceType)
        {
            Contract.Requires<ArgumentNullException>(referenceType != null, "referenceType");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(referenceType.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        #endregion

        #region IClassNameFilter Members

        public void AddClassExclusionFilter(string classPattern)
        {
            throw new NotImplementedException();
        }

        public void AddClassFilter(string classPattern)
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
