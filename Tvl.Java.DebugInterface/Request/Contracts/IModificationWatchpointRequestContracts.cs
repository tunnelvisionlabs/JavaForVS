namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IModificationWatchpointRequest))]
    internal abstract class IModificationWatchpointRequestContracts : IModificationWatchpointRequest
    {
        #region IWatchpointRequest Members

        public IField Field
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region IEventRequest Members

        public bool IsEnabled
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public SuspendPolicy SuspendPolicy
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public object GetProperty(object key)
        {
            throw new System.NotImplementedException();
        }

        public void PutProperty(object key, object value)
        {
            throw new System.NotImplementedException();
        }

        public void AddCountFilter(int count)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IClassFilter Members

        public void AddClassFilter(IReferenceType referenceType)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IClassNameFilter Members

        public void AddClassExclusionFilter(string classPattern)
        {
            throw new System.NotImplementedException();
        }

        public void AddClassFilter(string classPattern)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IInstanceFilter Members

        public void AddInstanceFilter(IObjectReference instance)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IThreadFilter Members

        public void AddThreadFilter(IThreadReference thread)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
