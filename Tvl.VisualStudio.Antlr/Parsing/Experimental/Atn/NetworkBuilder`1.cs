namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    public static class NetworkBuilder<T>
        where T : NetworkBuilder, new()
    {
        private static Network _network;
        private static readonly object _lock = new object();

        public static Network GetOrBuildNetwork()
        {
            if (_network != null)
                return _network;

            lock (_lock)
            {
                if (_network != null)
                    return _network;

                NetworkBuilder builder = new T();
                _network = builder.BuildNetwork();
                return _network;
            }
        }
    }
}
