namespace Tvl.VisualStudio.Text.Navigation
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IEditorNavigationTypeDefinitionMetadata
    {
        string Name
        {
            get;
        }

        [DefaultValue(null)]
        IEnumerable<string> BaseDefinition
        {
            get;
        }
    }
}
