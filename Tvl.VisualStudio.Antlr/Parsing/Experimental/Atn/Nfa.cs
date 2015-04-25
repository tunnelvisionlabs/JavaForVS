namespace Tvl.VisualStudio.Language.Parsing.Experimental.Atn
{
    using Tvl.VisualStudio.Language.Parsing.Collections;
    using ArgumentNullException = System.ArgumentNullException;
    using Contract = System.Diagnostics.Contracts.Contract;
    using Interval = Tvl.VisualStudio.Language.Parsing.Collections.Interval;

    public class Nfa
    {
        public readonly State StartState;
        public readonly State EndState;

        public Nfa(State startState, State endState)
        {
            Contract.Requires<ArgumentNullException>(startState != null, "startState");
            Contract.Requires<ArgumentNullException>(endState != null, "endState");

            StartState = startState;
            EndState = endState;
        }

        public static void BindRule(RuleBinding ruleBinding, Nfa body)
        {
            Contract.Requires<ArgumentNullException>(ruleBinding != null, "ruleBinding");
            Contract.Requires<ArgumentNullException>(body != null, "body");

            ruleBinding.StartState.AddTransition(new EpsilonTransition(body.StartState));
            body.EndState.AddTransition(new EpsilonTransition(ruleBinding.EndState));
        }

        public static Nfa Epsilon()
        {
            State state = new State();
            return new Nfa(state, state);
        }

        public static Nfa Closure(Nfa nfa)
        {
            State startState = new State();
            startState.AddTransition(new EpsilonTransition(nfa.StartState));

            State endState = new State();
            nfa.EndState.AddTransition(new EpsilonTransition(endState));

            endState.AddTransition(new EpsilonTransition(startState));

            return Optional(new Nfa(startState, endState));
        }

        public static Nfa Optional(Nfa nfa)
        {
            State startState = new State();
            startState.AddTransition(new EpsilonTransition(nfa.StartState));

            State endState = new State();
            nfa.EndState.AddTransition(new EpsilonTransition(endState));

            startState.AddTransition(new EpsilonTransition(endState));

            return new Nfa(startState, endState);
        }

        public static Nfa Match(int symbol)
        {
            State startState = new State();
            State endState = new State();
            startState.AddTransition(new MatchRangeTransition(endState, Interval.FromBounds(symbol, symbol)));
            return new Nfa(startState, endState);
        }

        public static Nfa MatchAny(params int[] symbols)
        {
            State startState = new State();
            State endState = new State();
            foreach (var symbol in symbols)
                startState.AddTransition(new MatchRangeTransition(endState, Interval.FromBounds(symbol, symbol)));
            return new Nfa(startState, endState);
        }

        public static Nfa MatchRange(Interval range)
        {
            State startState = new State();
            State endState = new State();
            startState.AddTransition(new MatchRangeTransition(endState, range));
            return new Nfa(startState, endState);
        }

        public static Nfa MatchComplement(params Interval[] excludedIntervals)
        {
            State startState = new State();
            State endState = new State();

            IntervalSet set = new IntervalSet(excludedIntervals).Complement(IntervalSet.CompleteInterval);
            foreach (var interval in set.Intervals)
                startState.AddTransition(new MatchRangeTransition(endState, interval));

            return new Nfa(startState, endState);
        }

        public static Nfa Rule(RuleBinding ruleBinding)
        {
            State startState = new State();
            State endState = new State();

            PushContextTransition push = new PushContextTransition(ruleBinding.StartState, startState.Id);
            PopContextTransition pop = new PopContextTransition(endState, startState.Id);

            startState.AddTransition(push);
            ruleBinding.EndState.AddTransition(pop);

            return new Nfa(startState, endState);
        }

        public static Nfa Choice(params Nfa[] alternatives)
        {
            State startState = new State();
            State endState = new State();
            foreach (var alternative in alternatives)
            {
                startState.AddTransition(new EpsilonTransition(alternative.StartState));
                alternative.EndState.AddTransition(new EpsilonTransition(endState));
            }

            return new Nfa(startState, endState);
        }

        public static Nfa Sequence(params Nfa[] elements)
        {
            State startState = new State();
            State endState = new State();

            for (int i = 0; i < elements.Length; i++)
            {
                State source = (i == 0) ? startState : elements[i - 1].EndState;
                State target = elements[i].StartState;
                source.AddTransition(new EpsilonTransition(target));
            }

            elements[elements.Length - 1].EndState.AddTransition(new EpsilonTransition(endState));

            return new Nfa(startState, endState);
        }
    }
}
