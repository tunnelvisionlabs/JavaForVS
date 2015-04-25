namespace Tvl.Java.DebugInterface.Events
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IEventQueueContracts))]
    public interface IEventQueue : IMirror
    {
        IEventSet Remove();

        IEventSet Remove(TimeSpan timeout);
    }
}
