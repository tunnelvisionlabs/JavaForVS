namespace Tvl.Events
{
    using System;
    using System.Diagnostics.Contracts;

    public class WeakEventHandler<T> : IWeakEventHandler
        where T : class
    {
        private delegate void OpenEventHandler(T @this, object sender, EventArgs e);

        private WeakReference _target;
        private OpenEventHandler _openHandler;
        private EventHandler _handler;
        private Action<EventHandler> _unregister;

        public WeakEventHandler(EventHandler handler, Action<EventHandler> unregister)
        {
            Contract.Requires<ArgumentNullException>(handler != null, "handler");

            _target = new WeakReference(handler.Target);
            _openHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, handler.Method);
            _handler = Invoke;
            _unregister = unregister;
        }

        public EventHandler Handler
        {
            get
            {
                return _handler;
            }
        }

        public void Invoke(object sender, EventArgs e)
        {
            T target = (T)_target.Target;

            if (target != null)
            {
                _openHandler.Invoke(target, sender, e);
            }
            else if (this._unregister != null)
            {
                _unregister(_handler);
                _unregister = null;
            }
        }
    }
}
