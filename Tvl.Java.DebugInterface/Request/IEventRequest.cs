namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IEventRequestContracts))]
    public interface IEventRequest : IMirror
    {
        bool IsEnabled
        {
            get;
            set;
        }

        SuspendPolicy SuspendPolicy
        {
            get;
            set;
        }

        object GetProperty(object key);

        void PutProperty(object key, object value);

        void AddCountFilter(int count);
    }
}
