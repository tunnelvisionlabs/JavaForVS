namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class JavaDebugReference : IDebugReference2
    {
        #region IDebugReference2 Members

        public int Compare(enum_REFERENCE_COMPARE dwCompare, IDebugReference2 pReference)
        {
            throw new NotImplementedException();
        }

        public int EnumChildren(enum_DEBUGREF_INFO_FLAGS dwFields, uint dwRadix, enum_DBG_ATTRIB_FLAGS dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugReferenceInfo2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int GetDerivedMostReference(out IDebugReference2 ppDerivedMost)
        {
            throw new NotImplementedException();
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            throw new NotImplementedException();
        }

        public int GetParent(out IDebugReference2 ppParent)
        {
            throw new NotImplementedException();
        }

        public int GetReferenceInfo(enum_DEBUGREF_INFO_FLAGS dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_REFERENCE_INFO[] pReferenceInfo)
        {
            throw new NotImplementedException();
        }

        public int GetSize(out uint pdwSize)
        {
            throw new NotImplementedException();
        }

        public int SetReferenceType(enum_REFERENCE_TYPE dwRefType)
        {
            throw new NotImplementedException();
        }

        public int SetValueAsReference(IDebugReference2[] rgpArgs, uint dwArgCount, IDebugReference2 pValue, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
