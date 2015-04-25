namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal abstract class EventRequest : Mirror, IEventRequest
    {
        private readonly List<Types.EventRequestModifier> _modifiers = new List<Types.EventRequestModifier>();
        private readonly Dictionary<object, object> _properties = new Dictionary<object, object>();

        private bool _enabled;
        private SuspendPolicy _suspendPolicy;

        private Types.RequestId _requestId;

        public EventRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public bool IsEnabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled == value)
                    return;

                if (value)
                {
                    Types.RequestId requestId;
                    DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SetEvent(out requestId, EventKind, (Types.SuspendPolicy)_suspendPolicy, _modifiers.ToArray()));
                    _requestId = requestId;
                    _enabled = true;
                }
                else
                {
                    DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.ClearEvent(EventKind, _requestId));
                    _requestId = default(Types.RequestId);
                    _enabled = false;
                }
            }
        }

        public SuspendPolicy SuspendPolicy
        {
            get
            {
                return _suspendPolicy;
            }

            set
            {
                if (_suspendPolicy == value)
                    return;

                bool wasEnabled = IsEnabled;

                IsEnabled = false;
                _suspendPolicy = value;
                IsEnabled = wasEnabled;
            }
        }

        internal abstract Types.EventKind EventKind
        {
            get;
        }

        internal Types.RequestId RequestId
        {
            get
            {
                return _requestId;
            }
        }

        protected List<Types.EventRequestModifier> Modifiers
        {
            get
            {
                return _modifiers;
            }
        }

        public object GetProperty(object key)
        {
            object value;
            _properties.TryGetValue(key, out value);
            return value;
        }

        public void PutProperty(object key, object value)
        {
            _properties[key] = value;
        }

        public void AddCountFilter(int count)
        {
            Modifiers.Add(Types.EventRequestModifier.CountFilter(count));
        }
    }
}
