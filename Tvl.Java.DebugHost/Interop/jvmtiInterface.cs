// The field 'field_name' is never used
#pragma warning disable 169
// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;
    using ObsoleteAttribute = System.ObsoleteAttribute;

    [StructLayout(LayoutKind.Sequential)]
    internal class jvmtiInterface
    {
        /*   1 :  RESERVED */
        private IntPtr reserved1;

        /*   2 : Set Event Notification Mode */
        public UnsafeNativeMethods.SetEventNotificationMode SetEventNotificationMode;

        /*   3 :  RESERVED */
        private IntPtr reserved3;

        /*   4 : Get All Threads */
        public UnsafeNativeMethods.GetAllThreads GetAllThreads;

        /*   5 : Suspend Thread */
        public UnsafeNativeMethods.SuspendThread SuspendThread;

        /*   6 : Resume Thread */
        public UnsafeNativeMethods.ResumeThread ResumeThread;

        /*   7 : Stop Thread */
        public UnsafeNativeMethods.StopThread StopThread;

        /*   8 : Interrupt Thread */
        public UnsafeNativeMethods.InterruptThread InterruptThread;

        /*   9 : Get Thread Info */
        public UnsafeNativeMethods.GetThreadInfo GetThreadInfo;

        /*   10 : Get Owned Monitor Info */
        public UnsafeNativeMethods.GetOwnedMonitorInfo GetOwnedMonitorInfo;

        /*   11 : Get Current Contended Monitor */
        public UnsafeNativeMethods.GetCurrentContendedMonitor GetCurrentContendedMonitor;

        /*   12 : Run Agent Thread */
        public UnsafeNativeMethods.RunAgentThread RunAgentThread;

        /*   13 : Get Top Thread Groups */
        public UnsafeNativeMethods.GetTopThreadGroups GetTopThreadGroups;

        /*   14 : Get Thread Group Info */
        public UnsafeNativeMethods.GetThreadGroupInfo GetThreadGroupInfo;

        /*   15 : Get Thread Group Children */
        public UnsafeNativeMethods.GetThreadGroupChildren GetThreadGroupChildren;

        /*   16 : Get Frame Count */
        public UnsafeNativeMethods.GetFrameCount GetFrameCount;

        /*   17 : Get Thread State */
        public UnsafeNativeMethods.GetThreadState GetThreadState;

        /*   18 : Get Current Thread */
        public UnsafeNativeMethods.GetCurrentThread GetCurrentThread;

        /*   19 : Get Frame Location */
        public UnsafeNativeMethods.GetFrameLocation GetFrameLocation;

        /*   20 : Notify Frame Pop */
        public UnsafeNativeMethods.NotifyFramePop NotifyFramePop;

        /*   21 : Get Local Variable - Object */
        public UnsafeNativeMethods.GetLocalObject GetLocalObject;

        /*   22 : Get Local Variable - Int */
        public UnsafeNativeMethods.GetLocalInt GetLocalInt;

        /*   23 : Get Local Variable - Long */
        public UnsafeNativeMethods.GetLocalLong GetLocalLong;

        /*   24 : Get Local Variable - Float */
        public UnsafeNativeMethods.GetLocalFloat GetLocalFloat;

        /*   25 : Get Local Variable - Double */
        public UnsafeNativeMethods.GetLocalDouble GetLocalDouble;

        /*   26 : Set Local Variable - Object */
        public UnsafeNativeMethods.SetLocalObject SetLocalObject;

        /*   27 : Set Local Variable - Int */
        public UnsafeNativeMethods.SetLocalInt SetLocalInt;

        /*   28 : Set Local Variable - Long */
        public UnsafeNativeMethods.SetLocalLong SetLocalLong;

        /*   29 : Set Local Variable - Float */
        public UnsafeNativeMethods.SetLocalFloat SetLocalFloat;

        /*   30 : Set Local Variable - Double */
        public UnsafeNativeMethods.SetLocalDouble SetLocalDouble;

        /*   31 : Create Raw Monitor */
        public UnsafeNativeMethods.CreateRawMonitor CreateRawMonitor;

        /*   32 : Destroy Raw Monitor */
        public UnsafeNativeMethods.DestroyRawMonitor DestroyRawMonitor;

        /*   33 : Raw Monitor Enter */
        public UnsafeNativeMethods.RawMonitorEnter RawMonitorEnter;

        /*   34 : Raw Monitor Exit */
        public UnsafeNativeMethods.RawMonitorExit RawMonitorExit;

        /*   35 : Raw Monitor Wait */
        public UnsafeNativeMethods.RawMonitorWait RawMonitorWait;

        /*   36 : Raw Monitor Notify */
        public UnsafeNativeMethods.RawMonitorNotify RawMonitorNotify;

        /*   37 : Raw Monitor Notify All */
        public UnsafeNativeMethods.RawMonitorNotifyAll RawMonitorNotifyAll;

        /*   38 : Set Breakpoint */
        public UnsafeNativeMethods.SetBreakpoint SetBreakpoint;

        /*   39 : Clear Breakpoint */
        public UnsafeNativeMethods.ClearBreakpoint ClearBreakpoint;

        /*   40 :  RESERVED */
        private IntPtr reserved40;

        /*   41 : Set Field Access Watch */
        public UnsafeNativeMethods.SetFieldAccessWatch SetFieldAccessWatch;

        /*   42 : Clear Field Access Watch */
        public UnsafeNativeMethods.ClearFieldAccessWatch ClearFieldAccessWatch;

        /*   43 : Set Field Modification Watch */
        public UnsafeNativeMethods.SetFieldModificationWatch SetFieldModificationWatch;

        /*   44 : Clear Field Modification Watch */
        public UnsafeNativeMethods.ClearFieldModificationWatch ClearFieldModificationWatch;

        /*   45 : Is Modifiable Class */
        public UnsafeNativeMethods.IsModifiableClass IsModifiableClass;

        /*   46 : Allocate */
        public UnsafeNativeMethods.Allocate Allocate;

        /*   47 : Deallocate */
        public UnsafeNativeMethods.Deallocate Deallocate;

        /*   48 : Get Class Signature */
        public UnsafeNativeMethods.GetClassSignature GetClassSignature;

        /*   49 : Get Class Status */
        public UnsafeNativeMethods.GetClassStatus GetClassStatus;

        /*   50 : Get Source File Name */
        public UnsafeNativeMethods.GetSourceFileName GetSourceFileName;

        /*   51 : Get Class Modifiers */
        public UnsafeNativeMethods.GetClassModifiers GetClassModifiers;

        /*   52 : Get Class Methods */
        public UnsafeNativeMethods.GetClassMethods GetClassMethods;

        /*   53 : Get Class Fields */
        public UnsafeNativeMethods.GetClassFields GetClassFields;

        /*   54 : Get Implemented Interfaces */
        public UnsafeNativeMethods.GetImplementedInterfaces GetImplementedInterfaces;

        /*   55 : Is Interface */
        public UnsafeNativeMethods.IsInterface IsInterface;

        /*   56 : Is Array Class */
        public UnsafeNativeMethods.IsArrayClass IsArrayClass;

        /*   57 : Get Class Loader */
        public UnsafeNativeMethods.GetClassLoader GetClassLoader;

        /*   58 : Get Object Hash Code */
        public UnsafeNativeMethods.GetObjectHashCode GetObjectHashCode;

        /*   59 : Get Object Monitor Usage */
        public UnsafeNativeMethods.GetObjectMonitorUsage GetObjectMonitorUsage;

        /*   60 : Get Field Name (and Signature) */
        public UnsafeNativeMethods.GetFieldName GetFieldName;

        /*   61 : Get Field Declaring Class */
        public UnsafeNativeMethods.GetFieldDeclaringClass GetFieldDeclaringClass;

        /*   62 : Get Field Modifiers */
        public UnsafeNativeMethods.GetFieldModifiers GetFieldModifiers;

        /*   63 : Is Field Synthetic */
        public UnsafeNativeMethods.IsFieldSynthetic IsFieldSynthetic;

        /*   64 : Get Method Name (and Signature) */
        public UnsafeNativeMethods.GetMethodName GetMethodName;

        /*   65 : Get Method Declaring Class */
        public UnsafeNativeMethods.GetMethodDeclaringClass GetMethodDeclaringClass;

        /*   66 : Get Method Modifiers */
        public UnsafeNativeMethods.GetMethodModifiers GetMethodModifiers;

        /*   67 :  RESERVED */
        private IntPtr reserved67;

        /*   68 : Get Max Locals */
        public UnsafeNativeMethods.GetMaxLocals GetMaxLocals;

        /*   69 : Get Arguments Size */
        public UnsafeNativeMethods.GetArgumentsSize GetArgumentsSize;

        /*   70 : Get Line Number Table */
        public UnsafeNativeMethods.GetLineNumberTable GetLineNumberTable;

        /*   71 : Get Method Location */
        public UnsafeNativeMethods.GetMethodLocation GetMethodLocation;

        /*   72 : Get Local Variable Table */
        public UnsafeNativeMethods.GetLocalVariableTable GetLocalVariableTable;

        /*   73 : Set Native Method Prefix */
        public UnsafeNativeMethods.SetNativeMethodPrefix SetNativeMethodPrefix;

        /*   74 : Set Native Method Prefixes */
        public UnsafeNativeMethods.SetNativeMethodPrefixes SetNativeMethodPrefixes;

        /*   75 : Get Bytecodes */
        public UnsafeNativeMethods.GetBytecodes GetBytecodes;

        /*   76 : Is Method Native */
        public UnsafeNativeMethods.IsMethodNative IsMethodNative;

        /*   77 : Is Method Synthetic */
        public UnsafeNativeMethods.IsMethodSynthetic IsMethodSynthetic;

        /*   78 : Get Loaded Classes */
        public UnsafeNativeMethods.GetLoadedClasses GetLoadedClasses;

        /*   79 : Get Classloader Classes */
        public UnsafeNativeMethods.GetClassLoaderClasses GetClassLoaderClasses;

        /*   80 : Pop Frame */
        public UnsafeNativeMethods.PopFrame PopFrame;

        /*   81 : Force Early Return - Object */
        public UnsafeNativeMethods.ForceEarlyReturnObject ForceEarlyReturnObject;

        /*   82 : Force Early Return - Int */
        public UnsafeNativeMethods.ForceEarlyReturnInt ForceEarlyReturnInt;

        /*   83 : Force Early Return - Long */
        public UnsafeNativeMethods.ForceEarlyReturnLong ForceEarlyReturnLong;

        /*   84 : Force Early Return - Float */
        public UnsafeNativeMethods.ForceEarlyReturnFloat ForceEarlyReturnFloat;

        /*   85 : Force Early Return - Double */
        public UnsafeNativeMethods.ForceEarlyReturnDouble ForceEarlyReturnDouble;

        /*   86 : Force Early Return - Void */
        public UnsafeNativeMethods.ForceEarlyReturnVoid ForceEarlyReturnVoid;

        /*   87 : Redefine Classes */
        public UnsafeNativeMethods.RedefineClasses RedefineClasses;

        /*   88 : Get Version Number */
        public UnsafeNativeMethods.GetVersionNumber GetVersionNumber;

        /*   89 : Get Capabilities */
        public UnsafeNativeMethods.GetCapabilities GetCapabilities;

        /*   90 : Get Source Debug Extension */
        public UnsafeNativeMethods.GetSourceDebugExtension GetSourceDebugExtension;

        /*   91 : Is Method Obsolete */
        public UnsafeNativeMethods.IsMethodObsolete IsMethodObsolete;

        /*   92 : Suspend Thread List */
        public UnsafeNativeMethods.SuspendThreadList SuspendThreadList;

        /*   93 : Resume Thread List */
        public UnsafeNativeMethods.ResumeThreadList ResumeThreadList;

        /*   94 :  RESERVED */
        private IntPtr reserved94;

        /*   95 :  RESERVED */
        private IntPtr reserved95;

        /*   96 :  RESERVED */
        private IntPtr reserved96;

        /*   97 :  RESERVED */
        private IntPtr reserved97;

        /*   98 :  RESERVED */
        private IntPtr reserved98;

        /*   99 :  RESERVED */
        private IntPtr reserved99;

        /*   100 : Get All Stack Traces */
        public UnsafeNativeMethods.GetAllStackTraces GetAllStackTraces;

        /*   101 : Get Thread List Stack Traces */
        public UnsafeNativeMethods.GetThreadListStackTraces GetThreadListStackTraces;

        /*   102 : Get Thread Local Storage */
        public UnsafeNativeMethods.GetThreadLocalStorage GetThreadLocalStorage;

        /*   103 : Set Thread Local Storage */
        public UnsafeNativeMethods.SetThreadLocalStorage SetThreadLocalStorage;

        /*   104 : Get Stack Trace */
        public UnsafeNativeMethods.GetStackTrace GetStackTrace;

        /*   105 :  RESERVED */
        private IntPtr reserved105;

        /*   106 : Get Tag */
        public UnsafeNativeMethods.GetTag GetTag;

        /*   107 : Set Tag */
        public UnsafeNativeMethods.SetTag SetTag;

        /*   108 : Force Garbage Collection */
        public UnsafeNativeMethods.ForceGarbageCollection ForceGarbageCollection;

        /*   109 : Iterate Over Objects Reachable From Object */
        [Obsolete]
        private IntPtr IterateOverObjectsReachableFromObject;
        //public UnsafeNativeMethods.IterateOverObjectsReachableFromObject IterateOverObjectsReachableFromObject;

        /*   110 : Iterate Over Reachable Objects */
        [Obsolete]
        private IntPtr IterateOverReachableObjects;
        //public UnsafeNativeMethods.IterateOverReachableObjects IterateOverReachableObjects;

        /*   111 : Iterate Over Heap */
        [Obsolete]
        private IntPtr IterateOverHeap;
        //public UnsafeNativeMethods.IterateOverHeap IterateOverHeap;

        /*   112 : Iterate Over Instances Of Class */
        [Obsolete]
        private IntPtr IterateOverInstancesOfClass;
        //public UnsafeNativeMethods.IterateOverInstancesOfClass IterateOverInstancesOfClass;

        /*   113 :  RESERVED */
        private IntPtr reserved113;

        /*   114 : Get Objects With Tags */
        public UnsafeNativeMethods.GetObjectsWithTags GetObjectsWithTags;

        /*   115 : Follow References */
        public UnsafeNativeMethods.FollowReferences FollowReferences;

        /*   116 : Iterate Through Heap */
        public UnsafeNativeMethods.IterateThroughHeap IterateThroughHeap;

        /*   117 :  RESERVED */
        private IntPtr reserved117;

        /*   118 :  RESERVED */
        private IntPtr reserved118;

        /*   119 :  RESERVED */
        private IntPtr reserved119;

        /*   120 : Set JNI Function Table */
        public UnsafeNativeMethods.SetJNIFunctionTable SetJNIFunctionTable;

        /*   121 : Get JNI Function Table */
        public UnsafeNativeMethods.GetJNIFunctionTable GetJNIFunctionTable;

        /*   122 : Set Event Callbacks */
        public UnsafeNativeMethods.SetEventCallbacks SetEventCallbacks;

        /*   123 : Generate Events */
        public UnsafeNativeMethods.GenerateEvents GenerateEvents;

        /*   124 : Get Extension Functions */
        public UnsafeNativeMethods.GetExtensionFunctions GetExtensionFunctions;

        /*   125 : Get Extension Events */
        public UnsafeNativeMethods.GetExtensionEvents GetExtensionEvents;

        /*   126 : Set Extension Event Callback */
        public UnsafeNativeMethods.SetExtensionEventCallback SetExtensionEventCallback;

        /*   127 : Dispose Environment */
        public UnsafeNativeMethods.DisposeEnvironment DisposeEnvironment;

        /*   128 : Get Error Name */
        public UnsafeNativeMethods.GetErrorName GetErrorName;

        /*   129 : Get JLocation Format */
        public UnsafeNativeMethods.GetJLocationFormat GetJLocationFormat;

        /*   130 : Get System Properties */
        public UnsafeNativeMethods.GetSystemProperties GetSystemProperties;

        /*   131 : Get System Property */
        public UnsafeNativeMethods.GetSystemProperty GetSystemProperty;

        /*   132 : Set System Property */
        public UnsafeNativeMethods.SetSystemProperty SetSystemProperty;

        /*   133 : Get Phase */
        public UnsafeNativeMethods.GetPhase GetPhase;

        /*   134 : Get Current Thread CPU Timer Information */
        public UnsafeNativeMethods.GetCurrentThreadCpuTimerInfo GetCurrentThreadCpuTimerInfo;

        /*   135 : Get Current Thread CPU Time */
        public UnsafeNativeMethods.GetCurrentThreadCpuTime GetCurrentThreadCpuTime;

        /*   136 : Get Thread CPU Timer Information */
        public UnsafeNativeMethods.GetThreadCpuTimerInfo GetThreadCpuTimerInfo;

        /*   137 : Get Thread CPU Time */
        public UnsafeNativeMethods.GetThreadCpuTime GetThreadCpuTime;

        /*   138 : Get Timer Information */
        public UnsafeNativeMethods.GetTimerInfo GetTimerInfo;

        /*   139 : Get Time */
        public UnsafeNativeMethods.GetTime GetTime;

        /*   140 : Get Potential Capabilities */
        public UnsafeNativeMethods.GetPotentialCapabilities GetPotentialCapabilities;

        /*   141 :  RESERVED */
        private IntPtr reserved141;

        /*   142 : Add Capabilities */
        public UnsafeNativeMethods.AddCapabilities AddCapabilities;

        /*   143 : Relinquish Capabilities */
        public UnsafeNativeMethods.RelinquishCapabilities RelinquishCapabilities;

        /*   144 : Get Available Processors */
        public UnsafeNativeMethods.GetAvailableProcessors GetAvailableProcessors;

        /*   145 : Get Class Version Numbers */
        public UnsafeNativeMethods.GetClassVersionNumbers GetClassVersionNumbers;

        /*   146 : Get Constant Pool */
        public UnsafeNativeMethods.GetConstantPool GetConstantPool;

        /*   147 : Get Environment Local Storage */
        public UnsafeNativeMethods.GetEnvironmentLocalStorage GetEnvironmentLocalStorage;

        /*   148 : Set Environment Local Storage */
        public UnsafeNativeMethods.SetEnvironmentLocalStorage SetEnvironmentLocalStorage;

        /*   149 : Add To Bootstrap Class Loader Search */
        public UnsafeNativeMethods.AddToBootstrapClassLoaderSearch AddToBootstrapClassLoaderSearch;

        /*   150 : Set Verbose Flag */
        public UnsafeNativeMethods.SetVerboseFlag SetVerboseFlag;

        /*   151 : Add To System Class Loader Search */
        public UnsafeNativeMethods.AddToSystemClassLoaderSearch AddToSystemClassLoaderSearch;

        /*   152 : Retransform Classes */
        public UnsafeNativeMethods.RetransformClasses RetransformClasses;

        /*   153 : Get Owned Monitor Stack Depth Info */
        public UnsafeNativeMethods.GetOwnedMonitorStackDepthInfo GetOwnedMonitorStackDepthInfo;

        /*   154 : Get Object Size */
        public UnsafeNativeMethods.GetObjectSize GetObjectSize;

#if TOOLS_VERSION_1_2
        /*   155 : Get Local Instance */
        public UnsafeNativeMethods.GetLocalInstance GetLocalInstance;
#endif
    }
}
