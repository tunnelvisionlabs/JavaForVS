namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant Float: {Value}")]
    public class ConstantFloat : ConstantPoolEntry
    {
        [DataMember]
        private readonly float _value;

        public ConstantFloat(float value)
        {
            _value = value;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.Float;
            }
        }

        public float Value
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
