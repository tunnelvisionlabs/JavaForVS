namespace Tvl.Java.DebugHost.Services
{
    using System.ServiceModel;
    using jvmtiClassStatus = Tvl.Java.DebugHost.Interop.jvmtiClassStatus;
    using jvmtiError = Tvl.Java.DebugHost.Interop.jvmtiError;
    using jvmtiThreadState = Tvl.Java.DebugHost.Interop.jvmtiThreadState;

    [ServiceContract]
    public interface IJvmToolsInterfaceService
    {
        #region Memory Management

        [OperationContract]
        jvmtiError Allocate(JvmVirtualMachineRemoteHandle virtualMachine, long size, out long address);

        [OperationContract]
        jvmtiError Deallocate(JvmVirtualMachineRemoteHandle virtualMachine, long address);

        #endregion

        #region Thread

        [OperationContract]
        jvmtiError GetThreadState(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out jvmtiThreadState threadState);

        [OperationContract]
        jvmtiError GetCurrentThread(JvmVirtualMachineRemoteHandle virtualMachine, out JvmThreadRemoteHandle thread);

        [OperationContract]
        jvmtiError GetAllThreads(JvmVirtualMachineRemoteHandle virtualMachine, out JvmThreadRemoteHandle[] threads);

        [OperationContract]
        jvmtiError SuspendThread(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread);

        [OperationContract]
        jvmtiError SuspendThreads(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle[] threads);

        [OperationContract]
        jvmtiError ResumeThread(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread);

        [OperationContract]
        jvmtiError ResumeThreads(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle threads);

        [OperationContract]
        jvmtiError StopThread(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, JvmObjectRemoteHandle exception);

        [OperationContract]
        jvmtiError InterruptThread(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread);

        [OperationContract]
        jvmtiError GetThreadInfo(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out JvmThreadRemoteInfo info);

        [OperationContract]
        jvmtiError GetOwnedMonitorInfo(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out JvmObjectRemoteHandle[] ownedMonitors);

        //[OperationContract]
        //jvmtiError GetOwnedMonitorStackDepthInfo(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out JvmMonitorInfo[] ownedMonitors);

        [OperationContract]
        jvmtiError GetCurrentContendedMonitor(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out JvmObjectRemoteHandle monitor);

        //[OperationContract]
        //jvmtiError RunAgentThread(.....);

        [OperationContract]
        jvmtiError SetThreadLocalStorage(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, long data);

        [OperationContract]
        jvmtiError GetThreadLocalStorage(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out long data);

        #endregion

        #region Thread Group

        [OperationContract]
        jvmtiError GetTopThreadGroups(JvmVirtualMachineRemoteHandle virtualMachine, out JvmThreadGroupRemoteHandle[] groups);

        //[OperationContract]
        //jvmtiError GetThreadGroupInfo(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadGroupRemoteHandle group, out JvmThreadGroupInfo info);

        [OperationContract]
        jvmtiError GetThreadGroupChildren(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadGroupRemoteHandle group, out JvmThreadRemoteHandle[] threads, out JvmThreadGroupRemoteHandle[] groups);

        #endregion

        #region Stack Frame

        [OperationContract]
        jvmtiError GetStackTrace(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int startDepth, int maxFrameCount, out JvmRemoteLocation[] frames);

        [OperationContract]
        jvmtiError GetAllStackTraces(JvmVirtualMachineRemoteHandle virtualMachine, int maxFrameCount, out JvmRemoteStackInfo[] stackTraces);

        [OperationContract]
        jvmtiError GetThreadListStackTraces(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle[] threads, int maxFrameCount, out JvmRemoteStackInfo[] stackTraces);

        [OperationContract]
        jvmtiError GetFrameCount(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, out int frameCount);

        [OperationContract]
        jvmtiError PopFrame(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread);

        [OperationContract]
        jvmtiError GetFrameLocation(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, out JvmRemoteLocation location);

        [OperationContract]
        jvmtiError NotifyFramePop(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth);

        #endregion

        #region Force Early Return

        [OperationContract]
        jvmtiError ForceEarlyReturnObject(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, JvmObjectRemoteHandle value);

        [OperationContract]
        jvmtiError ForceEarlyReturnInt(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int value);

        [OperationContract]
        jvmtiError ForceEarlyReturnLong(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, long value);

        [OperationContract]
        jvmtiError ForceEarlyReturnFloat(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, float value);

        [OperationContract]
        jvmtiError ForceEarlyReturnDouble(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, double value);

        [OperationContract]
        jvmtiError ForceEarlyReturnVoid(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread);

        #endregion

        #region Heap

        [OperationContract]
        jvmtiError GetTag(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle @object, out long tag);

        [OperationContract]
        jvmtiError SetTag(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle @object, long tag);

        [OperationContract]
        jvmtiError GetObjectsWithTags(JvmVirtualMachineRemoteHandle virtualMachine, long[] tagFilter, out JvmObjectRemoteHandle[] objects, out long[] tags);

        [OperationContract]
        jvmtiError ForceGarbageCollection(JvmVirtualMachineRemoteHandle virtualMachine);

        #endregion

        #region Local Variable

        [OperationContract]
        jvmtiError GetLocalObject(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, out JvmObjectRemoteHandle value);

        [OperationContract]
        jvmtiError GetLocalInstance(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, out JvmObjectRemoteHandle value);

        [OperationContract]
        jvmtiError GetLocalInt(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, out int value);

        [OperationContract]
        jvmtiError GetLocalLong(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, out long value);

        [OperationContract]
        jvmtiError GetLocalFloat(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, out float value);

        [OperationContract]
        jvmtiError GetLocalDouble(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, out double value);

        [OperationContract]
        jvmtiError SetLocalObject(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, JvmObjectRemoteHandle value);

        [OperationContract]
        jvmtiError SetLocalInt(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, int value);

        [OperationContract]
        jvmtiError SetLocalLong(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, long value);

        [OperationContract]
        jvmtiError SetLocalFloat(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, float value);

        [OperationContract]
        jvmtiError SetLocalDouble(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, int depth, int slot, double value);

        #endregion

        #region Breakpoint

        [OperationContract]
        jvmtiError SetBreakpoint(JvmVirtualMachineRemoteHandle virtualMachine, JvmRemoteLocation location);

        [OperationContract]
        jvmtiError ClearBreakpoint(JvmVirtualMachineRemoteHandle virtualMachine, JvmRemoteLocation location);

        #endregion

        #region Watched Field

        [OperationContract]
        jvmtiError SetFieldAccessWatch(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field);

        [OperationContract]
        jvmtiError ClearFieldAccessWatch(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field);

        [OperationContract]
        jvmtiError SetFieldModificationWatch(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field);

        [OperationContract]
        jvmtiError ClearFieldModificationWatch(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field);

        #endregion

        #region Class

        [OperationContract]
        jvmtiError GetLoadedClasses(JvmVirtualMachineRemoteHandle virtualMachine, out JvmClassRemoteHandle[] classes);

        [OperationContract]
        jvmtiError GetClassLoaderClasses(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle initiatingLoader, out JvmClassRemoteHandle[] classes);

        [OperationContract]
        jvmtiError GetClassSignature(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out string signature, out string generic);

        [OperationContract]
        jvmtiError GetClassStatus(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out jvmtiClassStatus status);

        [OperationContract]
        jvmtiError GetSourceFileName(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out string sourceName);

        [OperationContract]
        jvmtiError GetClassModifiers(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out JvmAccessModifiers modifiers);

        [OperationContract]
        jvmtiError GetClassMethods(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out JvmMethodRemoteHandle[] methods);

        [OperationContract]
        jvmtiError GetClassFields(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out JvmFieldRemoteHandle[] fields);

        [OperationContract]
        jvmtiError GetImplementedInterfaces(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out JvmClassRemoteHandle[] interfaces);

        [OperationContract]
        jvmtiError GetClassVersionNumbers(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out int minorVersion, out int majorVersion);

        [OperationContract]
        jvmtiError GetConstantPool(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out byte[] constantPool);

        [OperationContract]
        jvmtiError IsInterface(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out bool isInterface);

        [OperationContract]
        jvmtiError IsArrayClass(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out bool isArrayClass);

        [OperationContract]
        jvmtiError IsModifiableClass(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out bool isModifiableClass);

        [OperationContract]
        jvmtiError GetClassLoader(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out JvmObjectRemoteHandle classLoader);

        [OperationContract]
        jvmtiError GetSourceDebugExtension(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle @class, out string sourceDebugExtension);

        [OperationContract]
        jvmtiError RetransformClasses(JvmVirtualMachineRemoteHandle virtualMachine, JvmClassRemoteHandle[] classes);

        //[OperationContract]
        //jvmtiError RedefineClasses(JvmVirtualMachineRemoteHandle virtualMachine, jvmtiClassDefinition[] classes);

        #endregion

        #region Object

        [OperationContract]
        jvmtiError GetObjectSize(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle @object, out long size);

        [OperationContract]
        jvmtiError GetObjectHashCode(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle @object, out int hashCode);

        //[OperationContract]
        //jvmtiError GetObjectMonitorUsage(JvmVirtualMachineRemoteHandle virtualMachine, JvmObjectRemoteHandle @object, out jvmtiMonitorUsage info);

        #endregion

        #region Field

        [OperationContract]
        jvmtiError GetFieldName(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field, out string name, out string signature, out string generic);

        [OperationContract]
        jvmtiError GetFieldDeclaringClass(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field, out JvmClassRemoteHandle declaringClass);

        [OperationContract]
        jvmtiError GetFieldModifiers(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field, out JvmAccessModifiers modifiers);

        [OperationContract]
        jvmtiError IsFieldSynthetic(JvmVirtualMachineRemoteHandle virtualMachine, JvmFieldRemoteHandle field, out bool isSynthetic);

        #endregion

        #region Method

        [OperationContract]
        jvmtiError GetMethodName(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out string name, out string signature, out string generic);

        [OperationContract]
        jvmtiError GetMethodDeclaringClass(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out JvmClassRemoteHandle declaringClass);

        [OperationContract]
        jvmtiError GetMethodModifiers(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out JvmAccessModifiers modifiers);

        [OperationContract]
        jvmtiError GetMaxLocals(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out int maxLocals);

        [OperationContract]
        jvmtiError GetArgumentsSize(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out int size);

        [OperationContract]
        jvmtiError GetLineNumberTable(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out JvmLineNumberEntry[] lineNumbers);

        [OperationContract]
        jvmtiError GetMethodLocation(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out JvmRemoteLocation startLocation, out JvmRemoteLocation endLocation);

        [OperationContract]
        jvmtiError GetLocalVariableTable(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out JvmLocalVariableEntry[] localVariables);

        [OperationContract]
        jvmtiError GetBytecodes(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out byte[] bytecode);

        [OperationContract]
        jvmtiError IsMethodNative(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out bool isNative);

        [OperationContract]
        jvmtiError IsMethodSynthetic(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out bool isSynthetic);

        [OperationContract]
        jvmtiError IsMethodObsolete(JvmVirtualMachineRemoteHandle virtualMachine, JvmMethodRemoteHandle method, out bool isObsolete);

        [OperationContract]
        jvmtiError SetNativeMethodPrefix(JvmVirtualMachineRemoteHandle virtualMachine, string prefix);

        [OperationContract]
        jvmtiError SetNativeMethodPrefixes(JvmVirtualMachineRemoteHandle virtualMachine, string[] prefixes);

        #endregion

        #region Raw Monitor

        #endregion

        #region JNI Function Interception

        #endregion

        #region Event Management

        #endregion

        #region Extension Mechanism

        #endregion

        #region Capability

        #endregion

        #region Timers

        #endregion

        #region Class Loader Search

        #endregion

        #region System Properties

        #endregion

        #region General

        #endregion

        #region JNI

#if false
        /*
         * Find a method
         */

        JvmMethodRemoteHandle GetInstanceMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, string name, string signature);
        JvmMethodRemoteHandle GetStaticMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, string name, string signature);

        /*
         * Call a method
         */

        JvmObjectRemoteHandle JniCallObjectMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        bool JniCallBooleanMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        byte JniCallByteMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        char JniCallCharMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        short JniCallShortMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        int JniCallIntMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        long JniCallLongMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        float JniCallFloatMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        double JniCallDoubleMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);
        void JniCallVoidMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmMethodRemoteHandle method, long[] arguments);

        JvmObjectRemoteHandle JniCallNonVirtualObjectMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        bool JniCallNonVirtualBooleanMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        byte JniCallNonVirtualByteMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        char JniCallNonVirtualCharMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        short JniCallNonVirtualShortMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        int JniCallNonVirtualIntMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        long JniCallNonVirtualLongMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        float JniCallNonVirtualFloatMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        double JniCallNonVirtualDoubleMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        void JniCallNonVirtualVoidMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);

        JvmObjectRemoteHandle JniCallStaticObjectMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        bool JniCallStaticBooleanMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        byte JniCallStaticByteMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        char JniCallStaticCharMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        short JniCallStaticShortMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        int JniCallStaticIntMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        long JniCallStaticLongMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        float JniCallStaticFloatMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        double JniCallStaticDoubleMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);
        void JniCallStaticVoidMethod(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmMethodRemoteHandle method, long[] arguments);

        /*
         * Find a field
         */

        JvmFieldRemoteHandle GetInstanceField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, string name, string signature);
        JvmFieldRemoteHandle GetStaticField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, string name, string signature);

        /*
         * Read a field
         */

        JvmObjectRemoteHandle JniGetObjectField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        bool JniGetBooleanField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        byte JniGetByteField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        char JniGetCharField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        short JniGetShortField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        int JniGetIntField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        long JniGetLongField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        float JniGetFloatField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        double JniGetDoubleField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);
        void JniGetVoidField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmObjectRemoteHandle @object, JvmFieldRemoteHandle Field, long[] arguments);

        JvmObjectRemoteHandle JniGetStaticObjectField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        bool JniGetStaticBooleanField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        byte JniGetStaticByteField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        char JniGetStaticCharField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        short JniGetStaticShortField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        int JniGetStaticIntField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        long JniGetStaticLongField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        float JniGetStaticFloatField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        double JniGetStaticDoubleField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);
        void JniGetStaticVoidField(JvmNativeEnvironmentRemoteHandle nativeEnvironment, JvmClassRemoteHandle @class, JvmFieldRemoteHandle Field, long[] arguments);

        /*
         * Write a field
         */
#endif

        #endregion
    }
}
