namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Marshal = System.Runtime.InteropServices.Marshal;

    public static class IVsDebuggerExtensions
    {
        public static DBGMODE GetMode(this IVsDebugger debugger)
        {
            DBGMODE[] mode = new DBGMODE[1];
            if (ErrorHandler.Failed(debugger.GetMode(mode)))
                return DBGMODE.DBGMODE_Design;

            return mode[0];
        }

        public static DBGMODE GetInternalDebugMode(this IVsDebugger2 debugger)
        {
            DBGMODE[] mode = new DBGMODE[1];
            if (ErrorHandler.Failed(debugger.GetInternalDebugMode(mode)))
                return DBGMODE.DBGMODE_Design;

            return mode[0];
        }

        public static int LaunchDebugTargets(this IVsDebugger2 debugger, params DebugTargetInfo[] targets)
        {
            VsDebugTargetInfo2[] vstargets = new VsDebugTargetInfo2[targets.Length];

            try
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    vstargets[i].bstrArg = targets[i].Arguments;
                    vstargets[i].bstrCurDir = targets[i].CurrentDirectory;
                    vstargets[i].bstrEnv = GetEnvironmentString(targets[i].Environment);
                    vstargets[i].bstrExe = targets[i].Executable;
                    vstargets[i].bstrOptions = targets[i].Options;
                    vstargets[i].bstrPortName = targets[i].PortName;
                    vstargets[i].bstrRemoteMachine = targets[i].RemoteMachine;
                    vstargets[i].cbSize = (uint)Marshal.SizeOf(typeof(VsDebugTargetInfo2));
                    vstargets[i].dlo = (uint)targets[i].LaunchOperation;
                    vstargets[i].dwProcessId = targets[i].ProcessId;
                    vstargets[i].dwReserved = 0;
                    vstargets[i].fSendToOutputWindow = targets[i].SendToOutputWindow ? 1 : 0;
                    vstargets[i].guidLaunchDebugEngine = Guid.Empty;
                    vstargets[i].guidPortSupplier = targets[i].PortSupplier;
                    vstargets[i].guidProcessLanguage = targets[i].ProcessLanguage;
                    //vstargets[i].hStdError = targets[i].StdError;
                    //vstargets[i].hStdInput = targets[i].StdInput;
                    //vstargets[i].hStdOutput = targets[i].StdOutput;
                    vstargets[i].LaunchFlags = (uint)targets[i].LaunchFlags;
                    vstargets[i].pUnknown = null;

                    vstargets[i].dwDebugEngineCount = (uint)targets[i].DebugEngines.Length;
                    vstargets[i].pDebugEngines = Marshal.AllocHGlobal(targets[i].DebugEngines.Length * Marshal.SizeOf(typeof(Guid)));
                    for (int j = 0; j < targets[i].DebugEngines.Length; j++)
                    {
                        Marshal.StructureToPtr(targets[i].DebugEngines[j], new IntPtr(vstargets[i].pDebugEngines.ToInt64() + j * Marshal.SizeOf(typeof(Guid))), false);
                    }
                }

                return debugger.LaunchDebugTargets(vstargets);
            }
            finally
            {
                for (int i = 0; i < vstargets.Length; i++)
                {
                    if (vstargets[i].pDebugEngines != IntPtr.Zero)
                        Marshal.FreeHGlobal(vstargets[i].pDebugEngines);
                }
            }
        }

        private static string GetEnvironmentString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                return null;

            StringBuilder builder = new StringBuilder();
            foreach (var pair in dictionary)
            {
                builder.Append(pair.Key);
                builder.Append('=');
                builder.Append(pair.Value);
                builder.Append('\0');
            }

            builder.Append('\0');
            return builder.ToString();
        }

        public static int LaunchDebugTargets(this IVsDebugger2 debugger, params VsDebugTargetInfo2[] targets)
        {
            IntPtr ptr = IntPtr.Zero;
            int marshalledCount = 0;

            try
            {
                ptr = Marshal.AllocHGlobal(targets.Length * Marshal.SizeOf(typeof(VsDebugTargetInfo2)));
                for (int i = 0; i < targets.Length; i++)
                {
                    IntPtr current = new IntPtr(ptr.ToInt64() + i * Marshal.SizeOf(typeof(VsDebugTargetInfo2)));
                    Marshal.StructureToPtr(targets[i], current, false);
                    marshalledCount++;
                }

                return debugger.LaunchDebugTargets2((uint)targets.Length, ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    for (int i = 0; i < marshalledCount; i++)
                    {
                        IntPtr current = new IntPtr(ptr.ToInt64() + i * Marshal.SizeOf(typeof(VsDebugTargetInfo2)));
                        Marshal.DestroyStructure(current, typeof(VsDebugTargetInfo2));
                    }

                    Marshal.FreeHGlobal(ptr);
                    ptr = IntPtr.Zero;
                }
            }
        }
    }
}
