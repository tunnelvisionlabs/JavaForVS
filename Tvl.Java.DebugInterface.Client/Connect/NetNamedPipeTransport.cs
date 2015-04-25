namespace Tvl.Java.DebugInterface.Client.Connect
{
    using Tvl.Java.DebugInterface.Connect;

    internal sealed class NetNamedPipeTransport : ITransport
    {
        public string Name
        {
            get
            {
                return "WCF Named Pipe Transport";
            }
        }
    }
}
