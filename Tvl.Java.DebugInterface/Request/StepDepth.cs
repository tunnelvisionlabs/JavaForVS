namespace Tvl.Java.DebugInterface.Request
{
    public enum StepDepth
    {
        /// <summary>
        /// Step into any newly pushed frames.
        /// </summary>
        Into = 0,

        /// <summary>
        /// Step over any newly pushed frames.
        /// </summary>
        Over = 1,

        /// <summary>
        /// Step out of the current frame.
        /// </summary>
        Out = 2,
    }
}
