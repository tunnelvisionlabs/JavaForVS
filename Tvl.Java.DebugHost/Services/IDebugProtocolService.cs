namespace Tvl.Java.DebugHost.Services
{
    using System.ServiceModel;
    using Tvl.Java.DebugInterface.Types;
    using Tvl.Java.DebugInterface.Types.Loader;

    [ServiceContract(CallbackContract = typeof(IDebugProcotolCallback), SessionMode = SessionMode.Required)]
    public interface IDebugProtocolService
    {
        #region VirtualMachine command set

        [OperationContract]
        Error Attach();

        [OperationContract]
        Error GetVersion(out string description, out int majorVersion, out int minorVersion, out string vmVersion, out string vmName);

        [OperationContract]
        Error GetClassesBySignature(string signature, out ReferenceTypeData[] classes);

        [OperationContract]
        Error GetAllClasses(out ReferenceTypeData[] classes);

        [OperationContract]
        Error GetAllThreads(out ThreadId[] threads);

        [OperationContract]
        Error GetTopLevelThreadGroups(out ThreadGroupId[] groups);

        [OperationContract]
        Error Dispose();

        [OperationContract]
        Error Suspend();

        [OperationContract]
        Error Resume();

        [OperationContract]
        Error Exit(int exitCode);

        [OperationContract]
        Error CreateString(string value, out StringId stringObject);

        [OperationContract]
        Error GetCapabilities(out Capabilities capabilities);

        [OperationContract]
        Error GetClassPaths(out string baseDirectory, out string[] classPaths, out string[] bootClassPaths);

        [OperationContract]
        Error DisposeObjects(ObjectReferenceCountData[] requests);

        [OperationContract]
        Error HoldEvents();

        [OperationContract]
        Error ReleaseEvents();

        [OperationContract]
        Error RedefineClasses(ClassDefinitionData[] definitions);

        [OperationContract]
        Error SetDefaultStratum(string stratumId);

        #endregion

        #region ReferenceType command set

        [OperationContract]
        Error GetSignature(ReferenceTypeId referenceType, out string signature, out string genericSignature);

        [OperationContract]
        Error GetClassLoader(ReferenceTypeId referenceType, out ClassLoaderId classLoader);

        [OperationContract]
        Error GetModifiers(ReferenceTypeId referenceType, out AccessModifiers modifiers);

        [OperationContract]
        Error GetFields(ReferenceTypeId referenceType, out DeclaredFieldData[] fields);

        [OperationContract]
        Error GetMethods(ReferenceTypeId referenceType, out DeclaredMethodData[] methods);

        [OperationContract]
        Error GetReferenceTypeValues(ReferenceTypeId referenceType, FieldId[] fields, out Value[] values);

        [OperationContract]
        Error GetSourceFile(ReferenceTypeId referenceType, out string sourceFile);

        [OperationContract]
        Error GetNestedTypes(ReferenceTypeId referenceType, out TaggedReferenceTypeId[] classes);

        [OperationContract]
        Error GetReferenceTypeStatus(ReferenceTypeId referenceType, out ClassStatus status);

        [OperationContract]
        Error GetInterfaces(ReferenceTypeId referenceType, out InterfaceId[] interfaces);

        [OperationContract]
        Error GetClassObject(ReferenceTypeId referenceType, out ClassObjectId classObject);

        [OperationContract]
        Error GetSourceDebugExtension(ReferenceTypeId referenceType, out string extension);

        [OperationContract]
        Error GetInstances(ReferenceTypeId referenceType, int maxInstances, out TaggedObjectId[] instances);

        [OperationContract]
        Error GetClassFileVersion(ReferenceTypeId referenceType, out int majorVersion, out int minorVersion);

        [OperationContract]
        Error GetConstantPool(ReferenceTypeId referenceType, out int constantPoolCount, out byte[] data);

        #endregion

        #region ClassType command set

        [OperationContract]
        Error GetSuperclass(ClassId @class, out ClassId superclass);

        [OperationContract]
        Error SetClassValues(ClassId @class, FieldId[] fields, Value[] values);

        [OperationContract]
        Error InvokeClassMethod(ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments, out Value returnValue, out TaggedObjectId thrownException);

        [OperationContract]
        Error CreateClassInstance(ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments, out TaggedObjectId newObject, out TaggedObjectId thrownException);

        #endregion

        #region ArrayType command set

        [OperationContract]
        Error CreateArrayInstance(ArrayTypeId arrayType, int length, out TaggedObjectId newArray);

        #endregion

        #region InterfaceType command set

        #endregion

        #region Method command set

        [OperationContract]
        Error GetMethodExceptionTable(ReferenceTypeId referenceType, MethodId method, out ExceptionTableEntry[] entries);

        [OperationContract]
        Error GetMethodLineTable(ReferenceTypeId referenceType, MethodId method, out long start, out long end, out LineNumberData[] lines);

        [OperationContract]
        Error GetMethodVariableTable(ReferenceTypeId referenceType, MethodId method, out VariableData[] slots);

        [OperationContract]
        Error GetMethodBytecodes(ReferenceTypeId referenceType, MethodId method, out byte[] bytecode);

        [OperationContract]
        Error GetMethodIsObsolete(ReferenceTypeId referenceType, MethodId method, out bool result);

        #endregion

        #region Field command set

        #endregion

        #region ObjectReference command set

        [OperationContract]
        Error GetObjectReferenceType(ObjectId @object, out TypeTag typeTag, out ReferenceTypeId typeId);

        [OperationContract]
        Error GetObjectValues(ObjectId @object, FieldId[] fields, out Value[] values);

        [OperationContract]
        Error SetObjectValues(ObjectId @object, FieldId[] fields, Value[] values);

        [OperationContract]
        Error GetObjectMonitorInfo(ObjectId @object, out ThreadId owner, out int entryCount, out ThreadId[] waiters);

        [OperationContract]
        Error InvokeObjectMethod(ObjectId @object, ThreadId thread, ClassId @class, MethodId method, InvokeOptions options, Value[] arguments, out Value returnValue, out TaggedObjectId thrownException);

        [OperationContract]
        Error DisableObjectCollection(ObjectId @object);

        [OperationContract]
        Error EnableObjectCollection(ObjectId @object);

        [OperationContract]
        Error GetIsObjectCollected(ObjectId @object, out bool result);

        #endregion

        #region StringReference command set

        [OperationContract]
        Error GetStringValue(ObjectId stringObject, out string stringValue);

        #endregion

        #region ThreadReference command set

        [OperationContract]
        Error GetThreadName(ThreadId thread, out string name);

        [OperationContract]
        Error SuspendThread(ThreadId thread);

        [OperationContract]
        Error ResumeThread(ThreadId thread);

        [OperationContract]
        Error GetThreadStatus(ThreadId thread, out ThreadStatus threadStatus, out SuspendStatus suspendStatus);

        [OperationContract]
        Error GetThreadGroup(ThreadId thread, out ThreadGroupId threadGroup);

        [OperationContract]
        Error GetThreadFrames(ThreadId thread, int startFrame, int length, out FrameLocationData[] frames);

        [OperationContract]
        Error GetThreadFrameCount(ThreadId thread, out int frameCount);

        [OperationContract]
        Error GetThreadOwnedMonitors(ThreadId thread, out TaggedObjectId[] monitors);

        [OperationContract]
        Error GetThreadCurrentContendedMonitor(ThreadId thread, out TaggedObjectId monitor);

        [OperationContract]
        Error StopThread(ThreadId thread, ObjectId throwable);

        [OperationContract]
        Error InterruptThread(ThreadId thread);

        [OperationContract]
        Error GetThreadSuspendCount(ThreadId thread, out int suspendCount);

        #endregion

        #region ThreadGroupReference command set

        [OperationContract]
        Error GetThreadGroupName(ThreadGroupId group, out string groupName);

        [OperationContract]
        Error GetThreadGroupParent(ThreadGroupId group, out ThreadGroupId parentGroup);

        [OperationContract]
        Error GetThreadGroupChildren(ThreadGroupId group, out ThreadId[] childThreads, out ThreadGroupId[] childGroups);

        #endregion

        #region ArrayReference command set

        [OperationContract]
        Error GetArrayLength(ArrayId arrayObject, out int arrayLength);

        [OperationContract]
        Error GetArrayValues(ArrayId arrayObject, int firstIndex, int length, out Value[] values);

        [OperationContract]
        Error SetArrayValues(ArrayId arrayObject, int firstIndex, Value[] values);

        #endregion

        #region ClassLoaderReference command set

        [OperationContract]
        Error GetClassLoaderVisibleClasses(ClassLoaderId classLoaderObject, out TaggedReferenceTypeId[] classes);

        #endregion

        #region EventRequest commands set

        [OperationContract]
        Error SetEvent(EventKind eventKind, SuspendPolicy suspendPolicy, EventRequestModifier[] modifiers, out RequestId requestId);

        [OperationContract]
        Error ClearEvent(EventKind eventKind, RequestId requestId);

        [OperationContract]
        Error ClearAllBreakpoints();

        #endregion

        #region StackFrame command set

        [OperationContract]
        Error GetValues(ThreadId thread, FrameId frame, int[] slots, out Value[] values);

        [OperationContract]
        Error SetValues(ThreadId thread, FrameId frame, int[] slots, Value[] values);

        [OperationContract]
        Error GetThisObject(ThreadId thread, FrameId frame, out TaggedObjectId thisObject);

        [OperationContract]
        Error PopFrames(ThreadId thread, FrameId frame);

        #endregion

        #region ClassObjectReference command set

        [OperationContract]
        Error GetReflectedType(ClassObjectId classObject, out TypeTag typeTag, out ReferenceTypeId typeId);

        #endregion

        #region Event command set

        #endregion
    }
}
