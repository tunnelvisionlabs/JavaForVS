namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IStepRequest))]
    internal abstract class IStepRequestContracts : IStepRequest
    {
        #region IStepRequest Members

        public StepDepth Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public StepSize Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IThreadReference Thread
        {
            get
            {
                Contract.Ensures(Contract.Result<IThreadReference>() == null || this.GetVirtualMachine().Equals(Contract.Result<IThreadReference>().GetVirtualMachine()));

                throw new NotImplementedException();
            }
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

        #region IInstanceFilter Members

        public void AddInstanceFilter(IObjectReference instance)
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
