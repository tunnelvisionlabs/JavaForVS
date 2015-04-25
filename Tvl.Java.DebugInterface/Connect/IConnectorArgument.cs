namespace Tvl.Java.DebugInterface.Connect
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Specification for and value of a <see cref="IConnector"/> argument.
    /// </summary>
    [ContractClass(typeof(Contracts.IConnectorArgumentContracts))]
    public interface IConnectorArgument
    {
        /// <summary>
        /// Returns a human-readable description of this argument and its purpose.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Returns a short human-readable label for this argument.
        /// </summary>
        string Label
        {
            get;
        }

        /// <summary>
        /// Indicates whether the argument must be specified.
        /// </summary>
        bool Required
        {
            get;
        }

        /// <summary>
        /// Returns a short, unique identifier for the argument.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets or sets the current value of the argument.
        /// </summary>
        string StringValue
        {
            get;
            set;
        }

        /// <summary>
        /// Performs basic sanity check of argument.
        /// </summary>
        bool IsValid(string value);
    }
}
