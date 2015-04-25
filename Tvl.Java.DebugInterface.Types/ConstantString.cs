namespace Tvl.Java.DebugInterface.Types
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerDisplay("Constant String: #{StringIndex}")]
    public class ConstantString : ConstantPoolEntry
    {
        [DataMember]
        private readonly ushort _stringIndex;

        public ConstantString(ushort stringIndex)
        {
            _stringIndex = stringIndex;
        }

        public override ConstantType Type
        {
            get
            {
                return ConstantType.String;
            }
        }

        public ushort StringIndex
        {
            get
            {
                return _stringIndex;
            }
        }

        public override string ToString(ReadOnlyCollection<ConstantPoolEntry> constantPool)
        {
            string value = null;
            ConstantUtf8 utf8 = constantPool[_stringIndex - 1] as ConstantUtf8;
            if (utf8 != null)
                value = utf8.Value;

            if (value == null)
                return "<invalid string>";

            return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }
    }
}
