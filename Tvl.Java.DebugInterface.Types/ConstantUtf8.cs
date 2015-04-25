namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant UTF8: {Value}")]
    public class ConstantUtf8 : ConstantPoolEntry
    {
        [DataMember]
        private readonly string _value;

        public ConstantUtf8(string value)
        {
            Contract.Requires<ArgumentNullException>(value != null, "value");
            _value = value;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.Utf8;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            return Value;
        }
    }
}
