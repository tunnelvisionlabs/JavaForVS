namespace Tvl.VisualStudio.Language.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;

    using Contract = System.Diagnostics.Contracts.Contract;

    public abstract class AntlrClassifierBase : IClassifier
    {
        private readonly List<LineStateInfo> _lineStates = new List<LineStateInfo>();
        private readonly ITextBuffer _textBuffer;

        private int? _firstDirtyLine;
        private int? _lastDirtyLine;

        private int? _firstChangedLine;
        private int? _lastChangedLine;

        public AntlrClassifierBase(ITextBuffer textBuffer)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");

            _textBuffer = textBuffer;

            _lineStates.AddRange(Enumerable.Repeat(LineStateInfo.Dirty, textBuffer.CurrentSnapshot.LineCount));
            SubscribeEvents();
            ForceReclassifyLines(0, textBuffer.CurrentSnapshot.LineCount - 1);
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public virtual IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            Contract.Ensures(Contract.Result<IList<ClassificationSpan>>() != null);

            Span requestedSpan = span;
            AdjustParseSpan(ref span);

            ICharStream input = CreateInputStream(span);
            ITokenSource lexer = CreateLexer(input);
            List<ClassificationSpan> classificationSpans = new List<ClassificationSpan>();

            IToken previousToken = null;
            bool previousTokenEndsLine = false;

            /* this is held outside the loop because only tokens which end at the end of a line
             * impact its value.
             */
            bool lineStateChanged = false;

            int extendMultilineSpanToLine = 0;
            SnapshotSpan extendedSpan = span;
            bool spanExtended = false;

            while (true)
            {
                IToken token = lexer.NextToken();

                bool inBounds = token.StartIndex < span.End.Position;

                int startLineCurrent;
                if (token.Type == CharStreamConstants.EndOfFile)
                    startLineCurrent = span.Snapshot.LineCount;
                else
                    startLineCurrent = token.Line;

                if (previousToken == null || previousToken.Line < startLineCurrent - 1)
                {
                    // endLinePrevious is the line number the previous token ended on
                    int endLinePrevious;
                    if (previousToken != null)
                        endLinePrevious = span.Snapshot.GetLineNumberFromPosition(previousToken.StopIndex + 1);
                    else
                        endLinePrevious = span.Snapshot.GetLineNumberFromPosition(span.Start) - 1;

                    if (startLineCurrent > endLinePrevious + 1)
                    {
                        int firstMultilineLine = endLinePrevious;
                        if (previousToken == null || previousTokenEndsLine)
                            firstMultilineLine++;

                        for (int i = firstMultilineLine; i < startLineCurrent; i++)
                        {
                            if (!_lineStates[i].MultilineToken || lineStateChanged)
                                extendMultilineSpanToLine = i + 1;

                            if (inBounds)
                                SetLineState(i, LineStateInfo.Multiline);
                        }
                    }
                }

                if (token.Type == CharStreamConstants.EndOfFile)
                    break;

                previousToken = token;
                previousTokenEndsLine = TokenEndsAtEndOfLine(span.Snapshot, lexer, token);

                if (IsMultilineToken(span.Snapshot, lexer, token))
                {
                    int startLine = span.Snapshot.GetLineNumberFromPosition(token.StartIndex);
                    int stopLine = span.Snapshot.GetLineNumberFromPosition(token.StopIndex + 1);
                    for (int i = startLine; i < stopLine; i++)
                    {
                        if (!_lineStates[i].MultilineToken)
                            extendMultilineSpanToLine = i + 1;

                        if (inBounds)
                            SetLineState(i, LineStateInfo.Multiline);
                    }
                }

                bool tokenEndsLine = previousTokenEndsLine;
                if (tokenEndsLine)
                {
                    int line = span.Snapshot.GetLineNumberFromPosition(token.StopIndex + 1);
                    lineStateChanged = _lineStates[line].MultilineToken;

                    // even if the state didn't change, we call SetLineState to make sure the _first/_lastChangedLine values get updated.
                    if (inBounds)
                        SetLineState(line, new LineStateInfo());

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

                if (token.StartIndex >= span.End.Position)
                    break;

                if (token.StopIndex < requestedSpan.Start)
                    continue;

                var tokenClassificationSpans = GetClassificationSpansForToken(token, span.Snapshot);
                if (tokenClassificationSpans != null)
                    classificationSpans.AddRange(tokenClassificationSpans);

                if (!inBounds)
                    break;
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
                ForceReclassifyLines(firstLine, lastLine);
            }

            return classificationSpans;
        }

        protected virtual void SetLineState(int line, LineStateInfo state)
        {
            _lineStates[line] = state;
            if (!state.IsDirty && _firstDirtyLine.HasValue && _firstDirtyLine == line)
            {
                _firstDirtyLine++;
            }

            if (!state.IsDirty && _lastDirtyLine.HasValue && _lastDirtyLine == line)
            {
                _firstDirtyLine = null;
                _lastDirtyLine = null;
            }
        }

        private void AdjustParseSpan(ref SnapshotSpan span)
        {
            int start = span.Start.Position;
            int endPosition = span.End.Position;

            ITextSnapshotLine firstDirtyLine = null;
            if (_firstDirtyLine.HasValue)
            {
                firstDirtyLine = span.Snapshot.GetLineFromLineNumber(_firstDirtyLine.Value);
                start = Math.Min(start, firstDirtyLine.Start.Position);
            }

            int startLine = span.Snapshot.GetLineNumberFromPosition(start);
            while (startLine > 0)
            {
                LineStateInfo lineState = _lineStates[startLine - 1];
                if (!lineState.MultilineToken)
                    break;

                startLine--;
            }

            start = span.Snapshot.GetLineFromLineNumber(startLine).Start;
            int length = endPosition - start;
            span = new SnapshotSpan(span.Snapshot, start, length);
        }

        private static bool TokensSkippedLines(ITextSnapshot snapshot, int endLinePrevious, IToken token)
        {
            int startLineCurrent = snapshot.GetLineNumberFromPosition(token.StartIndex);
            return startLineCurrent > endLinePrevious + 1;
        }

        protected virtual bool IsMultilineToken(ITextSnapshot snapshot, ITokenSource lexer, IToken token)
        {
            Lexer lexerLexer = lexer as Lexer;
            if (lexerLexer != null && lexerLexer.Line >= token.Line)
                return false;

            int startLine = snapshot.GetLineNumberFromPosition(token.StartIndex);
            int stopLine = snapshot.GetLineNumberFromPosition(token.StopIndex + 1);
            return startLine != stopLine;
        }

        protected virtual bool TokenEndsAtEndOfLine(ITextSnapshot snapshot, ITokenSource lexer, IToken token)
        {
            Lexer lexerLexer = lexer as Lexer;
            if (lexerLexer != null)
            {
                int c = lexerLexer.CharStream.LA(1);
                return c == '\r' || c == '\n';
            }

            ITextSnapshotLine line = snapshot.GetLineFromPosition(token.StopIndex + 1);
            return line.End <= token.StopIndex + 1 && line.EndIncludingLineBreak >= token.StopIndex + 1;
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

        protected abstract ITokenSource CreateLexer(ICharStream input);

        protected virtual IEnumerable<ClassificationSpan> GetClassificationSpansForToken(IToken token, ITextSnapshot snapshot)
        {
            Contract.Requires<ArgumentNullException>(token != null, "token");
            Contract.Requires<ArgumentNullException>(snapshot != null, "snapshot");
            Contract.Ensures(Contract.Result<IEnumerable<ClassificationSpan>>() != null);

            var classification = ClassifyToken(token);
            if (classification != null)
            {
                SnapshotSpan span = new SnapshotSpan(snapshot, token.StartIndex, token.StopIndex - token.StartIndex + 1);
                return new ClassificationSpan[] { new ClassificationSpan(span, classification) };
            }

            return Enumerable.Empty<ClassificationSpan>();
        }

        protected virtual IClassificationType ClassifyToken(IToken token)
        {
            Contract.Requires<ArgumentNullException>(token != null, "token");
            return null;
        }

        protected virtual void OnClassificationChanged(ClassificationChangedEventArgs e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

            var t = ClassificationChanged;
            if (t != null)
                t(this, e);
        }

        private static bool IsMultilineClassificationSpan(ClassificationSpan span)
        {
            Contract.Requires<ArgumentNullException>(span != null, "span");

            if (span.Span.IsEmpty)
                return false;

            return span.Span.Start.GetContainingLine().LineNumber != span.Span.End.GetContainingLine().LineNumber;
        }

        #region Line state information

        public virtual void ForceReclassifyLines(int startLine, int endLine)
        {
            _firstDirtyLine = _firstDirtyLine.HasValue ? Math.Min(_firstDirtyLine.Value, startLine) : startLine;
            _lastDirtyLine = _lastDirtyLine.HasValue ? Math.Max(_lastDirtyLine.Value, endLine) : endLine;

            ITextSnapshot snapshot = _textBuffer.CurrentSnapshot;
            int start = snapshot.GetLineFromLineNumber(startLine).Start;
            int end = snapshot.GetLineFromLineNumber(endLine).EndIncludingLineBreak;
            var e = new ClassificationChangedEventArgs(new SnapshotSpan(_textBuffer.CurrentSnapshot, Span.FromBounds(start, end)));
            OnClassificationChanged(e);
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
            if (e.After == _textBuffer.CurrentSnapshot)
            {
                if (_firstChangedLine.HasValue && _lastChangedLine.HasValue)
                {
                    int startLine = _firstChangedLine.Value;
                    int endLine = Math.Min(_lastChangedLine.Value, e.After.LineCount - 1);

                    _firstChangedLine = null;
                    _lastChangedLine = null;

                    ForceReclassifyLines(startLine, endLine);
                }
            }
        }

        protected virtual void HandleTextBufferChangedHighPriority(object sender, TextContentChangedEventArgs e)
        {
            foreach (ITextChange change in e.Changes)
            {
                int lineNumberFromPosition = e.After.GetLineNumberFromPosition(change.NewPosition);
                int num2 = e.After.GetLineNumberFromPosition(change.NewEnd);
                if (change.LineCountDelta < 0)
                {
                    _lineStates.RemoveRange(lineNumberFromPosition, Math.Abs(change.LineCountDelta));
                }
                else if (change.LineCountDelta > 0)
                {
                    LineStateInfo element = new LineStateInfo();
                    _lineStates.InsertRange(lineNumberFromPosition, Enumerable.Repeat(element, change.LineCountDelta));
                }

                if (_lastDirtyLine.HasValue && _lastDirtyLine.Value > lineNumberFromPosition)
                {
                    _lastDirtyLine += change.LineCountDelta;
                }

                if (_lastChangedLine.HasValue && _lastChangedLine.Value > lineNumberFromPosition)
                {
                    _lastChangedLine += change.LineCountDelta;
                }

                for (int i = lineNumberFromPosition; i <= num2; i++)
                {
                    LineStateInfo info2 = new LineStateInfo(true);
                    _lineStates[i] = info2;
                }

                _firstChangedLine = _firstChangedLine.HasValue ? Math.Min(_firstChangedLine.Value, lineNumberFromPosition) : lineNumberFromPosition;
                _lastChangedLine = _lastChangedLine.HasValue ? Math.Max(_lastChangedLine.Value, num2) : num2;
            }
        }

        protected struct LineStateInfo
        {
            public static readonly LineStateInfo Multiline =
                new LineStateInfo()
                {
                    MultilineToken = true
                };

            public static readonly LineStateInfo Dirty =
                new LineStateInfo()
                {
                    IsDirty = true
                };

            public bool IsDirty;
            public bool MultilineToken;

            public LineStateInfo(bool isDirty)
            {
                IsDirty = isDirty;
                MultilineToken = false;
            }
        }

        #endregion Line state information
    }
}
