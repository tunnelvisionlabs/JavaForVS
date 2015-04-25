namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassFilterContracts))]
    public interface IClassFilter : IClassNameFilter
    {
        void AddClassFilter(IReferenceType referenceType);
    }
}
