namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IFloatValue))]
    internal abstract class IFloatValueContracts : IFloatValue
    {
        #region IFloatValue Members

        public float GetValue()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<IFloatValue> Members

        public int CompareTo(IFloatValue other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IFloatValue> Members

        public bool Equals(IFloatValue other)
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
