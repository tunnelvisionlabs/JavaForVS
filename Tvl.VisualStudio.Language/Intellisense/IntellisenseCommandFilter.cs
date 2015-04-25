namespace Tvl.VisualStudio.Language.Intellisense
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.VisualStudio.Shell;
    using Tvl.VisualStudio.Text;
    using OLECMDEXECOPT = Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT;
    using VSOBJGOTOSRCTYPE = Microsoft.VisualStudio.Shell.Interop.VSOBJGOTOSRCTYPE;

    public class IntellisenseCommandFilter : TextViewCommandFilter
    {
        public const VSConstants.VSStd2KCmdID ECMD_INCREASEFILTER = (VSConstants.VSStd2KCmdID)0x91;
        public const VSConstants.VSStd2KCmdID ECMD_SMARTTASKS = (VSConstants.VSStd2KCmdID)0x93;

        private readonly IntellisenseController _controller;
        private bool _isInConsumeFirstCompletionMode;

        public IntellisenseCommandFilter(IVsTextView textViewAdapter, IntellisenseController controller)
            : base(textViewAdapter)
        {
            Contract.Requires(textViewAdapter != null);
            Contract.Requires<ArgumentNullException>(controller != null, "controller");

            _controller = controller;
        }

        public IntellisenseController Controller
        {
            get
            {
                Contract.Ensures(Contract.Result<IntellisenseController>() != null);

                return _controller;
            }
        }

        public bool IsInConsumeFirstCompletionMode
        {
            get
            {
                return _isInConsumeFirstCompletionMode;
            }

            protected set
            {
                _isInConsumeFirstCompletionMode = value;
            }
        }

        public static IntellisenseKeyboardCommand? TranslateKeyboardCommand(Guid group, uint id)
        {
            if (group == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VSConstants.VSStd2KCmdID)id)
                {
                case VSConstants.VSStd2KCmdID.UP:
                    return IntellisenseKeyboardCommand.Up;

                case VSConstants.VSStd2KCmdID.DOWN:
                    return IntellisenseKeyboardCommand.Down;

                case VSConstants.VSStd2KCmdID.PAGEUP:
                    return IntellisenseKeyboardCommand.PageUp;

                case VSConstants.VSStd2KCmdID.PAGEDN:
                    return IntellisenseKeyboardCommand.PageDown;

                case VSConstants.VSStd2KCmdID.TOPLINE:
                    return IntellisenseKeyboardCommand.TopLine;

                case VSConstants.VSStd2KCmdID.BOTTOMLINE:
                    return IntellisenseKeyboardCommand.BottomLine;

                case VSConstants.VSStd2KCmdID.BOL:
                    return IntellisenseKeyboardCommand.Home;

                case VSConstants.VSStd2KCmdID.EOL:
                    return IntellisenseKeyboardCommand.End;

                case VSConstants.VSStd2KCmdID.RETURN:
                    return IntellisenseKeyboardCommand.Enter;

                case VSConstants.VSStd2KCmdID.CANCEL:
                    return IntellisenseKeyboardCommand.Escape;

                case ECMD_INCREASEFILTER:
                    return IntellisenseKeyboardCommand.IncreaseFilterLevel;

                case VSConstants.VSStd2KCmdID.ECMD_DECREASEFILTER:
                    return IntellisenseKeyboardCommand.DecreaseFilterLevel;

                default:
                    break;
                }
            }

            return null;
        }

        protected override bool HandlePreExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            bool handled = Controller.PreprocessCommand(ref commandGroup, commandId, (OLECMDEXECOPT)executionOptions, pvaIn, pvaOut);

            try
            {
                if (!handled)
                {
                    IIntellisenseCommandTarget sessionStack = Controller.IntellisenseSessionStack as IIntellisenseCommandTarget;
                    IntellisenseKeyboardCommand? command = TranslateKeyboardCommand(commandGroup, commandId);
                    if (sessionStack != null && command.HasValue)
                        handled = sessionStack.ExecuteKeyboardCommand(command.Value);
                }

                if (commandGroup == VsMenus.guidStandardCommandSet97)
                {
                    VSOBJGOTOSRCTYPE? gotoSourceType = null;
                    switch ((VSConstants.VSStd97CmdID)commandId)
                    {
                    case VSConstants.VSStd97CmdID.GotoDecl:
                        {
                            if (!Controller.SupportsGotoDeclaration)
                                throw new NotSupportedException("The IntelliSense controller does not support the Go To Declaration operation.");

                            gotoSourceType = VSOBJGOTOSRCTYPE.GS_DECLARATION;
                            handled = true;
                            break;
                        }

                    case VSConstants.VSStd97CmdID.GotoDefn:
                        {
                            if (!Controller.SupportsGotoDefinition)
                                throw new NotSupportedException("The IntelliSense controller does not support the Go To Definition operation.");

                            gotoSourceType = VSOBJGOTOSRCTYPE.GS_DEFINITION;
                            handled = true;
                            break;
                        }

                    case VSConstants.VSStd97CmdID.GotoRef:
                        {
                            if (!Controller.SupportsGotoReference)
                                throw new NotSupportedException("The IntelliSense controller does not support the Go To Reference operation.");

                            gotoSourceType = VSOBJGOTOSRCTYPE.GS_REFERENCE;
                            handled = true;
                            break;
                        }

                    default:
                        break;
                    }

                    if (gotoSourceType.HasValue)
                    {
                        ITextView textView = Controller.TextView;
                        SnapshotPoint? point = textView.Caret.Position.Point.GetPoint(textView.TextBuffer, PositionAffinity.Predecessor);
                        ITrackingPoint triggerPoint = textView.TextBuffer.CurrentSnapshot.CreateTrackingPoint(point.Value.Position, PointTrackingMode.Positive);
                        Controller.GoToSource(gotoSourceType.Value, triggerPoint);
                        handled = true;
                    }
                }
                else if (commandGroup == VsMenus.guidStandardCommandSet2K)
                {
                    switch ((VSConstants.VSStd2KCmdID)commandId)
                    {
                    case VSConstants.VSStd2KCmdID.UP_EXT:
                        if (DismissAllCompletionSessions() || AnySignatureHelpSessions())
                        {
                            commandId = (uint)VSConstants.VSStd2KCmdID.UP;
                            handled = false;
                        }
                        break;

                    case VSConstants.VSStd2KCmdID.DOWN_EXT:
                        if (DismissAllCompletionSessions() || AnySignatureHelpSessions())
                        {
                            commandId = (uint)VSConstants.VSStd2KCmdID.DOWN;
                            handled = false;
                        }
                        break;

                    case ECMD_SMARTTASKS:
                        ExpandSmartTagUnderCaret();
                        handled = true;
                        break;

                    case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                        CompletionHelper.DoTriggerCompletion(Controller, CompletionInfoType.ContextInfo, false, IntellisenseInvocationType.Default);
                        handled = true;
                        break;

                    case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                        CompletionHelper.DoTriggerCompletion(Controller, CompletionInfoType.GlobalInfo, false, IntellisenseInvocationType.Default);
                        handled = true;
                        break;

                    case VSConstants.VSStd2KCmdID.PARAMINFO:
                        CompletionHelper.DoTriggerCompletion(Controller, CompletionInfoType.ContextInfo, true, IntellisenseInvocationType.Default);
                        handled = true;
                        break;

                    case VSConstants.VSStd2KCmdID.QUICKINFO:
                        throw new NotImplementedException();

                    case VSConstants.VSStd2KCmdID.ToggleConsumeFirstCompletionMode:
                        _isInConsumeFirstCompletionMode = !_isInConsumeFirstCompletionMode;
                        handled = true;
                        break;

                    default:
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Controller.PostprocessCommand();
                e.PreserveStackTrace();
                throw;
            }

            if (handled)
                Controller.PostprocessCommand();

            return handled;
        }

        protected override void HandlePostExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            Controller.PostprocessCommand();
            base.HandlePostExec(ref commandGroup, commandId, executionOptions, pvaIn, pvaOut);
        }

        protected virtual bool AnySignatureHelpSessions()
        {
            IntellisenseController controller = Controller;
            if (controller == null)
                return false;

            IIntellisenseSessionStack sessionStack = controller.IntellisenseSessionStack;
            if (sessionStack == null)
                return false;

            ReadOnlyCollection<IIntellisenseSession> sessions = sessionStack.Sessions;
            if (sessions == null)
                return false;

            return sessions.OfType<ISignatureHelpSession>().Any();
        }

        protected virtual bool DismissAllCompletionSessions()
        {
            IntellisenseController controller = Controller;
            if (controller == null)
                return false;

            IIntellisenseSessionStack sessionStack = controller.IntellisenseSessionStack;
            if (sessionStack == null)
                return false;

            ReadOnlyCollection<IIntellisenseSession> sessions = sessionStack.Sessions;
            if (sessions == null)
                return false;

            List<ICompletionSession> completionSessions = new List<ICompletionSession>(sessions.OfType<ICompletionSession>());
            foreach (var session in completionSessions)
                session.Dismiss();

            return completionSessions.Count > 0;
        }

        protected virtual void ExpandSmartTagUnderCaret()
        {
            ITextView textView = Controller.TextView;
            SnapshotPoint? insertionPoint = textView.Caret.Position.Point.GetInsertionPoint(buffer => buffer == textView.TextBuffer);
            if (!insertionPoint.HasValue)
                throw new InvalidOperationException();

            ITextSnapshot snapshot = insertionPoint.Value.Snapshot;
            SnapshotSpan caretSpan = new SnapshotSpan(insertionPoint.Value, 0);
            IEnumerable<ISmartTagSession> sessions = Controller.Provider.SmartTagBroker.GetSessions(textView);
            ISmartTagSession session = sessions.FirstOrDefault(i => i.ApplicableToSpan.GetSpan(snapshot).IntersectsWith(caretSpan) && i.Type == SmartTagType.Factoid);
            List<ISmartTagSession> source = sessions.Where(i => i.Type == SmartTagType.Ephemeral).ToList();

            if (session == null)
                session = source.FirstOrDefault(i => i.ApplicableToSpan.GetSpan(snapshot).IntersectsWith(caretSpan));

            if (session == null && source.Count == 1)
                session = source[0];

            if (session != null)
                session.State = SmartTagState.Expanded;
        }

        protected override CommandStatus QueryCommandStatus(ref Guid group, uint id)
        {
            if (group == VsMenus.guidStandardCommandSet97)
            {
                switch ((VSConstants.VSStd97CmdID)id)
                {
                case VSConstants.VSStd97CmdID.GotoDecl:
                    if (Controller.SupportsGotoDeclaration)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd97CmdID.GotoDefn:
                    if (Controller.SupportsGotoDefinition)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd97CmdID.GotoRef:
                    if (Controller.SupportsGotoReference)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                default:
                    break;
                }
            }
            else if (group == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VSConstants.VSStd2KCmdID)id)
                {
                case ECMD_INCREASEFILTER:
                case VSConstants.VSStd2KCmdID.ECMD_DECREASEFILTER:
                case ECMD_SMARTTASKS:
                case VSConstants.VSStd2KCmdID.ECMD_COPYTIP:
                case VSConstants.VSStd2KCmdID.ECMD_PASTETIP:
                    return CommandStatus.Supported | CommandStatus.Enabled;

                case VSConstants.VSStd2KCmdID.ToggleConsumeFirstCompletionMode:
                    var result = CommandStatus.Supported | CommandStatus.Enabled;
                    if (_isInConsumeFirstCompletionMode)
                        result |= CommandStatus.Latched;

                    return result;

                case VSConstants.VSStd2KCmdID.FORMATDOCUMENT:
                case VSConstants.VSStd2KCmdID.FORMATSELECTION:
                    if (Controller.SupportsFormatting)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.COMMENTBLOCK:
                case VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK:
                    if (Controller.SupportsCommenting)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                    if (Controller.SupportsCompletion)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.PARAMINFO:
                    if (Controller.SupportsSignatureHelp)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.QUICKINFO:
                    if (Controller.SupportsQuickInfo)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                //case VSConstants.VSStd2KCmdID.OUTLN_START_AUTOHIDING:

                default:
                    break;
                }
            }

            return base.QueryCommandStatus(ref group, id);
        }

