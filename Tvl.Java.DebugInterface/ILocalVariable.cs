namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A local variable in the target VM. Each variable declared within a <see cref="IMethod"/> has its own
    /// LocalVariable object. Variables of the same name declared in different scopes have different
    /// LocalVariable objects. LocalVariables can be used alone to retrieve static information about their
    /// declaration, or can be used in conjunction with a <see cref="IStackFrame"/> to set and get values.
    /// </summary>
    [ContractClass(typeof(Contracts.ILocalVariableContracts))]
    public interface ILocalVariable : IMirror, IEquatable<ILocalVariable>
    {
        /// <summary>
        /// Gets the generic signature for this variable if there is one.
        /// </summary>
        /// <returns></returns>
        string GetGenericSignature();

        /// <summary>
        /// Determines if this variable is an argument to its method.
        /// </summary>
        bool GetIsArgument();

        /// <summary>
        /// Determines whether this variable can be accessed from the given <see cref="IStackFrame"/>.
        /// </summary>
        bool GetIsVisible(IStackFrame frame);

        /// <summary>
        /// Determines whether this variable can be accessed from the given <see cref="ILocation"/>.
        /// </summary>
        bool GetIsVisible(ILocation location);

        int GetSlot();

        /// <summary>
        /// Gets the name of the local variable.
        /// </summary>
        string GetName();

        /// <summary>
        /// Gets the JNI signature of the local variable.
        /// </summary>
        string GetSignature();

        /// <summary>
        /// Returns the type of this variable.
        /// </summary>
        IType GetLocalType();

        /// <summary>
        /// Returns a text representation of the type of this variable.
        /// </summary>
        string GetLocalTypeName();
    }
}
