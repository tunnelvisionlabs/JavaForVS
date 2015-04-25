namespace Tvl.Java.DebugInterface.Connect
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IConnectorStringArgumentContracts))]
    public interface IConnectorStringArgument : IConnectorArgument
    {
    }
}
