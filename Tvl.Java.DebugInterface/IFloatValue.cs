namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive float value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IFloatValueContracts))]
    public interface IFloatValue : IPrimitiveValue, IComparable<IFloatValue>, IEquatable<IFloatValue>
    {
        /// <summary>
        /// Returns this IFloatValue as a float.
        /// </summary>
        float GetValue();
    }
}
