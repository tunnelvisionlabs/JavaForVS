namespace Tvl.VisualStudio.Language.Intellisense
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;

    public static class CompletionHelper
    {
        public static bool DoCallMatch(IntellisenseController completionTarget)
        {
            Contract.Requires<ArgumentNullException>(completionTarget != null, "completionTarget");

#if true
            return false;
#else
            bool flag2 = false;
            ICompletionSession session = completionTarget.CompletionSession;
            if (session != null)
            {
                ITextSnapshot snapshot = session.TextView.TextSnapshot;
                string text = snapshot.GetText(completionTarget.CompletionInfo.ApplicableTo.GetSpan(snapshot));
                if (string.IsNullOrEmpty(text))
                    return false;

                session.Match();
                CompletionSet set1 = null;
                CompletionSet set2 = null;
                CompletionSet set3 = null;
                CompletionSet set4 = null;
                bool flag3 = false;
                bool flag4 = false;
                foreach (CompletionSet set in session.CompletionSets.Where(i => i != null && i.SelectionStatus != null && i.SelectionStatus.Completion != null))
                {
                    flag2 = true;
                    bool isAllTab = false;
                    if (isAllTab)
                    {
                        set3 = set;
                        flag3 = string.Equals(text, set.SelectionStatus.Completion.DisplayText, StringComparison.CurrentCultureIgnoreCase);
                    }
                    else
                    {
                        set4 = set;
                        flag4 = string.Equals(text, set.SelectionStatus.Completion.DisplayText, StringComparison.CurrentCultureIgnoreCase);
                    }
                }

                if (flag3 && !flag4)
                {
                    set1 = set3;
                }
                else if (set2 != null)
                {
                    if (set2 != set3 && set4 == null)
                        set1 = set3;
                }
                else if (set4 != null)
                {
                    set1 = set4;
                }
                else
                {
                    set1 = set3;
                }

                if (set1 != null)
                {
                    session.SelectedCompletionSet = set1;
                }
            }

            return flag2;
#endif
        }

        public static void DoTriggerCompletion(IntellisenseController controller, CompletionInfoType infoType, bool signatureHelpOnly, IntellisenseInvocationType invocationType)
        {
            Contract.Requires<ArgumentNullException>(controller != null, "controller");

            var completionInfo = controller.CompletionInfo;
            ITextView textView = controller.TextView;
            SnapshotPoint? point = textView.Caret.Position.Point.GetPoint(textView.TextBuffer, PositionAffinity.Predecessor);
            if (point.HasValue)
            {
                ITrackingPoint trackingPoint = textView.TextBuffer.CurrentSnapshot.CreateTrackingPoint(point.Value.Position, PointTrackingMode.Positive);
                if (!signatureHelpOnly)
                {
                    controller.TriggerCompletion(trackingPoint, infoType, invocationType);
                    DoCallMatch(controller);
                }

                if (signatureHelpOnly /*|| (completionInfo.CompletionFlags & CompletionFlags.HasParameterInfo) != 0*/)
                {
                    controller.TriggerSignatureHelp(trackingPoint);
                }

                if (controller.CompletionSession != null)
                {
                    controller.IntellisenseSessionStack.MoveSessionToTop(controller.CompletionSession);
                }
            }
        }

        public static bool IsCompletionPresenterActive(IntellisenseController controller, bool evenIfUsingDefaultPresenter)
        {
            Contract.Requires<ArgumentNullException>(controller != null, "controller");

            if (controller.Provider.CompletionBroker == null || controller.CompletionSession == null || controller.CompletionSession.IsDismissed)
                return false;

            //if (!evenIfUsingDefaultPresenter && HostableEditor._useDefaultPresenter)
            //    return false;

            if (controller.Provider.CompletionBroker.IsCompletionActive(controller.TextView))
                return true;

            if (controller.Provider.SignatureHelpBroker == null)
                return false;

            return controller.Provider.SignatureHelpBroker.IsSignatureHelpActive(controller.TextView);
        }
    }
}
