namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Language.Parsing;

    internal sealed class OutliningTagger : ITagger<IOutliningRegionTag>
    {
        private List<ITagSpan<IOutliningRegionTag>> _outliningRegions;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public OutliningTagger(ITextBuffer textBuffer, IBackgroundParser backgroundParser)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Requires<ArgumentNullException>(backgroundParser != null, "backgroundParser");

            this.TextBuffer = textBuffer;
            this.BackgroundParser = backgroundParser;

            _outliningRegions = new List<ITagSpan<IOutliningRegionTag>>();

            BackgroundParser.ParseComplete += HandleBackgroundParseComplete;
            BackgroundParser.RequestParse(false);
        }

        public ITextBuffer TextBuffer
        {
            get;
            private set;
        }

        public IBackgroundParser BackgroundParser
        {
            get;
            private set;
        }

        public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return _outliningRegions;
        }

        private void OnTagsChanged(SnapshotSpanEventArgs e)
        {
            var t = TagsChanged;
            if (t != null)
                t(this, e);
        }

        private void HandleBackgroundParseComplete(object sender, ParseResultEventArgs e)
        {
            AntlrParseResultEventArgs antlrParseResultArgs = e as AntlrParseResultEventArgs;
            if (antlrParseResultArgs == null)
                return;

            UpdateTags(antlrParseResultArgs);
        }

        private void UpdateTags(AntlrParseResultEventArgs antlrParseResultArgs)
        {
            List<ITagSpan<IOutliningRegionTag>> outliningRegions = new List<ITagSpan<IOutliningRegionTag>>();

            IAstRuleReturnScope resultArgs = antlrParseResultArgs.Result as IAstRuleReturnScope;
            var result = resultArgs != null ? resultArgs.Tree as CommonTree : null;
            if (result != null)
            {
                ITextSnapshot snapshot = antlrParseResultArgs.Snapshot;

                // outline all the imports
                IList<ITree> children = result.Children ?? new ITree[0];
                for (int i = 0; i < children.Count; i++)
                {
                    /*
                     *  ^('import' 'static'? IDENTIFIER+ '*'?)
                     *  
                     *  ^('import' 'static'? IDENTIFIER+ '*'? ';')
                     *  
                     *  ^('import' 'static'? IDENTIFIER+ '*'? ';') ^('import' 'static'? IDENTIFIER+ '*'? ';')+
                     *  
                     *  ^('import' .* ';') ^('import' .* ';')+
                     */
                    if (children[i].Type != Java2Lexer.IMPORT)
                        continue;

                    int firstImport = i;
                    while (i < children.Count - 1 && children[i + 1].Type == Java2Lexer.IMPORT)
                        i++;

                    int lastImport = i;

                    // start 1 token after the first 'import' token
                    var startToken = antlrParseResultArgs.Tokens[children[firstImport].TokenStartIndex + 1];
                    var stopToken = antlrParseResultArgs.Tokens[children[lastImport].TokenStopIndex];
                    Span span = new Span(startToken.StartIndex, stopToken.StopIndex - startToken.StartIndex + 1);
                    if (snapshot.GetLineNumberFromPosition(span.Start) == snapshot.GetLineNumberFromPosition(span.End))
                        continue;

                    SnapshotSpan snapshotSpan = new SnapshotSpan(antlrParseResultArgs.Snapshot, span);
                    IOutliningRegionTag tag = new OutliningRegionTag("...", snapshotSpan.GetText());
                    TagSpan<IOutliningRegionTag> tagSpan = new TagSpan<IOutliningRegionTag>(snapshotSpan, tag);
                    outliningRegions.Add(tagSpan);
                }

                /*
                 * ^(TYPE_BODY .* '}')
                 * 
                 * ^(METHOD_BODY .* '}')
                 */

                // outline the type and method bodies
                for (CommonTreeNodeStream treeNodeStream = new CommonTreeNodeStream(result);
                    treeNodeStream.LA(1) != CharStreamConstants.EndOfFile;
                    treeNodeStream.Consume())
                {
                    switch (treeNodeStream.LA(1))
                    {
                    case Java2Lexer.TYPE_BODY:
                    case Java2Lexer.METHOD_BODY:
                        CommonTree child = treeNodeStream.LT(1) as CommonTree;
                        if (child != null)
                        {
                            var startToken = antlrParseResultArgs.Tokens[child.TokenStartIndex];
                            var stopToken = antlrParseResultArgs.Tokens[child.TokenStopIndex];
                            Span span = new Span(startToken.StartIndex, stopToken.StopIndex - startToken.StartIndex + 1);
                            if (snapshot.GetLineNumberFromPosition(span.Start) == snapshot.GetLineNumberFromPosition(span.End))
                                continue;

                            SnapshotSpan snapshotSpan = new SnapshotSpan(antlrParseResultArgs.Snapshot, span);
                            IOutliningRegionTag tag = new OutliningRegionTag("...", snapshotSpan.GetText());
                            TagSpan<IOutliningRegionTag> tagSpan = new TagSpan<IOutliningRegionTag>(snapshotSpan, tag);
                            outliningRegions.Add(tagSpan);
                        }

                        break;

                    default:
                        continue;
                    }
                }
            }

            this._outliningRegions = outliningRegions;
            OnTagsChanged(new SnapshotSpanEventArgs(new SnapshotSpan(antlrParseResultArgs.Snapshot, new Span(0, antlrParseResultArgs.Snapshot.Length))));
        }
    }
}
