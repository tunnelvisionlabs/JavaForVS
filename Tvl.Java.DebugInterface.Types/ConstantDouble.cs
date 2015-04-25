namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Double: {Value}")]
    public class ConstantDouble : ConstantPoolEntry
    {
        [DataMember]
        private readonly double _value;

        public ConstantDouble(double value)
        {
            _value = value;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.Double;
            }
        }

        public double Value
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
