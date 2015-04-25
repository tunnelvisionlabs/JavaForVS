namespace Tvl.Java.DebugInterface
{
    public enum InvokeOptions
    {
        None = 0,

        /// <summary>
        /// Perform method invocation with only the invoking thread resumed.
        /// </summary>
        SingleThreaded = 0x0001,

        /// <summary>
        /// Perform non-virtual method invocation.
        /// </summary>
        NonVirtual = 0x0002,
    }
}
