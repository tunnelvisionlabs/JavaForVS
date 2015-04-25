namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;

    internal class ClassLoaderReference : ObjectReference, IClassLoaderReference
    {
        internal ClassLoaderReference(VirtualMachine virtualMachine, ClassLoaderId classLoaderId)
            : base(virtualMachine, classLoaderId, null)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal ClassLoaderReference(VirtualMachine virtualMachine, ClassLoaderId classLoaderId, IReferenceType classLoaderType)
            : base(virtualMachine, classLoaderId, classLoaderType)
        {
            Contract.Requires(virtualMachine != null);
        }

        public ClassLoaderId ClassLoaderId
        {
            get
            {
                return (ClassLoaderId)base.ObjectId;
            }
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.ClassLoader, ObjectId.Handle);
        }

        public ReadOnlyCollection<IReferenceType> GetDefinedClasses()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IReferenceType> GetVisibleClasses()
        {
            TaggedReferenceTypeId[] classes;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetClassLoaderVisibleClasses(out classes, ClassLoaderId));
            return new ReadOnlyCollection<IReferenceType>(Array.ConvertAll(classes, VirtualMachine.GetMirrorOf));
        }
    }
}
