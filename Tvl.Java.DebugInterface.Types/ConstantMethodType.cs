namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Method Type: Descriptor #{DescriptorIndex}")]
    public class ConstantMethodType : ConstantPoolEntry
    {
        [DataMember]
        private readonly ushort _descriptorIndex;

        public ConstantMethodType(ushort descriptorIndex)
        {
            _descriptorIndex = descriptorIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.MethodType;
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
            ConstantPoolEntry descriptor = constantPool[DescriptorIndex - 1];
            return descriptor.ToString(constantPool);
        }
    }
}
