namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    public sealed class MethodInfo
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

        public AccessModifiers Modifiers
        {
            get
            {
                return (AccessModifiers)_accessFlags;
            }
        }

        public ushort NameIndex
        {
            get
            {
                return _nameIndex;
            }
        }

        public ushort DescriptorIndex
        {
            get
            {
                return _descriptorIndex;
            }
        }

        public ReadOnlyCollection<AttributeInfo> Attributes
        {
            get
            {
                return new ReadOnlyCollection<AttributeInfo>(_attributes);
            }
        }

        public static MethodInfo FromMemory(IList<ConstantPoolEntry> constantPool, IntPtr ptr, int offset)
        {
            Contract.Requires<ArgumentException>(ptr != IntPtr.Zero);

            int initialOffset = offset;

            MethodInfo methodInfo = new MethodInfo();
            methodInfo._accessFlags = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            methodInfo._nameIndex = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + sizeof(ushort)));
            methodInfo._descriptorIndex = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 2 * sizeof(ushort)));

            int attributesCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 3 * sizeof(ushort)));

            offset += 4 * sizeof(ushort);
            List<AttributeInfo> attributes = new List<AttributeInfo>();
            for (int i = 0; i < attributesCount; i++)
            {
                AttributeInfo info = AttributeInfo.FromMemory(constantPool, ptr, offset);
                offset += info.SerializedSize;
                attributes.Add(info);
            }

            methodInfo._attributes = attributes.ToArray();
            methodInfo._serializedSize = offset - initialOffset;
            return methodInfo;
        }
    }
}
