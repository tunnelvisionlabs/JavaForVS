namespace Tvl.Java.DebugInterface.Connect
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IConnectorIntegerArgumentContracts))]
    public interface IConnectorIntegerArgument : IConnectorArgument
    {
        /// <summary>
        /// Gets or sets the value of the argument as an integer.
        /// </summary>
        int Value
        {
            get;
            set;
        }

        int MinimumValue
        {
            get;
        }

        int MaximumValue
        {
            get;
        }

        bool IsValid(int value);

        string GetStringValueOf(int value);
    }
}
