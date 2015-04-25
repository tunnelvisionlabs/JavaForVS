namespace Tvl.Java.DebugInterface.Types
{
    public enum ModifierKind
    {
        Invalid = 0,
        Count = 1,
        Conditional = 2,
        ThreadFilter = 3,
        ClassTypeFilter = 4,
        ClassMatchFilter = 5,
        ClassExcludeFilter = 6,
        LocationFilter = 7,
        ExceptionFilter = 8,
        FieldFilter = 9,
        Step = 10,
        InstanceFilter = 11,
        SourceNameMatchFilter = 12,
    }
}
