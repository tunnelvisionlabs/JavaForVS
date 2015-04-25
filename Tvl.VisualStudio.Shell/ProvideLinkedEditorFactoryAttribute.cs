namespace Tvl.VisualStudio.Shell
{
    using System;
    using Contract = System.Diagnostics.Contracts.Contract;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ProvideLinkedEditorFactoryAttribute : EditorFactoryRegistrationAttribute
    {
        private readonly Type _linkedFactoryType;

        public ProvideLinkedEditorFactoryAttribute(Type factoryType, Type linkedFactoryType, short nameResourceID)
            : base(factoryType, nameResourceID)
        {
            Contract.Requires(factoryType != null);
            Contract.Requires<ArgumentNullException>(linkedFactoryType != null, "linkedFactoryType");

            _linkedFactoryType = linkedFactoryType;
        }

        public Type LinkedFactoryType
        {
            get
            {
                return _linkedFactoryType;
            }
        }

        public override void Register(RegistrationContext context)
        {
            using (Key key = context.CreateKey(EditorRegKey))
            {
                key.SetValue("LinkedEditorGuid", LinkedFactoryType.GUID.ToString("B"));
            }
        }
    }
}
