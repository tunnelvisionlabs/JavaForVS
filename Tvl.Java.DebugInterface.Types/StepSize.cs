namespace Tvl.Java.DebugInterface.Types
{
    public enum StepSize
    {
        /// <summary>
        /// Step by the minimum possible amount (often a bytecode instruction).
        /// </summary>
        Instruction = 0,

        /// <summary>
        /// Step to the next source line unless there is no line number information in which case a <see cref="Instruction"/> step is done instead.
        /// </summary>
        Line = 1,

        Statement = 2,
    }
}
