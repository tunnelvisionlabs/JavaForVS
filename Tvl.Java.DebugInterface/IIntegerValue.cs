namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive int value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IIntegerValueContracts))]
    public interface IIntegerValue : IPrimitiveValue, IComparable<IIntegerValue>, IEquatable<IIntegerValue>
    {
        /// <summary>
        /// Returns this IIntegerValue as an int.
        /// </summary>
        int GetValue();
    }
}
