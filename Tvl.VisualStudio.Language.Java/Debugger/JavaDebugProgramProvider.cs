namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    [Guid(JavaDebuggerConstants.JavaProgramProviderGuidString)]
    public class JavaDebugProgramProvider : IDebugProgramProvider2
    {
        private readonly HashSet<ProviderRequestData> _subscribers = new HashSet<ProviderRequestData>();
        private CultureInfo _culture;

        public JavaDebugProgramProvider()
        {
            _culture = CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Retrieves a list of running programs from a specified process.
        /// </summary>
        /// <param name="flags">A combination of flags from the <see cref="PROVIDER_FLAGS"/> enumeration.</param>
        /// <param name="port">The port the calling process is running on.</param>
        /// <param name="processId">An <see cref="AD_PROCESS_ID"/> structure holding the ID of the process that contains the program in question.</param>
        /// <param name="engineFilter">An array of GUIDs for debug engines assigned to debug this process (these will be used to filter the programs that are actually returned based on what the supplied engines support; if no engines are specified, then all programs will be returned).</param>
        /// <param name="process">A <see cref="PROVIDER_PROCESS_DATA"/> structure that is filled in with the requested information.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// This method is normally called by a process to obtain a list of programs running in that process. The returned information is a list of <see cref="IDebugProgramNode2"/> objects.
        /// </remarks>
        public int GetProviderProcessData(enum_PROVIDER_FLAGS flags, IDebugDefaultPort2 port, AD_PROCESS_ID processId, CONST_GUID_ARRAY engineFilter, PROVIDER_PROCESS_DATA[] process)
        {
            /* The following flags are typical for this call:
             *
             *   PFLAG_REMOTE_PORT:             Caller is running on remote machine.
             *   PFLAG_DEBUGGEE:                Caller is currently being debugged (additional information about marshalling is returned for each node).
             *   PFLAG_ATTACHED_TO_DEBUGGEE:    Caller was attached to but not launched by the debugger.
             *   PFLAG_GET_PROGRAM_NODES:       Caller is asking for a list of program nodes to be returned.
             */

#if false
            // make sure this request is relevant to this debug engine
            if (!engineFilter.AsEnumerable().Contains(JavaDebuggerConstants.JavaDebugEngineGuid))
                return VSConstants.E_INVALIDARG;
#endif

            return VSConstants.S_FALSE;
        }

        /// <summary>
        /// Retrieves the program node for a specific program.
        /// </summary>
        /// <param name="flags">A combination of flags from the <see cref="PROVIDER_FLAGS"/> enumeration.</param>
        /// <param name="port">The port the calling process is running on.</param>
        /// <param name="processId">An <see cref="AD_PROCESS_ID"/> structure holding the ID of the process that contains the program in question.</param>
        /// <param name="guidEngine">GUID of the debug engine that the program is attached to (if any).</param>
        /// <param name="programId">ID of the program for which to get the program node.</param>
        /// <param name="programNode">An <see cref="IDebugProgramNode2"/> object representing the requested program node.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        public int GetProviderProgramNode(enum_PROVIDER_FLAGS flags, IDebugDefaultPort2 port, AD_PROCESS_ID processId, ref Guid guidEngine, ulong programId, out IDebugProgramNode2 programNode)
        {
            /* The following flags are typical for this call:
             *
             *   PFLAG_REMOTE_PORT:             Caller is running on remote machine.
             *   PFLAG_DEBUGGEE:                Caller is currently being debugged (additional information about marshalling will be returned for each node).
             *   PFLAG_ATTACHED_TO_DEBUGGEE:    Caller was attached to but not launched by the debugger.
             */

            programNode = null;
            return VSConstants.E_NOTIMPL;
        }

        public int SetLocale(ushort languageId)
        {
            _culture = CultureInfo.GetCultureInfo(languageId) ?? CultureInfo.CurrentUICulture;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Allows the process to be notified of port events.
        /// </summary>
        /// <param name="flags">A combination of flags from the <see cref="PROVIDER_FLAGS"/> enumeration.</param>
        /// <param name="port">The port the calling process is running on.</param>
        /// <param name="processId">An <see cref="AD_PROCESS_ID"/> structure holding the ID of the process that contains the program in question.</param>
        /// <param name="engineFilter">An array of GUIDs of debug engines associated with the process.</param>
        /// <param name="guidLaunchingEngine">GUID of the debug engine that launched this process (if any).</param>
        /// <param name="eventCallback">An <see cref="IDebugPortNotify2"/> object that receives the event notifications.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// When a caller wants to remove an event handler established with a previous call to this method,
        /// the caller passes the same parameters as it did the first time but leaves off the PFLAG_REASON_WATCH flag.
        /// </remarks>
        public int WatchForProviderEvents(enum_PROVIDER_FLAGS flags, IDebugDefaultPort2 port, AD_PROCESS_ID processId, CONST_GUID_ARRAY engineFilter, ref Guid guidLaunchingEngine, IDebugPortNotify2 eventCallback)
        {
            /* The following flags are typical for this call:
             *
             *   PFLAG_REMOTE_PORT:             Caller is running on remote machine.
             *   PFLAG_DEBUGGEE:                Caller is currently being debugged (additional information about marshalling is returned for each node).
             *   PFLAG_ATTACHED_TO_DEBUGGEE:    Caller was attached to but not launched by the debugger.
             *   PFLAG_REASON_WATCH:            Caller wants to watch for events. If this flag is not set, then the callback event is removed and the caller no longer receives notifications.
             */

            // make sure this request is relevant to this debug engine
            if (!engineFilter.AsEnumerable().Contains(JavaDebuggerConstants.JavaDebugEngineGuid))
                return VSConstants.S_OK;

            ProviderRequestData requestData = new ProviderRequestData(flags, port, processId, guidLaunchingEngine, eventCallback);
            if ((flags & enum_PROVIDER_FLAGS.PFLAG_REASON_WATCH) == 0)
            {
                _subscribers.Remove(requestData);
                return VSConstants.S_OK;
            }

            _subscribers.Add(requestData);
            eventCallback.AddProgramNode(new JavaDebugProgram(port.GetProcess(processId)));
            return VSConstants.S_OK;
        }

        private sealed class ProviderRequestData : IEquatable<ProviderRequestData>
        {
            private const enum_PROVIDER_FLAGS FlagsMask = ~enum_PROVIDER_FLAGS.PFLAG_REASON_WATCH;

            private readonly enum_PROVIDER_FLAGS _flags;
            private readonly Guid _port;
            private readonly uint _processId;
            private readonly Guid _launchingEngine;
            private readonly IDebugPortNotify2 _callback;

            public ProviderRequestData(enum_PROVIDER_FLAGS flags, IDebugDefaultPort2 port, AD_PROCESS_ID processId, Guid launchingEngine, IDebugPortNotify2 callback)
            {
                _flags = flags;
                _port = port != null ? port.GetPortId() : Guid.Empty;
                _processId = processId.dwProcessId;
                _launchingEngine = launchingEngine;
                _callback = callback;
            }

            public bool Equals(ProviderRequestData other)
            {
                if (other == null)
                    return false;

                if ((_flags & FlagsMask) != (other._flags & FlagsMask))
                    return false;

                return _port == other._port
                    && _processId == other._processId
                    && _launchingEngine == other._launchingEngine;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as ProviderRequestData);
            }

            public override int GetHashCode()
            {
                return (_flags & FlagsMask).GetHashCode()
                    ^ _port.GetHashCode()
                    ^ _processId.GetHashCode()
                    ^ _launchingEngine.GetHashCode();
            }
        }
    }
}
