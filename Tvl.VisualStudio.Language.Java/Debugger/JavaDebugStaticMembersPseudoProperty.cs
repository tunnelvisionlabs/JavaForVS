namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.VisualStudio.Language.Java.Debugger.Collections;

    internal class JavaDebugStaticMembersPseudoProperty : IDebugProperty2
    {
        private readonly IDebugProperty2 _parent;
        private readonly IReferenceType _referenceType;
        private readonly IField[] _fields;

        public JavaDebugStaticMembersPseudoProperty(IDebugProperty2 parent, IReferenceType referenceType)
            : this(parent, referenceType, null)
        {
        }

        public JavaDebugStaticMembersPseudoProperty(IDebugProperty2 parent, IReferenceType referenceType, IEnumerable<IField> fields)
        {
            Contract.Requires<ArgumentNullException>(parent != null, "parent");
            Contract.Requires<ArgumentNullException>(referenceType != null, "referenceType");

            _parent = parent;
            _referenceType = referenceType;
            if (fields != null)
                _fields = fields.ToArray();
        }

        #region IDebugProperty2 Members

        public int EnumChildren(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, ref Guid guidFilter, enum_DBG_ATTRIB_FLAGS dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugPropertyInfo2 ppEnum)
        {
            ppEnum = null;

            bool getFullName = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME) != 0;
            bool getName = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME) != 0;
            bool getType = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE) != 0;
            bool getValue = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE) != 0;
            bool getAttributes = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB) != 0;
            bool getProperty = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP) != 0;

            bool useAutoExpandValue = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_AUTOEXPAND) != 0;
            bool noFormatting = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_RAW) != 0;
            bool noToString = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_NO_TOSTRING) != 0;

            List<DEBUG_PROPERTY_INFO> properties = new List<DEBUG_PROPERTY_INFO>();
            DEBUG_PROPERTY_INFO[] propertyInfo = new DEBUG_PROPERTY_INFO[1];

            IList<IField> fields = _fields;
            if (fields == null)
            {
                ReadOnlyCollection<IField> allFields = _referenceType.GetFields(false);
                fields = new List<IField>(allFields.Where(i => i.GetIsStatic()));
            }

            string typeName = _referenceType.GetName();

            foreach (var field in fields)
            {
                propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                if (getValue || getProperty)
                {
                    IDebugProperty2 property;
                    try
                    {
                        string name = field.GetName();
                        IType propertyType = field.GetFieldType();
                        IValue value = _referenceType.GetValue(field);
                        property = new JavaDebugProperty(this, name, typeName + "." + name, propertyType, value, false, field);
                        ErrorHandler.ThrowOnFailure(property.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                    }
                    catch (Exception e)
                    {
                        if (ErrorHandler.IsCriticalException(e))
                            throw;

                        string name = field.GetName();
                        IType propertyType = field.GetFieldType();
                        IValue value = field.GetVirtualMachine().GetMirrorOf(0);
                        property = new JavaDebugProperty(this, name, typeName + "." + name, propertyType, value, false, field);
                        ErrorHandler.ThrowOnFailure(property.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                    }
                }
                else
                {
                    if (getFullName)
                    {
                        propertyInfo[0].bstrFullName = typeName + "." + field.GetName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
                    }

                    if (getName)
                    {
                        propertyInfo[0].bstrName = field.GetName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
                    }

                    if (getType)
                    {
                        propertyInfo[0].bstrType = field.GetFieldTypeName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
                    }

                    if (getAttributes)
                    {
                        if (field.GetIsStatic())
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_STORAGE_STATIC;
                        if (field.GetIsPrivate())
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PRIVATE;
                        if (field.GetIsProtected())
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PROTECTED;
                        if (field.GetIsPublic())
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PUBLIC;

                        propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
#if false
                            bool expandable;
                            bool hasId;
                            bool canHaveId;
                            bool readOnly;
                            bool error;
                            bool sideEffect;
                            bool overloadedContainer;
                            bool boolean;
                            bool booleanTrue;
                            bool invalid;
                            bool notAThing;
                            bool autoExpanded;
                            bool timeout;
                            bool rawString;
                            bool customViewer;

                            bool accessNone;
                            bool accessPrivate;
                            bool accessProtected;
                            bool accessPublic;

                            bool storageNone;
                            bool storageGlobal;
                            bool storageStatic;
                            bool storageRegister;

                            bool noModifiers;
                            bool @virtual;
                            bool constant;
                            bool synchronized;
                            bool @volatile;

                            bool dataField;
                            bool method;
                            bool property;
                            bool @class;
                            bool baseClass;
                            bool @interface;
                            bool innerClass;
                            bool mostDerived;

                            bool multiCustomViewers;
#endif
                    }
                }

                properties.Add(propertyInfo[0]);
                continue;
            }

            ppEnum = new EnumDebugPropertyInfo(properties);
            return VSConstants.S_OK;
        }

        public int GetDerivedMostProperty(out IDebugProperty2 ppDerivedMost)
        {
            ppDerivedMost = null;
            return AD7Constants.S_GETDERIVEDMOST_NO_DERIVED_MOST;
        }

        public int GetExtendedInfo(ref Guid guidExtendedInfo, out object pExtendedInfo)
        {
            pExtendedInfo = null;
            return AD7Constants.S_GETEXTENDEDINFO_NO_EXTENDEDINFO;
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            ppMemoryBytes = null;
            return AD7Constants.S_GETMEMORYBYTES_NO_MEMORY_BYTES;
        }

        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            ppMemory = null;
            return AD7Constants.S_GETMEMORYCONTEXT_NO_MEMORY_CONTEXT;
        }

        public int GetParent(out IDebugProperty2 ppParent)
        {
            ppParent = _parent;
            return VSConstants.S_OK;
        }

        public int GetPropertyInfo(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_PROPERTY_INFO[] pPropertyInfo)
        {
            bool getFullName = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME) != 0;
            bool getName = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME) != 0;
            bool getType = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE) != 0;
            bool getValue = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE) != 0;
            bool getAttributes = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB) != 0;
            bool getProperty = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP) != 0;

            bool useAutoExpandValue = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_AUTOEXPAND) != 0;
            bool noFormatting = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_RAW) != 0;
            bool noToString = (dwFields & enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_NO_TOSTRING) != 0;

            if (getFullName)
            {
                pPropertyInfo[0].bstrFullName = "Static Members";
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
            }

            if (getName)
            {
                pPropertyInfo[0].bstrName = "Static Members";
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
            }

            /* no type */
            //if (getType)
            //{
            //}

            /* no value */
            //if (getValue)
            //{
            //}

            if (getAttributes)
            {
                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OVERLOADED_CONTAINER;
                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_CLASS;
                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PRIVATE;
                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
            }

            if (getProperty)
            {
                pPropertyInfo[0].pProperty = this;
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP;
            }

            return VSConstants.S_OK;
        }

        public int GetReference(out IDebugReference2 ppReference)
        {
            ppReference = null;
            return AD7Constants.E_GETREFERENCE_NO_REFERENCE;
        }

        public int GetSize(out uint pdwSize)
        {
            pdwSize = 0;
            return AD7Constants.S_GETSIZE_NO_SIZE;
        }

        public int SetValueAsReference(IDebugReference2[] rgpArgs, uint dwArgCount, IDebugReference2 pValue, uint dwTimeout)
        {
            return AD7Constants.E_SETVALUE_VALUE_CANNOT_BE_SET;
        }

        public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
        {
            return AD7Constants.E_SETVALUE_VALUE_CANNOT_BE_SET;
        }

        #endregion
    }
}
