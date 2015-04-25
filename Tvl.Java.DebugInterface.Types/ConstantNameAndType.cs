namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Name and Type: Name #{NameIndex}, Descriptor #{DescriptorIndex}")]
    public class ConstantNameAndType : ConstantPoolEntry
    {
        [DataMember]
        private ushort _nameIndex;
        [DataMember]
        private ushort _descriptorIndex;

        public ConstantNameAndType(ushort nameIndex, ushort descriptorIndex)
        {
            _nameIndex = nameIndex;
            _descriptorIndex = descriptorIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.NameAndType;
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

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            ConstantPoolEntry nameEntry = constantPool[NameIndex - 1];
            ConstantPoolEntry descriptorEntry = constantPool[DescriptorIndex - 1];
            return nameEntry.ToString(constantPool) + " " + descriptorEntry.ToString(constantPool);
        }
    }
}
