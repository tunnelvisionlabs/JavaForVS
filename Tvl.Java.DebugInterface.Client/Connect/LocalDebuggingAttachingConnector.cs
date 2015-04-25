namespace Tvl.Java.DebugInterface.Client.Connect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tvl.Java.DebugInterface.Connect;

    public sealed class LocalDebuggingAttachingConnector : IAttachingConnector
    {
        private readonly ITransport _transport = new NetNamedPipeTransport();

        private readonly Dictionary<string, IConnectorArgument> _defaultArguments =
            new Dictionary<string, IConnectorArgument>();

        public LocalDebuggingAttachingConnector()
        {
            _defaultArguments.Add("pid", new ConnectorIntegerArgument("pid", "Process ID", "The system process ID of the process to attach to.", true, 0, 0, int.MaxValue));
            _defaultArguments.Add("sourcePaths", new ConnectorStringArgument("sourcePaths", "Source Paths", "A semicolon-delimited list of root paths containing source code for the program.", true, string.Empty));
        }

        public event EventHandler AttachComplete;

        #region IAttachingConnector Members

        public IVirtualMachine Attach(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments)
        {
            var pid = (IConnectorIntegerArgument)arguments.Single(i => i.Key == "pid").Value;
            var sourcePaths = (IConnectorStringArgument)arguments.SingleOrDefault(i => i.Key == "sourcePaths").Value;

            VirtualMachine virtualMachine = VirtualMachine.BeginAttachToProcess(pid.Value, sourcePaths.StringValue.Split(';'));
            virtualMachine.AttachComplete += OnAttachComplete;
            return virtualMachine;
        }

        #endregion

        #region IConnector Members

        public IDictionary<string, IConnectorArgument> DefaultArguments
        {
            get
            {
                return _defaultArguments;
            }
        }

        public string Description
        {
            get
            {
                return "Local debugging attaching connector";
            }
        }

        public string Name
        {
            get
            {
                return "Local debugging attaching connector";
            }
        }

        public ITransport Transport
        {
            get
            {
                return _transport;
            }
        }

        #endregion

        private void OnAttachComplete(object machine, EventArgs e)
        {
            var t = AttachComplete;
            if (t != null)
                t(machine, e);
        }
    }
}
