namespace Tvl.VisualStudio.Text
{
    using System.Collections.Generic;

    public interface IContentTypeMetadata
    {
        IEnumerable<string> ContentTypes
        {
            get;
        }
    }
}
