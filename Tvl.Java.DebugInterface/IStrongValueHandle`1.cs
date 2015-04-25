namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IStrongValueHandleContracts<>))]
    public interface IStrongValueHandle<out T> : IMirror, IDisposable
        where T : IValue
    {
        T Value
        {
            get;
        }
    }
}
