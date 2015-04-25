namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ProvideDebugEngineAttribute : DebuggerRegistrationAttribute
    {
        public ProvideDebugEngineAttribute(Type debugEngine, string debugEngineName)
            : base(MetricTypes.DebugEngine, debugEngine.GUID)
        {
            DebugEngine = debugEngine;
            ClassGuid = debugEngine.GUID;
            Name = debugEngineName;
        }

        public Type DebugEngine
        {
            get;
            private set;
        }

        [DebugMetric("CLSID")]
        public Guid ClassGuid
        {
            get;
            private set;
        }

        [DebugMetric("Name")]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Set to true to indicate support for address breakpoints.
        /// </summary>
        [DebugMetric("AddressBP")]
        public bool AddressBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true in order to always load the debug engine locally.
        /// </summary>
        [DebugMetric("AlwaysLoadLocal")]
        public bool AlwaysLoadLocal
        {
            get;
            set;
        }

        //// not used
        //public bool LoadInDebuggeeSession
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Set to true to indicate that the debug engine will always be loaded with or by the program being debugged.
        /// </summary>
        [DebugMetric("LoadedByDebuggee")]
        public bool LoadedByDebuggee
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for attachment to existing programs.
        /// </summary>
        [DebugMetric("Attach")]
        public bool Attach
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for call stack breakpoints.
        /// </summary>
        [DebugMetric("CallStackBP")]
        public bool CallStackBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for the setting of conditional breakpoints.
        /// </summary>
        [DebugMetric("ConditionalBP")]
        public bool ConditionalBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for the setting of breakpoints on changes in data.
        /// </summary>
        [DebugMetric("DataBP")]
        public bool DataBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for the production of a disassembly listing.
        /// </summary>
        [DebugMetric("Disassembly")]
        public bool Disassembly
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for dump writing (the dumping of memory to an output device).
        /// </summary>
        [DebugMetric("DumpWriting")]
        public bool DumpWriting
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for Edit and Continue.
        /// </summary>
        /// <remarks>A custom debug engine should never set this or should always set it to false.</remarks>
        [DebugMetric("ENC")]
        public bool EditAndContinue
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for exceptions.
        /// </summary>
        [DebugMetric("Exceptions")]
        public bool Exceptions
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to exclude this debug engine from the Attach to Process manual selection list
        /// </summary>
        [DebugMetric("ExcludeManualSelect")]
        public bool ExcludeManualSelect
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for named breakpoints (breakpoints that break when a certain function name is called).
        /// </summary>
        [DebugMetric("FunctionBP")]
        public bool FunctionBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for the setting of "hit point" breakpoints (breakpoints that are triggered only after being hit a certain number of times).
        /// </summary>
        [DebugMetric("HitCountBP")]
        public bool HitCountBreakpoints
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for just-in-time debugging (the debugger is launched when an exception occurs in a running process).
        /// </summary>
        [DebugMetric("JITDebug")]
        public bool JustInTimeDebugging
        {
            get;
            set;
        }

        //// not used
        //public object Memory
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Set this to the CLSID of the port supplier if one is implemented.
        /// </summary>
        public string PortSupplier
        {
            get
            {
                return PortSuppliers[0];
            }

            set
            {
                PortSuppliers = new string[] { value };
            }
        }

        [DebugMetric("PortSupplier")]
        public Guid[] PortSupplierGuids
        {
            get;
            set;
        }

        public string[] PortSuppliers
        {
            get
            {
                return PortSupplierGuids.Select(g => g.ToString("B")).ToArray();
            }

            set
            {
                PortSupplierGuids = value.Select(s => new Guid(s)).ToArray();
            }
        }

        //// not used
        //public object Registers
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Set to true to indicate support for setting the next statement (which skips execution of intermediate statements).
        /// </summary>
        [DebugMetric("SetNextStatement")]
        public bool SetNextStatement
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate support for suspending thread execution.
        /// </summary>
        [DebugMetric("SuspendThread")]
        public bool SuspendThread
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true to indicate that the user should be notified if there are no symbols.
        /// </summary>
        [DebugMetric("WarnIfNoSymbols")]
        public bool WarnIfNoSymbols
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to the CLSID of the program provider.
        /// </summary>
        [DebugMetric("ProgramProvider")]
        public string ProgramProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the program provider should always be loaded locally.
        /// </summary>
        [DebugMetric("AlwaysLoadProgramProviderLocal")]
        public bool AlwaysLoadProgramProviderLocal
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the debug engine will watch for process events instead of the program provider.
        /// </summary>
        [DebugMetric("EngineCanWatchProcess")]
        public bool EngineCanWatchProcess
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate support for remote debugging.
        /// </summary>
        [DebugMetric("RemoteDebugging")]
        public bool RemoteDebugging
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the Edit and Continue Manager should use the debug engine's encbuild.dll to build for Edit and Continue.
        /// </summary>
        /// <remarks>A custom debug engine should never set this or should always set it to false.</remarks>
        [DebugMetric("EncUseNativeBuilder")]
        public bool EditAndContinueUseNativeBuilder
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the debug engine should be loaded in the debuggee process
        /// under WOW when debugging a 64-bit process; otherwise, the debug engine will be loaded in the
        /// Visual Studio process (which is running under WOW64).
        /// </summary>
        [DebugMetric("LoadUnderWOW64")]
        public bool LoadUnderWOW64
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the program provider should be loaded in the debuggee process when debugging
        /// a 64-bit process under WOW; otherwise, it will be loaded in the Visual Studio process.
        /// </summary>
        [DebugMetric("LoadProgramProviderUnderWOW64")]
        public bool LoadProgramProviderUnderWOW64
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to true to indicate that the process should stop if an unhandled exception is thrown across managed/unmanaged code boundaries.
        /// </summary>
        [DebugMetric("StopOnExceptionCrossingManagedBoundary")]
        public bool StopOnExceptionCrossingManagedBoundary
        {
            get;
            set;
        }

        /// <summary>
        /// Set this to a priority for automatic selection of the debug engine (higher values equals higher priority).
        /// </summary>
        [DebugMetric("AutoSelectPriority")]
        public int AutoSelectPriority
        {
            get;
            set;
        }

        /// <summary>
        /// Registry key containing entries that specify GUIDs for debug engines to be ignored in automatic selection.
        /// These entries are a number (0, 1, 2, and so on) with a GUID expressed as a string.
        /// </summary>
        [DebugMetric("AutoSelectIncompatibleList")]
        public Guid[] AutoSelectIncompatibleListGuids
        {
            get;
            set;
        }

        public string[] AutoSelectIncompatibleList
        {
            get
            {
                return AutoSelectIncompatibleListGuids.Select(g => g.ToString("B")).ToArray();
            }

            set
            {
                AutoSelectIncompatibleListGuids = value.Select(s => new Guid(s)).ToArray();
            }
        }

        /// <summary>
        /// Registry key containing entries that specify GUIDs for debug engines that are incompatible with this debug engine.
        /// </summary>
        [DebugMetric("IncompatibleList")]
        public Guid[] IncompatibleListGuids
        {
            get;
            set;
        }

        public string[] IncompatibleList
        {
            get
            {
                return IncompatibleListGuids.Select(g => g.ToString("B")).ToArray();
            }

            set
            {
                IncompatibleListGuids = value.Select(s => new Guid(s)).ToArray();
            }
        }

        /// <summary>
        /// Set this to true to indicate that just-in-time optimizations (for managed code) should be disabled during debugging.
        /// </summary>
        [DebugMetric("DisableJITOptimization")]
        public bool DisableJitOptimization
        {
            get;
            set;
        }

        public override void Register(RegistrationContext context)
        {
            if (AutoSelectIncompatibleListGuids != null)
                throw new NotImplementedException("registry has name/guid pairs for this list?");
            if (IncompatibleListGuids != null)
                throw new NotImplementedException("registry has name/guid pairs for this list?");

            base.Register(context);
        }
    }
}
