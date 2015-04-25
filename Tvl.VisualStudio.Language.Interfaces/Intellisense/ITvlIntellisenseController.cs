namespace Tvl.VisualStudio.Language.Intellisense
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Guid = System.Guid;
    using ICompletionSession = Microsoft.VisualStudio.Language.Intellisense.ICompletionSession;
    using IIntellisenseController = Microsoft.VisualStudio.Language.Intellisense.IIntellisenseController;
    using IIntellisenseSessionStack = Microsoft.VisualStudio.Language.Intellisense.IIntellisenseSessionStack;
    using IMouseProcessor = Microsoft.VisualStudio.Text.Editor.IMouseProcessor;
    using IntPtr = System.IntPtr;
    using IQuickInfoSession = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoSession;
    using ISignatureHelpSession = Microsoft.VisualStudio.Language.Intellisense.ISignatureHelpSession;
    using ISmartTagSession = Microsoft.VisualStudio.Language.Intellisense.ISmartTagSession;
    using ITextView = Microsoft.VisualStudio.Text.Editor.ITextView;
    using ITrackingPoint = Microsoft.VisualStudio.Text.ITrackingPoint;
    using OLECMDEXECOPT = Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT;
    using SmartTagState = Microsoft.VisualStudio.Language.Intellisense.SmartTagState;
    using SmartTagType = Microsoft.VisualStudio.Language.Intellisense.SmartTagType;
    using VSOBJGOTOSRCTYPE = Microsoft.VisualStudio.Shell.Interop.VSOBJGOTOSRCTYPE;

    [ContractClass(typeof(Contracts.ITvlIntellisenseControllerContracts))]
    public interface ITvlIntellisenseController : IIntellisenseController
    {
        IIntellisenseSessionStack IntellisenseSessionStack
        {
            get;
        }

        ITextView TextView
        {
            get;
        }

        bool SupportsCommenting
        {
            get;
        }

        bool SupportsFormatting
        {
            get;
        }

        bool SupportsCompletion
        {
            get;
        }

        bool SupportsSignatureHelp
        {
            get;
        }

        bool SupportsQuickInfo
        {
            get;
        }

        bool SupportsGotoDefinition
        {
            get;
        }

        bool SupportsGotoDeclaration
        {
            get;
        }

        bool SupportsGotoReference
        {
            get;
        }

        IMouseProcessor CustomMouseProcessor
        {
            get;
        }

        ICompletionSession CompletionSession
        {
            get;
        }

        ISignatureHelpSession SignatureHelpSession
        {
            get;
        }

        IQuickInfoSession QuickInfoSession
        {
            get;
        }

        ISmartTagSession SmartTagSession
        {
            get;
        }

        bool IsCompletionActive
        {
            get;
        }

        void GoToSource(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint);

        Task<IEnumerable<INavigateToTarget>> GoToSourceAsync(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint);

        IEnumerable<INavigateToTarget> GoToSourceImpl(VSOBJGOTOSRCTYPE gotoSourceType, ITrackingPoint triggerPoint);

        void TriggerCompletion(ITrackingPoint triggerPoint);

        void TriggerCompletion(ITrackingPoint triggerPoint, CompletionInfoType completionInfoType, IntellisenseInvocationType intellisenseInvocationType);

        void TriggerSignatureHelp(ITrackingPoint triggerPoint);

        void TriggerQuickInfo(ITrackingPoint triggerPoint);

        void TriggerSmartTag(ITrackingPoint triggerPoint, SmartTagType type, SmartTagState state);

        void DismissCompletion();

        void DismissQuickInfo();

        void DismissSignatureHelp();

        void DismissSmartTag();

        void DismissAll();

        bool PreprocessCommand(ref Guid commandGroup, uint commandId, OLECMDEXECOPT executionOptions, IntPtr pvaIn, IntPtr pvaOut);

        void PostprocessCommand();

        bool IsCommitChar(char c);

        bool CommitCompletion();
    }
}
