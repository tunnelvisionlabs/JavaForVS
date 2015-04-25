namespace Tvl.Java.DebugInterface.Connect
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Specification for and value of a Connector argument, whose value is Boolean. Boolean
    /// values are represented by the localized versions of the strings "true" and "false".
    /// </summary>
    [ContractClass(typeof(Contracts.IConnectorBooleanArgumentContracts))]
    public interface IConnectorBooleanArgument : IConnectorArgument
    {
        /// <summary>
        /// Gets or sets the value of the argument as a bool.
        /// </summary>
        bool Value
        {
            get;
            set;
        }

        /// <summary>
        /// Return the string representation of the <param name="value"/> parameter.
        /// </summary>
        string GetStringValueOf(bool value);
    }
}
