namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IPathSearchingVirtualMachine))]
    internal abstract class IPathSearchingVirtualMachineContracts : IPathSearchingVirtualMachine
    {
        #region IPathSearchingVirtualMachine Members

        public string GetBaseDirectory()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetBootClassPath()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), name => !string.IsNullOrEmpty(name)));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetClassPath()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), name => !string.IsNullOrEmpty(name)));
#endif

            throw new NotImplementedException();
        }

        #endregion

        #region IVirtualMachine Members

        public ReadOnlyCollection<IReferenceType> GetAllClasses()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadReference> GetAllThreads()
        {
            throw new NotImplementedException();
        }

        public bool GetCanAddMethod()
        {
            throw new NotImplementedException();
        }

        public bool GetCanBeModified()
        {
            throw new NotImplementedException();
        }

        public bool GetCanForceEarlyReturn()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetBytecodes()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetClassFileVersion()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetConstantPool()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetCurrentContendedMonitor()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetInstanceInfo()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetMethodReturnValues()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetMonitorFrameInfo()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetMonitorInfo()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetOwnedMonitorInfo()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetSourceDebugExtension()
        {
            throw new NotImplementedException();
        }

        public bool GetCanGetSyntheticAttribute()
        {
            throw new NotImplementedException();
        }

        public bool GetCanPopFrames()
        {
            throw new NotImplementedException();
        }

        public bool GetCanRedefineClasses()
        {
            throw new NotImplementedException();
        }

        public bool GetCanRequestMonitorEvents()
        {
            throw new NotImplementedException();
        }

        public bool GetCanRequestVMDeathEvent()
        {
            throw new NotImplementedException();
        }

        public bool GetCanUnrestrictedlyRedefineClasses()
        {
            throw new NotImplementedException();
        }

        public bool GetCanUseInstanceFilters()
        {
            throw new NotImplementedException();
        }

        public bool GetCanUseSourceNameFilters()
        {
            throw new NotImplementedException();
        }

        public bool GetCanWatchFieldAccess()
        {
            throw new NotImplementedException();
        }

        public bool GetCanWatchFieldModification()
        {
            throw new NotImplementedException();
        }

        public bool GetCanStepByStatement()
        {
            throw new NotImplementedException();
        }

        public bool GetCanInvokeWithoutThread()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IReferenceType> GetClassesByName(string className)
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public Events.IEventQueue GetEventQueue()
        {
            throw new NotImplementedException();
        }

        public Request.IEventRequestManager GetEventRequestManager()
        {
            throw new NotImplementedException();
        }

        public void Exit(int exitCode)
        {
            throw new NotImplementedException();
        }

        public string GetDefaultStratum()
        {
            throw new NotImplementedException();
        }

        public long[] GetInstanceCounts(IEnumerable<IReferenceType> referenceTypes)
        {
            throw new NotImplementedException();
        }

        public IBooleanValue GetMirrorOf(bool value)
        {
            throw new NotImplementedException();
        }

        public IByteValue GetMirrorOf(byte value)
        {
            throw new NotImplementedException();
        }

        public ICharValue GetMirrorOf(char value)
        {
            throw new NotImplementedException();
        }

        public IDoubleValue GetMirrorOf(double value)
        {
            throw new NotImplementedException();
        }

        public IFloatValue GetMirrorOf(float value)
        {
            throw new NotImplementedException();
        }

        public IIntegerValue GetMirrorOf(int value)
        {
            throw new NotImplementedException();
        }

        public ILongValue GetMirrorOf(long value)
        {
            throw new NotImplementedException();
        }

        public IShortValue GetMirrorOf(short value)
        {
            throw new NotImplementedException();
        }

        public IStrongValueHandle<IStringReference> GetMirrorOf(string value)
        {
            throw new NotImplementedException();
        }

        public IVoidValue GetMirrorOfVoid()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public void RedefineClasses(IEnumerable<KeyValuePair<IReferenceType, byte[]>> classes)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void SetDebugTraceMode(TraceModes modes)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultStratum(string stratum)
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadGroupReference> GetTopLevelThreadGroups()
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
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
