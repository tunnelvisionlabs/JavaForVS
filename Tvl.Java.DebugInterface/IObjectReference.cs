namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An object that currently exists in the target VM. An IObjectReference mirrors only the object
    /// itself and is not specific to any <see cref="IField"/> or <see cref="ILocalVariable"/> to which
    /// it is currently assigned. An IObjectReference can have 0 or more references from field(s) and/or
    /// variable(s).
    /// </summary>
    [ContractClass(typeof(Contracts.IObjectReferenceContracts))]
    public interface IObjectReference : IValue, IEquatable<IObjectReference>
    {
        /// <summary>
        /// Prevents garbage collection for this object.
        /// </summary>
        void DisableCollection();

        /// <summary>
        /// Permits garbage collection for this object.
        /// </summary>
        void EnableCollection();

        /// <summary>
        /// Returns the number times this object's monitor has been entered by the current owning thread.
        /// </summary>
        int GetEntryCount();

        /// <summary>
        /// Gets the value of a given instance or static field in this object.
        /// </summary>
        IValue GetValue(IField field);

        /// <summary>
        /// Gets the value of multiple instance and/or static fields in this object.
        /// </summary>
        IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields);

        /// <summary>
        /// Invokes the specified <see cref="IMethod"/> on this object in the target VM.
        /// </summary>
        IStrongValueHandle<IValue> InvokeMethod(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments);

        /// <summary>
        /// Determines if this object has been garbage collected in the target VM.
        /// </summary>
        bool GetIsCollected();

        /// <summary>
        /// Returns an <see cref="IThreadReference"/> for the thread, if any, which currently owns this object's monitor.
        /// </summary>
        IThreadReference GetOwningThread();

        /// <summary>
        /// Gets the <see cref="IReferenceType"/> that mirrors the type of this object.
        /// </summary>
        IReferenceType GetReferenceType();

        /// <summary>
        /// Returns objects that directly reference this object.
        /// </summary>
        ReadOnlyCollection<IObjectReference> GetReferringObjects(long maxReferrers);

        /// <summary>
        /// Sets the value of a given instance or static field in this object.
        /// </summary>
        void SetValue(IField field, IValue value);

        /// <summary>
        /// Returns a unique identifier for this IObjectReference.
        /// </summary>
        /// <returns></returns>
        long GetUniqueId();

        /// <summary>
        /// Returns a list containing a <see cref="IThreadReference"/> for each thread currently waiting for this object's monitor.
        /// </summary>
        ReadOnlyCollection<IThreadReference> GetWaitingThreads();
    }
}
