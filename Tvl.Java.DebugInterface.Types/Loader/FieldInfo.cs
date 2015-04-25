namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    public sealed class FieldInfo
    {
        private ushort _accessFlags;
        private ushort _nameIndex;
        private ushort _descriptorIndex;
        private AttributeInfo[] _attributes;

        private int _serializedSize;

        public int SerializedSize
        {
            get
            {
                return _serializedSize;
            }
        }

        public static FieldInfo FromMemory(IList<ConstantPoolEntry> constantPool, IntPtr ptr, int offset)
        {
            Contract.Requires<ArgumentNullException>(constantPool != null, "constantPool");
            Contract.Requires<ArgumentException>(ptr != IntPtr.Zero);

            int initialOffset = offset;

            FieldInfo fieldInfo = new FieldInfo();
            fieldInfo._accessFlags = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            fieldInfo._nameIndex = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + sizeof(ushort)));
            fieldInfo._descriptorIndex = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 2 * sizeof(ushort)));

            int attributesCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 3 * sizeof(ushort)));

            offset += 4 * sizeof(ushort);

            List<AttributeInfo> attributes = new List<AttributeInfo>();
            for (int i = 0; i < attributesCount; i++)
            {
                AttributeInfo info = AttributeInfo.FromMemory(constantPool, ptr, offset);
                offset += info.SerializedSize;
                attributes.Add(info);
            }

            fieldInfo._attributes = attributes.ToArray();
            fieldInfo._serializedSize = offset - initialOffset;
            return fieldInfo;
        }
    }
}
