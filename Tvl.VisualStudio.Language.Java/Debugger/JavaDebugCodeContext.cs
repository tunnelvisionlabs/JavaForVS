namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;

    [ComVisible(true)]
    public class JavaDebugCodeContext : IDebugCodeContext3, IDebugCodeContext2, IDebugMemoryContext2
    {
        private readonly JavaDebugProgram _program;
        private readonly ILocation _location;

        public JavaDebugCodeContext(JavaDebugProgram program, ILocation location)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");
            Contract.Requires<ArgumentNullException>(location != null, "location");
            _program = program;
            _location = location;
        }

        public JavaDebugProgram Program
        {
            get
            {
                return _program;
            }
        }

        public ILocation Location
        {
            get
            {
                return _location;
            }
        }

        #region IDebugMemoryContext2 Members

        public int Add(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares the memory context to each context in the given array in the manner indicated by compare flags, returning an index of the first context that matches.
        /// </summary>
        /// <param name="Compare">[in] A value from the CONTEXT_COMPARE enumeration that determines the type of comparison.</param>
        /// <param name="rgpMemoryContextSet">[in] An array of references to the IDebugMemoryContext2 objects to compare against.</param>
        /// <param name="dwMemoryContextSetLen">[in] The number of contexts in the rgpMemoryContextSet array.</param>
        /// <param name="pdwMemoryContext">[out] Returns the index of the first memory context that satisfies the comparison.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code. Returns E_COMPARE_CANNOT_COMPARE if the two contexts cannot be compared.</returns>
        /// <remarks>
        /// A debug engine (DE) does not have to support all types of comparisons, but it must support at least CONTEXT_EQUAL, CONTEXT_LESS_THAN, CONTEXT_GREATER_THAN and CONTEXT_SAME_SCOPE.
        /// </remarks>
        public int Compare(enum_CONTEXT_COMPARE Compare, IDebugMemoryContext2[] rgpMemoryContextSet, uint dwMemoryContextSetLen, out uint pdwMemoryContext)
        {
            if (rgpMemoryContextSet == null)
                throw new ArgumentNullException("rgpMemoryContextSet");
            if (rgpMemoryContextSet.Length < dwMemoryContextSetLen)
                throw new ArgumentException();

            pdwMemoryContext = 0;

            bool relational = false;
            switch (Compare)
            {
            case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN:
            case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN_OR_EQUAL:
            case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN:
            case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN_OR_EQUAL:
                relational = true;
                break;
            }

            JavaDebugCodeContext[] contexts = new JavaDebugCodeContext[dwMemoryContextSetLen];
            for (int i = 0; i < contexts.Length; i++)
            {
                if (rgpMemoryContextSet[i] == null)
                    return VSConstants.E_INVALIDARG;

                contexts[i] = rgpMemoryContextSet[i] as JavaDebugCodeContext;
                if (contexts[i] == null)
                    return AD7Constants.E_COMPARE_CANNOT_COMPARE;

                // relational tests only work if the contexts are in the same scope
                if (relational && !this.Location.GetMethod().Equals(contexts[i].Location.GetMethod()))
                    return AD7Constants.E_COMPARE_CANNOT_COMPARE;
            }

            int index;

            switch (Compare)
            {
            case enum_CONTEXT_COMPARE.CONTEXT_EQUAL:
                index = Array.FindIndex(contexts, i => this.Location.Equals(i.Location));
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN:
                index = Array.FindIndex(contexts, i => i.Location.GetCodeIndex() > this.Location.GetCodeIndex());
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_GREATER_THAN_OR_EQUAL:
                index = Array.FindIndex(contexts, i => i.Location.GetCodeIndex() >= this.Location.GetCodeIndex());
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN:
                index = Array.FindIndex(contexts, i => i.Location.GetCodeIndex() < this.Location.GetCodeIndex());
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_LESS_THAN_OR_EQUAL:
                index = Array.FindIndex(contexts, i => i.Location.GetCodeIndex() <= this.Location.GetCodeIndex());
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_SAME_FUNCTION:
                index = Array.FindIndex(contexts, i => this.Location.GetMethod().Equals(i.Location.GetMethod()));
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_SAME_MODULE:
                throw new NotImplementedException();

            case enum_CONTEXT_COMPARE.CONTEXT_SAME_PROCESS:
                index = Array.FindIndex(contexts, i => this.Program.Process == i.Program.Process);
                break;

            case enum_CONTEXT_COMPARE.CONTEXT_SAME_SCOPE:
                index = Array.FindIndex(contexts, i => this.Location.GetMethod().Equals(i.Location.GetMethod()));
                break;

            default:
                throw new ArgumentException();
            }

            if (index < 0)
            {
                return VSConstants.E_FAIL;
            }

            pdwMemoryContext = (uint)index;
            return VSConstants.S_OK;
        }

        public int GetInfo(enum_CONTEXT_INFO_FIELDS dwFields, CONTEXT_INFO[] pinfo)
        {
            if (pinfo == null)
                throw new ArgumentNullException("pinfo");
            if (pinfo.Length < 1)
                throw new ArgumentException();

            bool getModuleUrl = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_MODULEURL) != 0;
            bool getFunction = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_FUNCTION) != 0;
            bool getFunctionOffset = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_FUNCTIONOFFSET) != 0;
            bool getAddress = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS) != 0;
            bool getAddressOffset = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSOFFSET) != 0;
            bool getAddressAbsolute = (dwFields & enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSABSOLUTE) != 0;

            if (getModuleUrl)
            {
                // The name of the module where the context is located
                //pinfo[0].bstrModuleUrl = "";
                //pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_MODULEURL;
            }

            if (getFunction)
            {
                // The function name where the context is located
                pinfo[0].bstrFunction = string.Format("{0}.{1}({2})", _location.GetDeclaringType().GetName(), _location.GetMethod().GetName(), string.Join(", ", _location.GetMethod().GetArgumentTypeNames()));
                pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_FUNCTION;
            }

            if (getFunctionOffset)
            {
                // A TEXT_POSITION structure that identifies the line and column offset of the function associated with the code context
                pinfo[0].posFunctionOffset.dwLine = (uint)_location.GetLineNumber();
                pinfo[0].posFunctionOffset.dwColumn = 0;
                pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_FUNCTIONOFFSET;
            }

            if (getAddress)
            {
                // The address in code where the given context is located
                //pinfo[0].bstrAddress = "";
                //pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS;
            }

            if (getAddressOffset)
            {
                // The offset of the address in code where the given context is located
                //pinfo[0].bstrAddressOffset = "";
                //pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSOFFSET;
            }

            if (getAddressAbsolute)
            {
                // The absolute address in memory where the given context is located
                //pinfo[0].bstrAddressAbsolute = "";
                //pinfo[0].dwFields |= enum_CONTEXT_INFO_FIELDS.CIF_ADDRESSABSOLUTE;
            }

            return VSConstants.S_OK;
        }

        public int GetName(out string pbstrName)
        {
            throw new NotImplementedException();
        }

        public int Subtract(ulong dwCount, out IDebugMemoryContext2 ppMemCxt)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugCodeContext2 Members

        public int GetDocumentContext(out IDebugDocumentContext2 ppSrcCxt)
        {
            ppSrcCxt = new JavaDebugDocumentContext(_location);
            return VSConstants.S_OK;
        }

        public int GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Constants.JavaLanguageName;
            pguidLanguage = Constants.JavaLanguageGuid;
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugCodeContext3 Members

        public int GetModule(out IDebugModule2 ppModule)
        {
            // TODO: implement modules?
            ppModule = null;
            return VSConstants.E_FAIL;
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = _program.Process;
            return VSConstants.S_OK;
        }

        #endregion
    }
}
