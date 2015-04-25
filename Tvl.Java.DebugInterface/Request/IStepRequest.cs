namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IStepRequestContracts))]
    public interface IStepRequest : IEventRequest, IClassFilter, IInstanceFilter
    {
        StepDepth Depth
        {
            get;
        }

        StepSize Size
        {
            get;
        }

        IThreadReference Thread
        {
            get;
        }
    }
}
