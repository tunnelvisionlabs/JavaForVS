namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IInstanceFilterContracts))]
    public interface IInstanceFilter : IMirror
    {
        void AddInstanceFilter(IObjectReference instance);
    }
}
