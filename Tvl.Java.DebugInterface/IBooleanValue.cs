namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to a primitive boolean value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IBooleanValueContracts))]
    public interface IBooleanValue : IPrimitiveValue, IEquatable<IBooleanValue>
    {
        /// <summary>
        /// Returns this IBooleanValue as a bool.
        /// </summary>
        bool GetValue();
    }
}
