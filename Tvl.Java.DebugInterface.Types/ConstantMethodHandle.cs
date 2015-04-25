namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Method Handle: Reference Kind #{ReferenceKind}, Reference Index #{ReferenceIndex}")]
    public class ConstantMethodHandle : ConstantPoolEntry
    {
        [DataMember]
        private readonly MethodHandleKind _referenceKind;
        [DataMember]
        private readonly ushort _referenceIndex;

        public ConstantMethodHandle(MethodHandleKind referenceKind, ushort referenceIndex)
        {
            _referenceKind = referenceKind;
            _referenceIndex = referenceIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.MethodHandle;
            }
        }

        public MethodHandleKind ReferenceKind
        {
            get
            {
                return _referenceKind;
            }
        }

        public ushort ReferenceIndex
        {
            get
            {
                return _referenceIndex;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            ConstantPoolEntry referenceEntry = constantPool[ReferenceIndex - 1];
            return ReferenceKind + " " + referenceEntry.ToString(constantPool);
        }
    }
}
