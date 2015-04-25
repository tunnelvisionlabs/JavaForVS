namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IExceptionRequestContracts))]
    public interface IExceptionRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
        IReferenceType Exception
        {
            get;
        }

        bool NotifyWhenCaught
        {
            get;
        }

        bool NotifyWhenUncaught
        {
            get;
        }
    }
}
