namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Shell;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ProvideComponentSelectorTabAttribute : RegistrationAttribute
    {
        private readonly Guid _componentSelectorTabGuid;
        private readonly Guid _packageGuid;
        private readonly string _name;

        private int _sortOrder = 0x35;

        public ProvideComponentSelectorTabAttribute(Type componentSelectorTabType, Type packageType, string name)
        {
            Contract.Requires<ArgumentNullException>(componentSelectorTabType != null, "componentSelectorTabType");
            Contract.Requires<ArgumentNullException>(packageType != null, "packageType");
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            _componentSelectorTabGuid = componentSelectorTabType.GUID;
            _packageGuid = packageType.GUID;
            _name = name;
        }

        public ProvideComponentSelectorTabAttribute(Guid componentSelectorTabGuid, Guid packageGuid, string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(componentSelectorTabGuid != Guid.Empty);
            Contract.Requires<ArgumentException>(packageGuid != Guid.Empty);

            _componentSelectorTabGuid = componentSelectorTabGuid;
            _packageGuid = packageGuid;
            _name = name;
        }

        public Guid ComponentSelectorTabGuid
        {
            get
            {
                return _componentSelectorTabGuid;
            }
        }

        public Guid PackageGuid
        {
            get
            {
                return _packageGuid;
            }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return _name;
            }
        }

        public int SortOrder
        {
            get
            {
                return _sortOrder;
            }

            set
            {
                _sortOrder = value;
            }
        }

        private string BaseRegistryKey
        {
            get
            {
                return string.Format(@"ComponentPickerPages\{0}", _name);
            }
        }

        public override void Register(RegistrationContext context)
        {
            using (var key = context.CreateKey(BaseRegistryKey))
            {
                key.SetValue(string.Empty, string.Empty);
                key.SetValue("Package", _packageGuid.ToString("B"));
                key.SetValue("Page", _componentSelectorTabGuid.ToString("B"));
                key.SetValue("Sort", _sortOrder);
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(BaseRegistryKey);
        }
    }
}
