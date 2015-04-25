namespace Tvl.VisualStudio.Language.Intellisense
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using IVsTextView = Microsoft.VisualStudio.TextManager.Interop.IVsTextView;
    using Marshal = System.Runtime.InteropServices.Marshal;
    using OLECMDEXECOPT = Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT;
    using VsCommands = Microsoft.VisualStudio.VSConstants.VSStd97CmdID;
    using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;
    using VsMenus = Microsoft.VisualStudio.Shell.VsMenus;
    using VSOBJGOTOSRCTYPE = Microsoft.VisualStudio.Shell.Interop.VSOBJGOTOSRCTYPE;

    public class IntellisenseController : ITvlIntellisenseController, IIntellisenseController
    {
        private readonly IntellisenseControllerProvider _provider;

        private ITextView _textView;
        private IntellisenseCommandFilter _commandFilter;
        private ICompletionSession _completionSession;
        private ISignatureHelpSession _signatureHelpSession;
        private IQuickInfoSession _quickInfoSession;
        private ISmartTagSession _smartTagSession;
        private readonly Lazy<CompletionInfo> _completionInfo;
        private bool _isProcessingCommand;

        public IntellisenseController(ITextView textView, IntellisenseControllerProvider provider)
        {
            Contract.Requires<ArgumentNullException>(textView != null, "textView");
            Contract.Requires<ArgumentNullException>(provider != null, "provider");

            _provider = provider;
            _completionInfo = new Lazy<CompletionInfo>(CreateCompletionInfo);
            Attach(textView);
        }

        public IntellisenseControllerProvider Provider
        {
            get
            {
                Contract.Ensures(Contract.Result<IntellisenseControllerProvider>() != null);

                return _provider;
            }
        }

        public IIntellisenseSessionStack IntellisenseSessionStack
        {
            get
            {
                return Provider.IntellisenseSessionStackMapService.GetStackForTextView(TextView);
            }
        }

        public ITextView TextView
        {
            get
            {
                return _textView;
            }
        }

        public virtual bool SupportsCommenting
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsFormatting
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsCompletion
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsSignatureHelp
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsQuickInfo
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsGotoDefinition
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsGotoDeclaration
        {
            get
            {
                return false;
            }
        }

        public virtual bool SupportsGotoReference
        {
            get
            {
                return false;
            }
        }

        public virtual IMouseProcessor CustomMouseProcessor
        {
            get
            {
                return null;
            }
        }

        public ICompletionSession CompletionSession
        {
            get
            {
                return _completionSession;
            }

            private set
            {
                _completionSession = value;
            }
        }

        public ISignatureHelpSession SignatureHelpSession
        {
            get
            {
                return _signatureHelpSession;
            }

            private set
            {
                _signatureHelpSession = value;
            }
        }

        public IQuickInfoSession QuickInfoSession
        {
            get
            {
                return _quickInfoSession;
            }

            private set
            {
                _quickInfoSession = value;
            }
        }

        public ISmartTagSession SmartTagSession
        {
            get
            {
                return _smartTagSession;
            }

            private set
            {
                _smartTagSession = value;
            }
        }

        public virtual bool IsCompletionActive
        {
            get
            {
                return CompletionSession != null;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public CompletionInfo CompletionInfo
        {
            get
            {
                return _completionInfo.Value;
            }
        }

        protected IntellisenseCommandFilter CommandFilter
        {
            get
            {
                return _commandFilter;
            }
        }

        protected bool IsProcessingCommand
        {
            get
            {
                return _isProcessingCommand;
            }
        }

        public virtual void GoToSource(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            Task<IEnumerable<INavigateToTarget>> task = GoToSourceAsync(gotoSourceType, triggerPoint).HandleNonCriticalExceptions();
            var resultContinuation = task.ContinueWith(HandleGoToSourceResult, TaskContinuationOptions.OnlyOnRanToCompletion).HandleNonCriticalExceptions();
        }

        public virtual Task<IEnumerable<INavigateToTarget>> GoToSourceAsync(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            return Task.Factory.StartNew(() => GoToSourceImpl(gotoSourceType, triggerPoint));
        }

        public virtual IEnumerable<INavigateToTarget> GoToSourceImpl(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            Contract.Ensures(Contract.Result<IEnumerable<INavigateToTarget>>() != null);

            return new INavigateToTarget[0];
        }

        protected virtual void HandleGoToSourceResult(Task<IEnumerable<INavigateToTarget>> task)
        {
            INavigateToTarget target = task.Result.FirstOrDefault();
            if (target != null)
                target.NavigateTo();
        }

        public virtual void TriggerCompletion(ITrackingPoint triggerPoint)
        {
            this.TriggerCompletion(triggerPoint, CompletionInfoType.NoInfo, IntellisenseInvocationType.Default);
        }

        public virtual void TriggerCompletion(ITrackingPoint triggerPoint, CompletionInfoType completionInfoType, IntellisenseInvocationType intellisenseInvocationType)
        {
            DismissCompletion();
            CompletionInfo.InfoType = completionInfoType;
            CompletionInfo.InvocationType = intellisenseInvocationType;
            ICompletionSession session = Provider.CompletionBroker.TriggerCompletion(TextView, triggerPoint, true);
            if (session != null)
            {
                session.Committed += HandleCompletionCommitted;
                session.Dismissed += HandleCompletionDismissed;
                CompletionSession = session;

                if (completionInfoType == CompletionInfoType.GlobalInfo
                    && !session.IsDismissed
                    && session.SelectedCompletionSet.SelectionStatus.IsSelected
                    && session.SelectedCompletionSet.SelectionStatus.IsUnique)
                {
                    session.Commit();
                }
            }
        }

        public virtual void TriggerSignatureHelp(ITrackingPoint triggerPoint)
        {
            DismissSignatureHelp();
            ISignatureHelpSession session = Provider.SignatureHelpBroker.TriggerSignatureHelp(TextView, triggerPoint, true);
            if (session != null)
            {
                session.Dismissed += HandleSignatureHelpDismissed;
                SignatureHelpSession = session;
            }
        }

        public virtual void TriggerQuickInfo(ITrackingPoint triggerPoint)
        {
            DismissQuickInfo();
            IQuickInfoSession session = Provider.QuickInfoBroker.TriggerQuickInfo(TextView, triggerPoint, true);
            if (session != null)
            {
                session.Dismissed += HandleQuickInfoDismissed;
                QuickInfoSession = session;
            }
        }

        public virtual void TriggerSmartTag(ITrackingPoint triggerPoint, SmartTagType type, SmartTagState state)
        {
            DismissSmartTag();
            ISmartTagSession session = Provider.SmartTagBroker.CreateSmartTagSession(TextView, type, triggerPoint, state);
            if (session != null)
            {
                session.Dismissed += HandleSmartTagDismissed;
                SmartTagSession = session;
            }
        }

        public virtual void DismissCompletion()
        {
            ICompletionSession session = CompletionSession;
            CompletionSession = null;
            if (session != null && !session.IsDismissed)
                session.Dismiss();
        }

        public virtual void DismissQuickInfo()
        {
            IQuickInfoSession session = QuickInfoSession;
            QuickInfoSession = null;
            if (session != null && !session.IsDismissed)
                session.Dismiss();
        }

        public virtual void DismissSignatureHelp()
        {
            ISignatureHelpSession session = SignatureHelpSession;
            SignatureHelpSession = null;
            if (session != null && !session.IsDismissed)
                session.Dismiss();
        }

        public virtual void DismissSmartTag()
        {
            ISmartTagSession session = SmartTagSession;
            SmartTagSession = null;
            if (session != null && !session.IsDismissed)
                session.Dismiss();
        }

        public virtual void DismissAll()
        {
            DismissCompletion();
            DismissSignatureHelp();
            DismissQuickInfo();
            DismissSmartTag();
        }

        public virtual bool PreprocessCommand(ref Guid commandGroup, uint commandId, OLECMDEXECOPT executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            _isProcessingCommand = true;
            if (!IsCompletionActive)
                return false;

            if (commandGroup == VsMenus.guidStandardCommandSet97)
            {
                if (commandId == (uint)VsCommands.Delete)
                    return false;
            }
            else if (commandGroup == VsMenus.guidStandardCommandSet2K)
            {
                switch ((VsCommands2K)commandId)
                {
                case VsCommands2K.WORDPREV:
                case VsCommands2K.WORDPREV_EXT:
                case VsCommands2K.WORDNEXT:
                case VsCommands2K.WORDNEXT_EXT:
                case VsCommands2K.CANCEL:
                case VsCommands2K.BACKSPACE:
                case VsCommands2K.DELETE:
                case VsCommands2K.LEFT:
                case VsCommands2K.LEFT_EXT:
                case VsCommands2K.RIGHT:
                case VsCommands2K.RIGHT_EXT:
                case VsCommands2K.UP:
                case VsCommands2K.UP_EXT:
                case VsCommands2K.DOWN:
                case VsCommands2K.DOWN_EXT:
                case VsCommands2K.BOL:
                case VsCommands2K.BOL_EXT:
                case VsCommands2K.FIRSTCHAR:
                case VsCommands2K.FIRSTCHAR_EXT:
                case VsCommands2K.EOL:
                case VsCommands2K.EOL_EXT:
                case VsCommands2K.PAGEUP:
                case VsCommands2K.PAGEUP_EXT:
                case VsCommands2K.PAGEDN:
                case VsCommands2K.PAGEDN_EXT:
                case VsCommands2K.TOPLINE:
                case VsCommands2K.TOPLINE_EXT:
                case VsCommands2K.BOTTOMLINE:
                case VsCommands2K.BOTTOMLINE_EXT:
                case VsCommands2K.LEFT_EXT_COL:
                case VsCommands2K.RIGHT_EXT_COL:
                case IntellisenseCommandFilter.ECMD_INCREASEFILTER:
                case VsCommands2K.ECMD_DECREASEFILTER:
                case VsCommands2K.ECMD_LEFTCLICK:
                    return false;

                case (VsCommands2K)95:
                case VsCommands2K.BACKTAB:
                case VsCommands2K.HOME:
                case VsCommands2K.HOME_EXT:
                case VsCommands2K.END:
                case VsCommands2K.END_EXT:
                case VsCommands2K.LASTCHAR:
                case VsCommands2K.LASTCHAR_EXT:
                    break;

                case VsCommands2K.TYPECHAR:
                    char c = Convert.ToChar(Marshal.GetObjectForNativeVariant(pvaIn));
                    if (IsCommitChar(c))
                    {
                        if (CompletionSession.SelectedCompletionSet.SelectionStatus.Completion == null)
                        {
                            DismissCompletion();
                        }
                        else
                        {
                            CompletionInfo.CommitChar = c;
                            return CommitCompletion();
                        }
                    }

                    return false;

                case VsCommands2K.RETURN:
                    CompletionInfo.CommitChar = '\n';
                    return CommitCompletion();

                case VsCommands2K.TAB:
                case VsCommands2K.OPENLINEABOVE:
                    var selectionStatus = CompletionSession.SelectedCompletionSet.SelectionStatus;
                    CompletionSession.SelectedCompletionSet.SelectionStatus = new CompletionSelectionStatus(selectionStatus.Completion, true, selectionStatus.IsUnique);
                    CompletionInfo.CommitChar = (VsCommands2K)commandId == VsCommands2K.TAB ? (char?)'\t' : null;
                    return CommitCompletion();

                case VsCommands2K.ToggleConsumeFirstCompletionMode:
                    return false;
                }
            }

            return false;
        }

        public virtual void PostprocessCommand()
        {
            _isProcessingCommand = false;
        }

        public virtual bool IsCommitChar(char c)
        {
            return char.IsWhiteSpace(c);
        }

        public virtual bool CommitCompletion()
        {
            ICompletionSession session = CompletionSession;
            if (session != null && session.SelectedCompletionSet != null && session.SelectedCompletionSet.SelectionStatus != null)
            {
                Completion completion = session.SelectedCompletionSet.SelectionStatus.Completion;
                if (completion != null && session.SelectedCompletionSet.SelectionStatus.IsSelected)
                {
                    ITrackingSpan applicableToSpan = session.SelectedCompletionSet.ApplicableTo;
                    if (applicableToSpan != null && applicableToSpan.GetSpan(applicableToSpan.TextBuffer.CurrentSnapshot).GetText() != completion.InsertionText)
                    {
                        session.Commit();
                        return true;
                    }
                }
            }

            DismissCompletion();
            return false;
        }

        public virtual void OnVsTextViewCreated(IVsTextView textViewAdapter)
        {
            Contract.Requires<ArgumentNullException>(textViewAdapter != null, "textViewAdapter");

            _commandFilter = CreateIntellisenseCommandFilter(textViewAdapter);
            if (_commandFilter != null)
                _commandFilter.Enabled = true;
        }

        void IIntellisenseController.ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            ConnectSubjectBuffer(subjectBuffer);
        }

        void IIntellisenseController.Detach(ITextView textView)
        {
            Detach(textView);
        }

        void IIntellisenseController.DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            DisconnectSubjectBuffer(subjectBuffer);
        }

        protected virtual void Attach(ITextView textView)
        {
            Contract.Requires<ArgumentNullException>(textView != null, "textView");

            if (_textView != null)
                throw new InvalidOperationException();

            _textView = textView;
            _textView.Selection.SelectionChanged += HandleViewSelectionChanged;
        }

        protected virtual void Detach(ITextView textView)
        {
            Contract.Requires<ArgumentNullException>(textView != null, "textView");

            DismissAll();

            if (_commandFilter != null)
                _commandFilter.Dispose();

            if (_textView != null)
                _textView.Selection.SelectionChanged -= HandleViewSelectionChanged;

            _textView = null;
            _commandFilter = null;
        }

        protected virtual void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            Contract.Requires<ArgumentNullException>(subjectBuffer != null, "subjectBuffer");
        }

        protected virtual void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            Contract.Requires<ArgumentNullException>(subjectBuffer != null, "subjectBuffer");
        }

        protected virtual CompletionInfo CreateCompletionInfo()
        {
            Contract.Ensures(Contract.Result<CompletionInfo>() != null);

            return new CompletionInfo(this);
        }

        protected virtual IntellisenseCommandFilter CreateIntellisenseCommandFilter(IVsTextView textViewAdapter)
        {
            Contract.Requires<ArgumentNullException>(textViewAdapter != null, "textViewAdapter");
            Contract.Ensures(Contract.Result<IntellisenseCommandFilter>() != null);

            return new IntellisenseCommandFilter(textViewAdapter, this);
        }

        protected virtual void HandleViewSelectionChanged(object sender, EventArgs e)
        {
            if (!TextView.Selection.IsEmpty)
                DismissAll();
        }

        protected virtual void HandleCompletionCommitted(object sender, EventArgs e)
        {
            ITextView textView = TextView;
            if (textView != null)
            {
                IWpfTextView wpfTextView = textView as IWpfTextView;
                if (wpfTextView != null)
                    wpfTextView.VisualElement.Focus();

                textView.Caret.EnsureVisible();
            }
        }

        protected virtual void HandleCompletionDismissed(object sender, EventArgs e)
        {
            CompletionSession = null;
        }

        protected virtual void HandleSignatureHelpDismissed(object sender, EventArgs e)
        {
            SignatureHelpSession = null;
        }

        protected virtual void HandleQuickInfoDismissed(object sender, EventArgs e)
        {
            QuickInfoSession = null;
        }

        protected virtual void HandleSmartTagDismissed(object sender, EventArgs e)
        {
            SmartTagSession = null;
        }
    }
}
