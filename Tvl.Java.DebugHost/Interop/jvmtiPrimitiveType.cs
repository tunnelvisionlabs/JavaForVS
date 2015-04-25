namespace Tvl.Java.DebugHost.Interop
{
    internal enum jvmtiPrimitiveType
    {
        Boolean = 90, // 'Z' - Java programming language boolean - JNI jboolean  
        Byte = 66, // 'B' - Java programming language byte - JNI jbyte  
        Char = 67, // 'C' - Java programming language char - JNI jchar  
        Short = 83, // 'S' - Java programming language short - JNI jshort  
        Int = 73, // 'I' - Java programming language int - JNI jint  
        Long = 74, // 'J' - Java programming language long - JNI jlong  
        Float = 70, // 'F' - Java programming language float - JNI jfloat  
        Double = 68, // 'D' - Java programming language double - JNI jdouble 
    }
}
