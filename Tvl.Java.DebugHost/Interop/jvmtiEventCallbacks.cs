// The field 'field_name' is never used
#pragma warning disable 169
// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;
    using jvmtiEventReserved = System.IntPtr;

    internal struct jvmtiEventCallbacks
    {
        public jvmtiEventVMInit VMInit;
        public jvmtiEventVMDeath VMDeath;
        public jvmtiEventThreadStart ThreadStart;
        public jvmtiEventThreadEnd ThreadEnd;
        public jvmtiEventClassFileLoadHook ClassFileLoadHook;
        public jvmtiEventClassLoad ClassLoad;
        public jvmtiEventClassPrepare ClassPrepare;
        public jvmtiEventVMStart VMStart;
        public jvmtiEventException Exception;
        public jvmtiEventExceptionCatch ExceptionCatch;
        public jvmtiEventSingleStep SingleStep;
        public jvmtiEventFramePop FramePop;
        public jvmtiEventBreakpoint Breakpoint;
        public jvmtiEventFieldAccess FieldAccess;
        public jvmtiEventFieldModification FieldModification;
        public jvmtiEventMethodEntry MethodEntry;
        public jvmtiEventMethodExit MethodExit;
        public jvmtiEventNativeMethodBind NativeMethodBind;
        public jvmtiEventCompiledMethodLoad CompiledMethodLoad;
        public jvmtiEventCompiledMethodUnload CompiledMethodUnload;
        public jvmtiEventDynamicCodeGenerated DynamicCodeGenerated;
        public jvmtiEventDataDumpRequest DataDumpRequest;
        private jvmtiEventReserved reserved72;
        public jvmtiEventMonitorWait MonitorWait;
        public jvmtiEventMonitorWaited MonitorWaited;
        public jvmtiEventMonitorContendedEnter MonitorContendedEnter;
        public jvmtiEventMonitorContendedEntered MonitorContendedEntered;
        private jvmtiEventReserved reserved77;
        private jvmtiEventReserved reserved78;
        private jvmtiEventReserved reserved79;
        public jvmtiEventResourceExhausted ResourceExhausted;
        public jvmtiEventGarbageCollectionStart GarbageCollectionStart;
        public jvmtiEventGarbageCollectionFinish GarbageCollectionFinish;
        public jvmtiEventObjectFree ObjectFree;
        public jvmtiEventVMObjectAlloc VMObjectAlloc;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventVMInit(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventVMDeath(jvmtiEnvHandle env, JNIEnvHandle jniEnv);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventThreadStart(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventThreadEnd(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventClassFileLoadHook(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jclass classBeingRedefined, jobject loader, IntPtr name, jobject protectionDomain, int classDataLength, IntPtr classData, ref int newClassDataLength, ref IntPtr newClassData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventClassLoad(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jclass @class);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventClassPrepare(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jclass @class);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventVMStart(jvmtiEnvHandle env, JNIEnvHandle jniEnv);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventException(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location, jobject exception, jmethodID catchMethod, jlocation catchLocation);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventExceptionCatch(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location, jobject exception);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventSingleStep(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventFramePop(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, [MarshalAs(UnmanagedType.I1)]bool wasPoppedByException);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventBreakpoint(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventFieldAccess(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location, jclass fieldClass, jobject @object, jfieldID field);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventFieldModification(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, jlocation location, jclass fieldClass, jobject @object, jfieldID field, byte signatureType, jvalue newValue);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMethodEntry(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMethodExit(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, [MarshalAs(UnmanagedType.I1)]bool wasPoppedByException, jvalue returnValue);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventNativeMethodBind(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jmethodID method, IntPtr address, ref IntPtr newAddressPtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventCompiledMethodLoad(jvmtiEnvHandle env, jmethodID method, int codeSize, IntPtr codeAddress, int mapLength, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]jvmtiAddressLocationMap[] map, IntPtr compileInfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventCompiledMethodUnload(jvmtiEnvHandle env, jmethodID method, IntPtr codeAddress);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventDynamicCodeGenerated(jvmtiEnvHandle env, IntPtr name, IntPtr address, int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventDataDumpRequest(jvmtiEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMonitorWait(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jobject @object, long timeout);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMonitorWaited(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jobject @object, [MarshalAs(UnmanagedType.I1)]bool timedOut);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMonitorContendedEnter(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jobject @object);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventMonitorContendedEntered(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jobject @object);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventResourceExhausted(jvmtiEnvHandle env, JNIEnvHandle jniEnv, JvmResourceExhaustedFlags flags, IntPtr reserved, IntPtr description);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventGarbageCollectionStart(jvmtiEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventGarbageCollectionFinish(jvmtiEnvHandle env);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventObjectFree(jvmtiEnvHandle env, long tag);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void jvmtiEventVMObjectAlloc(jvmtiEnvHandle env, JNIEnvHandle jniEnv, jthread thread, jobject @object, jclass objectClass, long size);
    }
}
