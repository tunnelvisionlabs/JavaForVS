namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Java.DebugInterface.Types;
    using InvokeOptions = Tvl.Java.DebugInterface.InvokeOptions;

    internal class ObjectReference : Value, IObjectReference
    {
        private readonly ObjectId _objectId;

        private bool _collectionDisabled;
        private IReferenceType _referenceType;

        internal ObjectReference(VirtualMachine virtualMachine, ObjectId objectId, IReferenceType referenceType)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentException>(objectId.Handle != 0);

            _objectId = objectId;
            _referenceType = referenceType;
        }

        public ObjectId ObjectId
        {
            get
            {
                return _objectId;
            }
        }

        public sealed override IType GetValueType()
        {
            return GetReferenceType();
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.Object, ObjectId.Handle);
        }

        public void DisableCollection()
        {
            if (_collectionDisabled)
                return;

            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.DisableObjectCollection(this.ObjectId));
            _collectionDisabled = true;
        }

        public void EnableCollection()
        {
            if (!_collectionDisabled)
                return;

            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.EnableObjectCollection(this.ObjectId));
            _collectionDisabled = false;
        }

        public int GetEntryCount()
        {
            ThreadId owner;
            int entryCount;
            ThreadId[] waiters;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectMonitorInfo(out owner, out entryCount, out waiters, ObjectId));
            return entryCount;
        }

        public IValue GetValue(IField field)
        {
            Field localField = field as Field;
            if (localField == null)
                throw new VirtualMachineMismatchException();

            Types.Value[] values;
            FieldId[] fields = { localField.FieldId };
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectValues(out values, ObjectId, fields));
            return VirtualMachine.GetMirrorOf(values.Single());
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            IField[] fieldsArray = fields.ToArray();

            FieldId[] fieldIds = new FieldId[fieldsArray.Length];
            // verify each field comes from this VM
            for (int i = 0; i < fieldsArray.Length; i++)
            {
                Field field = fieldsArray[i] as Field;
                if (field == null)
                    throw new VirtualMachineMismatchException();

                fieldIds[i] = field.FieldId;
            }

            Types.Value[] values;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectValues(out values, ObjectId, fieldIds));

            Dictionary<IField, IValue> result = new Dictionary<IField, IValue>();
            for (int i = 0; i < fieldIds.Length; i++)
                result[fieldsArray[i]] = VirtualMachine.GetMirrorOf(values[i]);

            return result;
        }

        public IStrongValueHandle<IValue> InvokeMethod(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments)
        {
            Types.Value returnValue;
            TaggedObjectId thrownException;
            if (thread != null || VirtualMachine.GetCanInvokeWithoutThread())
            {
                ThreadId threadId = default(ThreadId);
                if (thread != null)
                    threadId = ((ThreadReference)thread).ThreadId;

                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.InvokeObjectMethod(out returnValue, out thrownException, ObjectId, threadId, (ClassId)((Method)method).DeclaringType.TaggedReferenceTypeId, ((Method)method).MethodId, (Types.InvokeOptions)options, arguments.Cast<Value>().Select(Value.ToNetworkValue).ToArray()));
            }
            else
            {
                returnValue = default(Types.Value);
                thrownException = default(TaggedObjectId);
                Error errorCode = Error.ThreadNotSuspended;

                foreach (var vmThread in VirtualMachine.GetAllThreads())
                {
                    ThreadReference threadReference = vmThread as ThreadReference;
                    if (threadReference == null)
                        continue;

                    ThreadStatus threadStatus;
                    SuspendStatus suspendStatus;
                    DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadStatus(out threadStatus, out suspendStatus, threadReference.ThreadId));
                    if (threadStatus != ThreadStatus.Running)
                        continue;

                    if (suspendStatus != SuspendStatus.Suspended)
                        continue;

                    int suspendCount;
                    DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThreadSuspendCount(out suspendCount, threadReference.ThreadId));

                    errorCode = VirtualMachine.ProtocolService.InvokeObjectMethod(out returnValue, out thrownException, ObjectId, threadReference.ThreadId, (ClassId)((Method)method).DeclaringType.TaggedReferenceTypeId, ((Method)method).MethodId, (Types.InvokeOptions)options, arguments.Cast<Value>().Select(Value.ToNetworkValue).ToArray());
                    if (errorCode == Error.InvalidThread)
                        continue;

                    break;
                }

                DebugErrorHandler.ThrowOnFailure(errorCode);
            }

            if (thrownException.ObjectId != default(ObjectId))
            {
                throw new InternalException((int)Error.Internal, "An exception was thrown by the invoked method.");
            }

            Value returnValueMirror = VirtualMachine.GetMirrorOf(returnValue);
            if (returnValueMirror == null)
                return null;

            return new StrongValueHandle<Value>(VirtualMachine.GetMirrorOf(returnValue));
        }

        public bool GetIsCollected()
        {
            bool result;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetIsObjectCollected(out result, ObjectId));
            return result;
        }

        public IThreadReference GetOwningThread()
        {
            ThreadId owner;
            int entryCount;
            ThreadId[] waiters;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectMonitorInfo(out owner, out entryCount, out waiters, ObjectId));
            return VirtualMachine.GetMirrorOf(owner);
        }

        public IReferenceType GetReferenceType()
        {
            if (_referenceType == null)
            {
                TypeTag typeTag;
                ReferenceTypeId typeId;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectReferenceType(out typeTag, out typeId, ObjectId));
                _referenceType = VirtualMachine.GetMirrorOf(typeTag, typeId);
            }

            return _referenceType;
        }

        public ReadOnlyCollection<IObjectReference> GetReferringObjects(long maxReferrers)
        {
            throw new NotImplementedException();
        }

        public void SetValue(IField field, IValue value)
        {
            FieldId[] fields = { ((Field)field).FieldId };
            Types.Value[] values = { Value.ToNetworkValue((Value)value) };
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SetObjectValues(ObjectId, fields, values));
        }

        public long GetUniqueId()
        {
            return ObjectId.Handle;
        }

        public ReadOnlyCollection<IThreadReference> GetWaitingThreads()
        {
            ThreadId owner;
            int entryCount;
            ThreadId[] waiters;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetObjectMonitorInfo(out owner, out entryCount, out waiters, ObjectId));
            return new ReadOnlyCollection<IThreadReference>(Array.ConvertAll(waiters, VirtualMachine.GetMirrorOf));
        }

        public bool Equals(IObjectReference other)
        {
            if (object.ReferenceEquals(this, other))
                return true;

            ObjectReference objectReference = other as ObjectReference;
            if (objectReference == null)
                return false;

            if (!this.ObjectId.Equals(objectReference.ObjectId))
                return false;

            return this.VirtualMachine.Equals(objectReference.VirtualMachine);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ObjectReference);
        }

        public override int GetHashCode()
        {
            return VirtualMachine.GetHashCode() ^ ObjectId.GetHashCode();
        }
    }
}
