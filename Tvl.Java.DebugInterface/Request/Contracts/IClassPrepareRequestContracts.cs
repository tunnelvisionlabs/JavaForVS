namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IClassPrepareRequest))]
    internal abstract class IClassPrepareRequestContracts : IClassPrepareRequest
    {
        #region IClassPrepareRequest Members

        public void AddSourceNameFilter(string sourceNamePattern)
        {
            Contract.Requires<ArgumentNullException>(sourceNamePattern != null, "classPattern");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(sourceNamePattern));

            throw new NotImplementedException();
        }

        #endregion

        #region IClassFilter Members

        public void AddClassFilter(IReferenceType referenceType)
        {
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

        #region IEventRequest Members

        public bool IsEnabled
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

        public SuspendPolicy SuspendPolicy
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

        public object GetProperty(object key)
        {
            throw new NotImplementedException();
        }

        public void PutProperty(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void AddCountFilter(int count)
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
