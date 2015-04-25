namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive short value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IShortValueContracts))]
    public interface IShortValue : IPrimitiveValue, IComparable<IShortValue>, IEquatable<IShortValue>
    {
        /// <summary>
        /// Returns this IShortValue as a short.
        /// </summary>
        short GetValue();
    }
}
