namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Java.DebugInterface.Types;

    internal sealed class ArrayReference : ObjectReference, IArrayReference
    {
        // the length of an array can never change, so we can cache its value
        private int? _length;

        internal ArrayReference(VirtualMachine virtualMachine, ArrayId arrayId)
            : base(virtualMachine, arrayId, null)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal ArrayReference(VirtualMachine virtualMachine, ArrayId arrayId, IReferenceType arrayType)
            : base(virtualMachine, arrayId, arrayType)
        {
            Contract.Requires(virtualMachine != null);
        }

        public ArrayId ArrayId
        {
            get
            {
                return (ArrayId)base.ObjectId;
            }
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.Array, ObjectId.Handle);
        }

        public IValue GetValue(int index)
        {
            Types.Value[] values;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetArrayValues(out values, ArrayId, index, 1));
            return VirtualMachine.GetMirrorOf(values[0]);
        }

        public ReadOnlyCollection<IValue> GetValues()
        {
            return GetValues(0, GetLength());
        }

        public ReadOnlyCollection<IValue> GetValues(int index, int length)
        {
            Types.Value[] values;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetArrayValues(out values, ArrayId, index, length));
            return new ReadOnlyCollection<IValue>(Array.ConvertAll(values, VirtualMachine.GetMirrorOf));
        }

        public int GetLength()
        {
            if (!_length.HasValue)
            {
                int length;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetArrayLength(out length, ArrayId));
                _length = length;
            }

            return _length.Value;
        }

        public void SetValue(int index, IValue value)
        {
            SetValues(index, new[] { value }, index, 1);
        }

        public void SetValues(int index, IValue[] values, int sourceIndex, int length)
        {
            Types.Value[] converted = values.Cast<Value>().Skip(sourceIndex).Take(length).Select(Value.ToNetworkValue).ToArray();
            Contract.Assert(converted.Length == length);
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SetArrayValues(ArrayId, index, converted));
        }

        public void SetValues(IValue[] values)
        {
            SetValues(0, values, 0, GetLength());
        }
    }
}
