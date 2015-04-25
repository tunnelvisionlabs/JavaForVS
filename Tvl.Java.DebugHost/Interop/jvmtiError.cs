namespace Tvl.Java.DebugHost.Interop
{
    public enum jvmtiError
    {
        #region Universal Errors

        /// <summary>
        /// No error has occurred. This is the error code that is returned on successful completion of the function.
        /// </summary>
        None = 0,

        NullPointer = 100,
        //Pointer is unexpectedly NULL. 

        OutOfMemory = 110,
        //The function attempted to allocate memory and no more memory was available for allocation. 

        AccessDenied = 111,
        //The desired functionality has not been enabled in this virtual machine. 

        UnattachedThread = 115,
        //The thread being used to call this function is not attached to the virtual machine. Calls must be made from attached threads. See AttachCurrentThread in the JNI invocation API. 

        InvalidEnvironment = 116,
        //The JVM TI environment provided is no longer connected or is not an environment. 

        WrongPhase = 112,
        //The desired functionality is not available in the current phase. Always returned if the virtual machine has completed running. 

        Internal = 113,
        //An unexpected internal error has occurred. 

        #endregion

        #region Function Specific Required Errors

        InvalidPriority = 12,
        // Invalid priority. 

        ThreadNotSuspended = 13,
        // Thread was not suspended. 

        ThreadSuspended = 14,
        // Thread already suspended. 

        ThreadNotAlive = 15,
        // This operation requires the thread to be alive--that is, it must be started and not yet have died. 

        ClassNotPrepared = 22,
        // The class has been loaded but not yet prepared. 

        NoMoreFrames = 31,
        // There are no Java programming language or JNI stack frames at the specified depth. 

        OpaqueFrame = 32,
        // Information about the frame is not available (e.g. for native frames). 

        Duplicate = 40,
        // Item already set. 

        NotFound = 41,
        // Desired element (e.g. field or breakpoint) not found 

        NotMonitorOwner = 51,
        // This thread doesn't own the raw monitor. 

        Interrupt = 52,
        // The call has been interrupted before completion. 

        UnmodifiableClass = 79,
        // The class cannot be modified. 

        NotAvailable = 98,
        // The functionality is not available in this virtual machine. 

        AbsentInformation = 101,
        // The requested information is not available. 

        InvalidEventType = 102,
        // The specified event type ID is not recognized. 

        NativeMethod = 104,
        // The requested information is not available for native method. 

        ClassLoaderUnsupported = 106,
        //The class loader does not support this operation. 

        #endregion

        #region Function Specific Agent Errors

        InvalidThread = 10,
        // The passed thread is not a valid thread. 

        InvalidFieldid = 25,
        // Invalid field. 

        InvalidMethodid = 23,
        // Invalid method. 

        InvalidLocation = 24,
        // Invalid location. 

        InvalidObject = 20,
        // Invalid object. 

        InvalidClass = 21,
        // Invalid class. 

        TypeMismatch = 34,
        // The variable is not an appropriate type for the function used. 

        InvalidSlot = 35,
        // Invalid slot. 

        MustPossessCapability = 99,
        // The capability being used is false in this environment. 

        InvalidThreadGroup = 11,
        // Thread group invalid. 

        InvalidMonitor = 50,
        // Invalid raw monitor. 

        IllegalArgument = 103,
        // Illegal argument. 

        InvalidTypestate = 65,
        // The state of the thread has been modified, and is now inconsistent. 

        UnsupportedVersion = 68,
        // A new class file has a version number not supported by this VM. 

        InvalidClassFormat = 60,
        // A new class file is malformed (the VM would return a ClassFormatError). 

        CircularClassDefinition = 61,
        // The new class file definitions would lead to a circular definition (the VM would return a ClassCircularityError). 

        UnsupportedRedefinitionMethodAdded = 63,
        // A new class file would require adding a method. 

        UnsupportedRedefinitionSchemaChanged = 64,
        // A new class version changes a field. 

        FailsVerification = 62,
        // The class bytes fail verification. 

        UnsupportedRedefinitionHierarchyChanged = 66,
        // A direct superclass is different for the new class version, or the set of directly implemented interfaces is different. 

        UnsupportedRedefinitionMethodDeleted = 67,
        // A new class version does not declare a method declared in the old class version. 

        NamesDontMatch = 69,
        // The class name defined in the new class file is different from the name in the old class object. 

        UnsupportedRedefinitionClassModifiersChanged = 70,
        // A new class version has different modifiers. 

        UnsupportedRedefinitionMethodModifiersChanged = 71,
        // A method in the new class version has different modifiers than its counterpart in the old class version. 

        #endregion
    }
}
