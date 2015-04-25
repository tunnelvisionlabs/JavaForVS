namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Long: {Value}")]
    public class ConstantLong : ConstantPoolEntry
    {
        [DataMember]
        public readonly long _value;

        public ConstantLong(long value)
        {
            _value = value;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.Long;
            }
        }

        public long Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            return Value.ToString();
        }
    }
}
