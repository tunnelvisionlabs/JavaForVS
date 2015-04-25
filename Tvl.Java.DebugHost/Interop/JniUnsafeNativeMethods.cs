namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;
    using jarray = jobject;
    using jboolean = System.Byte;
    using jbooleanArray = jobject;
    using jbyte = System.Byte;
    using jbyteArray = jobject;
    using jchar = System.Char;
    using jcharArray = jobject;
    using jdouble = System.Double;
    using jdoubleArray = jobject;
    using jfloat = System.Single;
    using jfloatArray = jobject;
    using jint = System.Int32;
    using jintArray = jobject;
    using jlong = System.Int64;
    using jlongArray = jobject;
    using jobjectArray = jobject;
    using jshort = System.Int16;
    using jshortArray = jobject;
    using jsize = System.Int32;
    using jstring = jobject;
    using va_list = System.IntPtr;

    internal static class JniUnsafeNativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint GetVersion(JNIEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jclass DefineClass(JNIEnvHandle env, [MarshalAs(UnmanagedType.LPStr)]string name, jobject loader, jbyte[] buf, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jclass FindClass(JNIEnvHandle env, [MarshalAs(UnmanagedType.LPStr)]string name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jmethodID FromReflectedMethod(JNIEnvHandle env, jobject method);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfieldID FromReflectedField(JNIEnvHandle env, jobject field);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject ToReflectedMethod(JNIEnvHandle env, jclass cls, jmethodID methodID, jboolean isStatic);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jclass GetSuperclass(JNIEnvHandle env, jclass sub);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean IsAssignableFrom(JNIEnvHandle env, jclass sub, jclass sup);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject ToReflectedField(JNIEnvHandle env, jclass cls, jfieldID fieldID, jboolean isStatic);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint Throw(JNIEnvHandle env, jthrowable obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint ThrowNew(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPStr)]string msg);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jthrowable ExceptionOccurred(JNIEnvHandle env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ExceptionDescribe(JNIEnvHandle env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ExceptionClear(JNIEnvHandle env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void FatalError(JNIEnvHandle env, [MarshalAs(UnmanagedType.LPStr)]string msg);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint PushLocalFrame(JNIEnvHandle env, jint capacity);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject PopLocalFrame(JNIEnvHandle env, jobject result);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject NewGlobalRef(JNIEnvHandle env, jobject lobj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DeleteGlobalRef(JNIEnvHandle env, jobject gref);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DeleteLocalRef(JNIEnvHandle env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean IsSameObject(JNIEnvHandle env, jobject obj1, jobject obj2);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject NewLocalRef(JNIEnvHandle env, jobject @ref);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint EnsureLocalCapacity(JNIEnvHandle env, jint capacity);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject AllocObject(JNIEnvHandle env, jclass clazz);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jobject NewObject(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject NewObjectV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject NewObjectA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jclass GetObjectClass(JNIEnvHandle env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean IsInstanceOf(JNIEnvHandle env, jobject obj, jclass clazz);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jmethodID GetMethodID(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string sig);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jobject CallObjectMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallObjectMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallObjectMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jboolean CallBooleanMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallBooleanMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallBooleanMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jbyte CallByteMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallByteMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallByteMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jchar CallCharMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallCharMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallCharMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jshort CallShortMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallShortMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallShortMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jint CallIntMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallIntMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallIntMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jlong CallLongMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallLongMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallLongMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jfloat CallFloatMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallFloatMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallFloatMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jdouble CallDoubleMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallDoubleMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallDoubleMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallVoidMethod(JNIEnvHandle env, jobject obj, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallVoidMethodV(JNIEnvHandle env, jobject obj, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallVoidMethodA(JNIEnvHandle env, jobject obj, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jobject CallNonvirtualObjectMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallNonvirtualObjectMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallNonvirtualObjectMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jboolean CallNonvirtualBooleanMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallNonvirtualBooleanMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallNonvirtualBooleanMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jbyte CallNonvirtualByteMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallNonvirtualByteMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallNonvirtualByteMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jchar CallNonvirtualCharMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallNonvirtualCharMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallNonvirtualCharMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jshort CallNonvirtualShortMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallNonvirtualShortMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallNonvirtualShortMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jint CallNonvirtualIntMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallNonvirtualIntMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallNonvirtualIntMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jlong CallNonvirtualLongMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallNonvirtualLongMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallNonvirtualLongMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jfloat CallNonvirtualFloatMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallNonvirtualFloatMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallNonvirtualFloatMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jdouble CallNonvirtualDoubleMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallNonvirtualDoubleMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallNonvirtualDoubleMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallNonvirtualVoidMethod(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallNonvirtualVoidMethodV(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallNonvirtualVoidMethodA(JNIEnvHandle env, jobject obj, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfieldID GetFieldID(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string sig);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject GetObjectField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean GetBooleanField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte GetByteField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar GetCharField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort GetShortField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint GetIntField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong GetLongField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat GetFloatField(JNIEnvHandle env, jobject obj, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble GetDoubleField(JNIEnvHandle env, jobject obj, jfieldID fieldID);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetObjectField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jobject val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetBooleanField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jboolean val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetByteField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jbyte val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetCharField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jchar val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetShortField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jshort val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetIntField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jint val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetLongField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jlong val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetFloatField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jfloat val);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetDoubleField(JNIEnvHandle env, jobject obj, jfieldID fieldID, jdouble val);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jmethodID GetStaticMethodID(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string sig);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jobject CallStaticObjectMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallStaticObjectMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject CallStaticObjectMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jboolean CallStaticBooleanMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallStaticBooleanMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean CallStaticBooleanMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jbyte CallStaticByteMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallStaticByteMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte CallStaticByteMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jchar CallStaticCharMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallStaticCharMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar CallStaticCharMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jshort CallStaticShortMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallStaticShortMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort CallStaticShortMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jint CallStaticIntMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallStaticIntMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint CallStaticIntMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jlong CallStaticLongMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallStaticLongMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong CallStaticLongMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jfloat CallStaticFloatMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallStaticFloatMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat CallStaticFloatMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jdouble CallStaticDoubleMethod(JNIEnvHandle env, jclass clazz, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallStaticDoubleMethodV(JNIEnvHandle env, jclass clazz, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble CallStaticDoubleMethodA(JNIEnvHandle env, jclass clazz, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallStaticVoidMethod(JNIEnvHandle env, jclass cls, jmethodID methodID/*, __arglist*/);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallStaticVoidMethodV(JNIEnvHandle env, jclass cls, jmethodID methodID, va_list args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallStaticVoidMethodA(JNIEnvHandle env, jclass cls, jmethodID methodID, [MarshalAs(UnmanagedType.LPArray)]params jvalue[] args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfieldID GetStaticFieldID(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string sig);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject GetStaticObjectField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean GetStaticBooleanField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyte GetStaticByteField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jchar GetStaticCharField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshort GetStaticShortField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint GetStaticIntField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong GetStaticLongField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloat GetStaticFloatField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdouble GetStaticDoubleField(JNIEnvHandle env, jclass clazz, jfieldID fieldID);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticObjectField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jobject value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticBooleanField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jboolean value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticByteField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jbyte value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticCharField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jchar value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticShortField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jshort value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticIntField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jint value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticLongField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jlong value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticFloatField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jfloat value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetStaticDoubleField(JNIEnvHandle env, jclass clazz, jfieldID fieldID, jdouble value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jstring NewString(JNIEnvHandle env, [MarshalAs(UnmanagedType.LPWStr)]string unicode, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jsize GetStringLength(JNIEnvHandle env, jstring str);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetStringChars(JNIEnvHandle env, jstring str, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseStringChars(JNIEnvHandle env, jstring str, IntPtr chars);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jstring NewStringUTF(JNIEnvHandle env, [MarshalAs(UnmanagedType.LPStr)]string utf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jsize GetStringUTFLength(JNIEnvHandle env, jstring str);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetStringUTFChars(JNIEnvHandle env, jstring str, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseStringUTFChars(JNIEnvHandle env, jstring str, IntPtr chars);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jsize GetArrayLength(JNIEnvHandle env, jarray array);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobjectArray NewObjectArray(JNIEnvHandle env, jsize len, jclass clazz, jobject init);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject GetObjectArrayElement(JNIEnvHandle env, jobjectArray array, jsize index);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetObjectArrayElement(JNIEnvHandle env, jobjectArray array, jsize index, jobject val);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbooleanArray NewBooleanArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jbyteArray NewByteArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jcharArray NewCharArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jshortArray NewShortArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jintArray NewIntArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlongArray NewLongArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jfloatArray NewFloatArray(JNIEnvHandle env, jsize len);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jdoubleArray NewDoubleArray(JNIEnvHandle env, jsize len);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetBooleanArrayElements(JNIEnvHandle env, jbooleanArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetByteArrayElements(JNIEnvHandle env, jbyteArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetCharArrayElements(JNIEnvHandle env, jcharArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetShortArrayElements(JNIEnvHandle env, jshortArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetIntArrayElements(JNIEnvHandle env, jintArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetLongArrayElements(JNIEnvHandle env, jlongArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetFloatArrayElements(JNIEnvHandle env, jfloatArray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetDoubleArrayElements(JNIEnvHandle env, jdoubleArray array, out jboolean isCopy);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseBooleanArrayElements(JNIEnvHandle env, jbooleanArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseByteArrayElements(JNIEnvHandle env, jbyteArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseCharArrayElements(JNIEnvHandle env, jcharArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseShortArrayElements(JNIEnvHandle env, jshortArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseIntArrayElements(JNIEnvHandle env, jintArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseLongArrayElements(JNIEnvHandle env, jlongArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseFloatArrayElements(JNIEnvHandle env, jfloatArray array, IntPtr elems, jint mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseDoubleArrayElements(JNIEnvHandle env, jdoubleArray array, IntPtr elems, jint mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetBooleanArrayRegion(JNIEnvHandle env, jbooleanArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)]bool[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetByteArrayRegion(JNIEnvHandle env, jbyteArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jbyte[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetCharArrayRegion(JNIEnvHandle env, jcharArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jchar[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetShortArrayRegion(JNIEnvHandle env, jshortArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jshort[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetIntArrayRegion(JNIEnvHandle env, jintArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jint[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetLongArrayRegion(JNIEnvHandle env, jlongArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jlong[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetFloatArrayRegion(JNIEnvHandle env, jfloatArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jfloat[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetDoubleArrayRegion(JNIEnvHandle env, jdoubleArray array, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jdouble[] buf);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetBooleanArrayRegion(JNIEnvHandle env, jbooleanArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)]bool[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetByteArrayRegion(JNIEnvHandle env, jbyteArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jbyte[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetCharArrayRegion(JNIEnvHandle env, jcharArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jchar[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetShortArrayRegion(JNIEnvHandle env, jshortArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jshort[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetIntArrayRegion(JNIEnvHandle env, jintArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jint[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetLongArrayRegion(JNIEnvHandle env, jlongArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jlong[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetFloatArrayRegion(JNIEnvHandle env, jfloatArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jfloat[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SetDoubleArrayRegion(JNIEnvHandle env, jdoubleArray array, jsize start, jsize len, [MarshalAs(UnmanagedType.LPArray)]jdouble[] buf);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint RegisterNatives(JNIEnvHandle env, jclass clazz, [MarshalAs(UnmanagedType.LPArray)]JNINativeMethod[] methods, jint nMethods);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint UnregisterNatives(JNIEnvHandle env, jclass clazz);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint MonitorEnter(JNIEnvHandle env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint MonitorExit(JNIEnvHandle env, jobject obj);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jint GetJavaVM(JNIEnvHandle env, out JavaVMHandle vm);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetStringRegion(JNIEnvHandle env, jstring str, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]jchar[] buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetStringUTFRegion(JNIEnvHandle env, jstring str, jsize start, jsize len, [Out, MarshalAs(UnmanagedType.LPArray)]byte[] buf);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetPrimitiveArrayCritical(JNIEnvHandle env, jarray array, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleasePrimitiveArrayCritical(JNIEnvHandle env, jarray array, IntPtr carray, jint mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetStringCritical(JNIEnvHandle env, jstring @string, out jboolean isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ReleaseStringCritical(JNIEnvHandle env, jstring @string, jchar[] cstring);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jweak NewWeakGlobalRef(JNIEnvHandle env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DeleteWeakGlobalRef(JNIEnvHandle env, jweak @ref);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jboolean ExceptionCheck(JNIEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobject NewDirectByteBuffer(JNIEnvHandle env, IntPtr address, jlong capacity);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GetDirectBufferAddress(JNIEnvHandle env, jobject buf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jlong GetDirectBufferCapacity(JNIEnvHandle env, jobject buf);

        /* New JNI 1.6 Features */

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jobjectRefType GetObjectRefType(JNIEnvHandle env, jobject obj);
    }
}
