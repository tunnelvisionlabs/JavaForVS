namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.Shell.Interop;

    public sealed class DebugTargetInfo
    {
        public string Arguments
        {
            get;
            set;
        }

        public string CurrentDirectory
        {
            get;
            set;
        }

        public string Environment
        {
            get;
            set;
        }

        public string Executable
        {
            get;
            set;
        }

        public string Options
        {
            get;
            set;
        }

        public string PortName
        {
            get;
            set;
        }

        public string RemoteMachine
        {
            get;
            set;
        }

        public DEBUG_LAUNCH_OPERATION LaunchOperation
        {
            get;
            set;
        }

        // ok performance for this property to return an array
        [SuppressMessage("Microsoft.Performance", "CA1819")]
        public Guid[] DebugEngines
        {
            get;
            set;
        }

        public uint ProcessId
        {
            get;
            set;
        }

        public bool SendToOutputWindow
        {
            get;
            set;
        }

        public Guid PortSupplier
        {
            get;
            set;
        }

        public Guid ProcessLanguage
        {
            get;
            set;
        }

        // need the 'Flags' suffix to resemble the mirrored enum
        [SuppressMessage("Microsoft.Naming", "CA1726")]
        public __VSDBGLAUNCHFLAGS LaunchFlags
        {
            get;
            set;
        }

        public object Unknown
        {
            get;
            set;
        }
    }
}
