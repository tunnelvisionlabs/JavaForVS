namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Events;
    using Tvl.Java.DebugInterface.Request;

    /// <summary>
    /// A virtual machine targeted for debugging.
    /// </summary>
    [ContractClass(typeof(Contracts.IVirtualMachineContracts))]
    public interface IVirtualMachine : IDisposable, IMirror
    {
        /// <summary>
        /// Returns all loaded types.
        /// </summary>
        ReadOnlyCollection<IReferenceType> GetAllClasses();

        /// <summary>
        /// Returns a list of the currently running threads.
        /// </summary>
        ReadOnlyCollection<IThreadReference> GetAllThreads();

        /// <summary>
        /// Determines if the target VM supports the addition of methods when performing class redefinition.
        /// </summary>
        bool GetCanAddMethod();

        /// <summary>
        /// Determines if the target VM is a read-only VM.
        /// </summary>
        bool GetCanBeModified();

        /// <summary>
        /// Determines if the target VM supports the forcing of a method to return early.
        /// </summary>
        bool GetCanForceEarlyReturn();

        /// <summary>
        /// Determines if the target VM supports the retrieval of a method's bytecodes.
        /// </summary>
        bool GetCanGetBytecodes();

        /// <summary>
        /// Determines if the target VM supports reading class file major and minor versions.
        /// </summary>
        bool GetCanGetClassFileVersion();

        /// <summary>
        /// Determines if the target VM supports getting constant pool information of a class.
        /// </summary>
        bool GetCanGetConstantPool();

        /// <summary>
        /// Determines if the target VM supports the retrieval of the monitor for which a thread is currently waiting.
        /// </summary>
        bool GetCanGetCurrentContendedMonitor();

        /// <summary>
        /// Determines if the target VM supports the accessing of class instances, instance counts, and referring objects.
        /// </summary>
        bool GetCanGetInstanceInfo();

        /// <summary>
        /// Determines if the target VM supports the inclusion of return values in <see cref="IMethodExitEvent"/>s.
        /// </summary>
        bool GetCanGetMethodReturnValues();

        /// <summary>
        /// Determines if the target VM supports getting which frame has acquired a monitor.
        /// </summary>
        bool GetCanGetMonitorFrameInfo();

        /// <summary>
        /// Determines if the target VM supports the retrieval of the monitor information for an object.
        /// </summary>
        bool GetCanGetMonitorInfo();

        /// <summary>
        /// Determines if the target VM supports the retrieval of the monitors owned by a thread.
        /// </summary>
        bool GetCanGetOwnedMonitorInfo();

        /// <summary>
        /// Determines if the target VM supports getting the source debug extension.
        /// </summary>
        bool GetCanGetSourceDebugExtension();

        /// <summary>
        /// Determines if the target VM supports the query of the synthetic attribute of a method or field.
        /// </summary>
        bool GetCanGetSyntheticAttribute();

        /// <summary>
        /// Determines if the target VM supports popping frames of a threads stack.
        /// </summary>
        bool GetCanPopFrames();

        /// <summary>
        /// Determines if the target VM supports any level of class redefinition.
        /// </summary>
        bool GetCanRedefineClasses();

        /// <summary>
        /// Determines if the target VM supports the creation of <see cref="IMonitorContendedEnterRequest"/>s.
        /// </summary>
        bool GetCanRequestMonitorEvents();

        /// <summary>
        /// Determines if the target VM supports the creation of <see cref="IVirtualMachineDeathRequest"/>s.
        /// </summary>
        bool GetCanRequestVMDeathEvent();

        /// <summary>
        /// Determines if the target VM supports unrestricted changes when performing class redefinition.
        /// </summary>
        bool GetCanUnrestrictedlyRedefineClasses();

        /// <summary>
        /// Determines if the target VM supports filtering events by specific instance object.
        /// </summary>
        bool GetCanUseInstanceFilters();

        /// <summary>
        /// Determines if the target VM supports the filtering of class prepare events by source name.
        /// </summary>
        bool GetCanUseSourceNameFilters();

        /// <summary>
        /// Determines if the target VM supports watchpoints for field access.
        /// </summary>
        bool GetCanWatchFieldAccess();

        /// <summary>
        /// Determines if the target VM supports watchpoints for field modification.
        /// </summary>
        bool GetCanWatchFieldModification();

        /// <summary>
        /// Determines if the target VM supports the <see cref="StepSize.Statement"/> step size.
        /// </summary>
        bool GetCanStepByStatement();

        /// <summary>
        /// Determines if the target VM supports invoking methods without explicitly providing a thread.
        /// </summary>
        bool GetCanInvokeWithoutThread();

        /// <summary>
        /// Returns the loaded reference types that match a given name.
        /// </summary>
        ReadOnlyCollection<IReferenceType> GetClassesByName(string className);

        /// <summary>
        /// Returns text information on the target VM and the debugger support that mirrors it.
        /// </summary>
        string GetDescription();

        /// <summary>
        /// Returns the event queue for this virtual machine.
        /// </summary>
        IEventQueue GetEventQueue();

        /// <summary>
        /// Returns the event request manager for this virtual machine.
        /// </summary>
        IEventRequestManager GetEventRequestManager();

        /// <summary>
        /// Causes the mirrored VM to terminate with the given error code.
        /// </summary>
        void Exit(int exitCode);

        /// <summary>
        /// Return this VM's default stratum.
        /// </summary>
        string GetDefaultStratum();

        /// <summary>
        /// Returns the number of instances of each <see cref="IReferenceType"/> in <param name="referenceTypes"/>.
        /// </summary>
        long[] GetInstanceCounts(IEnumerable<IReferenceType> referenceTypes);

        /// <summary>
        /// Creates a <see cref="IBooleanValue"/> for the given value.
        /// </summary>
        IBooleanValue GetMirrorOf(bool value);

        /// <summary>
        /// Creates a <see cref="IByteValue"/> for the given value.
        /// </summary>
        IByteValue GetMirrorOf(byte value);

        /// <summary>
        /// Creates a <see cref="ICharValue"/> for the given value.
        /// </summary>
        ICharValue GetMirrorOf(char value);

        /// <summary>
        /// Creates a <see cref="IDoubleValue"/> for the given value.
        /// </summary>
        IDoubleValue GetMirrorOf(double value);

        /// <summary>
        /// Creates a <see cref="IFloatValue"/> for the given value.
        /// </summary>
        IFloatValue GetMirrorOf(float value);

        /// <summary>
        /// Creates a <see cref="IIntegerValue"/> for the given value.
        /// </summary>
        IIntegerValue GetMirrorOf(int value);

        /// <summary>
        /// Creates a <see cref="ILongValue"/> for the given value.
        /// </summary>
        ILongValue GetMirrorOf(long value);

        /// <summary>
        /// Creates a <see cref="IShortValue"/> for the given value.
        /// </summary>
        IShortValue GetMirrorOf(short value);

        /// <summary>
        /// Creates a string in this virtual machine.
        /// </summary>
        IStrongValueHandle<IStringReference> GetMirrorOf(string value);

        /// <summary>
        /// Creates a <see cref="IVoidValue"/>.
        /// </summary>
        IVoidValue GetMirrorOfVoid();

        /// <summary>
        /// Returns the name of the target VM as reported by the property <c>java.vm.name</c>.
        /// </summary>
        string GetName();

        /// <summary>
        /// All classes given are redefined according to the definitions supplied.
        /// </summary>
        void RedefineClasses(IEnumerable<KeyValuePair<IReferenceType, byte[]>> classes);

        /// <summary>
        /// Continues the execution of the application running in this virtual machine.
        /// </summary>
        void Resume();

        /// <summary>
        /// Traces the activities performed by the com.sun.jdi implementation.
        /// </summary>
        void SetDebugTraceMode(TraceModes modes);

        /// <summary>
        /// Set this VM's default stratum (see Location for a discussion of strata).
        /// </summary>
        void SetDefaultStratum(string stratum);

        /// <summary>
        /// Suspends the execution of the application running in this virtual machine.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Returns each thread group which does not have a parent.
        /// </summary>
        ReadOnlyCollection<IThreadGroupReference> GetTopLevelThreadGroups();

        /// <summary>
        /// Returns the version of the Java Runtime Environment in the target VM as reported by the property <c>java.version</c>.
        /// </summary>
        string GetVersion();
    }
}
