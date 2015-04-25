namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;

    public class DeterministicTraceTransition
    {
        public readonly DeterministicTransition Transition;
        public readonly NetworkInterpreter Interpreter;
        public readonly int Symbol;
        public readonly int TokenIndex;

        public DeterministicTraceTransition(DeterministicTransition transition, int symbol, int symbolPosition, NetworkInterpreter interpreter)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");
            Contract.Requires<ArgumentNullException>(interpreter != null, "interpreter");

            Transition = transition;
            Interpreter = interpreter;
            Symbol = symbol;
            TokenIndex = symbolPosition;
        }

        public IToken Token
        {
            get
            {
                return Interpreter.Input.Get(TokenIndex);
            }
        }
    }
}
