namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct VariableData
    {
        /// <summary>
        /// The local variable's index in its frame.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Slot;

        /// <summary>
        /// First code index at which the variable is visible (unsigned). Used in conjunction with
        /// <c>Length</c>. The variable can be get or set only when the current <c>CodeIndex</c> &lt;=
        /// current frame code index &lt; <c>CodeIndex + Length</c>.
        /// </summary>
        [DataMember(IsRequired = true)]
        public ulong CodeIndex;

        /// <summary>
        /// Unsigned value used in conjunction with <c>CodeIndex</c>. The variable can be get or set only
        /// when the current <c>CodeIndex</c> &lt;= current frame code index &lt; <c>CodeIndex + Length</c>.
        /// </summary>
        [DataMember(IsRequired = true)]
        public uint Length;

        /// <summary>
        /// The variable's name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name;

        /// <summary>
        /// The variable type's JNI signature.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Signature;

        /// <summary>
        /// The variable type's generic signature or an empty string if there is none.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string GenericSignature;

        public VariableData(int slot, ulong codeIndex, uint length, string name, string signature, string genericSignature)
        {
            Slot = slot;
            CodeIndex = codeIndex;
            Length = length;
            Name = name;
            Signature = signature;
            GenericSignature = genericSignature;
        }
    }
}
