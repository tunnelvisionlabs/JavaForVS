namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IIntegerValue))]
    internal abstract class IIntegerValueContracts : IIntegerValue
    {
        #region IIntegerValue Members

        public int GetValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<IIntegerValue> Members

        public int CompareTo(IIntegerValue other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IIntegerValue> Members

        public bool Equals(IIntegerValue other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPrimitiveValue Members

        public bool GetBooleanValue()
        {
            throw new NotImplementedException();
        }

        public byte GetByteValue()
        {
            throw new NotImplementedException();
        }

        public char GetCharValue()
        {
            throw new NotImplementedException();
        }

        public double GetDoubleValue()
        {
            throw new NotImplementedException();
        }

        public float GetFloatValue()
        {
            throw new NotImplementedException();
        }

        public int GetIntValue()
        {
            throw new NotImplementedException();
        }

        public long GetLongValue()
        {
            throw new NotImplementedException();
        }

        public short GetShortValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IValue Members

        public IType GetValueType()
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
