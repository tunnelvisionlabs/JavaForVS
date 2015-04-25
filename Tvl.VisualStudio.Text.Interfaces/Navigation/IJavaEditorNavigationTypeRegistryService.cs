namespace Tvl.VisualStudio.Text.Navigation
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IJavaEditorNavigationTypeRegistryServiceContracts))]
    public interface IJavaEditorNavigationTypeRegistryService
    {
        IEditorNavigationType CreateEditorNavigationType(JavaEditorNavigationTypeDefinition definition, string type, IEnumerable<IEditorNavigationType> baseTypes);
        IEditorNavigationType CreateTransientEditorNavigationType(IEnumerable<IEditorNavigationType> baseTypes);
        IEditorNavigationType CreateTransientEditorNavigationType(params IEditorNavigationType[] baseTypes);
        IEditorNavigationType GetEditorNavigationType(string type);
    }
}
