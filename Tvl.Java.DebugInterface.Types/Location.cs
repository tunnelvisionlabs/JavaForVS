namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct Location
    {
        [DataMember(IsRequired = true)]
        public TypeTag TypeTag;

        [DataMember(IsRequired = true)]
        public ClassId Class;

        [DataMember(IsRequired = true)]
        public MethodId Method;

        [DataMember(IsRequired = true)]
        public ulong Index;

        public Location(TaggedReferenceTypeId declaringClass, MethodId method, ulong index)
            : this(declaringClass.TypeTag, (ClassId)declaringClass.TypeId, method, index)
        {
        }

        public Location(TypeTag typeTag, ClassId @class, MethodId method, ulong index)
        {
            TypeTag = typeTag;
            Class = @class;
            Method = method;
            Index = index;
        }
    }
}
