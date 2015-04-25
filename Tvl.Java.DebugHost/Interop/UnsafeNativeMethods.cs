// The field 'field_name' is never used
#pragma warning disable 169

namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;

    using IntPtr = System.IntPtr;

    using jboolean = System.Byte;
    using jdouble = System.Double;
    using jfloat = System.Single;
    using jint = System.Int32;
    using jlong = System.Int64;
    using jrawMonitorID = System.IntPtr;

    internal static class UnsafeNativeMethods
    {
        #region Memory Management

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError Allocate(jvmtiEnvHandle env, jlong size, out IntPtr memPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError Deallocate(jvmtiEnvHandle env, IntPtr mem);

        #endregion

        #region Thread

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiStartFunction(jvmtiEnvHandle env, JNIEnvHandle jniEnv, IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadState(jvmtiEnvHandle env, jthread thread, out jvmtiThreadState threadStatePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetCurrentThread(jvmtiEnvHandle env, out jthread threadPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetAllThreads(jvmtiEnvHandle env, out jint threadsCountPtr, out IntPtr threadsPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SuspendThread(jvmtiEnvHandle env, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SuspendThreadList(jvmtiEnvHandle env, jint requestCount, [MarshalAs(UnmanagedType.LPArray)]jthread[] requestList, [Out, MarshalAs(UnmanagedType.LPArray)]jvmtiError[] results);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ResumeThread(jvmtiEnvHandle env, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ResumeThreadList(jvmtiEnvHandle env, jint requestCount, [MarshalAs(UnmanagedType.LPArray)]jthread[] requestList, [Out, MarshalAs(UnmanagedType.LPArray)]jvmtiError[] results);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError StopThread(jvmtiEnvHandle env, jthread thread, jobject exception);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError InterruptThread(jvmtiEnvHandle env, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadInfo(jvmtiEnvHandle env, jthread thread, out jvmtiThreadInfo infoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetOwnedMonitorInfo(jvmtiEnvHandle env, jthread thread, out jint ownedMonitorCountPtr, out IntPtr ownedMonitorsPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetOwnedMonitorStackDepthInfo(jvmtiEnvHandle env, jthread thread, out int monitorInfoCountPtr, out IntPtr monitorInfoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetCurrentContendedMonitor(jvmtiEnvHandle env, jthread thread, out jobject monitorPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RunAgentThread(jvmtiEnvHandle env, jthread thread, jvmtiStartFunction proc, IntPtr arg, JvmThreadPriority priority);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetThreadLocalStorage(jvmtiEnvHandle env, jthread thread, IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadLocalStorage(jvmtiEnvHandle env, jthread thread, out IntPtr dataPtr);

        #endregion

        #region Thread Group

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetTopThreadGroups(jvmtiEnvHandle env, out int groupCountPtr, out IntPtr groupsPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadGroupInfo(jvmtiEnvHandle env, jthreadGroup group, out jvmtiThreadGroupInfo infoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadGroupChildren(jvmtiEnvHandle env, jthreadGroup group, out int threadCountPtr, out IntPtr threadsPtr, out int groupCountPtr, out IntPtr groupsPtr);

        #endregion

        #region Stack Frame

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetStackTrace(jvmtiEnvHandle env, jthread thread, int startDepth, int maxFrameCount, [Out, MarshalAs(UnmanagedType.LPArray)]jvmtiFrameInfo[] frameBuffer, out int countPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetAllStackTraces(jvmtiEnvHandle env, int maxFrameCount, out IntPtr stackInfoPtr, out int threadCountPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadListStackTraces(jvmtiEnvHandle env, int threadCount, [MarshalAs(UnmanagedType.LPArray)]jthread[] threadList, int maxFrameCount, out IntPtr stackInfoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetFrameCount(jvmtiEnvHandle env, jthread thread, out int countPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError PopFrame(jvmtiEnvHandle env, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetFrameLocation(jvmtiEnvHandle env, jthread thread, int depth, out jmethodID methodPtr, out jlocation locationPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError NotifyFramePop(jvmtiEnvHandle env, jthread thread, int depth);

        #endregion

        #region Force Early Return

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnObject(jvmtiEnvHandle env, jthread thread, jobject value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnInt(jvmtiEnvHandle env, jthread thread, int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnLong(jvmtiEnvHandle env, jthread thread, jlong value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnFloat(jvmtiEnvHandle env, jthread thread, jfloat value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnDouble(jvmtiEnvHandle env, jthread thread, jdouble value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceEarlyReturnVoid(jvmtiEnvHandle env, jthread thread);

        #endregion

        #region Heap

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiHeapIterationCallback(jlong classTag, jlong size, ref jlong tagPtr, jint length, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiHeapReferenceCallback(jvmtiHeapReferenceKind referenceKind, IntPtr referenceInfo, jlong classTag, jlong referrerClassTag, jlong size, ref jlong tagPtr, ref jlong referrerTagPtr, jint length, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiPrimitiveFieldCallback(jvmtiHeapReferenceKind referenceKind, IntPtr info, jlong objectClassTag, ref jlong objectTagPtr, jvalue value, jvmtiPrimitiveType valueType, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiArrayPrimitiveValueCallback(jlong classTag, jlong size, ref jlong tagPtr, int elementCount, jvmtiPrimitiveType elementType, IntPtr elements, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiStringPrimitiveValueCallback(jlong classTag, jlong size, ref jlong tagPtr, IntPtr value, int valueLength, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int jvmtiReservedCallback();

        public struct jvmtiHeapCallbacks
        {
            jvmtiHeapIterationCallback heapIterationCallback;
            jvmtiHeapReferenceCallback heapReferenceCallback;
            jvmtiPrimitiveFieldCallback primitiveFieldCallback;
            jvmtiArrayPrimitiveValueCallback arrayPrimitiveValueCallback;
            jvmtiStringPrimitiveValueCallback stringPrimitiveValueCallback;
            jvmtiReservedCallback reserved5;
            jvmtiReservedCallback reserved6;
            jvmtiReservedCallback reserved7;
            jvmtiReservedCallback reserved8;
            jvmtiReservedCallback reserved9;
            jvmtiReservedCallback reserved10;
            jvmtiReservedCallback reserved11;
            jvmtiReservedCallback reserved12;
            jvmtiReservedCallback reserved13;
            jvmtiReservedCallback reserved14;
            jvmtiReservedCallback reserved15;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError FollowReferences(jvmtiEnvHandle env, int heapFilter, jclass @class, jobject initialObject, ref jvmtiHeapCallbacks callbacks, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IterateThroughHeap(jvmtiEnvHandle env, int heapFilter, jclass @class, ref jvmtiHeapCallbacks callbacks, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetTag(jvmtiEnvHandle env, jobject @object, out jlong tagPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetTag(jvmtiEnvHandle env, jobject @object, jlong tag);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetObjectsWithTags(jvmtiEnvHandle env, int tagCount, [MarshalAs(UnmanagedType.LPArray)]jlong[] tags, out int countPtr, out IntPtr objectResultPtr, out IntPtr tagResultPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ForceGarbageCollection(jvmtiEnvHandle env);

        #endregion

        #region Local Variable

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalObject(jvmtiEnvHandle env, jthread thread, int depth, int slot, out jobject valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalInstance(jvmtiEnvHandle env, jthread thread, int depth, out jobject valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalInt(jvmtiEnvHandle env, jthread thread, int depth, int slot, out int valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalLong(jvmtiEnvHandle env, jthread thread, int depth, int slot, out long valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalFloat(jvmtiEnvHandle env, jthread thread, int depth, int slot, out float valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalDouble(jvmtiEnvHandle env, jthread thread, int depth, int slot, out double valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetLocalObject(jvmtiEnvHandle env, jthread thread, int depth, int slot, jobject value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetLocalInt(jvmtiEnvHandle env, jthread thread, int depth, int slot, int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetLocalLong(jvmtiEnvHandle env, jthread thread, int depth, int slot, long value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetLocalFloat(jvmtiEnvHandle env, jthread thread, int depth, int slot, float value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetLocalDouble(jvmtiEnvHandle env, jthread thread, int depth, int slot, double value);

        #endregion

        #region Breakpoint

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetBreakpoint(jvmtiEnvHandle env, jmethodID method, jlocation location);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ClearBreakpoint(jvmtiEnvHandle env, jmethodID method, jlocation location);

        #endregion

        #region Watched Field

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetFieldAccessWatch(jvmtiEnvHandle env, jclass @class, jfieldID field);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ClearFieldAccessWatch(jvmtiEnvHandle env, jclass @class, jfieldID field);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetFieldModificationWatch(jvmtiEnvHandle env, jclass @class, jfieldID field);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError ClearFieldModificationWatch(jvmtiEnvHandle env, jclass @class, jfieldID field);

        #endregion

        #region Class

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLoadedClasses(jvmtiEnvHandle env, out int classCountPtr, out IntPtr classesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassLoaderClasses(jvmtiEnvHandle env, jobject initiatingLoader, out int classCountPtr, out IntPtr classesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassSignature(jvmtiEnvHandle env, jclass @class, out IntPtr signaturePtr, out IntPtr genericPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassStatus(jvmtiEnvHandle env, jclass @class, out jvmtiClassStatus statusPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetSourceFileName(jvmtiEnvHandle env, jclass @class, out IntPtr sourceNamePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassModifiers(jvmtiEnvHandle env, jclass @class, out JvmAccessModifiers modifiersPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassMethods(jvmtiEnvHandle env, jclass @class, out int methodCountPtr, out IntPtr methodsPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassFields(jvmtiEnvHandle env, jclass @class, out int fieldCountPtr, out IntPtr fieldPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetImplementedInterfaces(jvmtiEnvHandle env, jclass @class, out int interfacesCountPtr, out IntPtr interfacesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassVersionNumbers(jvmtiEnvHandle env, jclass @class, out int minorVersionPtr, out int majorVersionPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetConstantPool(jvmtiEnvHandle env, jclass @class, out int constantPoolCountPtr, out int constantPoolByteCountPtr, out IntPtr constantPoolBytesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsInterface(jvmtiEnvHandle env, jclass @class, out jboolean isInterfacePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsArrayClass(jvmtiEnvHandle env, jclass @class, out jboolean isArrayClassPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsModifiableClass(jvmtiEnvHandle env, jclass @class, out jboolean isModifiableClassPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetClassLoader(jvmtiEnvHandle env, jclass @class, out jobject classloaderPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetSourceDebugExtension(jvmtiEnvHandle env, jclass @class, out IntPtr sourceDebugExtensionPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RetransformClasses(jvmtiEnvHandle env, int classCount, [MarshalAs(UnmanagedType.LPArray)]jclass[] classes);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RedefineClasses(jvmtiEnvHandle env, int classCount, [MarshalAs(UnmanagedType.LPArray)]jvmtiClassDefinition[] classDefinitions);

        #endregion

        #region Object

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetObjectSize(jvmtiEnvHandle env, jobject @object, out long sizePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetObjectHashCode(jvmtiEnvHandle env, jobject @object, out int hashCodePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetObjectMonitorUsage(jvmtiEnvHandle env, jobject @object, out jvmtiMonitorUsage infoPtr);

        #endregion

        #region Field

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetFieldName(jvmtiEnvHandle env, jclass @class, jfieldID field, out IntPtr namePtr, out IntPtr signaturePtr, out IntPtr genericPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetFieldDeclaringClass(jvmtiEnvHandle env, jclass @class, jfieldID field, out jclass declaringClassPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetFieldModifiers(jvmtiEnvHandle env, jclass @class, jfieldID field, out JvmAccessModifiers modifiersPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsFieldSynthetic(jvmtiEnvHandle env, jclass @class, jfieldID field, out jboolean isSyntheticPtr);

        #endregion

        #region Method

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetMethodName(jvmtiEnvHandle env, jmethodID method, out IntPtr namePtr, out IntPtr signaturePtr, out IntPtr genericPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetMethodDeclaringClass(jvmtiEnvHandle env, jmethodID method, out jclass declaringClassPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetMethodModifiers(jvmtiEnvHandle env, jmethodID method, out JvmAccessModifiers modifiersPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetMaxLocals(jvmtiEnvHandle env, jmethodID method, out int maxPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetArgumentsSize(jvmtiEnvHandle env, jmethodID method, out int sizePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLineNumberTable(jvmtiEnvHandle env, jmethodID method, out int entryCountPtr, out IntPtr tablePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetMethodLocation(jvmtiEnvHandle env, jmethodID method, out jlocation startLocationPtr, out jlocation endLocationPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetLocalVariableTable(jvmtiEnvHandle env, jmethodID method, out int entryCountPtr, out IntPtr tablePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetBytecodes(jvmtiEnvHandle env, jmethodID method, out int bytecodeCountPtr, out IntPtr bytecodePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsMethodNative(jvmtiEnvHandle env, jmethodID method, out jboolean isNativePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsMethodSynthetic(jvmtiEnvHandle env, jmethodID method, out jboolean isSyntheticPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError IsMethodObsolete(jvmtiEnvHandle env, jmethodID method, out jboolean isObsoletePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetNativeMethodPrefix(jvmtiEnvHandle env, ModifiedUTF8StringData prefix);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetNativeMethodPrefixes(jvmtiEnvHandle env, int prefixCount, [MarshalAs(UnmanagedType.LPArray)]IntPtr[] prefixes);

        #endregion

        #region Raw Monitor

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError CreateRawMonitor(jvmtiEnvHandle env, ModifiedUTF8StringData name, out jrawMonitorID monitorPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError DestroyRawMonitor(jvmtiEnvHandle env, jrawMonitorID monitor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RawMonitorEnter(jvmtiEnvHandle env, jrawMonitorID monitor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RawMonitorExit(jvmtiEnvHandle env, jrawMonitorID monitor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RawMonitorWait(jvmtiEnvHandle env, jrawMonitorID monitor, long millis);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RawMonitorNotify(jvmtiEnvHandle env, jrawMonitorID monitor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RawMonitorNotifyAll(jvmtiEnvHandle env, jrawMonitorID monitor);

        #endregion

        #region JNI Function Interception

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetJNIFunctionTable(jvmtiEnvHandle env, ref jniNativeInterface functionTable);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetJNIFunctionTable(jvmtiEnvHandle env, out IntPtr functionTable);

        #endregion

        #region Event Management

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetEventCallbacks(jvmtiEnvHandle env, ref jvmtiEventCallbacks callbacks, int sizeOfCallbacks);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate jvmtiError SetEventNotificationMode(jvmtiEnvHandle env, JvmEventMode mode, JvmEventType eventType, jthread eventThread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GenerateEvents(jvmtiEnvHandle env, JvmEventType eventType);

        #endregion

        #region Extension Mechanism

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError jvmtiExtensionFunction(jvmtiEnvHandle env, IntPtr vaargs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetExtensionFunctions(jvmtiEnvHandle env, out int extensionCountPtr, out IntPtr extensions);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError jvmtiExtensionEvent(jvmtiEnvHandle env, IntPtr vaargs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetExtensionEvents(jvmtiEnvHandle env, out int extensionCountPtr, out IntPtr extensions);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetExtensionEventCallback(jvmtiEnvHandle env, int extensionEventIndex, jvmtiExtensionEvent callback);

        #endregion

        #region Capability

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetPotentialCapabilities(jvmtiEnvHandle env, out jvmtiCapabilities capabilitiesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError AddCapabilities(jvmtiEnvHandle env, ref jvmtiCapabilities capabilitiesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError RelinquishCapabilities(jvmtiEnvHandle env, ref jvmtiCapabilities capabilitiesPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetCapabilities(jvmtiEnvHandle env, out jvmtiCapabilities capabilitiesPtr);

        #endregion

        #region Timers

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetCurrentThreadCpuTimerInfo(jvmtiEnvHandle env, out jvmtiTimerInfo infoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetCurrentThreadCpuTime(jvmtiEnvHandle env, out jlong nanosPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadCpuTimerInfo(jvmtiEnvHandle env, out jvmtiTimerInfo infoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetThreadCpuTime(jvmtiEnvHandle env, jthread thread, out jlong nanosPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetTimerInfo(jvmtiEnvHandle env, out jvmtiTimerInfo infoPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetTime(jvmtiEnvHandle env, out jlong nanosPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetAvailableProcessors(jvmtiEnvHandle env, out int processorCountPtr);

        #endregion

        #region Class Loader Search

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError AddToBootstrapClassLoaderSearch(jvmtiEnvHandle env, ModifiedUTF8StringData segment);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError AddToSystemClassLoaderSearch(jvmtiEnvHandle env, ModifiedUTF8StringData segment);

        #endregion

        #region System Properties

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetSystemProperties(jvmtiEnvHandle env, out int countPtr, out IntPtr propertyPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetSystemProperty(jvmtiEnvHandle env, ModifiedUTF8StringData property, out IntPtr valuePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetSystemProperty(jvmtiEnvHandle env, ModifiedUTF8StringData property, ModifiedUTF8StringData value);

        #endregion

        #region General

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetPhase(jvmtiEnvHandle env, out jvmtiPhase phasePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError DisposeEnvironment(jvmtiEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetEnvironmentLocalStorage(jvmtiEnvHandle env, IntPtr data);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetEnvironmentLocalStorage(jvmtiEnvHandle env, out IntPtr dataPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetVersionNumber(jvmtiEnvHandle env, out int versionPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetErrorName(jvmtiEnvHandle env, jvmtiError error, out IntPtr namePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError SetVerboseFlag(jvmtiEnvHandle env, jvmtiVerboseFlag flag, jboolean value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate jvmtiError GetJLocationFormat(jvmtiEnvHandle env, out jvmtiJLocationFormat formatPtr);

        #endregion
    }
}
