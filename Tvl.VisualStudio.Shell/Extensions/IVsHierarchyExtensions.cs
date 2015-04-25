namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    public static class IVsHierarchyExtensions
    {
        public delegate void ProcessHierarchyNode(IVsHierarchy hierarchyNode, uint itemid, int recursionLevel);

        public static object GetExtensibilityObject(this IVsHierarchy hierarchy, uint itemId, bool nothrow)
        {
            Contract.Requires<ArgumentNullException>(hierarchy != null, "hierarchy");

            try
            {
                object obj;

                int hr = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out obj);
                if (!nothrow)
                    ErrorHandler.ThrowOnFailure(hr);

                if (ErrorHandler.Succeeded(hr))
                    return obj;
            }
            catch (Exception ex)
            {
                if (ErrorHandler.IsCriticalException(ex))
                    throw;

                if (!nothrow)
                    throw;
            }

            return null;
        }

        public static object GetExtensibilityObject(this IVsHierarchy hierarchy, uint itemId)
        {
            Contract.Requires(hierarchy != null);
            return GetExtensibilityObject(hierarchy, itemId, false);
        }

        public static object GetExtensibilityObjectOrDefault(this IVsHierarchy hierarchy, uint itemId)
        {
            Contract.Requires(hierarchy != null);
            return GetExtensibilityObject(hierarchy, itemId, true);
        }

        public static object GetExtensibilityObject(this IVsHierarchy hierarchy)
        {
            Contract.Requires(hierarchy != null);
            return GetExtensibilityObject(hierarchy, VSConstants.VSITEMID_ROOT, true);
        }

        public static void EnumHierarchyItems(this IVsHierarchy hierarchy, uint itemid, int recursionLevel, bool isSolution, bool visibleOnly, ProcessHierarchyNode processNode)
        {
            Contract.Requires(hierarchy != null);
            Contract.Requires(processNode != null);
            EnumHierarchyItems(hierarchy, itemid, recursionLevel, isSolution, visibleOnly, false, processNode);
        }

        public static void EnumHierarchyItems(this IVsHierarchy hierarchy, uint itemid, int recursionLevel, bool isSolution, bool visibleOnly, bool nothrow, ProcessHierarchyNode processNode)
        {
            Contract.Requires<ArgumentNullException>(hierarchy != null, "hierarchy");
            Contract.Requires<ArgumentNullException>(processNode != null, "processNode");

            int hr;
            IntPtr nestedHierarchyObj;
            uint nestedItemId;
            Guid hierGuid = typeof(IVsHierarchy).GUID;

            hr = hierarchy.GetNestedHierarchy(itemid, ref hierGuid, out nestedHierarchyObj, out nestedItemId);
            if (hr == VSConstants.S_OK && nestedHierarchyObj != IntPtr.Zero)
            {
                IVsHierarchy nestedHierarchy = Marshal.GetObjectForIUnknown(nestedHierarchyObj) as IVsHierarchy;
                Marshal.Release(nestedHierarchyObj);
                if (nestedHierarchy != null)
                {
                    EnumHierarchyItems(nestedHierarchy, nestedItemId, recursionLevel, false, visibleOnly, nothrow, processNode);
                }
            }
            else
            {
                object pVar;

                processNode(hierarchy, itemid, recursionLevel);

                recursionLevel++;

                hr = hierarchy.GetProperty(itemid, ((visibleOnly || (isSolution && recursionLevel == 1)) ? (int)__VSHPROPID.VSHPROPID_FirstVisibleChild : (int)__VSHPROPID.VSHPROPID_FirstChild), out pVar);
                if (!nothrow)
                    ErrorHandler.ThrowOnFailure(hr);

                if (hr == VSConstants.S_OK)
                {
                    uint childId = GetItemId(pVar);
                    while (childId != VSConstants.VSITEMID_NIL)
                    {
                        EnumHierarchyItems(hierarchy, childId, recursionLevel, false, visibleOnly, nothrow, processNode);

                        hr = hierarchy.GetProperty(childId, ((visibleOnly || (isSolution && recursionLevel == 1)) ? (int)__VSHPROPID.VSHPROPID_NextVisibleSibling : (int)__VSHPROPID.VSHPROPID_NextSibling), out pVar);
                        if (!nothrow)
                            ErrorHandler.ThrowOnFailure(hr);

                        if (hr == VSConstants.S_OK)
                        {
                            childId = GetItemId(pVar);
                        }
                        else
                        {
                            childId = VSConstants.VSITEMID_NIL;
                        }
                    }
                }
            }
        }

        private static uint GetItemId(object pvar)
        {
            if (pvar == null)
                return VSConstants.VSITEMID_NIL;
            if (pvar is int)
                return (uint)(int)pvar;
            if (pvar is uint)
                return (uint)pvar;
            if (pvar is short)
                return (uint)(short)pvar;
            if (pvar is ushort)
                return (uint)(ushort)pvar;
            if (pvar is long)
                return (uint)(long)pvar;
            return VSConstants.VSITEMID_NIL;
        }
    }
}
