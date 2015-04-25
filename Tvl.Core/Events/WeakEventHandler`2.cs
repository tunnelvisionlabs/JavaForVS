namespace Tvl.Events
{
    using System;
    using System.Diagnostics.Contracts;

    public class WeakEventHandler<T, TEventArgs> : IWeakEventHandler<TEventArgs>
        where T : class
        where TEventArgs : EventArgs
    {
        private delegate void OpenEventHandler(T @this, object sender, TEventArgs e);

        private WeakReference _target;
        private OpenEventHandler _openHandler;
        private EventHandler<TEventArgs> _handler;
        private Action<EventHandler<TEventArgs>> _unregister;

        public WeakEventHandler(EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> unregister)
        {
            Contract.Requires<ArgumentNullException>(handler != null, "handler");

            _target = new WeakReference(handler.Target);
            _openHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, handler.Method);
            _handler = Invoke;
            _unregister = unregister;
        }

        public EventHandler<TEventArgs> Handler
        {
            get
            {
                return _handler;
            }
        }

        public void Invoke(object sender, TEventArgs e)
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
