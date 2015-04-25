namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;


    /// <summary>
    /// Provides access to a primitive void value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IVoidValueContracts))]
    public interface IVoidValue : IValue, IEquatable<IVoidValue>
    {
    }
}
