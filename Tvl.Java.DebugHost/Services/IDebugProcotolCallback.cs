namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.ServiceModel;
    using Tvl.Java.DebugInterface.Types;

    public interface IDebugProcotolCallback
    {
        [OperationContract(IsOneWay = true)]
        void VirtualMachineStart(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread);

        [OperationContract(IsOneWay = true)]
        void SingleStep(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location);

        [OperationContract(IsOneWay = true)]
        void Breakpoint(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location);

        [OperationContract(IsOneWay = true)]
        void MethodEntry(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location);

        [OperationContract(IsOneWay = true)]
        void MethodExit(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location, Value returnValue);

        [OperationContract(IsOneWay = true)]
        void MonitorContendedEnter(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Location location);

        [OperationContract(IsOneWay = true)]
        void MonitorContendedEntered(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Location location);

        [OperationContract(IsOneWay = true)]
        void MonitorContendedWait(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Location location, TimeSpan timeout);

        [OperationContract(IsOneWay = true)]
        void MonitorContendedWaited(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TaggedObjectId @object, Location location, bool timedOut);

        [OperationContract(IsOneWay = true)]
        void Exception(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location, TaggedObjectId exception, Location catchLocation);

        [OperationContract(IsOneWay = true)]
        void ThreadStart(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread);

        [OperationContract(IsOneWay = true)]
        void ThreadDeath(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread);

        [OperationContract(IsOneWay = true)]
        void ClassPrepare(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, TypeTag typeTag, ReferenceTypeId typeId, string signature, ClassStatus status);

        [OperationContract(IsOneWay = true)]
        void ClassUnload(SuspendPolicy suspendPolicy, RequestId requestId, string signature);

        [OperationContract(IsOneWay = true)]
        void FieldAccess(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location, TypeTag typeTag, ReferenceTypeId typeId, FieldId field, TaggedObjectId @object);

        [OperationContract(IsOneWay = true)]
        void FieldModification(SuspendPolicy suspendPolicy, RequestId requestId, ThreadId thread, Location location, TypeTag typeTag, ReferenceTypeId typeId, FieldId field, TaggedObjectId @object, Value newValue);

        [OperationContract(IsOneWay = true)]
        void VirtualMachineDeath(SuspendPolicy suspendPolicy, RequestId requestId);
    }
}
