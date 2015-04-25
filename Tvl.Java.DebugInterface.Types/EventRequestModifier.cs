namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct EventRequestModifier
    {
        [DataMember(IsRequired = true)]
        public ModifierKind Kind;

        /// <summary>
        /// Count before event, or one for one-off.
        /// </summary>
        [DataMember]
        public int Count;

        [DataMember]
        public int ExpressionId;

        [DataMember]
        public ThreadId Thread;

        [DataMember]
        public ReferenceTypeId Class;

        // used by ClassMatch and ClassExclude modifiers
        [DataMember]
        public string ClassPattern;

        [DataMember]
        public Location Location;

        [DataMember]
        public ReferenceTypeId ExceptionOrNull;

        [DataMember]
        public bool Caught;

        [DataMember]
        public bool Uncaught;

        [DataMember]
        public ReferenceTypeId DeclaringClass;

        [DataMember]
        public FieldId Field;

        [DataMember]
        public StepSize StepSize;

        [DataMember]
        public StepDepth StepDepth;

        [DataMember]
        public ObjectId Instance;

        [DataMember]
        public string SourceNamePattern;

        public static EventRequestModifier CountFilter(int count)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.Count,
                Count = count,
            };
        }

        public static EventRequestModifier ConditionalFilter(int expressionId)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.Conditional,
                ExpressionId = expressionId,
            };
        }

        public static EventRequestModifier ThreadFilter(ThreadId thread)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.ThreadFilter,
                Thread = thread,
            };
        }

        public static EventRequestModifier ClassTypeFilter(ReferenceTypeId @class)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.ClassTypeFilter,
                Class = @class,
            };
        }

        public static EventRequestModifier ClassMatchFilter(string classPattern)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.ClassMatchFilter,
                ClassPattern = classPattern,
            };
        }

        public static EventRequestModifier ClassExcludeFilter(string classPattern)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.ClassExcludeFilter,
                ClassPattern = classPattern,
            };
        }

        public static EventRequestModifier LocationFilter(Location location)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.LocationFilter,
                Location = location,
            };
        }

        public static EventRequestModifier ExceptionFilter(ReferenceTypeId exceptionOrNull, bool caught, bool uncaught)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.ExceptionFilter,
                ExceptionOrNull = exceptionOrNull,
                Caught = caught,
                Uncaught = uncaught,
            };
        }

        public static EventRequestModifier FieldFilter(ReferenceTypeId declaringClass, FieldId field)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.FieldFilter,
                DeclaringClass = declaringClass,
                Field = field,
            };
        }

        public static EventRequestModifier Step(ThreadId thread, StepSize size, StepDepth depth)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.Step,
                Thread = thread,
                StepSize = size,
                StepDepth = depth,
            };
        }

        public static EventRequestModifier InstanceFilter(ObjectId instance)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.InstanceFilter,
                Instance = instance,
            };
        }

        public static EventRequestModifier SourceNameMatch(string sourceNamePattern)
        {
            return new EventRequestModifier()
            {
                Kind = ModifierKind.SourceNameMatchFilter,
                SourceNamePattern = sourceNamePattern,
            };
        }
    }
}
