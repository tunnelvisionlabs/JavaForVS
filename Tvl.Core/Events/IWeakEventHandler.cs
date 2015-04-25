namespace Tvl.Events
{
    using System;

    public interface IWeakEventHandler
    {
        EventHandler Handler
        {
            get;
        }
    }
}
