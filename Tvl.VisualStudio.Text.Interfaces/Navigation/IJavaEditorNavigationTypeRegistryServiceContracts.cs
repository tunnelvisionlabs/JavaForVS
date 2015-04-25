namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [ContractClassFor(typeof(IJavaEditorNavigationTypeRegistryService))]
    public abstract class IJavaEditorNavigationTypeRegistryServiceContracts : IJavaEditorNavigationTypeRegistryService
    {
        IEditorNavigationType IJavaEditorNavigationTypeRegistryService.CreateEditorNavigationType(JavaEditorNavigationTypeDefinition definition, string type, IEnumerable<IEditorNavigationType> baseTypes)
        {
            Contract.Requires<ArgumentNullException>(definition != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Ensures(Contract.Result<IEditorNavigationType>() != null);

            throw new NotImplementedException();
        }

        IEditorNavigationType IJavaEditorNavigationTypeRegistryService.CreateTransientEditorNavigationType(IEnumerable<IEditorNavigationType> baseTypes)
        {
            Contract.Requires<ArgumentNullException>(baseTypes != null);
            Contract.Requires<ArgumentException>(baseTypes.Any());
            Contract.Ensures(Contract.Result<IEditorNavigationType>() != null);

            throw new NotImplementedException();
        }

        IEditorNavigationType IJavaEditorNavigationTypeRegistryService.CreateTransientEditorNavigationType(params IEditorNavigationType[] baseTypes)
        {
            Contract.Requires<ArgumentNullException>(baseTypes != null);
            Contract.Requires<ArgumentException>(baseTypes.Length > 0);
            Contract.Ensures(Contract.Result<IEditorNavigationType>() != null);

            throw new NotImplementedException();
        }

        IEditorNavigationType IJavaEditorNavigationTypeRegistryService.GetEditorNavigationType(string type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            throw new NotImplementedException();
        }
    }
}
