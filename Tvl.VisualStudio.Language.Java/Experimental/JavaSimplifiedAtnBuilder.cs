namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using Tvl.VisualStudio.Language.Parsing.Collections;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    internal class JavaSimplifiedAtnBuilder : JavaAtnBuilder
    {
        private static readonly int[] BinaryOperators =
            {
                Java2Lexer.BARBAR,
                Java2Lexer.AMPAMP,
                Java2Lexer.BAR,
                Java2Lexer.CARET,
                Java2Lexer.AMP,
                Java2Lexer.EQEQ,
                Java2Lexer.BANGEQ,
                Java2Lexer.PLUS,
                Java2Lexer.SUB,
                Java2Lexer.STAR,
                Java2Lexer.SLASH,
                Java2Lexer.PERCENT,
            };

        private IntervalSet _definitionSourceSet;
        private IntervalSet _referenceSourceSet;
        private IntervalSet _definitionOnlySourceSet;
        private IntervalSet _referenceOnlySourceSet;

        private IntervalSet _definitionFollowSet;
        private IntervalSet _referenceFollowSet;
        private IntervalSet _definitionOnlyFollowSet;
        private IntervalSet _referenceOnlyFollowSet;

        public IntervalSet DefinitionSourceSet
        {
            get
            {
                return _definitionSourceSet;
            }
        }

        public IntervalSet ReferenceSourceSet
        {
            get
            {
                return _referenceSourceSet;
            }
        }

        public IntervalSet DefinitionOnlySourceSet
        {
            get
            {
                return _definitionOnlySourceSet;
            }
        }

        public IntervalSet ReferenceOnlySourceSet
        {
            get
            {
                return _referenceOnlySourceSet;
            }
        }

        public IntervalSet DefinitionFollowSet
        {
            get
            {
                return _definitionFollowSet;
            }
        }

        public IntervalSet ReferenceFollowSet
        {
            get
            {
                return _referenceFollowSet;
            }
        }

        public IntervalSet DefinitionOnlyFollowSet
        {
            get
            {
                return _definitionOnlyFollowSet;
            }
        }

        public IntervalSet ReferenceOnlyFollowSet
        {
            get
            {
                return _referenceOnlyFollowSet;
            }
        }

        protected override void BindRulesImpl()
        {
            base.BindRulesImpl();

            _definitionSourceSet = Bindings.SymbolDefinitionIdentifier.StartState.GetSourceSet();
            _referenceSourceSet = Bindings.SymbolReferenceIdentifier.StartState.GetSourceSet();
            _definitionOnlySourceSet = _definitionSourceSet.Except(_referenceSourceSet);
            _referenceOnlySourceSet = _referenceSourceSet.Except(_definitionSourceSet);

            _definitionFollowSet = Bindings.SymbolDefinitionIdentifier.EndState.GetFollowSet();
            _referenceFollowSet = Bindings.SymbolReferenceIdentifier.EndState.GetFollowSet();
            _definitionOnlyFollowSet = _definitionFollowSet.Except(_referenceFollowSet);
            _referenceOnlyFollowSet = _referenceFollowSet.Except(_definitionFollowSet);
        }

        protected override Nfa ConditionalOrExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.UnaryExpression),
                Nfa.Closure(
                    Nfa.Choice(
                        Nfa.Sequence(
                            Nfa.Choice(
                                Nfa.MatchAny(BinaryOperators),
                                Nfa.Rule(Bindings.ShiftOp),
                                Nfa.Rule(Bindings.RelationalOp)),
                            Nfa.Rule(Bindings.UnaryExpression)),
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.INSTANCEOF),
                            Nfa.Rule(Bindings.Type)))));
        }

        protected override Nfa ConditionalAndExpression()
        {
            return null;
        }

        protected override Nfa InclusiveOrExpression()
        {
            return null;
        }

        protected override Nfa ExclusiveOrExpression()
        {
            return null;
        }

        protected override Nfa AndExpression()
        {
            return null;
        }

        protected override Nfa EqualityExpression()
        {
            return null;
        }

        protected override Nfa InstanceOfExpression()
        {
            return null;
        }

        protected override Nfa RelationalExpression()
        {
            return null;
        }

        protected override Nfa ShiftExpression()
        {
            return null;
        }

        protected override Nfa AdditiveExpression()
        {
            return null;
        }

        protected override Nfa MultiplicativeExpression()
        {
            return null;
        }
    }
}
