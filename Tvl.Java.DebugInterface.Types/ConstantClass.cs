namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Class: Name #{NameIndex}")]
    public sealed class ConstantClass : ConstantPoolEntry
    {
        [DataMember]
        public readonly ushort _nameIndex;

        public ConstantClass(ushort nameIndex)
        {
            _nameIndex = nameIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.Class;
            }
        }

        public ushort NameIndex
        {
            get
            {
                return _nameIndex;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            ConstantPoolEntry entry = constantPool[NameIndex - 1];
            return entry.ToString(constantPool);
        }
    }
}
