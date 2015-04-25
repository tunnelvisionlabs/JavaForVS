namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;

    internal class ThreadGroupReference : ObjectReference, IThreadGroupReference
    {
        internal ThreadGroupReference(VirtualMachine virtualMachine, ThreadGroupId threadGroupId)
            : base(virtualMachine, threadGroupId, null)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal ThreadGroupReference(VirtualMachine virtualMachine, ThreadGroupId threadGroupId, IReferenceType threadGroupType)
            : base(virtualMachine, threadGroupId, threadGroupType)
        {
            Contract.Requires(virtualMachine != null);
        }

        public ThreadGroupId ThreadGroupId
        {
            get
            {
                return (ThreadGroupId)base.ObjectId;
            }
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.ThreadGroup, ObjectId.Handle);
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public IThreadGroupReference GetParent()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadGroupReference> GetThreadGroups()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadReference> GetThreads()
        {
            throw new NotImplementedException();
        }
    }
}
