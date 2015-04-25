namespace Tvl.VisualStudio.Text.Navigation
{
    using System.Collections.Generic;

    public interface IEditorNavigationType
    {
        IEnumerable<IEditorNavigationType> BaseTypes
        {
            get;
        }

        string Type
        {
            get;
        }

        JavaEditorNavigationTypeDefinition Definition
        {
            get;
        }

        bool IsOfType(string type);
    }
}
