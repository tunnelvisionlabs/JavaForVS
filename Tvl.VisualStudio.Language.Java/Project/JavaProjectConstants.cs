namespace Tvl.VisualStudio.Language.Java.Project
{
    using Guid = System.Guid;

    public static class JavaProjectConstants
    {
        public const int JavaProjectResourceId = 200;
        public const string JavaProjectPackageNameResourceString = "#210";
        public const string JavaProjectPackageDetailsResourceString = "#211";
        public const string JavaProjectPackageProductVersionString = "1.0";

        public const string JavaProjectPackageGuidString = "E9AB1381-6EDF-4F80-A342-8005A678B89B";
        public static readonly Guid JavaProjectPackageGuid = new Guid("{" + JavaProjectPackageGuidString + "}");

        public const string JavaProjectFactoryGuidString = "BE0A4C86-FC82-4F70-8ED6-D24C8B13E7BF";
        public static readonly Guid JavaProjectGuid = new Guid("{" + JavaProjectFactoryGuidString + "}");

        // Property pages
        public const string JavaGeneralPropertyPageGuidString = "88F03B6B-FB44-4403-B6D9-45E8C55BA694";
        public static readonly Guid JavaGeneralPropertyPageGuid = new Guid("{" + JavaGeneralPropertyPageGuidString + "}");

        public const string JavaApplicationPropertyPageGuidString = "9AEC2F56-7F7B-4939-887C-A3EA832613E0";
        public static readonly Guid JavaApplicationPropertyPageGuid = new Guid("{" + JavaApplicationPropertyPageGuidString + "}");

        public const string JavaBuildEventsPropertyPageGuidString = "65E17C43-D7A8-4B97-A4D8-A66CFE97558C";
        public static readonly Guid JavaBuildEventsPropertyPageGuid = new Guid("{" + JavaBuildEventsPropertyPageGuidString + "}");

        public const string JavaBuildPropertyPageGuidString = "396E8C81-6642-4E74-8A44-DA16A6B57B7A";
        public static readonly Guid JavaBuildPropertyPageGuid = new Guid("{" + JavaBuildPropertyPageGuidString + "}");

        public const string JavaDebugPropertyPageGuidString = "9CFF1FD3-FFF7-4A96-AB40-4921BC9A95EC";
        public static readonly Guid JavaDebugPropertyPageGuid = new Guid("{" + JavaDebugPropertyPageGuidString + "}");

        // Component selector pages
        public const string MavenComponentSelectorGuidString = "9EECD938-1C70-4A5A-9179-95CDC5A42877";
        public static readonly Guid MavenComponentSelectorGuid = new Guid("{" + MavenComponentSelectorGuidString + "}");
    }
}
