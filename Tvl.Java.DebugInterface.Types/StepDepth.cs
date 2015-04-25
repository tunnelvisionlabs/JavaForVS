namespace Tvl.Java.DebugInterface.Types
{
    public enum StepDepth
    {
        /// <summary>
        /// Step into any method calls that occur before the end of the step.
        /// </summary>
        Into = 0,

        /// <summary>
        /// Step into any method calls that occur before the end of the step.
        /// </summary>
        Over = 1,

        /// <summary>
        /// Step out of the current method.
        /// </summary>
        Out = 2,
    }
}
