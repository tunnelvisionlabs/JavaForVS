namespace Tvl.Java.DebugInterface
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The state of one method invocation on a thread's call stack.
    /// </summary>
    [ContractClass(typeof(Contracts.IStackFrameContracts))]
    public interface IStackFrame : IMirror, ILocatable
    {
        bool GetHasVariableInfo();

        /// <summary>
        /// Returns the values of all arguments in this frame.
        /// </summary>
        ReadOnlyCollection<IValue> GetArgumentValues();

        /// <summary>
        /// Gets the Value of a <see cref="ILocalVariable"/> in this frame.
        /// </summary>
        IValue GetValue(ILocalVariable variable);

        /// <summary>
        /// Returns the values of multiple local variables in this frame.
        /// </summary>
        IDictionary<ILocalVariable, IValue> GetValues(IEnumerable<ILocalVariable> variables);

        /// <summary>
        /// Sets the <see cref="IValue"/> of a <see cref="ILocalVariable"/> in this frame.
        /// </summary>
        void SetValue(ILocalVariable variable, IValue value);

        /// <summary>
        /// Returns the value of 'this' for the current frame.
        /// </summary>
        IObjectReference GetThisObject();

        /// <summary>
        /// Returns the thread under which this frame's method is running.
        /// </summary>
        IThreadReference GetThread();

        /// <summary>
        /// Finds a <see cref="ILocalVariable"/> that matches the given name and is visible at the current frame location.
        /// </summary>
        ILocalVariable GetVisibleVariableByName(string name);

        /// <summary>
        /// Returns a list containing each <see cref="ILocalVariable"/> that can be accessed from this frame's location.
        /// </summary>
        ReadOnlyCollection<ILocalVariable> GetVisibleVariables();
    }
}
