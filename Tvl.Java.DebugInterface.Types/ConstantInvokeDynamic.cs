namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Invoke Dynamic: Bootstrap Method #{BootstrapMethodAttrIndex}, Name and Type #{NameAndTypeIndex}")]
    public class ConstantInvokeDynamic : ConstantPoolEntry
    {
        [DataMember]
        private readonly ushort _bootstrapMethodAttrIndex;
        [DataMember]
        private readonly ushort _nameAndTypeIndex;

        public ConstantInvokeDynamic(ushort bootstrapMethodAttrIndex, ushort nameAndTypeIndex)
        {
            _bootstrapMethodAttrIndex = bootstrapMethodAttrIndex;
            _nameAndTypeIndex = nameAndTypeIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.InvokeDynamic;
            }
        }

        public ushort BootstrapMethodAttrIndex
        {
            get
            {
                return _bootstrapMethodAttrIndex;
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
            ConstantPoolEntry bootstrapMethodAttrEntry = constantPool[BootstrapMethodAttrIndex - 1];
            ConstantPoolEntry nameAndTypeEntry = constantPool[NameAndTypeIndex - 1];
            return bootstrapMethodAttrEntry.ToString(constantPool) + "." + nameAndTypeEntry.ToString(constantPool);
        }
    }
}
