namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.VisualStudio.Language.Java.Debugger.Collections;

    [ComVisible(true)]
    public class JavaDebugStackFrame
        : IDebugStackFrame3
        , IDebugStackFrame2
        , IDebugQueryEngine2
    {
        private readonly JavaDebugThread _thread;
        private readonly IStackFrame _stackFrame;
        private readonly StackFrameDebugProperty _debugProperty;
        private readonly bool _nativeMethod;

        public JavaDebugStackFrame(JavaDebugThread thread, IStackFrame stackFrame)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");
            Contract.Requires<ArgumentNullException>(stackFrame != null, "stackFrame");

            _thread = thread;
            _stackFrame = stackFrame;

            _nativeMethod = stackFrame.GetLocation().GetMethod().GetIsNative();
            if (!_nativeMethod)
                _debugProperty = new StackFrameDebugProperty(this);
        }

        public JavaDebugThread Thread
        {
            get
            {
                return _thread;
            }
        }

        public IStackFrame StackFrame
        {
            get
            {
                return _stackFrame;
            }
        }

        #region IDebugStackFrame2 Members

        /// <summary>
        /// Creates an enumerator for properties associated with the stack frame, such as local variables.
        /// </summary>
        /// <param name="dwFields">A combination of flags from the DEBUGPROP_INFO_FLAGS enumeration that specifies which fields in the enumerated DEBUG_PROPERTY_INFO structures are to be filled in.</param>
        /// <param name="nRadix">The radix to be used in formatting any numerical information.</param>
        /// <param name="guidFilter">A GUID of a filter used to select which DEBUG_PROPERTY_INFO structures are to be enumerated, such as guidFilterLocals.</param>
        /// <param name="dwTimeout">Maximum time, in milliseconds, to wait before returning from this method. Use INFINITE to wait indefinitely.</param>
        /// <param name="pcelt">Returns the number of properties enumerated. This is the same as calling the IEnumDebugPropertyInfo2::GetCount method.</param>
        /// <param name="ppEnum">Returns an IEnumDebugPropertyInfo2 object containing a list of the desired properties.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// Because this method allows all selected properties to be retrieved with a single call, it is faster than
        /// sequentially calling the IDebugStackFrame2.GetDebugProperty and IDebugProperty2.EnumChildren methods.
        /// </remarks>
        public int EnumProperties(enum_DEBUGPROP_INFO_FLAGS dwFields, uint nRadix, ref Guid guidFilter, uint dwTimeout, out uint pcelt, out IEnumDebugPropertyInfo2 ppEnum)
        {
            pcelt = 0;
            ppEnum = null;

            if (_nativeMethod || !_stackFrame.GetHasVariableInfo())
                return VSConstants.E_FAIL;

            ReadOnlyCollection<ILocalVariable> locals = _stackFrame.GetVisibleVariables();

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

            if (!_nativeMethod && !_stackFrame.GetLocation().GetMethod().GetIsStatic())
            {
                // get the 'this' property
                propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                if (getValue || getProperty)
                {
                    string name = "this";
                    IType propertyType = _stackFrame.GetLocation().GetDeclaringType();
                    IValue value = _stackFrame.GetThisObject();
                    JavaDebugProperty property = new JavaDebugProperty(_debugProperty, name, name, propertyType, value, false);
                    int hr = property.GetPropertyInfo(dwFields, nRadix, dwTimeout, null, 0, propertyInfo);
                    if (ErrorHandler.Failed(hr))
                        return hr;

                    properties.Add(propertyInfo[0]);
                }
                else
                {
                    if (getFullName)
                    {
                        propertyInfo[0].bstrFullName = "this";
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
                    }

                    if (getName)
                    {
                        propertyInfo[0].bstrName = "this";
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
                    }

                    if (getType)
                    {
                        propertyInfo[0].bstrType = _stackFrame.GetLocation().GetDeclaringType().GetName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
                    }

                    if (getAttributes)
                    {
                        propertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
                    }
                }
            }

            foreach (var local in locals)
            {
                propertyInfo[0] = default(DEBUG_PROPERTY_INFO);

                if (getValue || getProperty)
                {
                    string name = local.GetName();
                    IType propertyType = local.GetLocalType();
                    IValue value = _stackFrame.GetValue(local);
                    JavaDebugProperty property = new JavaDebugProperty(_debugProperty, name, name, propertyType, value, false);
                    int hr = property.GetPropertyInfo(dwFields, nRadix, dwTimeout, null, 0, propertyInfo);
                    if (ErrorHandler.Failed(hr))
                        return hr;

                    properties.Add(propertyInfo[0]);
                    continue;
                }
                else
                {
                    if (getFullName)
                    {
                        propertyInfo[0].bstrFullName = local.GetName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
                    }

                    if (getName)
                    {
                        propertyInfo[0].bstrName = local.GetName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
                    }

                    if (getType)
                    {
                        propertyInfo[0].bstrType = local.GetLocalTypeName();
                        propertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
                    }

                    if (getAttributes)
                    {
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
            }

            ppEnum = new EnumDebugPropertyInfo(properties);
            pcelt = (uint)properties.Count;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the code context for this stack frame.
        /// </summary>
        /// <param name="ppCodeCxt">Returns an IDebugCodeContext2 object that represents the current instruction pointer in this stack frame.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        public int GetCodeContext(out IDebugCodeContext2 ppCodeCxt)
        {
            if (_nativeMethod)
            {
                ppCodeCxt = null;
                return AD7Constants.E_NO_CODE_CONTEXT;
            }

            ppCodeCxt = new JavaDebugCodeContext(_thread.Program, _stackFrame.GetLocation());
            return VSConstants.S_OK;
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            if (_nativeMethod)
            {
                ppProperty = null;
                return VSConstants.E_FAIL;
            }

            ppProperty = _debugProperty;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the document context for this stack frame.
        /// </summary>
        /// <param name="ppCxt">Returns an IDebugDocumentContext2 object that represents the current position in a source document.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// This method is faster than calling the IDebugStackFrame2.GetCodeContext method and then calling
        /// the IDebugCodeContext2.GetDocumentContext method on the code context. However, it is not guaranteed
        /// that every debug engine (DE) will implement this method.
        /// </remarks>
        public int GetDocumentContext(out IDebugDocumentContext2 ppCxt)
        {
            if (_nativeMethod)
            {
                ppCxt = null;
                return VSConstants.E_FAIL;
            }

            ppCxt = new JavaDebugDocumentContext(_stackFrame.GetLocation());
            return VSConstants.S_OK;
        }

        public int GetExpressionContext(out IDebugExpressionContext2 ppExprCxt)
        {
            if (_nativeMethod)
            {
                ppExprCxt = null;
                return VSConstants.E_FAIL;
            }

            ppExprCxt = new JavaDebugExpressionContext(this);
            return VSConstants.S_OK;
        }

        public int GetInfo(enum_FRAMEINFO_FLAGS dwFieldSpec, uint nRadix, FRAMEINFO[] pFrameInfo)
        {
            // evaluation restrictions
            bool suppressFuncEval = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_NO_FUNC_EVAL) != 0;
            bool filterNonUserCode = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FILTER_NON_USER_CODE) != 0;
            bool suppressToString = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_NO_TOSTRING) != 0;

            // basic info items
            bool getFunctionName = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME) != 0;
            bool getReturnType = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_RETURNTYPE) != 0;
            bool getArguments = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS) != 0;
            bool getLanguage = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_LANGUAGE) != 0;
            bool getModuleName = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_MODULE) != 0;
            bool getStackRange = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_STACKRANGE) != 0;
            bool getFrame = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FRAME) != 0;
            bool getDebugInfo = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO) != 0;
            bool getStaleCode = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_STALECODE) != 0;
            //bool getAnnotatedFrame = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ANNOTATEDFRAME) != 0;
            bool getModule = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_DEBUG_MODULEP) != 0;

            // additional flags for the arguments
            bool argsArgumentTypes = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_TYPES) != 0;
            bool argsArgumentNames = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_NAMES) != 0;
            bool argsArgumentValues = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_VALUES) != 0;
            bool argsUnformatted = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_ARGS_NOFORMAT) != 0;

            // additional flags for the function name
            bool funcNameReturnType = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_RETURNTYPE) != 0;
            bool funcNameArguments = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS) != 0;
            bool funcNameArgumentTypes = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_TYPES) != 0;
            bool funcNameArgumentNames = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_NAMES) != 0;
            bool funcNameArgumentValues = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_VALUES) != 0;
            bool funcNameLanguage = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_LANGUAGE) != 0;
            bool funcNameOffset = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_OFFSET) != 0;
            bool funcNameOffsetFromLineStart = (dwFieldSpec & enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_LINES) != 0;

            FRAMEINFO frameInfo = new FRAMEINFO();

            ILocation location = null;
            IMethod method = null;
            int? lineNumber = null;

            if (getFunctionName || getReturnType || (!_nativeMethod && getDebugInfo))
            {
                location = _stackFrame.GetLocation();

                try
                {
                    if (!_nativeMethod)
                        lineNumber = location.GetLineNumber();
                }
                catch (MissingInformationException)
                {
                }

                if (getFunctionName || getReturnType)
                    method = location.GetMethod();
            }

            if (getFunctionName)
            {
                frameInfo.m_bstrFuncName = method.GetName();
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME;

                string typeName = method.GetDeclaringType().GetName();
                string methodName = method.GetName();
                frameInfo.m_bstrFuncName = typeName + "." + methodName;

                if (funcNameReturnType)
                {
                    frameInfo.m_bstrFuncName = method.GetReturnTypeName() + " " + frameInfo.m_bstrFuncName;
                    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_RETURNTYPE;
                }

                if (funcNameArguments)
                {
                    frameInfo.m_bstrFuncName += "(";

                    ReadOnlyCollection<string> argumentTypeNames = method.GetArgumentTypeNames();

                    ReadOnlyCollection<ILocalVariable> arguments = null;
                    if (funcNameArgumentNames && !_nativeMethod && method.GetHasVariableInfo())
                    {
                        arguments = method.GetArguments();
                    }

                    for (int i = 0; i < argumentTypeNames.Count; i++)
                    {
                        List<string> argumentParts = new List<string>();

                        if (funcNameArgumentTypes)
                        {
                            string argumentType = argumentTypeNames[i];
                            if (argumentType.Substring(0, argumentType.LastIndexOf('.') + 1) == "java.lang.")
                                argumentType = argumentType.Substring("java.lang.".Length);

                            argumentParts.Add(argumentType);
                            frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_TYPES;
                        }

                        if (funcNameArgumentNames && arguments != null)
                        {
                            argumentParts.Add(arguments[i].GetName());
                            frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_ARGS_NAMES;
                        }

                        if (i > 0)
                            frameInfo.m_bstrFuncName += ", ";

                        frameInfo.m_bstrFuncName += string.Join(" ", argumentParts);
                    }

                    frameInfo.m_bstrFuncName += ")";
                }

                if (funcNameOffset && !_nativeMethod)
                {
                    if (funcNameOffsetFromLineStart && lineNumber.HasValue)
                        frameInfo.m_bstrFuncName += string.Format(" Line {0}", lineNumber);
                    else
                        frameInfo.m_bstrFuncName += string.Format(" + 0x{0:x2} bytes", _stackFrame.GetLocation().GetCodeIndex());

                    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_OFFSET;
                }

                if (funcNameArguments)
                    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS;
                if (funcNameArgumentTypes)
                    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_TYPES;
                //if (funcNameArgumentNames)
                //    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_NAMES;
                //if (funcNameArgumentValues)
                //    frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FUNCNAME_ARGS_VALUES;
            }

            if (getArguments)
            {
                frameInfo.m_bstrArgs = string.Join(", ", method.GetArgumentTypeNames());
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_ARGS | enum_FRAMEINFO_FLAGS.FIF_ARGS_TYPES;
            }

            if (getReturnType)
            {
                frameInfo.m_bstrReturnType = method.GetReturnTypeName();
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_RETURNTYPE;
            }

            if (getLanguage && !_nativeMethod)
            {
                frameInfo.m_bstrLanguage = Constants.JavaLanguageName;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_LANGUAGE;
            }

            if (getModule)
            {
            }

            if (getModuleName)
            {
            }

            if (getStackRange)
            {
            }

            if (getFrame)
            {
                frameInfo.m_pFrame = this;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_FRAME;
            }

            if (getDebugInfo)
            {
                frameInfo.m_fHasDebugInfo = lineNumber.HasValue ? 1 : 0;
                frameInfo.m_dwValidFields |= enum_FRAMEINFO_FLAGS.FIF_DEBUGINFO;
            }

            if (getStaleCode)
            {
            }

            pFrameInfo[0] = frameInfo;
            return VSConstants.S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Constants.JavaLanguageName;
            pguidLanguage = Constants.JavaLanguageGuid;
            return VSConstants.S_OK;
        }

        public int GetName(out string pbstrName)
        {
            throw new NotImplementedException();
        }

        public int GetPhysicalStackRange(out ulong paddrMin, out ulong paddrMax)
        {
            throw new NotImplementedException();
        }

        public int GetThread(out IDebugThread2 ppThread)
        {
            ppThread = _thread;
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugStackFrame3 Members

        public int GetUnwindCodeContext(out IDebugCodeContext2 ppCodeContext)
        {
            throw new NotImplementedException();
        }

        public int InterceptCurrentException(enum_INTERCEPT_EXCEPTION_ACTION dwFlags, out ulong pqwCookie)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugQueryEngine2 Members

        public int GetEngineInterface(out object ppUnk)
        {
            return _thread.GetEngineInterface(out ppUnk);
        }

        #endregion

        private class StackFrameDebugProperty : IDebugProperty3, IDebugProperty2
        {
            private readonly JavaDebugStackFrame _stackFrame;

            public StackFrameDebugProperty(JavaDebugStackFrame stackFrame)
            {
                Contract.Requires<ArgumentNullException>(stackFrame != null, "stackFrame");
                _stackFrame = stackFrame;
            }

            #region IDebugProperty2 Members

            public int EnumChildren(enum_DEBUGPROP_INFO_FLAGS dwFields, uint dwRadix, ref Guid guidFilter, enum_DBG_ATTRIB_FLAGS dwAttribFilter, string pszNameFilter, uint dwTimeout, out IEnumDebugPropertyInfo2 ppEnum)
            {
                uint count;
                return _stackFrame.EnumProperties(dwFields, dwRadix, ref guidFilter, dwTimeout, out count, out ppEnum);
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
                ppParent = null;
                return AD7Constants.S_GETPARENT_NO_PARENT;
            }

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
                    if (ErrorHandler.Succeeded(_stackFrame.GetName(out pPropertyInfo[0].bstrFullName)))
                        pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME;
                }

                if (getName)
                {
                    if (ErrorHandler.Succeeded(_stackFrame.GetName(out pPropertyInfo[0].bstrName)))
                        pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME;
                }

                if (getType)
                {
                    pPropertyInfo[0].bstrType = string.Empty;
                    pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE;
                }

                if (getValue)
                {
                    // value??
                }

                if (getAttributes)
                {
                    pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_OBJ_IS_EXPANDABLE;
                    pPropertyInfo[0].dwAttrib |= enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_READONLY;
                    pPropertyInfo[0].dwFields |= enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB;
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
                return AD7Constants.E_SETVALUEASREFERENCE_NOTSUPPORTED;
            }

            public int SetValueAsString(string pszValue, uint dwRadix, uint dwTimeout)
            {
                return AD7Constants.E_SETVALUE_VALUE_CANNOT_BE_SET;
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
}
