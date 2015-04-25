namespace Tvl.Java.DebugInterface.Connect
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IConnectorSelectedArgumentContracts))]
    public interface IConnectorSelectedArgument : IConnectorArgument
    {
        ReadOnlyCollection<string> Choices
        {
            get;
        }
    }
}
