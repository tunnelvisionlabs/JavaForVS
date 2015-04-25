namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;

    public interface IEditorNavigationSource
    {
        event EventHandler NavigationTargetsChanged;

        IEnumerable<IEditorNavigationType> GetNavigationTypes();

        IEnumerable<IEditorNavigationTarget> GetNavigationTargets();
    }
}
