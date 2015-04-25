namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides information on the accessibility of a type or type component. Mirrors for program elements
    /// which allow an an access specifier (private, protected, public) provide information on that part of
    /// the declaration through this interface.
    /// </summary>
    [ContractClass(typeof(Contracts.IAccessibleContracts))]
    public interface IAccessible
    {
        /// <summary>
        /// Determines if this object mirrors a package private item.
        /// </summary>
        bool GetIsPackagePrivate();

        /// <summary>
        /// Determines if this object mirrors a private item.
        /// </summary>
        bool GetIsPrivate();

        /// <summary>
        /// Determines if this object mirrors a protected item.
        /// </summary>
        bool GetIsProtected();

        /// <summary>
        /// Determines if this object mirrors a public item.
        /// </summary>
        bool GetIsPublic();

        /// <summary>
        /// Returns the JavaTM programming language modifiers.
        /// </summary>
        AccessModifiers GetModifiers();
    }
}
