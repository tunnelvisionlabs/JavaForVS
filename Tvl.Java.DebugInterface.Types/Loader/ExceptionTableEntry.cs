namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class ExceptionTableEntry : IEquatable<ExceptionTableEntry>
    {
        [DataMember]
        private ushort _startPc;
        [DataMember]
        private ushort _endPc;
        [DataMember]
        private ushort _handlerPc;
        [DataMember]
        private ushort _catchType;

        public int StartOffset
        {
            get
            {
                return _startPc;
            }
        }

        public int EndOffset
        {
            get
            {
                return _endPc;
            }
        }

        public int HandlerOffset
        {
            get
            {
                return _handlerPc;
            }
        }

        public int SerializedSize
        {
            get
            {
                return 4 * sizeof(ushort);
            }
        }

        public static ExceptionTableEntry FromMemory(IntPtr ptr, int offset)
        {
            return new ExceptionTableEntry()
            {
                _startPc = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset)),
                _endPc = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + sizeof(ushort))),
                _handlerPc = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 2 * sizeof(ushort))),
                _catchType = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 3 * sizeof(ushort))),
            };
        }

        public static ExceptionTableEntry FromBytes(byte[] data, int offset)
        {
            return new ExceptionTableEntry()
            {
                _startPc = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(data, offset)),
                _endPc = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(data, offset + sizeof(ushort))),
                _handlerPc = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(data, offset + 2 * sizeof(ushort))),
                _catchType = ConstantPoolEntry.ByteSwap(BitConverter.ToUInt16(data, offset + 3 * sizeof(ushort))),
            };
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExceptionTableEntry);
        }

        public bool Equals(ExceptionTableEntry other)
        {
            if (other == null)
                return false;
            if (other == this)
                return true;

            return _startPc == other._startPc
                && _endPc == other._endPc
                && _handlerPc == other._handlerPc
                && _catchType == other._catchType;
        }

        public override int GetHashCode()
        {
            int hash = 31;
            hash = 7 * hash + _startPc;
            hash = 7 * hash + _endPc;
            hash = 7 * hash + _handlerPc;
            hash = 7 * hash + _catchType;
            return hash;
        }

        public override string ToString()
        {
            string behavior = _catchType == 0 ? "finally" : string.Format("catch #{0}", _catchType);
            return string.Format("try [{0}..{1}): {2} @ {3}", StartOffset, EndOffset, behavior, HandlerOffset);
        }
    }
}