#if false
        protected override bool HandlePreExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (commandGroup == VsMenus.guidStandardCommandSet97)
            {
                switch ((VSConstants.VSStd97CmdID)commandId)
                {
                case VSConstants.VSStd97CmdID.GotoDecl:
                case VSConstants.VSStd97CmdID.GotoDefn:
                case VSConstants.VSStd97CmdID.GotoRef:
                    HandleGoto((VSConstants.VSStd97CmdID)commandId);
                    return true;

                default:
                    break;
                }
            }
            else if (commandGroup == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VSConstants.VSStd2KCmdID)commandId)
                {
                case VSConstants.VSStd2KCmdID.FORMATDOCUMENT:
                    ReformatDocument();
                    return true;

                case VSConstants.VSStd2KCmdID.FORMATSELECTION:
                    ReformatSelection();
                    return true;

                case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.COMMENTBLOCK:
                    CommentSelection();
                    return true;

                case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK:
                    UncommentSelection();
                    return true;

                case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                    CompletionHelper.DoTriggerCompletion(CompletionTarget, CompletionInfoType.ContextInfo, false, IntellisenseInvocationType.Default);
                    return true;

                case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                    CompletionHelper.DoTriggerCompletion(CompletionTarget, CompletionInfoType.GlobalInfo, false, IntellisenseInvocationType.Default);
                    return true;

                case VSConstants.VSStd2KCmdID.PARAMINFO:
                    CompletionHelper.DoTriggerCompletion(CompletionTarget, CompletionInfoType.ContextInfo, true, IntellisenseInvocationType.Default);
                    return true;

                case VSConstants.VSStd2KCmdID.QUICKINFO:
                    throw new NotImplementedException();

                default:
                    break;
                }
            }

            return base.HandlePreExec(ref commandGroup, commandId, executionOptions, pvaIn, pvaOut);
        }

        protected override void HandlePostExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            base.HandlePostExec(ref commandGroup, commandId, executionOptions, pvaIn, pvaOut);
        }

        protected override CommandStatus QueryCommandStatus(ref Guid group, uint id)
        {
            if (group == VsMenus.guidStandardCommandSet97)
            {
                switch ((VSConstants.VSStd97CmdID)id)
                {
                case VSConstants.VSStd97CmdID.GotoDecl:
                    if (SupportsGotoDeclaration)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd97CmdID.GotoDefn:
                    if (SupportsGotoDefinition)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd97CmdID.GotoRef:
                    if (SupportsGotoReference)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                default:
                    break;
                }
            }
            else if (group == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VSConstants.VSStd2KCmdID)id)
                {
                case VSConstants.VSStd2KCmdID.FORMATDOCUMENT:
                case VSConstants.VSStd2KCmdID.FORMATSELECTION:
                    if (SupportsFormatting)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.COMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK:
                case VSConstants.VSStd2KCmdID.COMMENTBLOCK:
                case VSConstants.VSStd2KCmdID.UNCOMMENTBLOCK:
                    if (SupportsCommenting)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                    if (SupportsCompletion)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.PARAMINFO:
                    if (SupportsSignatureHelp)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                case VSConstants.VSStd2KCmdID.QUICKINFO:
                    if (SupportsQuickInfo)
                        return CommandStatus.Supported | CommandStatus.Enabled;
                    break;

                //case VSConstants.VSStd2KCmdID.OUTLN_START_AUTOHIDING:

                default:
                    break;
                }
            }

            return CommandStatus.None;
        }

        protected virtual void HandleGoto(VSConstants.VSStd97CmdID vSStd97CmdID)
        {
            throw new NotImplementedException();
        }

        protected virtual void ReformatDocument()
        {
            throw new NotImplementedException();
        }

        protected virtual void ReformatSelection()
        {
            throw new NotImplementedException();
        }

        protected virtual void CommentSelection()
        {
            throw new NotImplementedException();
        }

        protected virtual void UncommentSelection()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
