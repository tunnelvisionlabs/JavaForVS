namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A point within the executing code of the target VM. Locations are used to identify the current position
    /// of a suspended thread (analogous to an instruction pointer or program counter register in native programs).
    /// They are also used to identify the position at which to set a breakpoint.
    /// </summary>
    [ContractClass(typeof(Contracts.ILocationContracts))]
    public interface ILocation : IMirror, IComparable<ILocation>, IEquatable<ILocation>
    {
        /// <summary>
        /// Gets the code position within this location's method.
        /// </summary>
        long GetCodeIndex();

        /// <summary>
        /// Gets the type to which this Location belongs.
        /// </summary>
        IReferenceType GetDeclaringType();

        /// <summary>
        /// Gets the line number of this Location.
        /// </summary>
        int GetLineNumber();

        /// <summary>
        /// Gets the line number of this Location.
        /// </summary>
        int GetLineNumber(string stratum);

        /// <summary>
        /// Gets the method containing this Location.
        /// </summary>
        IMethod GetMethod();

        /// <summary>
        /// Gets an identifing name for the source corresponding to this location.
        /// </summary>
        string GetSourceName();

        /// <summary>
        /// Gets an identifing name for the source corresponding to this location.
        /// </summary>
        string GetSourceName(string stratum);

        /// <summary>
        /// Gets the path to the source corresponding to this location.
        /// </summary>
        string GetSourcePath();

        /// <summary>
        /// Gets the path to the source corresponding to this location.
        /// </summary>
        string GetSourcePath(string stratum);
    }
}
