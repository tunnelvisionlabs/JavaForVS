namespace Tvl.Threading
{
    using System;
    using System.Diagnostics.Contracts;

    public class DeferredPeriodicOperation : IDisposable
    {
        private Action _action;
        private System.Timers.Timer _timer;
        private DateTimeOffset _lastDefer;
        private TimeSpan _deferPeriod;
        private TimeSpan _minimumPeriod;
        private bool _dirty;
        //private int _operating;

        public DeferredPeriodicOperation(Action action, TimeSpan deferPeriod, TimeSpan minimumPeriod, bool requiresInitialOperation)
        {
            Contract.Requires<ArgumentNullException>(action != null, "action");

            this._action = action;
            this._lastDefer = DateTimeOffset.MinValue;
            this._deferPeriod = deferPeriod;
            this._minimumPeriod = minimumPeriod;
            this._dirty = requiresInitialOperation;
        }

        public TimeSpan DeferPeriod
        {
            get
            {
                return _deferPeriod;
            }
            set
            {
                _deferPeriod = value;
            }
        }

        public TimeSpan MinimumPeriod
        {
            get
            {
                return _minimumPeriod;
            }
            set
            {
                _minimumPeriod = value;
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public void Defer()
        {
            _lastDefer = DateTimeOffset.Now;
        }

        public void MarkDirty()
        {
            Defer();
            this._dirty = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                _timer.Dispose();
                _action = null;
                _timer = null;
                IsDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
