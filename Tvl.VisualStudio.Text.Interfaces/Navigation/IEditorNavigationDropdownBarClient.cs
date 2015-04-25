namespace Tvl.VisualStudio.Text.Navigation
{
    using Microsoft.VisualStudio.TextManager.Interop;

    public interface IEditorNavigationDropdownBarClient : IVsDropdownBarClient
    {
        int DropdownCount
        {
            get;
        }
    }
}
