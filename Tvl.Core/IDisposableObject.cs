namespace Tvl
{
    using System;

    public interface IDisposableObject : IDisposable
    {
        event EventHandler Disposed;

        bool IsDisposed
        {
            get;
        }

        bool IsDisposing
        {
            get;
        }
    }
}
