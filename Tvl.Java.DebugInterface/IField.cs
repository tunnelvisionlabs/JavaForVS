namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A class or instance variable in the target VM. See <see cref="ITypeComponent"/> for
    /// general information about Field and Method mirrors.
    /// </summary>
    [ContractClass(typeof(Contracts.IFieldContracts))]
    public interface IField : ITypeComponent, IEquatable<IField>
    {
        /// <summary>
        /// Determine if this is a field that represents an enum constant.
        /// </summary>
        bool GetIsEnumConstant();

        /// <summary>
        /// Determine if this is a transient field.
        /// </summary>
        bool GetIsTransient();

        /// <summary>
        /// Determine if this is a volatile field.
        /// </summary>
        bool GetIsVolatile();

        /// <summary>
        /// Returns the type of this field.
        /// </summary>
        IType GetFieldType();

        /// <summary>
        /// Returns a text representation of the type of this field.
        /// </summary>
        string GetFieldTypeName();
    }
}
