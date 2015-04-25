namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct LineNumberData
    {
        [DataMember(IsRequired = true)]
        public long LineCodeIndex;

        [DataMember(IsRequired = true)]
        public int LineNumber;

        public LineNumberData(long lineCodeIndex, int lineNumber)
        {
            LineCodeIndex = lineCodeIndex;
            LineNumber = lineNumber;
        }
    }
}
