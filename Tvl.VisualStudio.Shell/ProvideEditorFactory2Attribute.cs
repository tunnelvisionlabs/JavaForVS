namespace Tvl.VisualStudio.Shell
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ProvideEditorFactory2Attribute : EditorFactoryRegistrationAttribute
    {
        public ProvideEditorFactory2Attribute(Type factoryType, short nameResourceID)
            : base(factoryType, nameResourceID)
        {
        }
    }
}
