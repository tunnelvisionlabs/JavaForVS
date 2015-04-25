namespace Tvl.Java.DebugInterface.Types
{
    public enum Error
    {
        /// <summary>
        /// No error has occurred. 
        /// </summary>
        None = 0,

        /// <summary>
        /// Passed thread is null, is not a valid thread or has exited. 
        /// </summary>
        InvalidThread = 10,

        /// <summary>
        /// Thread group invalid. 
        /// </summary>
        InvalidThreadGroup = 11,

        /// <summary>
        /// Invalid priority. 
        /// </summary>
        InvalidPriority = 12,

        /// <summary>
        /// If the specified thread has not been suspended by an event. 
        /// </summary>
        ThreadNotSuspended = 13,

        /// <summary>
        /// Thread already suspended. 
        /// </summary>
        ThreadSuspended = 14,

        /// <summary>
        /// If this reference type has been unloaded and garbage collected. 
        /// </summary>
        InvalidObject = 20,

        /// <summary>
        /// Invalid class. 
        /// </summary>
        InvalidClass = 21,

        /// <summary>
        /// Class has been loaded but not yet prepared. 
        /// </summary>
        ClassNotPrepared = 22,

        /// <summary>
        /// Invalid method. 
        /// </summary>
        InvalidMethodid = 23,

        /// <summary>
        /// Invalid location. 
        /// </summary>
        InvalidLocation = 24,

        /// <summary>
        /// Invalid field. 
        /// </summary>
        InvalidFieldid = 25,

        /// <summary>
        /// Invalid jframeID. 
        /// </summary>
        InvalidFrameid = 30,

        /// <summary>
        /// There are no more Java or JNI frames on the call stack. 
        /// </summary>
        NoMoreFrames = 31,

        /// <summary>
        /// Information about the frame is not available. 
        /// </summary>
        OpaqueFrame = 32,

        /// <summary>
        /// Operation can only be performed on current frame. 
        /// </summary>
        NotCurrentFrame = 33,

        /// <summary>
        /// The variable is not an appropriate type for the function used. 
        /// </summary>
        TypeMismatch = 34,

        /// <summary>
        /// Invalid slot. 
        /// </summary>
        InvalidSlot = 35,

        /// <summary>
        /// Item already set. 
        /// </summary>
        Duplicate = 40,

        /// <summary>
        /// Desired element not found. 
        /// </summary>
        NotFound = 41,

        /// <summary>
        /// Invalid monitor. 
        /// </summary>
        InvalidMonitor = 50,

        /// <summary>
        /// This thread doesn't own the monitor. 
        /// </summary>
        NotMonitorOwner = 51,

        /// <summary>
        /// The call has been interrupted before completion. 
        /// </summary>
        Interrupt = 52,

        /// <summary>
        /// The virtual machine attempted to read a class file and determined that the file is malformed or otherwise cannot be interpreted as a class file. 
        /// </summary>
        InvalidClassFormat = 60,

        /// <summary>
        /// A circularity has been detected while initializing a class. 
        /// </summary>
        CircularClassDefinition = 61,

        /// <summary>
        /// The verifier detected that a class file, though well formed, contained some sort of internal inconsistency or security problem. 
        /// </summary>
        FailsVerification = 62,

        /// <summary>
        /// Adding methods has not been implemented. 
        /// </summary>
        AddMethodNotImplemented = 63,

        /// <summary>
        /// Schema change has not been implemented. 
        /// </summary>
        SchemaChangeNotImplemented = 64,

        /// <summary>
        /// The state of the thread has been modified, and is now inconsistent. 
        /// </summary>
        InvalidTypestate = 65,

        /// <summary>
        /// A direct superclass is different for the new class version, or the set of directly implemented interfaces is different and canUnrestrictedlyRedefineClasses is false. 
        /// </summary>
        HierarchyChangeNotImplemented = 66,

        /// <summary>
        /// The new class version does not declare a method declared in the old class version and canUnrestrictedlyRedefineClasses is false. 
        /// </summary>
        DeleteMethodNotImplemented = 67,

        /// <summary>
        /// A class file has a version number not supported by this VM. 
        /// </summary>
        UnsupportedVersion = 68,

        /// <summary>
        /// The class name defined in the new class file is different from the name in the old class object. 
        /// </summary>
        NamesDontMatch = 69,

        /// <summary>
        /// The new class version has different modifiers and and canUnrestrictedlyRedefineClasses is false. 
        /// </summary>
        ClassModifiersChangeNotImplemented = 70,

        /// <summary>
        /// A method in the new class version has different modifiers than its counterpart in the old class version and and canUnrestrictedlyRedefineClasses is false. 
        /// </summary>
        MethodModifiersChangeNotImplemented = 71,

        /// <summary>
        /// The functionality is not implemented in this virtual machine. 
        /// </summary>
        NotImplemented = 99,

        /// <summary>
        /// Invalid pointer. 
        /// </summary>
        NullPointer = 100,

        /// <summary>
        /// Desired information is not available. 
        /// </summary>
        AbsentInformation = 101,

        /// <summary>
        /// The specified event type id is not recognized. 
        /// </summary>
        InvalidEventType = 102,

        /// <summary>
        /// Illegal argument. 
        /// </summary>
        IllegalArgument = 103,

        /// <summary>
        /// The function needed to allocate memory and no more memory was available for allocation. 
        /// </summary>
        OutOfMemory = 110,

        /// <summary>
        /// Debugging has not been enabled in this virtual machine. JVMDI cannot be used. 
        /// </summary>
        AccessDenied = 111,

        /// <summary>
        /// The virtual machine is not running. 
        /// </summary>
        VmDead = 112,

        /// <summary>
        /// An unexpected internal error has occurred. 
        /// </summary>
        Internal = 113,

        /// <summary>
        /// The thread being used to call this function is not attached to the virtual machine. Calls must be made from attached threads. 
        /// </summary>
        UnattachedThread = 115,

        /// <summary>
        /// object type id or class tag. 
        /// </summary>
        InvalidTag = 500,

        /// <summary>
        /// Previous invoke not complete. 
        /// </summary>
        AlreadyInvoking = 502,

        /// <summary>
        /// Index is invalid. 
        /// </summary>
        InvalidIndex = 503,

        /// <summary>
        /// The length is invalid. 
        /// </summary>
        InvalidLength = 504,

        /// <summary>
        /// The string is invalid. 
        /// </summary>
        InvalidString = 506,

        /// <summary>
        /// The class loader is invalid. 
        /// </summary>
        InvalidClassLoader = 507,

        /// <summary>
        /// The array is invalid. 
        /// </summary>
        InvalidArray = 508,

        /// <summary>
        /// Unable to load the transport. 
        /// </summary>
        TransportLoad = 509,

        /// <summary>
        /// Unable to initialize the transport. 
        /// </summary>
        TransportInit = 510,

        /// <summary>
        /// 
        /// </summary>
        NativeMethod = 511,

        /// <summary>
        /// The count is invalid.
        /// </summary>
        InvalidCount = 512,
    }
}
