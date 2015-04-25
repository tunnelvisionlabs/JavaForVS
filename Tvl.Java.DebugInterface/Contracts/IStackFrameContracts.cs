namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IStackFrame))]
    internal abstract class IStackFrameContracts : IStackFrame
    {
        #region IStackFrame Members

        public bool GetHasVariableInfo()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IValue> GetArgumentValues()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IValue>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IValue>>(), value => value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public IValue GetValue(ILocalVariable variable)
        {
            Contract.Requires<ArgumentNullException>(variable != null, "variable");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(variable.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IValue>() == null || this.GetVirtualMachine().Equals(Contract.Result<IValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IDictionary<ILocalVariable, IValue> GetValues(IEnumerable<ILocalVariable> variables)
        {
            Contract.Requires<ArgumentNullException>(variables != null, "variables");
#if CONTRACTS_FORALL
            Contract.Requires<ArgumentException>(Contract.ForAll(variables, variable => variable != null));
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(variables, variable => this.GetVirtualMachine().Equals(variable.GetVirtualMachine())));
#endif
            Contract.Ensures(Contract.Result<IDictionary<ILocalVariable, IValue>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<IDictionary<ILocalVariable, IValue>>(), pair => pair.Key != null && this.GetVirtualMachine().Equals(pair.Key.GetVirtualMachine()) && (pair.Value == null || this.GetVirtualMachine().Equals(pair.Value.GetVirtualMachine()))));
#endif

            throw new NotImplementedException();
        }

        public void SetValue(ILocalVariable variable, IValue value)
        {
            Contract.Requires<ArgumentNullException>(variable != null, "variable");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(variable.GetVirtualMachine()));
            Contract.Requires<VirtualMachineMismatchException>(value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IObjectReference GetThisObject()
        {
            Contract.Ensures(Contract.Result<IObjectReference>() == null || this.GetVirtualMachine().Equals(Contract.Result<IObjectReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IThreadReference GetThread()
        {
            Contract.Ensures(Contract.Result<IThreadReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ILocalVariable GetVisibleVariableByName(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<ILocalVariable>() == null || this.GetVirtualMachine().Equals(Contract.Result<ILocalVariable>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocalVariable> GetVisibleVariables()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocalVariable>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocalVariable>>(), value => value != null && this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        #endregion

        #region ILocatable Members

        public ILocation GetLocation()
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
