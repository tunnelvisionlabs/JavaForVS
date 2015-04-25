namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;
    using SignatureHelper = Tvl.Java.DebugInterface.Types.SignatureHelper;

    internal sealed class LocalVariable : Mirror, ILocalVariable
    {
        private readonly Method _method;
        private readonly Types.VariableData _variableData;

        public LocalVariable(VirtualMachine virtualMachine, Method method, Types.VariableData variableData)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentNullException>(method != null, "method");

            _method = method;
            _variableData = variableData;
        }

        internal Method Method
        {
            get
            {
                return _method;
            }
        }

        internal int Slot
        {
            get
            {
                return _variableData.Slot;
            }
        }

        #region ILocalVariable Members

        public string GetGenericSignature()
        {
            return _variableData.GenericSignature;
        }

        public bool GetIsArgument()
        {
            int argumentSlots = Method.ArgumentTypeNames.Count;
            if (!Method.GetIsStatic())
                argumentSlots++;

            return Slot < argumentSlots;
        }

        public bool GetIsVisible(IStackFrame frame)
        {
            StackFrame stackFrame = frame as StackFrame;
            if (stackFrame == null)
                throw new VirtualMachineMismatchException();

            ulong codeIndex = (uint)stackFrame.Location.CodeIndex;
            return _variableData.CodeIndex <= codeIndex && codeIndex < _variableData.CodeIndex + _variableData.Length;
        }

        public bool GetIsVisible(ILocation location)
        {
            Location currentLocation = location as Location;
            if (currentLocation == null)
                throw new VirtualMachineMismatchException();

            ulong codeIndex = (uint)currentLocation.CodeIndex;
            return _variableData.CodeIndex <= codeIndex && codeIndex < _variableData.CodeIndex + _variableData.Length;
        }

        public int GetSlot()
        {
            return _variableData.Slot;
        }

        public string GetName()
        {
            return _variableData.Name;
        }

        public string GetSignature()
        {
            return _variableData.Signature;
        }

        public IType GetLocalType()
        {
            return VirtualMachine.FindType(GetSignature());
        }

        public string GetLocalTypeName()
        {
            return SignatureHelper.DecodeTypeName(GetSignature());
        }

        #endregion

        #region IEquatable<ILocalVariable> Members

        public bool Equals(ILocalVariable other)
        {
            LocalVariable localVariable = other as LocalVariable;
            if (localVariable == null)
                return false;

            return this.Slot == localVariable.Slot
                && this.Method.Equals(localVariable.Method);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LocalVariable);
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode() ^ Slot;
        }

        #endregion
    }
}
