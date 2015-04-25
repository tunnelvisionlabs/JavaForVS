namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Microsoft.VisualStudio.Utilities;
    using Guid = System.Guid;

    [ComVisible(true)]
    public abstract class DebugEvent : IDebugEvent2, IPropertyOwner
    {
        private readonly enum_EVENTATTRIBUTES _attributes;
        private readonly PropertyCollection _properties = new PropertyCollection();

        protected DebugEvent(enum_EVENTATTRIBUTES attributes)
        {
            _attributes = attributes;
        }

        public enum_EVENTATTRIBUTES Attributes
        {
            get
            {
                return _attributes;
            }
        }

        public PropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        public abstract Guid EventGuid
        {
            get;
        }

        public int GetAttributes(out uint pdwAttrib)
        {
            pdwAttrib = (uint)_attributes;
            return VSConstants.S_OK;
        }
    }
}
