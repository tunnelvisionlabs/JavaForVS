namespace Tvl.VisualStudio.Shell
{
    using System;
    using __VSEDITORTRUSTLEVEL = Microsoft.VisualStudio.Shell.Interop.__VSEDITORTRUSTLEVEL;
    using Contract = System.Diagnostics.Contracts.Contract;
    using CultureInfo = System.Globalization.CultureInfo;
    using LogicalView = Microsoft.VisualStudio.Shell.LogicalView;
    using ProvideViewAttribute = Microsoft.VisualStudio.Shell.ProvideViewAttribute;
    using RegistrationAttribute = Microsoft.VisualStudio.Shell.RegistrationAttribute;
    using TypeConverter = System.ComponentModel.TypeConverter;
    using TypeDescriptor = System.ComponentModel.TypeDescriptor;

    public abstract class EditorFactoryRegistrationAttribute : RegistrationAttribute
    {
        private readonly Type _factoryType;
        private readonly short _nameResourceID;
        private __VSEDITORTRUSTLEVEL _trustLevel;
        private int _commonPhysicalViewAttributes;

        protected EditorFactoryRegistrationAttribute(Type factoryType, short nameResourceID)
        {
            Contract.Requires<ArgumentNullException>(factoryType != null, "factoryType");

            _factoryType = factoryType;
            _nameResourceID = nameResourceID;
            _trustLevel = __VSEDITORTRUSTLEVEL.ETL_NeverTrusted;
        }

        protected string EditorRegKey
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"Editors\{0:B}", FactoryType.GUID);
            }
        }

        public int CommonPhysicalViewAttributes
        {
            get
            {
                return _commonPhysicalViewAttributes;
            }

            set
            {
                _commonPhysicalViewAttributes = value;
            }
        }

        public Type FactoryType
        {
            get
            {
                return _factoryType;
            }
        }

        public short NameResourceID
        {
            get
            {
                return _nameResourceID;
            }
        }

        public __VSEDITORTRUSTLEVEL TrustLevel
        {
            get
            {
                return _trustLevel;
            }

            set
            {
                _trustLevel = value;
            }
        }

        public override void Register(RegistrationContext context)
        {
            using (Key key = context.CreateKey(EditorRegKey))
            {
                key.SetValue(string.Empty, FactoryType.Name);
                key.SetValue("DisplayName", string.Format(CultureInfo.InvariantCulture, "#{0}", NameResourceID));
                key.SetValue("Package", context.ComponentType.GUID.ToString("B"));
                key.SetValue("CommonPhysicalViewAttributes", (int)_commonPhysicalViewAttributes);
                key.SetValue("EditorTrustLevel", (int)_trustLevel);
                using (Key key2 = key.CreateSubkey("LogicalViews"))
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(LogicalView));
                    object[] customAttributes = FactoryType.GetCustomAttributes(typeof(ProvideViewAttribute), true);
                    foreach (ProvideViewAttribute attribute in customAttributes)
                    {
                        if (attribute.LogicalView != LogicalView.Primary)
                        {
                            Guid guid = (Guid)converter.ConvertTo(attribute.LogicalView, typeof(Guid));
                            string physicalView = attribute.PhysicalView ?? string.Empty;
                            key2.SetValue(guid.ToString("B"), physicalView);
                        }
                    }
                }
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(EditorRegKey);
        }
    }
}
