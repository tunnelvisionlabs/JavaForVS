namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using IConnectionPoint = Microsoft.VisualStudio.OLE.Interop.IConnectionPoint;
    using IConnectionPointContainer = Microsoft.VisualStudio.OLE.Interop.IConnectionPointContainer;

    public static class IConnectionPointContainerExtensions
    {
        public static IDisposable Advise<TObject, TEventInterface>(this IConnectionPointContainer container, TObject @object)
            where TObject : class, TEventInterface
            where TEventInterface : class
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            Contract.Requires<ArgumentNullException>(@object != null, "object");
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            Guid eventGuid = typeof(TEventInterface).GUID;
            IConnectionPoint connectionPoint;
            container.FindConnectionPoint(eventGuid, out connectionPoint);
            if (connectionPoint == null)
                throw new ArgumentException();

            uint cookie;
            connectionPoint.Advise(@object, out cookie);
            return new ConnectionPointCookie(connectionPoint, cookie);
        }

        public static void Unadvise<TEventInterface>(this IConnectionPointContainer container, uint cookie)
            where TEventInterface : class
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            if (cookie == 0)
                return;

            Guid eventGuid = typeof(TEventInterface).GUID;
            IConnectionPoint connectionPoint;
            container.FindConnectionPoint(eventGuid, out connectionPoint);
            if (connectionPoint == null)
                throw new ArgumentException();

            connectionPoint.Unadvise(cookie);
        }

        private sealed class ConnectionPointCookie : IDisposable
        {
            private readonly WeakReference<IConnectionPoint> _connectionPoint;
            private uint _cookie;

            public ConnectionPointCookie(IConnectionPoint connectionPoint, uint cookie)
            {
                Contract.Requires(connectionPoint != null);

                _connectionPoint = new WeakReference<IConnectionPoint>(connectionPoint);
                _cookie = cookie;
            }

            public void Dispose()
            {
                if (_cookie != 0)
                {
                    IConnectionPoint connectionPoint = _connectionPoint.Target;
                    if (connectionPoint != null)
                    {
                        connectionPoint.Unadvise(_cookie);
                        _cookie = 0;
                    }
                }
            }
        }
    }
}
