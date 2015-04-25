namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Collections.ObjectModel;

    public sealed class Code : AttributeInfo
    {
        private ushort _maxStack;
        private ushort _maxLocals;
        private byte[] _code;
        private ExceptionTableEntry[] _exceptionTable;
        private AttributeInfo[] _attributes;

        public Code(ushort attributeNameIndex, byte[] info)
            : base(attributeNameIndex, info)
        {
            int offset = 0;
            _maxStack = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(info, offset));
            offset += 2;

            _maxLocals = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(info, offset));
            offset += 2;

            uint codeLength = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt32(info, offset));
            offset += 4;

            //_code = new byte[codeLength];
            offset += (int)codeLength;

            _exceptionTable = new ExceptionTableEntry[ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(info, offset))];
            offset += 2;

            for (int i = 0; i < _exceptionTable.Length; i++)
            {
                _exceptionTable[i] = ExceptionTableEntry.FromBytes(info, offset);
                offset += _exceptionTable[i].SerializedSize;
            }

            // TODO: handle attributes
        }

        public override string Name
        {
            get
            {
                return "Code";
            }
        }

        public ReadOnlyCollection<ExceptionTableEntry> ExceptionTable
        {
            get
            {
                return new ReadOnlyCollection<ExceptionTableEntry>(_exceptionTable);
            }
        }

        public ReadOnlyCollection<AttributeInfo> Attributes
        {
            get
            {
                //return new ReadOnlyCollection<AttributeInfo>(_attributes);
                throw new NotImplementedException();
            }
        }
    }
}
