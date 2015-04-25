namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Java.DebugInterface.Types;
    using AccessModifiers = Tvl.Java.DebugInterface.AccessModifiers;
    using InvokeOptions = Tvl.Java.DebugInterface.InvokeOptions;

    internal sealed class ClassType : ReferenceType, IClassType
    {
        // cached data
        private InterfaceType[] _allInterfaces;
        private InterfaceType[] _interfaces;

        internal ClassType(VirtualMachine virtualMachine, ClassId typeId)
            : base(virtualMachine, new TaggedReferenceTypeId(TypeTag.Class, typeId))
        {
            Contract.Requires(virtualMachine != null);
        }

        public ClassId ClassId
        {
            get
            {
                return (ClassId)base.ReferenceTypeId;
            }
        }

        public ReadOnlyCollection<IInterfaceType> GetInterfaces(bool includeInherited)
        {
            if (includeInherited && _allInterfaces == null)
            {
                List<InterfaceType> allInterfaces = new List<InterfaceType>();

                HashSet<ReferenceType> inheritedTypes = new HashSet<ReferenceType>();
                GetInheritedTypes(this, inheritedTypes);
                allInterfaces.AddRange(inheritedTypes.OfType<InterfaceType>());

                _allInterfaces = allInterfaces.ToArray();
            }
            else if (!includeInherited && _interfaces == null)
            {
                InterfaceId[] interfaceIds;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetInterfaces(out interfaceIds, ReferenceTypeId));
                InterfaceType[] interfaces = Array.ConvertAll(interfaceIds, VirtualMachine.GetMirrorOf);
                _interfaces = interfaces;
            }

            return new ReadOnlyCollection<IInterfaceType>(includeInherited ? _allInterfaces : _interfaces);
        }

        public IMethod GetConcreteMethod(string name, string signature)
        {
            var visibleMethods = GetVisibleMethods();
            return visibleMethods.SingleOrDefault(method => (method.GetDeclaringType() is IClassType) && method.GetName() == name && method.GetSignature() == signature);
        }

        public IStrongValueHandle<IValue> InvokeMethod(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments)
        {
            Types.Value returnValue;
            TaggedObjectId thrownException;
            ThreadId threadId = (thread != null) ? ((ThreadReference)thread).ThreadId : default(ThreadId);
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.InvokeClassMethod(out returnValue, out thrownException, ClassId, threadId, ((Method)method).MethodId, (Types.InvokeOptions)options, arguments.Cast<Value>().Select(Value.ToNetworkValue).ToArray()));
            if (thrownException != default(TaggedObjectId))
            {
                throw new NotImplementedException();
            }

            return new StrongValueHandle<Value>(VirtualMachine.GetMirrorOf(returnValue));
        }

        public bool GetIsEnum()
        {
            return (GetModifiers() & AccessModifiers.Enum) != 0;
        }

        public IStrongValueHandle<IObjectReference> CreateInstance(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments)
        {
            TaggedObjectId returnValue;
            TaggedObjectId thrownException;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.CreateClassInstance(out returnValue, out thrownException, ClassId, ((ThreadReference)thread).ThreadId, ((Method)method).MethodId, (Types.InvokeOptions)options, arguments.Cast<Value>().Select(Value.ToNetworkValue).ToArray()));
            if (thrownException != default(TaggedObjectId))
            {
                throw new NotImplementedException();
            }

            return new StrongValueHandle<ObjectReference>(VirtualMachine.GetMirrorOf(returnValue));
        }

        public void SetValue(IField field, IValue value)
        {
            FieldId[] fields = { ((Field)field).FieldId };
            Types.Value[] values = { Value.ToNetworkValue((Value)value) };
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SetClassValues(ClassId, fields, values));
        }

        public ReadOnlyCollection<IClassType> GetSubclasses()
        {
            throw new NotImplementedException();
        }

        public IClassType GetSuperclass()
        {
            ClassId superclass;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetSuperclass(out superclass, ClassId));
            return VirtualMachine.GetMirrorOf(superclass);
        }
    }
}
