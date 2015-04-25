namespace Tvl.VisualStudio.Shell.OutputWindow.Interfaces.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IOutputWindowService))]
    public abstract class IOutputWindowServiceContracts : IOutputWindowService
    {
        IOutputWindowPane IOutputWindowService.TryGetPane(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            throw new NotImplementedException();
        }
    }
}
