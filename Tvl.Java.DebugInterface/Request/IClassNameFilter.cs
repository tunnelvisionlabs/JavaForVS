namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassNameFilterContracts))]
    public interface IClassNameFilter : IMirror
    {
        void AddClassExclusionFilter(string classPattern);

        void AddClassFilter(string classPattern);
    }
}
