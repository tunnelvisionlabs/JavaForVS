namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadFilterContracts))]
    public interface IThreadFilter : IMirror
    {
        void AddThreadFilter(IThreadReference thread);
    }
}
