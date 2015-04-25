namespace Tvl.VisualStudio.Language.Intellisense.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using OLECMDEXECOPT = Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT;

    [ContractClassFor(typeof(ITvlIntellisenseController))]
    public abstract class ITvlIntellisenseControllerContracts : ITvlIntellisenseController
    {
        #region ITvlIntellisenseController Members

        IIntellisenseSessionStack ITvlIntellisenseController.IntellisenseSessionStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ITextView ITvlIntellisenseController.TextView
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsCommenting
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsFormatting
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsCompletion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsSignatureHelp
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsQuickInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsGotoDefinition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsGotoDeclaration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.SupportsGotoReference
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IMouseProcessor ITvlIntellisenseController.CustomMouseProcessor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ICompletionSession ITvlIntellisenseController.CompletionSession
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ISignatureHelpSession ITvlIntellisenseController.SignatureHelpSession
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IQuickInfoSession ITvlIntellisenseController.QuickInfoSession
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ISmartTagSession ITvlIntellisenseController.SmartTagSession
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ITvlIntellisenseController.IsCompletionActive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void ITvlIntellisenseController.GoToSource(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        Task<IEnumerable<INavigateToTarget>> ITvlIntellisenseController.GoToSourceAsync(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            Contract.Ensures(Contract.Result<Task<IEnumerable<INavigateToTarget>>>() != null);
            throw new NotImplementedException();
        }

        IEnumerable<INavigateToTarget> ITvlIntellisenseController.GoToSourceImpl(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            Contract.Ensures(Contract.Result<IEnumerable<INavigateToTarget>>() != null);
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.TriggerCompletion(ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.TriggerCompletion(ITrackingPoint triggerPoint, CompletionInfoType completionInfoType, IntellisenseInvocationType intellisenseInvocationType)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.TriggerSignatureHelp(ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.TriggerQuickInfo(ITrackingPoint triggerPoint)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.TriggerSmartTag(ITrackingPoint triggerPoint, SmartTagType type, SmartTagState state)
        {
            Contract.Requires<ArgumentNullException>(triggerPoint != null, "triggerPoint");
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.DismissCompletion()
        {
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.DismissQuickInfo()
        {
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.DismissSignatureHelp()
        {
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.DismissSmartTag()
        {
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.DismissAll()
        {
            throw new NotImplementedException();
        }

        bool ITvlIntellisenseController.PreprocessCommand(ref Guid commandGroup, uint commandId, OLECMDEXECOPT executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            throw new NotImplementedException();
        }

        void ITvlIntellisenseController.PostprocessCommand()
        {
            throw new NotImplementedException();
        }

        bool ITvlIntellisenseController.IsCommitChar(char c)
        {
            throw new NotImplementedException();
        }

        bool ITvlIntellisenseController.CommitCompletion()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIntellisenseController Members

        void IIntellisenseController.ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            throw new NotImplementedException();
        }

        void IIntellisenseController.Detach(ITextView textView)
        {
            throw new NotImplementedException();
        }

        void IIntellisenseController.DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
