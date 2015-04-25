namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive long value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.ILongValueContracts))]
    public interface ILongValue : IPrimitiveValue, IComparable<ILongValue>, IEquatable<ILongValue>
    {
        /// <summary>
        /// Returns this ILongValue as a long.
        /// </summary>
        long GetValue();
    }
}
