namespace Tvl.Java.DebugHost
{
    using System;
    using System.Collections.ObjectModel;

    public class DisposableObjectCollection<T> : Collection<T>
        where T : IDisposable
    {
    }
}
