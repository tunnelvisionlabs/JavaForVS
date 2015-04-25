namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to an array object and its components in the target VM. Each array component is mirrored
    /// by a <see cref="IValue"/> object. The array components, in aggregate, are placed in read only collections
    /// of objects instead of arrays for consistency with the rest of the API and for interoperability with other
    /// APIs.
    /// </summary>
    [ContractClass(typeof(Contracts.IArrayReferenceContracts))]
    public interface IArrayReference : IObjectReference
    {
        /// <summary>
        /// Returns an array component value.
        /// </summary>
        IValue GetValue(int index);

        /// <summary>
        /// Returns all of the components in this array.
        /// </summary>
        ReadOnlyCollection<IValue> GetValues();

        /// <summary>
        /// Returns a range of array components.
        /// </summary>
        ReadOnlyCollection<IValue> GetValues(int index, int length);

        /// <summary>
        /// Returns the number of components in this array.
        /// </summary>
        [Pure]
        int GetLength();

        /// <summary>
        /// Replaces an array component with another value.
        /// </summary>
        void SetValue(int index, IValue value);

        /// <summary>
        /// Replaces a range of array components with other values.
        /// </summary>
        void SetValues(int index, IValue[] values, int sourceIndex, int length);

        /// <summary>
        /// Replaces all array components with other values.
        /// </summary>
        void SetValues(IValue[] values);
    }
}
