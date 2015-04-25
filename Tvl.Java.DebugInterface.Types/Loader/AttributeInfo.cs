namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    public abstract class AttributeInfo
    {
        private ushort _attributeNameIndex;
        private byte[] _info;

        protected AttributeInfo(ushort attributeNameIndex, byte[] info)
        {
            _attributeNameIndex = attributeNameIndex;
            _info = info;
        }

        public abstract string Name
        {
            get;
        }

        public int SerializedSize
        {
            get
            {
                return _info.Length + 6;
            }
        }

        internal static AttributeInfo FromMemory(IList<ConstantPoolEntry> constantPool, IntPtr ptr, int offset)
        {
            Contract.Requires<ArgumentNullException>(constantPool != null, "constantPool");
            Contract.Ensures(Contract.Result<AttributeInfo>() != null);

            ushort attributeNameIndex = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            uint attributeLength = ConstantPoolEntry.ByteSwap((uint)Marshal.ReadInt32(ptr, offset + sizeof(ushort)));
            byte[] info = new byte[attributeLength];
            Marshal.Copy(ptr + offset + sizeof(ushort) + sizeof(uint), info, 0, (int)attributeLength);

            ConstantUtf8 entry = (ConstantUtf8)constantPool[attributeNameIndex - 1];
            switch (entry.Value)
            {
            case "Code":
                return new Code(attributeNameIndex, info);

            default:
                return new UnknownAttributeInfo(entry.Value, attributeNameIndex, info);
            }
        }
    }
}
