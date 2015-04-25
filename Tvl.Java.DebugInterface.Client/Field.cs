namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;
    using AccessModifiers = Tvl.Java.DebugInterface.AccessModifiers;

    internal sealed class Field : TypeComponent, IField
    {
        private readonly FieldId _fieldId;

        private IType _fieldType;

        internal Field(VirtualMachine virtualMachine, ReferenceType declaringType, string name, string signature, string genericSignature, AccessModifiers modifiers, FieldId fieldId)
            : base(virtualMachine, declaringType, name, signature, genericSignature, modifiers)
        {
            Contract.Requires(virtualMachine != null);
            _fieldId = fieldId;
        }

        public FieldId FieldId
        {
            get
            {
                return _fieldId;
            }
        }

        #region IField Members

        public bool GetIsEnumConstant()
        {
            return (GetModifiers() & AccessModifiers.Enum) != 0;
        }

        public bool GetIsTransient()
        {
            return (GetModifiers() & AccessModifiers.Transient) != 0;
        }

        public bool GetIsVolatile()
        {
            return (GetModifiers() & AccessModifiers.Volatile) != 0;
        }

        public IType GetFieldType()
        {
            if (_fieldType == null)
            {
                IType fieldType = VirtualMachine.FindType(GetSignature());
                if (fieldType is UnloadedReferenceType)
                    return fieldType;

                _fieldType = fieldType;
            }

            return _fieldType;
        }

        public string GetFieldTypeName()
        {
            return GetFieldType().GetName();
        }

        #endregion

        #region IEquatable<IField> Members

        public bool Equals(IField other)
        {
            Field field = other as Field;
            if (field == null)
                return false;

            return this.VirtualMachine.Equals(field.VirtualMachine)
                && this.FieldId == field.FieldId;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Field);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ this.FieldId.GetHashCode();
        }

        #endregion
    }
}
