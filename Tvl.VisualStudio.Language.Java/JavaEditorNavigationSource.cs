namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Text.Navigation;
    using ImageSource = System.Windows.Media.ImageSource;
    using StringBuilder = System.Text.StringBuilder;

    internal sealed class JavaEditorNavigationSource : IEditorNavigationSource
    {
        private List<IEditorNavigationTarget> _navigationTargets;
        private readonly JavaEditorNavigationSourceProvider _provider;

        public JavaEditorNavigationSource(ITextBuffer textBuffer, IBackgroundParser backgroundParser, JavaEditorNavigationSourceProvider provider)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
            Contract.Requires<ArgumentNullException>(backgroundParser != null, "backgroundParser");
            Contract.Requires<ArgumentNullException>(provider != null, "provider");

            this.TextBuffer = textBuffer;
            this.BackgroundParser = backgroundParser;
            this._provider = provider;

            this._navigationTargets = new List<IEditorNavigationTarget>();

            this.BackgroundParser.ParseComplete += HandleBackgroundParseComplete;
            this.BackgroundParser.RequestParse(false);
        }

        public event EventHandler NavigationTargetsChanged;

        private ITextBuffer TextBuffer
        {
            get;
            set;
        }

        private IBackgroundParser BackgroundParser
        {
            get;
            set;
        }

        private IJavaEditorNavigationTypeRegistryService EditorNavigationTypeRegistryService
        {
            get
            {
                return _provider.EditorNavigationTypeRegistryService;
            }
        }

        public IEnumerable<IEditorNavigationType> GetNavigationTypes()
        {
            yield return EditorNavigationTypeRegistryService.GetEditorNavigationType(PredefinedEditorNavigationTypes.Types);
            yield return EditorNavigationTypeRegistryService.GetEditorNavigationType(PredefinedEditorNavigationTypes.Members);
        }

        public IEnumerable<IEditorNavigationTarget> GetNavigationTargets()
        {
            return _navigationTargets;
        }

        private void OnNavigationTargetsChanged(EventArgs e)
        {
            var t = NavigationTargetsChanged;
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
            List<IEditorNavigationTarget> navigationTargets = new List<IEditorNavigationTarget>();

            IAstRuleReturnScope resultArgs = antlrParseResultArgs.Result as IAstRuleReturnScope;
            var result = resultArgs != null ? resultArgs.Tree as CommonTree : null;
            if (result != null)
            {
                ITextSnapshot snapshot = antlrParseResultArgs.Snapshot;

                string package = string.Empty;

                /* ^('package' qualifiedName)
                 *
                 * ^(CLASS_TYPE_IDENTIFIER modifiers .* ^(TYPE_BODY .* '}'))
                 *
                 * ^(INTERFACE_TYPE_IDENTIFIER modifiers .* ^(TYPE_BODY .* '}'))
                 *
                 * ^(ANNOTATION_TYPE_IDENTIFIER modifiers .* ^(TYPE_BODY .* '}'))
                 * 
                 * ^(FIELD_DECLARATION modifiers (.* ^(VARIABLE_IDENTIFIER .*))*)
                 * 
                 * ^(METHOD_IDENTIFIER modifiers .* ^(FORMAL_PARAMETERS .* ')') .* ^(METHOD_BODY .* '}'))
                 */

                /* STATEMENT COMPLETION (description unrelated to this file)
                 * 
                 * IDENTIFIER ('.' IDENTIFIER)*
                 * 
                 * ^(CALL IDENTIFIER .*)
                 * 
                 * ^('(' ^('==' .*) ')')
                 * 
                 */

                for (CommonTreeNodeStream treeNodeStream = new CommonTreeNodeStream(result);
                    treeNodeStream.LA(1) != CharStreamConstants.EndOfFile;
                    treeNodeStream.Consume())
                {
                    switch (treeNodeStream.LA(1))
                    {
                    case Java2Lexer.PACKAGE:
                        // ^('package' qualifiedName)
                        {
                            CommonTree child = treeNodeStream.LT(1) as CommonTree;
                            if (child != null && child.ChildCount > 0)
                            {
                                package = GetQualifiedIdentifier(child.GetChild(0));
                            }
                        }

                        break;

                    case Java2Lexer.VARIABLE_IDENTIFIER:
                        // ^(FIELD_DECLARATION (.* ^(VARIABLE_IDENTIFIER))*)
                        {
                            CommonTree child = treeNodeStream.LT(1) as CommonTree;
                            if (child != null && child.HasAncestor(Java2Lexer.FIELD_DECLARATION))
                            {
                                string name = child.Token.Text;
                                IEditorNavigationType navigationType = EditorNavigationTypeRegistryService.GetEditorNavigationType(PredefinedEditorNavigationTypes.Members);
                                var startToken = antlrParseResultArgs.Tokens[child.TokenStartIndex];
                                var stopToken = antlrParseResultArgs.Tokens[child.TokenStopIndex];
                                SnapshotSpan span = new SnapshotSpan(snapshot, new Span(startToken.StartIndex, stopToken.StopIndex - startToken.StartIndex + 1));
                                SnapshotSpan seek = new SnapshotSpan(snapshot, new Span(child.Token.StartIndex, 0));
                                StandardGlyphGroup glyphGroup = StandardGlyphGroup.GlyphGroupJSharpField;
                                StandardGlyphItem glyphItem = GetGlyphItemFromChildModifier((CommonTree)child.GetAncestor(Java2Lexer.FIELD_DECLARATION));
                                ImageSource glyph = _provider.GlyphService.GetGlyph(glyphGroup, glyphItem);
                                NavigationTargetStyle style = NavigationTargetStyle.None;
                                navigationTargets.Add(new EditorNavigationTarget(name, navigationType, span, seek, glyph, style));
                            }
                        }

                        break;

                    case Java2Lexer.METHOD_IDENTIFIER:
                        // ^(METHOD_IDENTIFIER ^(FORMAL_PARAMETERS formalParameterDecls?) ^(METHOD_BODY .* END_METHOD_BODY))
                        {
                            CommonTree child = treeNodeStream.LT(1) as CommonTree;
                            if (child != null)
                            {
                                string name = child.Token.Text;
                                IEnumerable<string> args = ProcessArguments((CommonTree)child.GetFirstChildWithType(Java2Lexer.FORMAL_PARAMETERS));
                                string sig = string.Format("{0}({1})", name, string.Join(", ", args));
                                IEditorNavigationType navigationType = EditorNavigationTypeRegistryService.GetEditorNavigationType(PredefinedEditorNavigationTypes.Members);
                                var startToken = antlrParseResultArgs.Tokens[child.TokenStartIndex];
                                var stopToken = antlrParseResultArgs.Tokens[child.TokenStopIndex];
                                SnapshotSpan span = new SnapshotSpan(snapshot, new Span(startToken.StartIndex, stopToken.StopIndex - startToken.StartIndex + 1));
                                SnapshotSpan seek = new SnapshotSpan(snapshot, new Span(child.Token.StartIndex, 0));
                                StandardGlyphGroup glyphGroup = StandardGlyphGroup.GlyphGroupJSharpMethod;
                                StandardGlyphItem glyphItem = GetGlyphItemFromChildModifier(child);
                                ImageSource glyph = _provider.GlyphService.GetGlyph(glyphGroup, glyphItem);
                                NavigationTargetStyle style = NavigationTargetStyle.None;
                                navigationTargets.Add(new EditorNavigationTarget(sig, navigationType, span, seek, glyph, style));
                            }
                        }

                        break;

                    case Java2Lexer.ENUM_TYPE_IDENTIFIER:
                    case Java2Lexer.ANNOTATION_TYPE_IDENTIFIER:
                    case Java2Lexer.INTERFACE_TYPE_IDENTIFIER:
                    case Java2Lexer.CLASS_TYPE_IDENTIFIER:
                        {
                            CommonTree child = treeNodeStream.LT(1) as CommonTree;
                            if (child != null)
                            {
                                string name = child.Token.Text;
                                for (ITree parent = child.Parent; parent != null; parent = parent.Parent)
                                {
                                    switch (parent.Type)
                                    {
                                    case Java2Lexer.ENUM_TYPE_IDENTIFIER:
                                    case Java2Lexer.ANNOTATION_TYPE_IDENTIFIER:
                                    case Java2Lexer.INTERFACE_TYPE_IDENTIFIER:
                                    case Java2Lexer.CLASS_TYPE_IDENTIFIER:
                                        name = parent.Text + "." + name;
                                        continue;

                                    default:
                                        continue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(package))
                                {
                                    name = package + "." + name;
                                }

                                IEditorNavigationType navigationType = EditorNavigationTypeRegistryService.GetEditorNavigationType(PredefinedEditorNavigationTypes.Types);
                                var startToken = antlrParseResultArgs.Tokens[child.TokenStartIndex];
                                var stopToken = antlrParseResultArgs.Tokens[child.TokenStopIndex];
                                SnapshotSpan span = new SnapshotSpan(snapshot, new Span(startToken.StartIndex, stopToken.StopIndex - startToken.StartIndex + 1));
                                SnapshotSpan seek = new SnapshotSpan(snapshot, new Span(child.Token.StartIndex, 0));

                                StandardGlyphGroup glyphGroup;
                                switch (child.Type)
                                {
                                case Java2Lexer.ENUM_TYPE_IDENTIFIER:
                                    glyphGroup = StandardGlyphGroup.GlyphGroupEnum;
                                    break;

                                case Java2Lexer.ANNOTATION_TYPE_IDENTIFIER:
                                case Java2Lexer.INTERFACE_TYPE_IDENTIFIER:
                                    glyphGroup = StandardGlyphGroup.GlyphGroupJSharpInterface;
                                    break;

                                case Java2Lexer.CLASS_TYPE_IDENTIFIER:
                                default:
                                    glyphGroup = StandardGlyphGroup.GlyphGroupJSharpClass;
                                    break;
                                }

                                StandardGlyphItem glyphItem = GetGlyphItemFromChildModifier(child);
                                ImageSource glyph = _provider.GlyphService.GetGlyph(glyphGroup, glyphItem);
                                NavigationTargetStyle style = NavigationTargetStyle.None;
                                navigationTargets.Add(new EditorNavigationTarget(name, navigationType, span, seek, glyph, style));
                            }
                        }

                        break;

                    default:
                        continue;
                    }
                }
            }

            this._navigationTargets = navigationTargets;
            OnNavigationTargetsChanged(EventArgs.Empty);
        }

        private static IEnumerable<string> ProcessArguments(CommonTree tree)
        {
            foreach (CommonTree child in tree.Children)
            {
                if (child.Type != Java2Lexer.RPAREN)
                {
                    IEnumerable<string> modifiers = GetParameterModifiers(child);
                    string name = child.Text;
                    string type = GetParameterType(child);

                    StringBuilder builder = new StringBuilder();
                    foreach (var mod in modifiers)
                        builder.Append(mod).Append(' ');

                    builder.Append(type).Append(' ');
                    builder.Append(name);
                    yield return builder.ToString();
                }
            }
        }

        private static string GetParameterType(CommonTree tree)
        {
            int index = 0;

            while (index < tree.ChildCount)
            {
                switch (tree.GetChild(index).Type)
                {
                case Java2Lexer.MONKEYS_AT:
                case Java2Lexer.FINAL:
                    index++;
                    continue;

                default:
                    break;
                }

                break;
            }

            string typeText = GetTypeText((CommonTree)tree.GetChild(index));

            if (tree.GetFirstChildWithType(Java2Lexer.ELLIPSIS) != null)
                typeText = typeText + "...";

            return typeText;
        }

        private static string GetTypeText(CommonTree tree)
        {
            if (tree.Type == Java2Lexer.ARRAY_TYPE)
                return GetTypeText((CommonTree)tree.GetChild(0)) + "[]";

            // this covers primitiveType
            if (tree.ChildCount == 0)
                return tree.Text;

            if (tree.Type == Java2Lexer.DOT)
                return GetTypeText((CommonTree)tree.GetChild(0)) + "." + GetTypeText((CommonTree)tree.GetChild(1));

            // if we get here, we know the tree is ^(IDENTIFIER typeArguments)
            string name = tree.Text;
            IEnumerable<string> typeArguments = ((CommonTree)tree.GetChild(0)).Children.Select(GetTypeArgumentText);
            return string.Format("{0}<{1}>", name, string.Join(", ", typeArguments));
        }

        private static string GetTypeArgumentText(ITree tree)
        {
            if (tree.Type != Java2Lexer.QUES)
                return GetTypeText((CommonTree)tree);

            if (tree.ChildCount == 0)
                return tree.Text;

            // if we get here, we know the tree is ^('?' (('extends' | 'super') type))
            string extendsOrSuper = tree.GetChild(0).Text;
            string extendedType = GetTypeText((CommonTree)tree.GetChild(1));
            return string.Format("? {0} {1}", extendsOrSuper, extendedType);
        }

        private static IEnumerable<string> GetParameterModifiers(CommonTree tree)
        {
            foreach (var child in tree.Children)
            {
                if (child.Type == Java2Lexer.FINAL)
                    return new string[] { "final" };
            }

            return new string[0];
        }

        private static string GetQualifiedIdentifier(ITree tree)
        {
            if (tree.Type == Java2Lexer.IDENTIFIER || tree.ChildCount != 2)
                return tree.Text;

            return GetQualifiedIdentifier(tree.GetChild(0)) + "." + tree.GetChild(1).Text;
        }

        private static StandardGlyphItem GetGlyphItemFromChildModifier(CommonTree tree)
        {
            bool isPublic = tree.Children.Any(i => i.Type == Java2Lexer.PUBLIC);
            bool isProtected = !isPublic && tree.Children.Any(i => i.Type == Java2Lexer.PROTECTED);
            bool isPrivate = !isPublic && !isProtected && tree.Children.Any(i => i.Type == Java2Lexer.PRIVATE);
            bool isModule = !isPublic && !isProtected && !isPrivate;
            StandardGlyphItem glyphItem =
                isPublic ? StandardGlyphItem.GlyphItemPublic :
                isProtected ? StandardGlyphItem.GlyphItemProtected :
                isPrivate ? StandardGlyphItem.GlyphItemPrivate :
                StandardGlyphItem.GlyphItemInternal;

            return glyphItem;
        }
    }
}
