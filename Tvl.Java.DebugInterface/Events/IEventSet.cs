namespace Tvl.Java.DebugInterface.Events
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    [ContractClass(typeof(Contracts.IEventSetContracts))]
    public interface IEventSet : IMirror, ICollection<IEvent>
    {
        void Resume();

        SuspendPolicy SuspendPolicy();
    }
}
