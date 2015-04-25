namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;

    internal abstract class TypeComponent : Mirror, ITypeComponent
    {
        private readonly ReferenceType _declaringType;
        private readonly string _name;
        private readonly string _signature;
        private readonly string _genericSignature;
        private readonly AccessModifiers _modifiers;

        protected TypeComponent(VirtualMachine virtualMachine, ReferenceType declaringType, string name, string signature, string genericSignature, AccessModifiers modifiers)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentNullException>(declaringType != null, "declaringType");

            _declaringType = declaringType;
            _name = name;
            _signature = signature;
            _genericSignature = genericSignature;
            _modifiers = modifiers;
        }

        public ReferenceType DeclaringType
        {
            get
            {
                return _declaringType;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Signature
        {
            get
            {
                return _signature;
            }
        }

        public string GenericSignature
        {
            get
            {
                return _genericSignature;
            }
        }

        public AccessModifiers Modifiers
        {
            get
            {
                return _modifiers;
            }
        }

        #region ITypeComponent Members

        public IReferenceType GetDeclaringType()
        {
            return _declaringType;
        }

        public string GetGenericSignature()
        {
            return _genericSignature;
        }

        public bool GetIsFinal()
        {
            return (GetModifiers() & AccessModifiers.Final) != 0;
        }

        public bool GetIsStatic()
        {
            return (GetModifiers() & AccessModifiers.Static) != 0;
        }

        public bool GetIsSynthetic()
        {
            return (GetModifiers() & AccessModifiers.Synthetic) != 0;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetSignature()
        {
            return _signature;
        }

        #endregion

        #region IAccessible Members

        public bool GetIsPackagePrivate()
        {
            return (GetModifiers() & (AccessModifiers.Private | AccessModifiers.Protected | AccessModifiers.Public)) == 0;
        }

        public bool GetIsPrivate()
        {
            return (GetModifiers() & AccessModifiers.Private) != 0;
        }

        public bool GetIsProtected()
        {
            return (GetModifiers() & AccessModifiers.Protected) != 0;
        }

        public bool GetIsPublic()
        {
            return (GetModifiers() & AccessModifiers.Public) != 0;
        }

        public AccessModifiers GetModifiers()
        {
            return _modifiers;
        }

        #endregion
    }
}
