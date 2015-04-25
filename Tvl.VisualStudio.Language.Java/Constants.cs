namespace Tvl.VisualStudio.Language.Java
{
    using System;

    public static class Constants
    {
        /* The language name (used for the language service) and content type must be the same
         * due to the way Visual Studio internally registers file extensions and content types.
         */
        public const string JavaLanguageName = "Java";
        public const string JavaContentType = JavaLanguageName;
        public const string JavaFileExtension = ".java";

        // product registration
        public const int JavaLanguageResourceId = 100;
        public const string JavaLanguagePackageNameResourceString = "#110";
        public const string JavaLanguagePackageDetailsResourceString = "#111";
        public const string JavaLanguagePackageProductVersionString = "1.0";

        public const string JavaLanguagePackageGuidString = "1782E1AA-0FBD-4982-B6A8-A1110D95CA57";
        public static readonly Guid JavaLanguagePackageGuid = new Guid("{" + JavaLanguagePackageGuidString + "}");

        public const string JavaLanguageGuidString = "54098E6C-FC60-47F9-9621-0298FDB102EA";
        public static readonly Guid JavaLanguageGuid = new Guid("{" + JavaLanguageGuidString + "}");

        public const string UIContextNoSolution = "ADFC4E64-0397-11D1-9F4E-00A0C911004F";
        public const string UIContextSolutionExists = "f1536ef8-92ec-443c-9ed7-fdadf150da82";
    }
}
