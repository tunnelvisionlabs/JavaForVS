namespace Tvl.VisualStudio.Language.Parsing.Experimental.Interpreter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    public class InterpretTrace : IEquatable<InterpretTrace>
    {
        private static readonly LinkedList<InterpretTraceTransition> EmptyTransitions = new LinkedList<InterpretTraceTransition>();

        public readonly ContextFrame StartContext;
        public readonly LinkedList<InterpretTraceTransition> Transitions = new LinkedList<InterpretTraceTransition>();
        public readonly ContextFrame EndContext;
        public readonly bool BoundedStart;
        public readonly bool BoundedEnd;

        public InterpretTrace(ContextFrame startContext, ContextFrame endContext)
            : this(startContext, endContext, EmptyTransitions, false, false, false)
        {
        }

        private InterpretTrace(ContextFrame startContext, ContextFrame endContext, LinkedList<InterpretTraceTransition> transitions, bool boundedStart, bool boundedEnd, bool copyTransitionsByReference)
        {
            StartContext = startContext;
            EndContext = endContext;
            if (copyTransitionsByReference)
                Transitions = transitions;
            else
                Transitions = new LinkedList<InterpretTraceTransition>(transitions);
            BoundedStart = boundedStart;
            BoundedEnd = boundedEnd;
        }

        public Network Network
        {
            get
            {
                return StartContext.Network;
            }
        }

        public NetworkInterpreter Interpreter
        {
            get
            {
                return StartContext.Interpreter;
            }
        }

        public bool TryStepBackward(Transition transition, int symbol, int symbolPosition, out InterpretTrace result)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");

            Contract.Assert(Network.States.ContainsKey(transition.SourceState.Id), "Attempted to step outside the network.");
            Contract.Assert(Network.States.ContainsKey(transition.TargetState.Id), "Attempted to step into the network.");

            bool boundedStart = BoundedStart;
            if (!boundedStart && Transitions.Count > 0)
            {
                bool ruleBoundary = false;
                PushContextTransition pushContextTransition = Transitions.First.Value.Transition as PushContextTransition;
                if (pushContextTransition != null)
                {
                    /* the rule boundary transfers from outside the rule to inside the rule (or
                     * inside a rule invoked by this rule)
                     */

                    // first, check if the transition goes to this rule
                    ruleBoundary = Interpreter.BoundaryRules.Contains(Network.StateRules[pushContextTransition.TargetState.Id]);

                    // next, check if the transition starts outside this rule and goes through this rule
                    if (!ruleBoundary)
                    {
                        ruleBoundary =
                            pushContextTransition.ContextIdentifiers
                            .Skip(1)
                            .Any(i => Interpreter.BoundaryRules.Contains(Network.ContextRules[i]));
                    }
                }

                if (ruleBoundary)
                {
                    bool nested = false;
                    for (ContextFrame parent = StartContext.Parent; parent != null; parent = parent.Parent)
                    {
                        Contract.Assert(parent.Context != null);

                        RuleBinding contextRule = Network.ContextRules[parent.Context.Value];
                        if (Interpreter.BoundaryRules.Contains(contextRule))
                            nested = true;
                    }

                    boundedStart = !nested;
                }
            }

            result = null;

            if (transition.IsMatch)
            {
                if (symbol != NetworkInterpreter.UnknownSymbol && !transition.MatchesSymbol(symbol))
                    return false;

                ContextFrame startContext = new ContextFrame(transition.SourceState, this.StartContext.Context, this.StartContext.Parent, Interpreter);
                result = new InterpretTrace(startContext, this.EndContext, this.Transitions, boundedStart, this.BoundedEnd, boundedStart);
                if (!boundedStart)
                {
                    if (!Interpreter.TrackContextTransitions && result.Transitions.Count > 0 && result.Transitions.First.Value.Symbol == null)
                        result.Transitions.RemoveFirst();

                    result.Transitions.AddFirst(new InterpretTraceTransition(transition, symbol, symbolPosition, Interpreter));
                }

                return true;
            }

            PreventContextType preventContextType = PreventContextType.None;
            if (transition.SourceState.IsOptimized && transition.IsContext)
            {
                if (transition is PushContextTransition)
                    preventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                else if (transition is PopContextTransition)
                    preventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
            }

            if (symbol != NetworkInterpreter.UnknownSymbol && !transition.SourceState.GetSourceSet(preventContextType).Contains(symbol))
                return false;

            if (transition.IsContext)
            {
                PopContextTransition popContextTransition = transition as PopContextTransition;
                if (popContextTransition != null)
                {
                    ContextFrame subContext = this.StartContext;
                    foreach (var label in popContextTransition.ContextIdentifiers.Reverse())
                        subContext = new ContextFrame(popContextTransition.SourceState, null, new ContextFrame(subContext.State, label, subContext.Parent, Interpreter), Interpreter);

                    result = new InterpretTrace(subContext, this.EndContext, this.Transitions, boundedStart, this.BoundedEnd, boundedStart);
                    if (!boundedStart)
                    {
                        if (!Interpreter.TrackContextTransitions && result.Transitions.Count > 0 && result.Transitions.First.Value.Symbol == null)
                            result.Transitions.RemoveFirst();

                        result.Transitions.AddFirst(new InterpretTraceTransition(transition, Interpreter));
                    }

                    return true;
                }

                PushContextTransition pushContextTransition = transition as PushContextTransition;
                if (pushContextTransition != null)
                {
                    ContextFrame startContext = this.StartContext;
                    ContextFrame endContext = this.EndContext;

                    for (int i = pushContextTransition.ContextIdentifiers.Count - 1; i >= 0; i--)
                    {
                        int label = pushContextTransition.ContextIdentifiers[i];
                        if (startContext.Parent != null)
                        {
                            Contract.Assert(startContext.Parent.Context.HasValue);

                            // if the start context has a state stack, pop an item off it
                            if (startContext.Parent.Context != label)
                                return false;

                            startContext = new ContextFrame(transition.SourceState, null, startContext.Parent.Parent, Interpreter);
                        }
                        else
                        {
                            int? headContext = endContext.HeadContext;
                            if (headContext != null)
                            {
                                if (!Network.Optimizer.CanNestContexts(label, headContext.Value))
                                    return false;
                            }
                            else
                            {
                                if (!Network.Optimizer.IsStateInContext(label, endContext.State.Id))
                                    return false;
                            }

                            ContextFrame headContextFrame = null;
                            for (int j = 0; j <= i; j++)
                                headContextFrame = new ContextFrame(null, pushContextTransition.ContextIdentifiers[j], headContextFrame, Interpreter);

                            // else we add a "predicate" to the end context
                            endContext = endContext.AddHeadContext(headContextFrame);
                            if (!object.ReferenceEquals(startContext.State, transition.SourceState))
                                startContext = new ContextFrame(transition.SourceState, startContext.Context, startContext.Parent, Interpreter);

                            break;
                        }
                    }

                    result = new InterpretTrace(startContext, endContext, this.Transitions, boundedStart, this.BoundedEnd, boundedStart);
                    if (!boundedStart)
                    {
                        if (!Interpreter.TrackContextTransitions && result.Transitions.Count > 0 && result.Transitions.First.Value.Symbol == null)
                            result.Transitions.RemoveFirst();

                        result.Transitions.AddFirst(new InterpretTraceTransition(transition, Interpreter));
                    }

                    return true;
                }

                throw new NotSupportedException("Unknown context transition.");
            }
            else if (transition.IsEpsilon)
            {
                ContextFrame startContext = new ContextFrame(transition.SourceState, this.StartContext.Context, this.StartContext.Parent, Interpreter);
                result = new InterpretTrace(startContext, this.EndContext, this.Transitions, boundedStart, this.BoundedEnd, true);
                return true;
            }

            throw new NotSupportedException("Unknown transition type.");
        }

        public bool TryStepForward(Transition transition, int symbol, int symbolPosition, out InterpretTrace result)
        {
            Contract.Requires<ArgumentNullException>(transition != null, "transition");
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out result) != null);

            Contract.Assert(Network.States.ContainsKey(transition.SourceState.Id), "Attempted to step into the network.");
            Contract.Assert(Network.States.ContainsKey(transition.TargetState.Id), "Attempted to step outside the network.");

            bool boundedEnd = BoundedEnd;
            if (!boundedEnd && Transitions.Count > 0)
            {
                bool ruleBoundary = false;
                PopContextTransition popContextTransition = Transitions.Last.Value.Transition as PopContextTransition;
                if (popContextTransition != null)
                {
                    /* the rule boundary transfers from outside the rule to inside the rule (or
                     * inside a rule invoked by this rule)
                     */

                    // first, check if the transition goes from this rule
                    ruleBoundary = Interpreter.BoundaryRules.Contains(Network.StateRules[popContextTransition.SourceState.Id]);

                    // next, check if the transition starts outside this rule and goes through this rule
                    if (!ruleBoundary)
                    {
                        ruleBoundary =
                            popContextTransition.ContextIdentifiers
                            .Take(popContextTransition.ContextIdentifiers.Count - 1)
                            .Any(i => Interpreter.BoundaryRules.Contains(Network.ContextRules[i]));
                    }
                }

                if (ruleBoundary)
                {
                    bool nested = false;
                    for (ContextFrame parent = EndContext.Parent; parent != null; parent = parent.Parent)
                    {
                        Contract.Assert(parent.Context != null);

                        RuleBinding contextRule = Network.ContextRules[parent.Context.Value];
                        if (Interpreter.BoundaryRules.Contains(contextRule))
                            nested = true;
                    }

                    boundedEnd = !nested;
                }
            }

            result = null;

            if (transition.IsMatch)
            {
                if (symbol != NetworkInterpreter.UnknownSymbol && !transition.MatchesSymbol(symbol))
                    return false;

                ContextFrame endContext = new ContextFrame(transition.TargetState, this.EndContext.Context, this.EndContext.Parent, Interpreter);
                result = new InterpretTrace(this.StartContext, endContext, this.Transitions, this.BoundedStart, boundedEnd, boundedEnd);
                if (!boundedEnd)
                {
                    if (!Interpreter.TrackContextTransitions && result.Transitions.Count > 0 && result.Transitions.Last.Value.Symbol == null)
                        result.Transitions.RemoveLast();

                    result.Transitions.AddLast(new InterpretTraceTransition(transition, symbol, symbolPosition, Interpreter));
                }

                return true;
            }

            PreventContextType preventContextType = PreventContextType.None;
            if (transition.SourceState.IsOptimized && transition.IsContext)
            {
                if (transition is PushContextTransition)
                    preventContextType = transition.IsRecursive ? PreventContextType.PushRecursive : PreventContextType.Push;
                else if (transition is PopContextTransition)
                    preventContextType = transition.IsRecursive ? PreventContextType.PopRecursive : PreventContextType.Pop;
            }

            if (symbol != NetworkInterpreter.UnknownSymbol && !transition.TargetState.GetFollowSet(preventContextType).Contains(symbol))
                return false;

            if (transition.IsContext)
            {
                PushContextTransition pushContextTransition = transition as PushContextTransition;
                if (pushContextTransition != null)
                {
                    ContextFrame subContext = this.EndContext;
                    foreach (var label in pushContextTransition.ContextIdentifiers)
                        subContext = new ContextFrame(pushContextTransition.TargetState, null, new ContextFrame(subContext.State, label, subContext.Parent, Interpreter), Interpreter);

                    result = new InterpretTrace(this.StartContext, subContext, this.Transitions, this.BoundedStart, boundedEnd, boundedEnd);
                    if (!boundedEnd)
                        result.Transitions.AddLast(new InterpretTraceTransition(transition, Interpreter));

                    return true;
                }

                PopContextTransition popContextTransition = transition as PopContextTransition;
                if (popContextTransition != null)
                {
                    ContextFrame startContext = this.StartContext;
                    ContextFrame endContext = this.EndContext;

                    for (int i = 0; i < popContextTransition.ContextIdentifiers.Count; i++)
                    {
                        int label = popContextTransition.ContextIdentifiers[i];
                        if (endContext.Parent != null)
                        {
                            Contract.Assert(endContext.Parent.Context.HasValue);

                            // if the end context has a state stack, pop an item off it
                            if (endContext.Parent.Context != label)
                                return false;

                            endContext = new ContextFrame(transition.TargetState, null, endContext.Parent.Parent, Interpreter);
                        }
                        else
                        {
                            int? headContext = startContext.HeadContext;
                            if (headContext != null)
                            {
                                if (!Network.Optimizer.CanNestContexts(label, headContext.Value))
                                    return false;
                            }
                            else
                            {
                                if (!Network.Optimizer.IsStateInContext(label, startContext.State.Id))
                                    return false;
                            }

                            ContextFrame headContextFrame = null;
                            for (int j = popContextTransition.ContextIdentifiers.Count - 1; j >= i; j--)
                                headContextFrame = new ContextFrame(null, popContextTransition.ContextIdentifiers[j], headContextFrame, Interpreter);

                            startContext = startContext.AddHeadContext(headContextFrame);
                            if (!object.ReferenceEquals(endContext.State, transition.TargetState))
                                endContext = new ContextFrame(transition.TargetState, endContext.Context, endContext.Parent, Interpreter);

                            break;
                        }
                    }

                    result = new InterpretTrace(startContext, endContext, this.Transitions, this.BoundedStart, boundedEnd, boundedEnd);
                    if (!boundedEnd)
                    {
                        if (!Interpreter.TrackContextTransitions && result.Transitions.Count > 0 && result.Transitions.Last.Value.Symbol == null)
                            result.Transitions.RemoveLast();

                        result.Transitions.AddLast(new InterpretTraceTransition(transition, Interpreter));
                    }

                    return true;
                }

                throw new NotSupportedException("Unknown context transition.");
            }
            else if (transition.IsEpsilon)
            {
                ContextFrame endContext = new ContextFrame(transition.TargetState, this.EndContext.Context, this.EndContext.Parent, Interpreter);
                result = new InterpretTrace(this.StartContext, endContext, this.Transitions, this.BoundedStart, boundedEnd, true);
                return true;
            }

            throw new NotSupportedException("Unknown transition type.");
        }

        public virtual bool Equals(InterpretTrace other)
        {
            if (other == null)
                return false;

            return StartContext.Equals(other.StartContext)
                && EndContext.Equals(other.EndContext)
                && Transitions.SequenceEqual(other.Transitions);
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as InterpretTrace);
        }

        public override int GetHashCode()
        {
            return StartContext.GetHashCode() ^ EndContext.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(
                "Transition Count: {0}{1}{2}",
                Transitions.Count,
                BoundedStart ? " (Bounded Start)" : string.Empty,
                BoundedEnd ? " (Bounded End)" : string.Empty);
        }
    }
}
