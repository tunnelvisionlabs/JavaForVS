namespace Tvl.VisualStudio.Text.Navigation
{
    using System.Collections.Generic;

    public interface IEditorNavigationSourceMetadata
    {
        IEnumerable<string> ContentTypes
        {
            get;
        }

        //IEnumerable<string> EditorNavigationType
        //{
        //    get;
        //}
    }
}
