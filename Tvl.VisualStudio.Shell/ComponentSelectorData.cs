namespace Tvl.VisualStudio.Shell
{
    using CultureInfo = System.Globalization.CultureInfo;
    using Guid = System.Guid;
    using Version = System.Version;
    using VSCOMPONENTTYPE = Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTTYPE;

    public class ComponentSelectorData
    {
        /// <summary>
        /// String containing the full path to component file.
        /// </summary>
        public string File
        {
            get;
            set;
        }

        /// <summary>
        /// String containing the project reference.
        /// </summary>
        public string ProjectReference
        {
            get;
            set;
        }

        /// <summary>
        /// String containing the human-readable name of component (not identity information).
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// GUID specifying the type library.
        /// </summary>
        public Guid TypeLibrary
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the locale of the library.
        /// </summary>
        public CultureInfo TypeLibraryCulture
        {
            get;
            set;
        }

        /// <summary>
        /// DWORD containing custom information.
        /// </summary>
        public int CustomInformation
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the component type. Values are taken from the VSCOMPONENTTYPE enumeration.
        /// </summary>
        public VSCOMPONENTTYPE ComponentType
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the file's version number.
        /// </summary>
        public Version FileVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the type library's version number.
        /// </summary>
        public Version TypeLibraryVersion
        {
            get;
            set;
        }
    }
}
