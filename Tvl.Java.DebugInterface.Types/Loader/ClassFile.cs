namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices;

    public sealed class ClassFile
    {
        private const uint HeaderMagic = 0xCAFEBABE;

        private ushort _minorVersion;
        private ushort _majorVersion;
        private ConstantPoolEntry[] _constantPool;
        private ushort _accessFlags;
        private ushort _thisClass;
        private ushort _superClass;
        private ushort[] _interfaces;
        private FieldInfo[] _fields;
        private MethodInfo[] _methods;
        private AttributeInfo[] _attributes;

        public ReadOnlyCollection<ConstantPoolEntry> ConstantPool
        {
            get
            {
                return new ReadOnlyCollection<ConstantPoolEntry>(_constantPool);
            }
        }

        public ConstantClass ThisClass
        {
            get
            {
                return (ConstantClass)_constantPool[_thisClass - 1];
            }
        }

        public ConstantClass SuperClass
        {
            get
            {
                if (_superClass == 0)
                    return null;

                return (ConstantClass)_constantPool[_superClass - 1];
            }
        }

        public ReadOnlyCollection<FieldInfo> Fields
        {
            get
            {
                return new ReadOnlyCollection<FieldInfo>(_fields);
            }
        }

        public ReadOnlyCollection<MethodInfo> Methods
        {
            get
            {
                return new ReadOnlyCollection<MethodInfo>(_methods);
            }
        }

        public ReadOnlyCollection<AttributeInfo> Attributes
        {
            get
            {
                return new ReadOnlyCollection<AttributeInfo>(_attributes);
            }
        }

        public static ClassFile FromMemory(IntPtr ptr, int offset)
        {
            int initialOffset = offset;

            uint magic = ConstantPoolEntry.ByteSwap((uint)Marshal.ReadInt32(ptr, offset));
            offset += sizeof(uint);

            ClassFile classFile = new ClassFile();

            classFile._minorVersion = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            classFile._majorVersion = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset + 2));
            offset += 2 * sizeof(ushort);

            ushort constantPoolCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += 2;
            IntPtr constantPoolPtr = ptr + offset;
            classFile._constantPool = new ConstantPoolEntry[constantPoolCount];
            for (int i = 0; i < constantPoolCount - 1; i++)
            {
                classFile._constantPool[i] = ConstantPoolEntry.FromMemory(ref constantPoolPtr);
                switch (classFile._constantPool[i].Type)
                {
                case ConstantType.Double:
                case ConstantType.Long:
                    // these entries take 2 slots
                    classFile._constantPool[i + 1] = ConstantPoolEntry.Reserved;
                    i++;
                    break;

                default:
                    break;
                }
            }

            offset = (int)(constantPoolPtr.ToInt64() - (ptr + initialOffset).ToInt64());

            classFile._accessFlags = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._thisClass = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._superClass = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            ushort interfacesCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._interfaces = new ushort[interfacesCount];
            for (int i = 0; i < interfacesCount; i++)
            {
                classFile._interfaces[i] = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
                offset += sizeof(ushort);
            }

            ushort fieldsCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._fields = new FieldInfo[fieldsCount];
            for (int i = 0; i < fieldsCount; i++)
            {
                classFile._fields[i] = FieldInfo.FromMemory(classFile._constantPool, ptr, offset);
                offset += classFile._fields[i].SerializedSize;
            }

            ushort methodsCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._methods = new MethodInfo[methodsCount];
            for (int i = 0; i < methodsCount; i++)
            {
                classFile._methods[i] = MethodInfo.FromMemory(classFile._constantPool, ptr, offset);
                offset += classFile._methods[i].SerializedSize;
            }

            ushort attributesCount = ConstantPoolEntry.ByteSwap((ushort)Marshal.ReadInt16(ptr, offset));
            offset += sizeof(ushort);

            classFile._attributes = new AttributeInfo[attributesCount];
            for (int i = 0; i < attributesCount; i++)
            {
                classFile._attributes[i] = AttributeInfo.FromMemory(classFile._constantPool, ptr, offset);
                offset += classFile._attributes[i].SerializedSize;
            }

            return classFile;
        }
    }
}
