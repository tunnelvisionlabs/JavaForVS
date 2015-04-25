namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.VisualStudio.Language.Java.Debugger.Collections;

    using Stopwatch = System.Diagnostics.Stopwatch;
    using Timeout = System.Threading.Timeout;

    [ComVisible(true)]
    public class JavaDebugProperty : IDebugProperty3, IDebugProperty2
    {
        private static readonly ICollection<string> _collectionInterfaces =
            new List<string>()
            {
                "java.util.Collection",
                "java.util.Map",
            };

        private readonly IDebugProperty2 _parent;
        private readonly EvaluatedExpression _evaluatedExpression;

        private bool _useRawValues;
        private bool _preventMostDerived;

        private JavaDebugProperty _mostDerivedProperty;
        private JavaDebugProperty _superProperty;
        private JavaDebugProperty _rawValuesProperty;

        public JavaDebugProperty(IDebugProperty2 parent, string name, string fullName, IType propertyType, IValue value, bool hasSideEffects, IField field = null, IMethod method = null)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(propertyType != null, "propertyType");

            IObjectReference referencer = null;
            IType valueType = propertyType;
            bool strongReference = false;
            _evaluatedExpression = new EvaluatedExpression(name, fullName, default(ILocalVariable), referencer, field, method, default(int?), value, valueType, strongReference, hasSideEffects);
        }

        public JavaDebugProperty(IDebugProperty2 parent, EvaluatedExpression evaluatedExpression)
        {
            Contract.Requires<ArgumentNullException>(evaluatedExpression != null, "evaluatedExpression");

            _parent = parent;
            _evaluatedExpression = evaluatedExpression;
        }

        private JavaDebugProperty(JavaDebugProperty parent, string name, IType type)
        {
            Contract.Requires<ArgumentNullException>(parent != null, "parent");
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(type != null, "type");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            _parent = parent;
            _evaluatedExpression = new EvaluatedExpression(
                name,
                parent._evaluatedExpression.FullName,
                parent._evaluatedExpression.LocalVariable,
                parent._evaluatedExpression.Referencer,
                parent._evaluatedExpression.Field,
                parent._evaluatedExpression.Method,
                parent._evaluatedExpression.Index,
                parent._evaluatedExpression.Value,
                type,
                parent._evaluatedExpression.StrongReference,
                parent._evaluatedExpression.HasSideEffects);

            _useRawValues = parent._useRawValues;
            _preventMostDerived = parent._preventMostDerived;
        }

        private JavaDebugProperty(JavaDebugProperty parent, string name)
        {
            Contract.Requires<ArgumentNullException>(parent != null, "parent");
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            _parent = parent;
            _evaluatedExpression = new EvaluatedExpression(
                name,
                parent._evaluatedExpression.FullName,
                parent._evaluatedExpression.LocalVariable,
                parent._evaluatedExpression.Referencer,
                parent._evaluatedExpression.Field,
                parent._evaluatedExpression.Method,
                parent._evaluatedExpression.Index,
                parent._evaluatedExpression.Value,
                parent._evaluatedExpression.ValueType,
                parent._evaluatedExpression.StrongReference,
                parent._evaluatedExpression.HasSideEffects);
        }

        #region IDebugProperty2 Members

        public int EnumChildren(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, ref Guid guidFilter, enum_DBG_ATTRIB_FLAGS dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugPropertyInfo2 ppEnum)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool infinite = dwTimeout == unchecked((uint)Timeout.Infinite);
            TimeSpan timeout = infinite ? TimeSpan.Zero : TimeSpan.FromMilliseconds(dwTimeout);

            IObjectReference objectReference = _evaluatedExpression.Value as IObjectReference;
            IReferenceType referenceType = _evaluatedExpression.ValueType as IReferenceType;
            if (objectReference == null || referenceType == null)
            {
                ppEnum = new EnumDebugPropertyInfo(Enumerable.Empty<DEBUG_PROPERTY_INFO>());
                return VSConstants.S_OK;
            }

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

            bool isCollection = false;
            int collectionSize = 0;
            bool haveCollectionValues = false;
            ReadOnlyCollection<IValue> collectionValues = null;
            IType componentType = null;

            if (!_useRawValues)
            {
                isCollection = TryGetCollectionSize(objectReference, out collectionSize);

                if (isCollection && (getValue || getProperty))
                {
                    haveCollectionValues = TryGetCollectionValues(objectReference, out collectionValues, out componentType);
                }
            }

            if (isCollection)
            {
                for (int i = 0; i < collectionSize; i++)
                {
                    propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                    if (haveCollectionValues)
                    {
                        string name = "[" + i + "]";
                        IType propertyType = componentType;
                        IValue value = collectionValues[i];
                        JavaDebugProperty property = new JavaDebugProperty(this, name, _evaluatedExpression.FullName + name, propertyType, value, _evaluatedExpression.HasSideEffects);

                        bool timedout = !infinite && timeout < stopwatch.Elapsed;
                        enum_DEBUGPROP_INFO_FLAGS fields = dwFields;
                        if (timedout)
                            fields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE_NO_TOSTRING;

                        int hr = property.GetPropertyInfo(fields, dwRadix, dwTimeout, null, 0, propertyInfo);
                        if (ErrorHandler.Failed(hr))
                            return hr;

                        if (timedout)
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_TIMEOUT;

                        properties.Add(propertyInfo[0]);
                        continue;
                    }
                    else
                    {
                        if (getFullName)
                        {
                            propertyInfo[0].bstrFullName = _evaluatedExpression.FullName + "[" + i + "]";
                            propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
                        }

                        if (getName)
                        {
                            propertyInfo[0].bstrName = "[" + i + "]";
                            propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
                        }

                        if (getType)
                        {
                            propertyInfo[0].bstrType = componentType.GetName();
                            propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
                        }

                        if (getAttributes)
                        {
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_AUTOEXPANDED;
                            propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                            if (_evaluatedExpression.HasSideEffects)
                                propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_SIDE_EFFECT;

                            propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
                        }
                    }
                }

                bool useRawValues = referenceType is IClassType;
                if (useRawValues)
                {
                    if (_rawValuesProperty == null)
                    {
                        _rawValuesProperty = new JavaDebugProperty(this, "Raw Values")
                            {
                                _useRawValues = true
                            };
                    }

                    propertyInfo[0] = default(DEBUG_PROPERTY_INFO);
                    ErrorHandler.ThrowOnFailure(_rawValuesProperty.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                    properties.Add(propertyInfo[0]);
                }
            }
            else
            {
                ReadOnlyCollection<IField> fields = referenceType.GetFields(false);
                List<IField> staticFields = new List<IField>(fields.Where(i => i.GetIsStatic()));

                bool useMostDerived = !_preventMostDerived && _evaluatedExpression.Value != null && !_evaluatedExpression.Value.GetValueType().Equals(referenceType);
                if (useMostDerived)
                {
                    if (_mostDerivedProperty == null)
                    {
                        _mostDerivedProperty = new JavaDebugProperty(this, string.Format("[{0}]", _evaluatedExpression.Value.GetValueType().GetName()), _evaluatedExpression.Value.GetValueType())
                            {
                                _preventMostDerived = true
                            };
                    }

                    propertyInfo[0] = default(DEBUG_PROPERTY_INFO);
                    ErrorHandler.ThrowOnFailure(_mostDerivedProperty.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                    properties.Add(propertyInfo[0]);
                }

                bool useSuper = false;
                if (!useMostDerived)
                {
                    IClassType valueType = _evaluatedExpression.ValueType as IClassType;
                    if (valueType != null)
                    {
                        IClassType superClass = valueType.GetSuperclass();
                        useSuper = superClass != null && superClass.GetName() != "java.lang.Object";

                        if (useSuper)
                        {
                            if (_superProperty == null)
                            {
                                _superProperty = new JavaDebugProperty(this, "[super]", superClass)
                                    {
                                        _preventMostDerived = true
                                    };
                            }

                            propertyInfo[0] = default(DEBUG_PROPERTY_INFO);
                            ErrorHandler.ThrowOnFailure(_superProperty.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                            properties.Add(propertyInfo[0]);
                        }
                    }
                }

                foreach (var field in fields)
                {
                    if (field.GetIsStatic())
                        continue;

                    propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                    if (getValue || getProperty)
                    {
                        IDebugProperty2 property;
                        try
                        {
                            string name = field.GetName();
                            IType propertyType = field.GetFieldType();
                            IValue value = objectReference.GetValue(field);
                            property = new JavaDebugProperty(this, name, _evaluatedExpression.FullName + "." + name, propertyType, value, _evaluatedExpression.HasSideEffects, field);
                            ErrorHandler.ThrowOnFailure(property.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                        }
                        catch (Exception e)
                        {
                            if (ErrorHandler.IsCriticalException(e))
                                throw;

                            string name = field.GetName();
                            IType propertyType = field.GetFieldType();
                            IValue value = field.GetVirtualMachine().GetMirrorOf(0);
                            property = new JavaDebugProperty(this, name, _evaluatedExpression.FullName + "." + name, propertyType, value, _evaluatedExpression.HasSideEffects, field);
                            ErrorHandler.ThrowOnFailure(property.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                        }
                    }
                    else
                    {
                        if (getFullName)
                        {
                            propertyInfo[0].bstrFullName = _evaluatedExpression.FullName + "." + field.GetName();
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

                if (staticFields.Count > 0)
                {
                    propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                    JavaDebugStaticMembersPseudoProperty property = new JavaDebugStaticMembersPseudoProperty(this, referenceType, staticFields);
                    ErrorHandler.ThrowOnFailure(property.GetPropertyInfo(dwFields, dwRadix, dwTimeout, null, 0, propertyInfo));
                    properties.Add(propertyInfo[0]);
                }
            }

            ppEnum = new EnumDebugPropertyInfo(properties);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the derived-most property of a property.
        /// </summary>
        /// <param name="ppDerivedMost">Returns an IDebugProperty2 object that represents the derived-most property.</param>
        /// <returns>If successful, returns S_OK; otherwise returns error code. Returns S_GETDERIVEDMOST_NO_DERIVED_MOST if there is no derived-most property to retrieve.</returns>
        /// <remarks>
        /// For example, if this property describes an object that implements ClassRoot but which is actually an instantiation
        /// of ClassDerived that is derived from ClassRoot, then this method returns an IDebugProperty2 object describing the
        /// ClassDerived object.
        /// </remarks>
        public int GetDerivedMostProperty(out IDebugProperty2 ppDerivedMost)
        {
            ppDerivedMost = null;
            if (_evaluatedExpression.Value == null)
                return AD7Constants.S_GETDERIVEDMOST_NO_DERIVED_MOST;

            IReferenceType propertyReferenceType = _evaluatedExpression.ValueType as IReferenceType;
            IReferenceType valueReferenceType = _evaluatedExpression.Value.GetValueType() as IReferenceType;
            if (propertyReferenceType == null || valueReferenceType == null)
                return AD7Constants.S_GETDERIVEDMOST_NO_DERIVED_MOST;

            if (valueReferenceType.Equals(propertyReferenceType))
                return AD7Constants.S_GETDERIVEDMOST_NO_DERIVED_MOST;

            string castName = string.Format("({0})({1})", valueReferenceType.GetName(), _evaluatedExpression.FullName);
            ppDerivedMost = new JavaDebugProperty(this, _evaluatedExpression.Name, castName, valueReferenceType, _evaluatedExpression.Value, _evaluatedExpression.HasSideEffects, _evaluatedExpression.Field);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets extended information for the property.
        /// </summary>
        /// <param name="guidExtendedInfo">GUID that determines the type of extended information to be retrieved. See Remarks for details.</param>
        /// <param name="pExtendedInfo">
        /// Returns a VARIANT (C++) or object (C#) that can be used to retrieve the extended property information.
        /// For example, this parameter might return an IUnknown interface that can be queried for an IDebugDocumentText2
        /// interface. See Remarks for details.
        /// </param>
        /// <returns>
        /// If successful, returns S_OK; otherwise returns error code. Returns S_GETEXTENDEDINFO_NO_EXTENDEDINFO
        /// if there is no extended information to retrieve.
        /// </returns>
        /// <remarks>
        /// This method exists for the purpose of retrieving information that does not lend itself to being retrieved
        /// by calling the IDebugProperty2.GetPropertyInfo method.
        /// 
        /// The following GUIDs are typically recognized by this method (the GUID values are specified for C# since the
        /// name is not available in any assembly). Additional GUIDs can be created for internal use.
        /// </remarks>
        public int GetExtendedInfo(ref Guid guidExtendedInfo, out object pExtendedInfo)
        {
            pExtendedInfo = null;
            return AD7Constants.S_GETEXTENDEDINFO_NO_EXTENDEDINFO;
        }

        /// <summary>
        /// Gets the memory bytes that compose the value of a property.
        /// </summary>
        /// <param name="ppMemoryBytes">[out] Returns an IDebugMemoryBytes2 object that can be used to retrieve the memory that contains the value of the property.</param>
        /// <returns>If successful, returns S_OK; otherwise returns error code. Returns S_GETMEMORYBYTES_NO_MEMORY_BYTES if there are no memory bytes to retrieve.</returns>
        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            ppMemoryBytes = null;
            return AD7Constants.S_GETMEMORYBYTES_NO_MEMORY_BYTES;
        }

        /// <summary>
        /// Gets the memory context of the property value.
        /// </summary>
        /// <param name="ppMemory">[out] Returns the IDebugMemoryContext2 object that represents the memory associated with this property.</param>
        /// <returns>If successful, returns S_OK; otherwise returns error code. Returns S_GETMEMORYCONTEXT_NO_MEMORY_CONTEXT if there is no memory context to retrieve.</returns>
        public int GetMemoryContext(out IDebugMemoryContext2 ppMemory)
        {
            ppMemory = null;
            return AD7Constants.S_GETMEMORYCONTEXT_NO_MEMORY_CONTEXT;
        }

        public int GetParent(out IDebugProperty2 ppParent)
        {
            ppParent = _parent;
            return ppParent != null ? VSConstants.S_OK : AD7Constants.S_GETPARENT_NO_PARENT;
        }

        /// <summary>
        /// Gets the DEBUG_PROPERTY_INFO structure that describes a property.
        /// </summary>
        /// <param name="dwFields">[in] A combination of values from the DEBUGPROP_INFO_FLAGS enumeration that specifies which fields are to be filled out in the pPropertyInfo structure.</param>
        /// <param name="dwRadix">[in] Radix to be used in formatting any numerical information.</param>
        /// <param name="dwTimeout">[in] Specifies the maximum time, in milliseconds, to wait before returning from this method. Use INFINITE to wait indefinitely.</param>
        /// <param name="rgpArgs">[in, out] Reserved for future use; set to a null value.</param>
        /// <param name="dwArgCount">[in] Reserved for future use; set to zero.</param>
        /// <param name="pPropertyInfo">[out] A DEBUG_PROPERTY_INFO structure that is filled in with the description of the property.</param>
        /// <returns>If successful, returns S_OK; otherwise returns error code.</returns>
        public int GetPropertyInfo(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, uint dwTimeout, IDebugReference2[] rgpArgs, uint dwArgCount, DEBUG_PROPERTY_INFO[] pPropertyInfo)
        {
            if (pPropertyInfo == null)
                throw new ArgumentNullException("pPropertyInfo");
            if (pPropertyInfo.Length == 0)
                throw new ArgumentException();

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
                pPropertyInfo[0].bstrFullName = _evaluatedExpression.FullName;
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
            }

            if (getName)
            {
                pPropertyInfo[0].bstrName = _evaluatedExpression.Name;
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
            }

            if (getType)
            {
                pPropertyInfo[0].bstrType = _evaluatedExpression.ValueType.GetName();
                if (_evaluatedExpression.Value != null && !_evaluatedExpression.Value.GetValueType().Equals(_evaluatedExpression.ValueType))
                    pPropertyInfo[0].bstrType += " {" + _evaluatedExpression.Value.GetValueType().GetName() + "}";

                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
            }

            if (getValue)
            {
                if (_evaluatedExpression.Value == null)
                {
                    pPropertyInfo[0].bstrValue = "null";
                }
                if (_evaluatedExpression.Value is IVoidValue)
                {
                    pPropertyInfo[0].bstrValue = "The expression has been evaluated and has no value.";
                }
                else if (_evaluatedExpression.Value is IPrimitiveValue)
                {
                    IBooleanValue booleanValue = _evaluatedExpression.Value as IBooleanValue;
                    if (booleanValue != null)
                    {
                        pPropertyInfo[0].bstrValue = booleanValue.GetValue().ToString().ToLowerInvariant();
                        pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_BOOLEAN;
                        if (booleanValue.GetValue())
                            pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_BOOLEAN_TRUE;
                    }

                    IByteValue byteValue = _evaluatedExpression.Value as IByteValue;
                    if (byteValue != null)
                        pPropertyInfo[0].bstrValue = byteValue.GetValue().ToString();

                    ICharValue charValue = _evaluatedExpression.Value as ICharValue;
                    if (charValue != null)
                    {
                        pPropertyInfo[0].bstrValue = EscapeSpecialCharacters(charValue.GetValue().ToString(), 10, '\'');
                    }

                    IShortValue shortValue = _evaluatedExpression.Value as IShortValue;
                    if (shortValue != null)
                        pPropertyInfo[0].bstrValue = shortValue.GetValue().ToString();

                    IIntegerValue integerValue = _evaluatedExpression.Value as IIntegerValue;
                    if (integerValue != null)
                        pPropertyInfo[0].bstrValue = integerValue.GetValue().ToString();

                    ILongValue longValue = _evaluatedExpression.Value as ILongValue;
                    if (longValue != null)
                        pPropertyInfo[0].bstrValue = longValue.GetValue().ToString();

                    IFloatValue floatValue = _evaluatedExpression.Value as IFloatValue;
                    if (floatValue != null)
                        pPropertyInfo[0].bstrValue = floatValue.GetValue().ToString();

                    IDoubleValue doubleValue = _evaluatedExpression.Value as IDoubleValue;
                    if (doubleValue != null)
                        pPropertyInfo[0].bstrValue = doubleValue.GetValue().ToString();
                }
                else if (_evaluatedExpression.Value is IArrayReference)
                {
                    IArrayReference arrayReference = _evaluatedExpression.Value as IArrayReference;
                    int length = arrayReference.GetLength();
                    IArrayType arrayType = (IArrayType)arrayReference.GetReferenceType();
                    pPropertyInfo[0].bstrValue = string.Format("{{{0}[{1}]}}", arrayType.GetComponentTypeName(), length);
                    if (length > 0)
                        pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                }
                else if (_evaluatedExpression.Value is IObjectReference)
                {
                    IStringReference stringReference = _evaluatedExpression.Value as IStringReference;
                    if (stringReference != null)
                    {
                        pPropertyInfo[0].bstrValue = EscapeSpecialCharacters(stringReference.GetValue(), 120, '"');
                    }
                    else
                    {
                        IObjectReference objectReference = _evaluatedExpression.Value as IObjectReference;
                        if (objectReference != null)
                        {
                            int collectionSize;
                            if (TryGetCollectionSize(objectReference, out collectionSize))
                            {
                                pPropertyInfo[0].bstrValue = string.Format("{{size() = {0}}}", collectionSize);
                                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                            }
                            else
                            {
                                string displayValue = null;
                                IClassType classType = objectReference.GetReferenceType() as IClassType;
                                if (noToString || classType == null)
                                {
                                    displayValue = objectReference.GetReferenceType().GetName();
                                }
                                else
                                {
                                    IMethod method = classType.GetConcreteMethod("toString", "()Ljava/lang/String;");
                                    using (IStrongValueHandle<IValue> result = objectReference.InvokeMethod(null, method, InvokeOptions.None))
                                    {
                                        if (result != null)
                                        {
                                            stringReference = result.Value as IStringReference;
                                            if (stringReference != null)
                                                displayValue = stringReference.GetValue();
                                        }
                                    }

                                    if (displayValue == null)
                                    {
                                        IClassType objectClass = classType;
                                        while (true)
                                        {
                                            IClassType parentClass = objectClass.GetSuperclass();
                                            if (parentClass != null)
                                                objectClass = parentClass;
                                            else
                                                break;
                                        }

                                        IMethod objectToStringMethod = objectClass.GetConcreteMethod("toString", "()Ljava/lang/String;");

                                        // fall back to a non-virtual call
                                        using (IStrongValueHandle<IValue> result = objectReference.InvokeMethod(null, objectToStringMethod, InvokeOptions.NonVirtual | InvokeOptions.SingleThreaded))
                                        {
                                            if (result != null)
                                            {
                                                stringReference = result.Value as IStringReference;
                                                if (stringReference != null)
                                                    displayValue = stringReference.GetValue();
                                            }
                                        }
                                    }
                                }

                                pPropertyInfo[0].bstrValue = "{" + displayValue + "}";
                                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                            }
                        }
                        else
                        {
                            pPropertyInfo[0].bstrValue = "Unrecognized value";
                            pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_ERROR;
                        }
                    }
                }

                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE;
            }

            if (getAttributes)
            {
                if (_evaluatedExpression.Field != null)
                {
                    if (_evaluatedExpression.Field.GetIsPrivate())
                        pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PRIVATE;
                    if (_evaluatedExpression.Field.GetIsProtected())
                        pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PROTECTED;
                    if (_evaluatedExpression.Field.GetIsPublic())
                        pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_ACCESS_PUBLIC;
                }

                pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
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

            if (getProperty)
            {
                pPropertyInfo[0].pProperty = this;
                pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP;
            }

            return VSConstants.S_OK;
        }

        private static bool TryGetCollectionSize(IObjectReference objectReference, out int size)
        {
            size = 0;

            IArrayReference arrayReference = objectReference as IArrayReference;
            if (arrayReference != null)
            {
                size = arrayReference.GetLength();
                return true;
            }

            IClassType classType = objectReference.GetReferenceType() as IClassType;
            if (classType == null)
                return false;

            ReadOnlyCollection<IInterfaceType> interfaces = classType.GetInterfaces(true);
            if (interfaces.Any(i => _collectionInterfaces.Contains(i.GetName())))
            {
                IMethod sizeMethod = classType.GetConcreteMethod("size", "()I");
                using (IStrongValueHandle<IValue> result = objectReference.InvokeMethod(null, sizeMethod, InvokeOptions.None))
                {
                    if (result == null)
                        return false;

                    IIntegerValue integerValue = result.Value as IIntegerValue;
                    if (integerValue != null)
                    {
                        size = integerValue.GetValue();
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool TryGetCollectionValues(IObjectReference objectReference, out ReadOnlyCollection<IValue> values, out IType elementType)
        {
            IArrayReference arrayReference = objectReference as IArrayReference;
            if (arrayReference == null)
            {
                int size;
                if (TryGetCollectionSize(objectReference, out size))
                {
                    IClassType classType = objectReference.GetReferenceType() as IClassType;
                    if (classType != null)
                    {
                        IObjectReference collectionObject = null;

                        ReadOnlyCollection<IInterfaceType> interfaces = classType.GetInterfaces(true);
                        if (interfaces.Any(i => i.GetName() == "java.util.Collection"))
                        {
                            collectionObject = objectReference;
                        }
                        else if (interfaces.Any(i => i.GetName() == "java.util.Map"))
                        {
                            IMethod entrySetMethod = classType.GetConcreteMethod("entrySet", "()Ljava/util/Set;");
                            IStrongValueHandle<IValue> result = objectReference.InvokeMethod(null, entrySetMethod, InvokeOptions.None);
                            if (result != null)
                                collectionObject = result.Value as IObjectReference;
                        }

                        if (collectionObject != null)
                        {
                            IClassType collectionObjectType = collectionObject.GetReferenceType() as IClassType;
                            if (collectionObjectType != null)
                            {
                                IMethod toArrayMethod = collectionObjectType.GetConcreteMethod("toArray", "()[Ljava/lang/Object;");
                                IStrongValueHandle<IValue> result = collectionObject.InvokeMethod(null, toArrayMethod, InvokeOptions.None);
                                if (result != null)
                                    arrayReference = result.Value as IArrayReference;
                            }
                        }
                    }
                }
            }

            if (arrayReference != null)
            {
                values = arrayReference.GetValues();
                IArrayType arrayType = (IArrayType)arrayReference.GetReferenceType();
                elementType = arrayType.GetComponentType();
                return true;
            }

            values = null;
            elementType = null;
            return false;
        }

        private static string EscapeSpecialCharacters(string value, int maxLength, char quoteCharacter)
        {
            bool cropped = value.Length > maxLength;
            if (cropped)
                value = value.Substring(0, maxLength) + "...";

            StringBuilder result = new StringBuilder(value.Length + 2);
            result.Append(quoteCharacter);

            foreach (char ch in value)
            {
                switch (ch)
                {
                case '\\':
                    result.Append("\\\\");
                    continue;

                case '\r':
                    result.Append("\\r");
                    continue;

                case '\n':
                    result.Append("\\n");
                    continue;

                case '\t':
                    result.Append("\\t");
                    continue;

                case '\f':
                    result.Append("\\f");
                    continue;

                case '\'':
                    if (quoteCharacter != '\'')
                        goto default;

                    result.Append("\\'");
                    continue;

                case '"':
                    if (quoteCharacter != '"')
                        goto default;

                    result.Append("\\\"");
                    continue;

                case '\b':
                    result.Append("\\b");
                    continue;

                default:
                    // TODO: reduce these to octal escapes where possible
                    if (char.IsControl(ch))
                        result.Append("\\u").Append(((int)ch).ToString("X4"));
                    else
                        result.Append(ch);
                    continue;
                }
            }

            if (!cropped)
                result.Append(quoteCharacter);

            return result.ToString();
        }

        /// <summary>
        /// Returns a reference to the property's value.
        /// </summary>
        /// <param name="ppReference">[out] Returns an IDebugReference2 object representing a reference to the property's value.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code, typically E_NOTIMPL or E_GETREFERENCE_NO_REFERENCE.</returns>
        public int GetReference(out IDebugReference2 ppReference)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the size, in bytes, of the property value.
        /// </summary>
        /// <param name="pdwSize">[out] Returns the size, in bytes, of the property value.</param>
        /// <returns>If successful, returns S_OK; otherwise returns error code. Returns S_GETSIZE_NO_SIZE if the property has no size.</returns>
        public int GetSize(out uint pdwSize)
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

        #region IDebugProperty3 Members

        public int CreateObjectID()
        {
            throw new NotImplementedException();
        }

        public int DestroyObjectID()
        {
            throw new NotImplementedException();
        }

        public int GetCustomViewerCount(out uint pcelt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of custom viewers associated with this property.
        /// </summary>
        /// <param name="celtSkip">[in] The number of viewers to skip over.</param>
        /// <param name="celtRequested">[in] The number of viewers to retrieve (also specifies the size of the rgViewers array).</param>
        /// <param name="rgViewers">[in, out] Array of DEBUG_CUSTOM_VIEWER structures to be filled in.</param>
        /// <param name="pceltFetched">[out] The actual number of viewers returned.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// To support type visualizers, this method forwards the call to the IEEVisualizerService.GetCustomViewerList method.
        /// If the expression evaluator also supports custom viewers for this property's type, this method can append the
        /// appropriate custom viewers to the list.
        /// 
        /// See Type Visualizer and Custom Viewer for details on the differences between type visualizers and custom viewers.
        /// </remarks>
        public int GetCustomViewerList(uint celtSkip, uint celtRequested, DEBUG_CUSTOM_VIEWER[] rgViewers, out uint pceltFetched)
        {
            throw new NotImplementedException();
        }

        public int GetStringCharLength(out uint pLen)
        {
            throw new NotImplementedException();
        }

        public int GetStringChars(uint buflen, ushort[] rgString, out uint pceltFetched)
        {
            throw new NotImplementedException();
        }

        public int SetValueAsStringWithError(string pszValue, uint dwRadix, uint dwTimeout, out string errorString)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
