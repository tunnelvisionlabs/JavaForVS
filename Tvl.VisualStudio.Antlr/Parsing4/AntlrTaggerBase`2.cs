namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using ClassifierOptions = Tvl.VisualStudio.Language.Parsing.ClassifierOptions;
    using ICharStream = Antlr4.Runtime.ICharStream;
    using IntStreamConstants = Antlr4.Runtime.IntStreamConstants;
    using IToken = Antlr4.Runtime.IToken;
    using ITokenSource = Antlr4.Runtime.ITokenSource;
    using LockRecursionPolicy = System.Threading.LockRecursionPolicy;
    using OLEMSGBUTTON = Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON;
    using OLEMSGDEFBUTTON = Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON;
    using OLEMSGICON = Microsoft.VisualStudio.Shell.Interop.OLEMSGICON;
    using ReaderWriterLockSlim = System.Threading.ReaderWriterLockSlim;
    using VsShellUtilities = Microsoft.VisualStudio.Shell.VsShellUtilities;

    public abstract class AntlrTaggerBase<TState, TTag> : ITagger<TTag>
        where TTag : ITag
    {
        private static bool _timeoutReported;

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly ITextBuffer _textBuffer;
        private readonly IEqualityComparer<TState> _stateComparer;
        private readonly ClassifierOptions _options;

        private readonly ConditionalWeakTable<ITextSnapshot, TaggerState> _lineStatesCache =
            new ConditionalWeakTable<ITextSnapshot, TaggerState>();

        private bool _failedTimeout;

        public AntlrTaggerBase(ITextBuffer textBuffer)
            : this(textBuffer, EqualityComparer<TState>.Default)
        {
        }

        public AntlrTaggerBase(ITextBuffer textBuffer, IEqualityComparer<TState> stateComparer)
            : this(textBuffer, stateComparer, ClassifierOptions.None)
        {
        }

        public AntlrTaggerBase(ITextBuffer textBuffer, IEqualityComparer<TState> stateComparer, ClassifierOptions options)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Requires<ArgumentNullException>(stateComparer != null, "stateComparer");

            _textBuffer = textBuffer;
            _stateComparer = stateComparer;
            _options = options;

            ITextSnapshot currentSnapshot = textBuffer.CurrentSnapshot;
            TaggerState taggerState = new TaggerState(currentSnapshot);
            _lineStatesCache.Add(currentSnapshot, taggerState);
            if ((options & ClassifierOptions.ManualUpdate) == 0)
                SubscribeEvents();

            ForceRetagLines(taggerState, 0, currentSnapshot.LineCount - 1);
        }

        public ITextBuffer TextBuffer
        {
            get
            {
                Contract.Ensures(Contract.Result<ITextBuffer>() != null);
                return _textBuffer;
            }
        }

        public ClassifierOptions Options
        {
            get
            {
                return _options;
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public virtual IEnumerable<ITagSpan<TTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            Contract.Ensures(Contract.Result<IEnumerable<ITagSpan<TTag>>>() != null);

            if (spans.Count == 0)
                return Enumerable.Empty<ITagSpan<TTag>>();

            List<ITagSpan<TTag>> tagSpans = new List<ITagSpan<TTag>>();
            if (_failedTimeout)
                return tagSpans;

            bool spanExtended = false;

            int extendMultilineSpanToLine = 0;
            SnapshotSpan span = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End);
            SnapshotSpan extendedSpan = span;
            ITextSnapshot snapshot = span.Snapshot;

            TaggerState taggerState = _lineStatesCache.GetValue(snapshot, CreateTaggerState);

            using (_lock.UpgradableReadLock(TimeSpan.FromMilliseconds(250)))
            {
                Span requestedSpan = span;
                TState startState = AdjustParseSpan(taggerState, ref span);

                ICharStream input = CreateInputStream(span);
                ITokenSourceWithState<TState> lexer = CreateLexer(input, span.Start.GetContainingLine().LineNumber + 1, startState);
                lexer.TokenFactory = new SnapshotTokenFactory(snapshot, GetEffectiveTokenSource(lexer));

                IToken previousToken = null;
                bool previousTokenEndsLine = false;

                /* this is held outside the loop because only tokens which end at the end of a line
                 * impact its value.
                 */
                bool lineStateChanged = false;

                while (true)
                {
                    IToken token = lexer.NextToken();

                    // The latter is true for EOF token with span.End at the end of the document
                    bool inBounds = token.StartIndex < span.End.Position
                        || token.StopIndex < span.End.Position;

                    int startLineCurrent;
                    if (token.Type == IntStreamConstants.Eof)
                        startLineCurrent = span.Snapshot.LineCount - 1;
                    else
                        startLineCurrent = token.Line - 1;

                    // endLinePrevious is the line number the previous token ended on
                    int endLinePrevious;
                    if (previousToken != null)
                    {
                        Contract.Assert(previousToken.StopIndex >= previousToken.StartIndex, "previousToken can't be EOF");
                        endLinePrevious = span.Snapshot.GetLineNumberFromPosition(previousToken.StopIndex);
                    }
                    else
                    {
                        endLinePrevious = span.Snapshot.GetLineNumberFromPosition(span.Start) - 1;
                    }

                    if (startLineCurrent > endLinePrevious + 1 || (startLineCurrent == endLinePrevious + 1 && !previousTokenEndsLine))
                    {
                        int firstMultilineLine = endLinePrevious;
                        if (previousToken == null || previousTokenEndsLine)
                            firstMultilineLine++;

                        for (int i = firstMultilineLine; i < startLineCurrent; i++)
                        {
                            if (!taggerState._lineStates[i].MultilineToken || lineStateChanged)
                                extendMultilineSpanToLine = i + 1;

                            SetLineState(taggerState, i, LineStateInfo.Multiline);
                        }
                    }

                    if (IsMultilineToken(span.Snapshot, lexer, token))
                    {
                        int startLine = span.Snapshot.GetLineNumberFromPosition(token.StartIndex);
                        int stopLine = span.Snapshot.GetLineNumberFromPosition(Math.Max(token.StartIndex, token.StopIndex));
                        for (int i = startLine; i < stopLine; i++)
                        {
                            if (!taggerState._lineStates[i].MultilineToken)
                                extendMultilineSpanToLine = i + 1;

                            SetLineState(taggerState, i, LineStateInfo.Multiline);
                        }
                    }

                    bool tokenEndsLine = TokenEndsAtEndOfLine(span.Snapshot, lexer, token);
                    if (tokenEndsLine)
                    {
                        TState stateAtEndOfLine = lexer.GetCurrentState();
                        int line = span.Snapshot.GetLineNumberFromPosition(Math.Max(token.StartIndex, token.StopIndex));
                        lineStateChanged =
                            taggerState._lineStates[line].MultilineToken
                            || !_stateComparer.Equals(taggerState._lineStates[line].EndLineState, stateAtEndOfLine);

                        // even if the state didn't change, we call SetLineState to make sure the _first/_lastChangedLine values get updated.
                        SetLineState(taggerState, line, new LineStateInfo(stateAtEndOfLine));

                        if (lineStateChanged)
                        {
                            if (line < span.Snapshot.LineCount - 1)
                            {
                                /* update the span's end position or the line state change won't be reflected
                                 * in the editor
                                 */
                                int endPosition = span.Snapshot.GetLineFromLineNumber(line + 1).EndIncludingLineBreak;
                                if (endPosition > extendedSpan.End)
                                {
                                    spanExtended = true;
                                    extendedSpan = new SnapshotSpan(extendedSpan.Snapshot, Span.FromBounds(extendedSpan.Start, endPosition));
                                }
                            }
                        }
                    }

                    if (token.Type == IntStreamConstants.Eof)
                        break;

                    if (token.StartIndex >= span.End.Position)
                        break;

                    previousToken = token;
                    previousTokenEndsLine = tokenEndsLine;

                    if (token.StopIndex < requestedSpan.Start)
                        continue;

                    var tokenTagSpans = GetTagSpansForToken(token, span.Snapshot);
                    if (tokenTagSpans != null)
                        tagSpans.AddRange(tokenTagSpans);

                    if (!inBounds)
                        break;
                }
            }

            if (extendMultilineSpanToLine > 0)
            {
                int endPosition = extendMultilineSpanToLine < span.Snapshot.LineCount ? span.Snapshot.GetLineFromLineNumber(extendMultilineSpanToLine).EndIncludingLineBreak : span.Snapshot.Length;
                if (endPosition > extendedSpan.End)
                {
                    spanExtended = true;
                    extendedSpan = new SnapshotSpan(extendedSpan.Snapshot, Span.FromBounds(extendedSpan.Start, endPosition));
                }
            }

            if (spanExtended)
            {
                /* Subtract 1 from each of these because the spans include the line break on their last
                 * line, forcing it to appear as the first position on the following line.
                 */
                int firstLine = extendedSpan.Snapshot.GetLineNumberFromPosition(span.End);
                int lastLine = extendedSpan.Snapshot.GetLineNumberFromPosition(extendedSpan.End) - 1;
                // when considering the last line of a document, span and extendedSpan may end on the same line
                ForceRetagLines(taggerState, firstLine, Math.Max(firstLine, lastLine));
            }

            return tagSpans;
        }

        protected virtual ITokenSource GetEffectiveTokenSource(ITokenSourceWithState<TState> lexer)
        {
            Contract.Ensures(Contract.Result<ITokenSource>() != null);
            return lexer;
        }

        protected virtual void SetLineState(TaggerState taggerState, int line, LineStateInfo state)
        {
            Contract.Requires(state.IsDirty || !taggerState._firstDirtyLine.HasValue || line <= taggerState._firstDirtyLine);

            using (_lock.WriteLock())
            {
                taggerState._lineStates[line] = state;
                if (!state.IsDirty && taggerState._firstDirtyLine == line)
                {
                    taggerState._firstDirtyLine++;
                }

                if (!state.IsDirty && taggerState._lastDirtyLine == line)
                {
                    taggerState._firstDirtyLine = null;
                    taggerState._lastDirtyLine = null;
                }
            }
        }

        private TaggerState CreateTaggerState(ITextSnapshot snapshot)
        {
            return new TaggerState(snapshot);
        }

        protected abstract TState GetStartState();

        protected virtual TState AdjustParseSpan(TaggerState taggerState, ref SnapshotSpan span)
        {
            int start = span.Start.Position;
            int endPosition = span.End.Position;

            ITextSnapshotLine firstDirtyLine = null;
            if (taggerState._firstDirtyLine.HasValue)
            {
                firstDirtyLine = span.Snapshot.GetLineFromLineNumber(taggerState._firstDirtyLine.Value);
                start = Math.Min(start, firstDirtyLine.Start.Position);
            }

            bool haveState = false;
            TState state = default(TState);
            int startLine = span.Snapshot.GetLineNumberFromPosition(start);
            while (startLine > 0)
            {
                LineStateInfo lineState = taggerState._lineStates[startLine - 1];
                if (!lineState.MultilineToken)
                {
                    haveState = true;
                    state = lineState.EndLineState;
                    break;
                }

                startLine--;
            }

            if (startLine == 0)
            {
                haveState = true;
                state = GetStartState();
            }

            start = span.Snapshot.GetLineFromLineNumber(startLine).Start;
            int length = endPosition - start;
            span = new SnapshotSpan(span.Snapshot, start, length);
            Contract.Assert(haveState);
            return state;
        }

        protected virtual bool TokensSkippedLines(ITextSnapshot snapshot, int endLinePrevious, IToken token)
        {
            int startLineCurrent = snapshot.GetLineNumberFromPosition(token.StartIndex);
            return startLineCurrent > endLinePrevious + 1;
        }

        protected virtual bool IsMultilineToken(ITextSnapshot snapshot, ITokenSourceWithState<TState> lexer, IToken token)
        {
            if (token.Type == IntStreamConstants.Eof)
                return false;

            int startLine = snapshot.GetLineNumberFromPosition(token.StartIndex);
            int stopLine = snapshot.GetLineNumberFromPosition(token.StopIndex);
            return startLine != stopLine;
        }

        protected virtual bool TokenEndsAtEndOfLine(ITextSnapshot snapshot, ITokenSourceWithState<TState> lexer, IToken token)
        {
            if (token.StopIndex + 1 >= snapshot.Length)
                return true;

            char c = snapshot[token.StopIndex + 1];
            return c == '\r' || c == '\n';
        }

        protected virtual ICharStream CreateInputStream(SnapshotSpan span)
        {
            SnapshotCharStream input;
            if (span.Length > 1000)
                input = new SnapshotCharStream(span.Snapshot, span.Span);
            else
                input = new SnapshotCharStream(span.Snapshot);

            input.Seek(span.Start.Position);
            return input;
        }

        protected abstract ITokenSourceWithState<TState> CreateLexer(ICharStream input, int startLine, TState startState);

        protected virtual IEnumerable<ITagSpan<TTag>> GetTagSpansForToken(IToken token, ITextSnapshot snapshot)
        {
            Contract.Requires<ArgumentNullException>(token != null, "token");
            Contract.Requires<ArgumentNullException>(snapshot != null, "snapshot");
            Contract.Ensures(Contract.Result<IEnumerable<ITagSpan<TTag>>>() != null);

            TTag tag;
            if (TryTagToken(token, out tag))
            {
                SnapshotSpan span = new SnapshotSpan(snapshot, token.StartIndex, token.StopIndex - token.StartIndex + 1);
                return new ITagSpan<TTag>[] { new TagSpan<TTag>(span, tag) };
            }

            return Enumerable.Empty<ITagSpan<TTag>>();
        }

        protected virtual bool TryTagToken(IToken token, out TTag tag)
        {
            Contract.Requires<ArgumentNullException>(token != null, "token");
            tag = default(TTag);
            return false;
        }

        protected virtual void OnTagsChanged(SnapshotSpanEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

            var t = TagsChanged;
            if (t != null)
                t(this, e);
        }

        protected virtual bool IsMultilineTagSpan(ITagSpan<TTag> span)
        {
            Contract.Requires<ArgumentNullException>(span != null, "span");
            if (span.Span.IsEmpty)
                return false;

            return span.Span.Start.GetContainingLine().LineNumber != span.Span.End.GetContainingLine().LineNumber;
        }

        #region Line state information

        protected virtual void ForceRetagLines(TaggerState taggerState, int startLine, int endLine)
        {
            taggerState._firstDirtyLine = Math.Min(taggerState._firstDirtyLine ?? startLine, startLine);
            taggerState._lastDirtyLine = Math.Max(taggerState._lastDirtyLine ?? endLine, endLine);

            ITextSnapshot snapshot = _textBuffer.CurrentSnapshot;
            int start = snapshot.GetLineFromLineNumber(startLine).Start;
            int end = snapshot.GetLineFromLineNumber(endLine).EndIncludingLineBreak;
            var e = new SnapshotSpanEventArgs(new SnapshotSpan(_textBuffer.CurrentSnapshot, Span.FromBounds(start, end)));
            OnTagsChanged(e);
        }

        protected virtual void SubscribeEvents()
        {
            _textBuffer.ChangedLowPriority += HandleTextBufferChangedLowPriority;
            _textBuffer.ChangedHighPriority += HandleTextBufferChangedHighPriority;
        }

        protected virtual void UnsubscribeEvents()
        {
            _textBuffer.ChangedLowPriority -= HandleTextBufferChangedLowPriority;
            _textBuffer.ChangedHighPriority -= HandleTextBufferChangedHighPriority;
        }

        protected virtual void HandleTextBufferChangedLowPriority(object sender, TextContentChangedEventArgs e)
        {
            if (_failedTimeout)
            {
                if (!_timeoutReported)
                {
                    IServiceProvider serviceProvider = null;
                    string message = null;
                    string title = null;
                    OLEMSGICON icon = OLEMSGICON.OLEMSGICON_CRITICAL;
                    OLEMSGBUTTON button = OLEMSGBUTTON.OLEMSGBUTTON_OK;
                    OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
                    if (serviceProvider != null)
                    {
                        _timeoutReported = true;
                        VsShellUtilities.ShowMessageBox(serviceProvider, message, title, icon, button, defaultButton);
                    }
                }

                return;
            }

            if (e.After == _textBuffer.CurrentSnapshot)
            {
                TaggerState taggerState = _lineStatesCache.GetValue(e.After, CreateTaggerState);
                if (taggerState._firstChangedLine.HasValue && taggerState._lastChangedLine.HasValue)
                {
                    int startLine = taggerState._firstChangedLine.Value;
                    int endLine = Math.Min(taggerState._lastChangedLine.Value, e.After.LineCount - 1);

                    taggerState._firstChangedLine = null;
                    taggerState._lastChangedLine = null;

                    ForceRetagLines(taggerState, startLine, endLine);
                }
            }
        }

        protected virtual void HandleTextBufferChangedHighPriority(object sender, TextContentChangedEventArgs e)
        {
            try
            {
                using (_lock.WriteLock(TimeSpan.FromSeconds(1)))
                {
                    TaggerState beforeState = _lineStatesCache.GetValue(e.Before, CreateTaggerState);

                    TaggerState afterState = _lineStatesCache.GetValue(e.After, CreateTaggerState);
                    afterState._firstChangedLine = beforeState._firstChangedLine;
                    afterState._lastChangedLine = beforeState._lastChangedLine;
                    afterState._firstDirtyLine = beforeState._firstDirtyLine;
                    afterState._lastDirtyLine = beforeState._lastDirtyLine;

                    List<LineStateInfo> lineStates = new List<LineStateInfo>(beforeState._lineStates);

                    foreach (ITextChange change in e.Changes)
                    {
                        int lineNumberFromPosition = e.After.GetLineNumberFromPosition(change.NewPosition);
                        int num2 = e.After.GetLineNumberFromPosition(change.NewEnd);
                        if (change.LineCountDelta < 0)
                        {
                            lineStates.RemoveRange(lineNumberFromPosition, Math.Abs(change.LineCountDelta));
                        }
                        else if (change.LineCountDelta > 0)
                        {
                            TState endLineState = lineStates[lineNumberFromPosition].EndLineState;
                            LineStateInfo element = new LineStateInfo(endLineState);
                            lineStates.InsertRange(lineNumberFromPosition, Enumerable.Repeat(element, change.LineCountDelta));
                        }

                        if (afterState._lastDirtyLine > lineNumberFromPosition)
                        {
                            afterState._lastDirtyLine += change.LineCountDelta;
                        }

                        if (afterState._lastChangedLine > lineNumberFromPosition)
                        {
                            afterState._lastChangedLine += change.LineCountDelta;
                        }

                        for (int i = lineNumberFromPosition; i <= num2; i++)
                        {
                            TState num5 = lineStates[i].EndLineState;
                            LineStateInfo info2 = new LineStateInfo(num5, true);
                            lineStates[i] = info2;
                        }

                        afterState._firstChangedLine = Math.Min(afterState._firstChangedLine ?? lineNumberFromPosition, lineNumberFromPosition);
                        afterState._lastChangedLine = Math.Max(afterState._lastChangedLine ?? num2, num2);
                    }

                    Contract.Assert(lineStates.Count == afterState._lineStates.Length);
                    lineStates.CopyTo(afterState._lineStates);
                }
            }
            catch (TimeoutException)
            {
                _failedTimeout = true;
                UnsubscribeEvents();
            }
        }

        // TODO: need to consider lookahead distance as well
        protected struct LineStateInfo
        {
            public static readonly LineStateInfo Multiline = new LineStateInfo(multilineToken: true);
            public static readonly LineStateInfo Dirty = new LineStateInfo(isDirty: true);

            public readonly TState EndLineState;
            public readonly bool IsDirty;
            public readonly bool MultilineToken;

            public LineStateInfo(TState endLineState)
            {
                EndLineState = endLineState;
                IsDirty = false;
                MultilineToken = false;
            }

            public LineStateInfo(TState endLineState, bool isDirty)
            {
                EndLineState = endLineState;
                IsDirty = isDirty;
                MultilineToken = false;
            }

            private LineStateInfo(bool isDirty = false, bool multilineToken = false)
            {
                EndLineState = default(TState);
                IsDirty = isDirty;
                MultilineToken = multilineToken;
            }

            public override string ToString()
            {
                if (IsDirty)
                    return "Dirty";
                else if (MultilineToken)
                    return "Multiline";
                else
                    return EndLineState.ToString();
            }
        }

        protected sealed class TaggerState
        {
            public readonly ITextVersion _lineStatesVersion;
            public readonly LineStateInfo[] _lineStates;

            public int? _firstDirtyLine;
            public int? _lastDirtyLine;

            public int? _firstChangedLine;
            public int? _lastChangedLine;

            public TaggerState(ITextSnapshot snapshot)
            {
                Contract.Requires<ArgumentNullException>(snapshot != null, "snapshot");
                _lineStatesVersion = snapshot.Version;
                _lineStates = new LineStateInfo[snapshot.LineCount];
                for (int i = 0; i < _lineStates.Length; i++)
                    _lineStates[i] = LineStateInfo.Dirty;

                _firstDirtyLine = 0;
                _lastDirtyLine = _lineStates.Length - 1;
            }

#if false
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(!_firstDirtyLine.HasValue || _lastDirtyLine.HasValue);
                Contract.Invariant(!_firstChangedLine.HasValue || _lastChangedLine.HasValue);

                Contract.Invariant(!_firstDirtyLine.HasValue || _firstDirtyLine <= _lastDirtyLine);
                Contract.Invariant(!_firstChangedLine.HasValue || _firstChangedLine <= _lastChangedLine);

                Contract.Invariant(!_firstDirtyLine.HasValue || Contract.ForAll(_lineStates, x => !x.IsDirty));

                Contract.Invariant(Contract.ForAll(0, _lineStates.Length, i => !_lineStates[i].IsDirty || _firstDirtyLine <= i));
                Contract.Invariant(Contract.ForAll(0, _lineStates.Length, i => !_lineStates[i].IsDirty || _lastDirtyLine >= i));
            }
#endif
        }

        #endregion Line state information
    }
}
