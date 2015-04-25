namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;

    internal abstract class PrimitiveValue : Value, IPrimitiveValue
    {
        protected PrimitiveValue(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

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
    }
}
