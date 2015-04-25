namespace Tvl.VisualStudio.Text.Tagging
{
    using Microsoft.VisualStudio.Text.Tagging;

    public interface ICodeBlockTag : ICodeReference, ITag
    {
        IBlockType BlockType
        {
            get;
        }

        int Level
        {
            get;
        }
    }
}
