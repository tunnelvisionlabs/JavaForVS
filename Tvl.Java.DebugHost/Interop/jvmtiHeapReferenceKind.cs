namespace Tvl.Java.DebugHost.Interop
{
    internal enum jvmtiHeapReferenceKind
    {
        Class = 1, // Reference from an object to its class.  
        Field = 2, // Reference from an object to the value of one of its instance fields.  
        ArrayElement = 3, // Reference from an array to one of its elements.  
        ClassLoader = 4, // Reference from a class to its class loader.  
        Signers = 5, // Reference from a class to its signers array.  
        ProtectionDomain = 6, // Reference from a class to its protection domain.  
        Interface = 7, // Reference from a class to one of its interfaces. Note: interfaces are defined via a constant pool reference, so the referenced interfaces may also be reported with a JVMTI_HEAP_REFERENCE_CONSTANT_POOL reference kind.  
        StaticField = 8, // Reference from a class to the value of one of its static fields.  
        ConstantPool = 9, // Reference from a class to a resolved entry in the constant pool.  
        Superclass = 10, // Reference from a class to its superclass. A callback is bot sent if the superclass is java.lang.Object. Note: loaded classes define superclasses via a constant pool reference, so the referenced superclass may also be reported with a JVMTI_HEAP_REFERENCE_CONSTANT_POOL reference kind.  
        JniGlobal = 21, // Heap root reference: JNI global reference.  
        SystemClass = 22, // Heap root reference: System class.  
        Monitor = 23, // Heap root reference: monitor.  
        StackLocal = 24, // Heap root reference: local variable on the stack.  
        JniLocal = 25, // Heap root reference: JNI local reference.  
        Thread = 26, // Heap root reference: Thread.  
        Other = 27, // Heap root reference: other heap root reference.  
    }
}
