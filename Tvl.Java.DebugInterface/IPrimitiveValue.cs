namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The value assigned to a field or variable of primitive type in a target VM. Each primitive
    /// value is accessed through a subinterface of this interface.
    /// </summary>
    [ContractClass(typeof(Contracts.IPrimitiveValueContracts))]
    public interface IPrimitiveValue : IValue
    {
        /// <summary>
        /// Converts this value to a <see cref="IBooleanValue"/> and returns the result as a bool.
        /// </summary>
        bool GetBooleanValue();

        /// <summary>
        /// Converts this value to a <see cref="IByteValue"/> and returns the result as a byte.
        /// </summary>
        byte GetByteValue();

        /// <summary>
        /// Converts this value to a <see cref="ICharValue"/> and returns the result as a char.
        /// </summary>
        char GetCharValue();

        /// <summary>
        /// Converts this value to a <see cref="IDoubleValue"/> and returns the result as a double.
        /// </summary>
        double GetDoubleValue();

        /// <summary>
        /// Converts this value to a <see cref="IFloatValue"/> and returns the result as a float.
        /// </summary>
        float GetFloatValue();

        /// <summary>
        /// Converts this value to a <see cref="IIntValue"/> and returns the result as a int.
        /// </summary>
        int GetIntValue();

        /// <summary>
        /// Converts this value to a <see cref="ILongValue"/> and returns the result as a long.
        /// </summary>
        long GetLongValue();

        /// <summary>
        /// Converts this value to a <see cref="IShortValue"/> and returns the result as a short.
        /// </summary>
        short GetShortValue();
    }
}
