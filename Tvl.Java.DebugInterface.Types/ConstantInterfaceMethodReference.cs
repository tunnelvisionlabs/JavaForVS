namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Interface Method: Class #{ClassIndex}, Name and Type #{NameAndTypeIndex}")]
    public class ConstantInterfaceMethodReference : ConstantPoolEntry, IConstantMemberReference
    {
        [DataMember]
        private readonly ushort _classIndex;
        [DataMember]
        private readonly ushort _nameAndTypeIndex;

        public ConstantInterfaceMethodReference(ushort classIndex, ushort nameAndTypeIndex)
        {
            _classIndex = classIndex;
            _nameAndTypeIndex = nameAndTypeIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.InterfaceMethodReference;
            }
        }

        public ushort ClassIndex
        {
            get
            {
                return _classIndex;
            }
        }

        public ushort NameAndTypeIndex
        {
            get
            {
                return _nameAndTypeIndex;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            ConstantPoolEntry classEntry = constantPool[ClassIndex - 1];
            ConstantPoolEntry nameAndTypeEntry = constantPool[NameAndTypeIndex - 1];
            return classEntry.ToString(constantPool) + "." + nameAndTypeEntry.ToString(constantPool);
        }
    }
}
