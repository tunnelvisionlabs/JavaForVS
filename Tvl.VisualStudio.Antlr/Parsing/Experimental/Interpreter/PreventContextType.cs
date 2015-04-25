namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    public enum PreventContextType
    {
        /// <summary>
        /// Don't block any transitions.
        /// </summary>
        None,

        /// <summary>
        /// Block all push transitions.
        /// </summary>
        Push,

        /// <summary>
        /// Block all recursive push transitions.
        /// </summary>
        PushRecursive,

        /// <summary>
        /// Block all pop transitions.
        /// </summary>
        Pop,

        /// <summary>
        /// Block all recursive pop transitions.
        /// </summary>
        PopRecursive,
    }
}
