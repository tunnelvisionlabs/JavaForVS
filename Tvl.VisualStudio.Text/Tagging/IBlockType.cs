namespace Tvl.VisualStudio.Text.Tagging
{
    using System.Collections.Generic;

    public interface IBlockType
    {
        IEnumerable<IBlockType> BaseTypes
        {
            get;
        }

        string BlockType
        {
            get;
        }

        bool IsOfType(string type);
    }
}
