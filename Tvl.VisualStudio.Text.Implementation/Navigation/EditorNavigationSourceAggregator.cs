namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using Tvl.Events;

    internal class EditorNavigationSourceAggregator : IEditorNavigationSourceAggregator
    {
        private readonly IEditorNavigationSource[] _sources;
        private int _isDisposing;

        public event EventHandler NavigationTargetsChanged;
        public event EventHandler Disposed;

        public EditorNavigationSourceAggregator(IEnumerable<IEditorNavigationSource> sources)
        {
            Contract.Requires<ArgumentNullException>(sources != null, "sources");

            this._sources = sources.ToArray();

            foreach (var source in this._sources)
            {
                source.NavigationTargetsChanged += WeakEvents.AsWeak(OnSourceNavigationTargetsChanged, eh => source.NavigationTargetsChanged -= eh);
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public bool IsDisposing
        {
            get
            {
                return this._isDisposing != 0;
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposing, 1, 0) != 0)
                throw new InvalidOperationException();

            try
            {
                Dispose(true);
                OnDisposed(EventArgs.Empty);
                GC.SuppressFinalize(this);
            }
            finally
            {
                _isDisposing = 0;
            }
        }

        public IEnumerable<IEditorNavigationType> GetNavigationTypes()
        {
            ThrowIfDisposed();
            return this._sources.SelectMany(source => source.GetNavigationTypes()).Distinct();
        }

        public IEnumerable<IEditorNavigationTarget> GetNavigationTargets()
        {
            ThrowIfDisposed();
            return this._sources.SelectMany(source => source.GetNavigationTargets());
        }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            var t = Disposed;
            if (t != null)
                t(this, e);
        }

        private void OnNavigationTargetsChanged(EventArgs e)
        {
            var t = NavigationTargetsChanged;
            if (t != null)
                t(this, e);
        }

        private void OnSourceNavigationTargetsChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            this.OnNavigationTargetsChanged(e);
        }

        private void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
