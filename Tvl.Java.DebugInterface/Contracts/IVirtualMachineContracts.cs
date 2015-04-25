namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Events;
    using Tvl.Java.DebugInterface.Request;

    [ContractClassFor(typeof(IVirtualMachine))]
    internal abstract class IVirtualMachineContracts : IVirtualMachine
    {
        #region IVirtualMachine Members

        public ReadOnlyCollection<IReferenceType> GetAllClasses()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IReferenceType>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IReferenceType>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadReference> GetAllThreads()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IThreadReference>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IThreadReference>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

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
            Contract.Requires<ArgumentNullException>(className != null, "className");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(className));
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IReferenceType>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IReferenceType>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public IEventQueue GetEventQueue()
        {
            Contract.Ensures(Contract.Result<IEventQueue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IEventQueue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IEventRequestManager GetEventRequestManager()
        {
            Contract.Ensures(Contract.Result<IEventRequestManager>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IEventRequestManager>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void Exit(int exitCode)
        {
            throw new NotImplementedException();
        }

        public string GetDefaultStratum()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public long[] GetInstanceCounts(IEnumerable<IReferenceType> referenceTypes)
        {
            Contract.Requires<ArgumentNullException>(referenceTypes != null, "referenceTypes");
#if CONTRACTS_FORALL
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(referenceTypes, type => this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif
            Contract.Ensures(Contract.Result<long[]>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<long[]>(), count => count >= 0));
#endif

            throw new NotImplementedException();
        }

        public IBooleanValue GetMirrorOf(bool value)
        {
            Contract.Ensures(Contract.Result<IBooleanValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IBooleanValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IByteValue GetMirrorOf(byte value)
        {
            Contract.Ensures(Contract.Result<IByteValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IByteValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ICharValue GetMirrorOf(char value)
        {
            Contract.Ensures(Contract.Result<ICharValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<ICharValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IDoubleValue GetMirrorOf(double value)
        {
            Contract.Ensures(Contract.Result<IDoubleValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IDoubleValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IFloatValue GetMirrorOf(float value)
        {
            Contract.Ensures(Contract.Result<IFloatValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IFloatValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IIntegerValue GetMirrorOf(int value)
        {
            Contract.Ensures(Contract.Result<IIntegerValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IIntegerValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ILongValue GetMirrorOf(long value)
        {
            Contract.Ensures(Contract.Result<ILongValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<ILongValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IShortValue GetMirrorOf(short value)
        {
            Contract.Ensures(Contract.Result<IShortValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IShortValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IStrongValueHandle<IStringReference> GetMirrorOf(string value)
        {
            Contract.Ensures(Contract.Result<IStrongValueHandle<IStringReference>>() == null || this.GetVirtualMachine().Equals(Contract.Result<IStrongValueHandle<IStringReference>>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IVoidValue GetMirrorOfVoid()
        {
            Contract.Ensures(Contract.Result<IVoidValue>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IVoidValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public void RedefineClasses(IEnumerable<KeyValuePair<IReferenceType, byte[]>> classes)
        {
            Contract.Requires<ArgumentNullException>(classes != null, "classes");
#if CONTRACTS_FORALL
            Contract.Requires<ArgumentException>(Contract.ForAll(classes, pair => pair.Key != null && pair.Value != null));
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(classes, pair => this.GetVirtualMachine().Equals(pair.Key.GetVirtualMachine())));
#endif

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
            Contract.Requires<ArgumentNullException>(stratum != null, "stratum");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(stratum));

            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadGroupReference> GetTopLevelThreadGroups()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IThreadGroupReference>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IThreadGroupReference>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

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
