namespace Tvl.VisualStudio.Text.Tagging
{
    public interface ICodeReference
    {
        IDeclaration Declaration
        {
            get;
        }

        void GoToSource();
    }
}
