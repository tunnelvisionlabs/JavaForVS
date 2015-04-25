namespace Tvl.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public static class WeakEvents
    {
        public static EventHandler AsWeak(EventHandler handler, Action<EventHandler> unregister)
        {
            Contract.Requires<ArgumentNullException>(handler != null, "handler");
            Contract.Ensures(Contract.Result<EventHandler>() != null);

            if (handler.Method.IsStatic)
                return handler;

            Type t = typeof(WeakEventHandler<>).MakeGenericType(handler.Method.DeclaringType);
            ConstructorInfo ctor = t.GetConstructor(new Type[] { typeof(EventHandler), typeof(Action<EventHandler>) });
            IWeakEventHandler weakHandler = (IWeakEventHandler)ctor.Invoke(new object[] { handler, unregister });
            return weakHandler.Handler;
        }

        public static EventHandler<TEventArgs> AsWeak<TEventArgs>(this EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> unregister)
            where TEventArgs : EventArgs
        {
            Contract.Requires<ArgumentNullException>(handler != null, "handler");
            Contract.Ensures(Contract.Result<EventHandler<TEventArgs>>() != null);

            if (handler.Method.IsStatic)
                return handler;

            Type t = typeof(WeakEventHandler<,>).MakeGenericType(handler.Method.DeclaringType, typeof(TEventArgs));
            ConstructorInfo ctor = t.GetConstructor(new Type[] { typeof(EventHandler<TEventArgs>), typeof(Action<EventHandler<TEventArgs>>) });
            IWeakEventHandler<TEventArgs> weakHandler = (IWeakEventHandler<TEventArgs>)ctor.Invoke(new object[] { handler, unregister });
            return weakHandler.Handler;
        }
    }
}
