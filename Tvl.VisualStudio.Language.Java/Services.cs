#pragma warning disable 169 // The field 'fieldname' is never used

namespace Tvl.VisualStudio.Language.Java
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;

    public static class Services
    {
        [Export]
        [Name(Constants.JavaContentType)]
        [BaseDefinition("code")]
        private static readonly ContentTypeDefinition JavaContentTypeDefinition;

        [Export]
        [FileExtension(Constants.JavaFileExtension)]
        [ContentType(Constants.JavaContentType)]
        private static readonly FileExtensionToContentTypeDefinition JavaFileExtensionToContentTypeDefinition;
    }
}
