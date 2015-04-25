namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive byte value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IByteValueContracts))]
    public interface IByteValue : IPrimitiveValue, IEquatable<IByteValue>, IComparable<IByteValue>
    {
        /// <summary>
        /// Returns this IByteValue as a byte.
        /// </summary>
        byte GetValue();
    }
}
