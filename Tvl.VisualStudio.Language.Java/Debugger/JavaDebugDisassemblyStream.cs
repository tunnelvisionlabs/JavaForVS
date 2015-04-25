namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Collections;
    using Tvl.Java.DebugInterface;
    using Tvl.Java.DebugInterface.Types;
    using Tvl.Java.DebugInterface.Types.Analysis;
    using Tvl.Java.DebugInterface.Types.Loader;

    [ComVisible(true)]
    public class JavaDebugDisassemblyStream : IDebugDisassemblyStream2
    {
        private readonly JavaDebugCodeContext _executionContext;
        private readonly byte[] _bytecode;
        private readonly DisassembledMethod _disassembledMethod;
        private readonly ImmutableList<int?> _evaluationStackDepths;

        private int _currentInstructionIndex;

        public JavaDebugDisassemblyStream(JavaDebugCodeContext executionContext)
        {
            Contract.Requires<ArgumentNullException>(executionContext != null, "executionContext");

            _executionContext = executionContext;
            _bytecode = _executionContext.Location.GetMethod().GetBytecodes();
            _disassembledMethod = BytecodeDisassembler.Disassemble(_bytecode);

            var constantPool = executionContext.Location.GetDeclaringType().GetConstantPool();
            ReadOnlyCollection<ExceptionTableEntry> exceptionTable;
            try
            {
                exceptionTable = executionContext.Location.GetMethod().GetExceptionTable();
            }
            catch (DebuggerException)
            {
                exceptionTable = new ReadOnlyCollection<ExceptionTableEntry>(new ExceptionTableEntry[0]);
            }

            _evaluationStackDepths = BytecodeDisassembler.GetEvaluationStackDepths(_disassembledMethod, constantPool, exceptionTable);
        }

        #region IDebugDisassemblyStream2 Members

        /// <summary>
        /// Returns a code context object corresponding to a specified code location identifier.
        /// </summary>
        /// <param name="uCodeLocationId">
        /// [in] Specifies the code location identifier. See the Remarks section for the
        /// IDebugDisassemblyStream2.GetCodeLocationId method for a description of a code location identifier.
        /// </param>
        /// <param name="ppCodeContext">[out] Returns an IDebugCodeContext2 object that represents the associated code context.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// The code location identifier can be returned from a call to the IDebugDisassemblyStream2.GetCurrentLocation
        /// method and can appear in the DisassemblyData structure.
        /// 
        /// To convert a code context into a code location identifier, call the IDebugDisassemblyStream2.GetCodeLocationId
        /// method.
        /// </remarks>
        public int GetCodeContext(ulong uCodeLocationId, out IDebugCodeContext2 ppCodeContext)
        {
            ppCodeContext = null;
            if (uCodeLocationId > (uint)_bytecode.Length)
                return VSConstants.E_INVALIDARG;

            ppCodeContext = new JavaDebugCodeContext(_executionContext.Program, _executionContext.Location.GetMethod().GetLocationOfCodeIndex((long)uCodeLocationId));
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns a code location identifier for a particular code context.
        /// </summary>
        /// <param name="pCodeContext">[in] An IDebugCodeContext2 object to be converted to an identifier.</param>
        /// <param name="puCodeLocationId">[out] Returns the code location identifier. See Remarks.</param>
        /// <returns>
        /// If successful, returns S_OK; otherwise, returns an error code. Returns E_CODE_CONTEXT_OUT_OF_SCOPE if
        /// the code context is valid but outside the scope.
        /// </returns>
        /// <remarks>
        /// The code location identifier is specific to the debug engine (DE) supporting the disassembly. This
        /// location identifier is used internally by the DE to track positions in the code and is typically an
        /// address or offset of some kind. The only requirement is that if the code context of one location is
        /// less than the code context of another location, then the corresponding code location identifier of
        /// the first code context must also be less than the code location identifier of the second code context.
        ///
        /// To retrieve the code context of a code location identifier, call the IDebugDisassemblyStream2.GetCodeContext method.
        /// </remarks>
        public int GetCodeLocationId(IDebugCodeContext2 pCodeContext, out ulong puCodeLocationId)
        {
            puCodeLocationId = 0;

            JavaDebugCodeContext codeContext = pCodeContext as JavaDebugCodeContext;
            if (codeContext == null)
                return VSConstants.E_INVALIDARG;
            else if (!codeContext.Location.GetMethod().Equals(this._executionContext.Location.GetMethod()))
                return AD7Constants.E_CODE_CONTEXT_OUT_OF_SCOPE;

            puCodeLocationId = checked((ulong)codeContext.Location.GetCodeIndex());
            return VSConstants.S_OK;
        }

        public int GetCurrentLocation(out ulong puCodeLocationId)
        {
            puCodeLocationId = (ulong)_disassembledMethod.Instructions[_currentInstructionIndex].Offset;
            return VSConstants.S_OK;
        }

        public int GetDocument(string bstrDocumentUrl, out IDebugDocument2 ppDocument)
        {
            ppDocument = null;

            IDebugDocumentContext2 documentContext;
            int result = _executionContext.GetDocumentContext(out documentContext);
            if (ErrorHandler.Failed(result))
                return result;

            return documentContext.GetDocument(out ppDocument);
        }

        public int GetScope(enum_DISASSEMBLY_STREAM_SCOPE[] pdwScope)
        {
            if (pdwScope == null)
                throw new ArgumentNullException("pdwScope");
            if (pdwScope.Length == 0)
                throw new ArgumentException();

            pdwScope[0] = enum_DISASSEMBLY_STREAM_SCOPE.DSS_FUNCTION;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the size in instructions of this disassembly stream.
        /// </summary>
        /// <param name="pnSize">[out] Returns the size, in instructions.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// The value returned from this method can be used to allocate an array of DisassemblyData
        /// structures which is then passed to the IDebugDisassemblyStream2.Read method.
        /// </remarks>
        public int GetSize(out ulong pnSize)
        {
            if (_bytecode == null)
            {
                pnSize = 0;
                return AD7Constants.S_GETSIZE_NO_SIZE;
            }

            pnSize = (uint)_disassembledMethod.Instructions.Count;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Reads instructions starting from the current position in the disassembly stream.
        /// </summary>
        /// <param name="dwInstructions">[in] The number of instructions to disassemble. This value is also the maximum length of the prgDisassembly array.</param>
        /// <param name="dwFields">[in] A combination of flags from the DISASSEMBLY_STREAM_FIELDS enumeration that indicate which fields of prgDisassembly are to be filled out.</param>
        /// <param name="pdwInstructionsRead">[out] Returns the number of instructions actually disassembled.</param>
        /// <param name="prgDisassembly">[out] An array of DisassemblyData structures that is filled in with the disassembled code, one structure per disassembled instruction. The length of this array is dictated by the dwInstructions parameter.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// The maximum number of instructions that are available in the current scope can be obtained by calling the IDebugDisassemblyStream2.GetSize method.
        /// 
        /// The current position where the next instruction is read from can be changed by calling the IDebugDisassemblyStream2.Seek method.
        /// 
        /// The DSF_OPERANDS_SYMBOLS flag can be added to the DSF_OPERANDS flag in the dwFields parameter to indicate that symbol names should be used when disassembling instructions.
        /// </remarks>
        public int Read(uint dwInstructions, enum_DISASSEMBLY_STREAM_FIELDS dwFields, out uint pdwInstructionsRead, DisassemblyData[] prgDisassembly)
        {
            pdwInstructionsRead = 0;

            uint actualInstructions = Math.Min(dwInstructions, (uint)(_disassembledMethod.Instructions.Count - _currentInstructionIndex));

            if (prgDisassembly == null || prgDisassembly.Length < dwInstructions)
                return VSConstants.E_INVALIDARG;

            IMethod method = _executionContext.Location.GetMethod();
            ReadOnlyCollection<ILocalVariable> localVariables = method.GetHasVariableInfo() ? method.GetVariables() : new ReadOnlyCollection<ILocalVariable>(new ILocalVariable[0]);
            ReadOnlyCollection<ConstantPoolEntry> constantPool = _executionContext.Location.GetDeclaringType().GetConstantPool();

            int addressFieldWidth = 1;
            int addressFieldRange = 10 * addressFieldWidth;
            while (addressFieldRange <= _bytecode.Length)
            {
                addressFieldWidth++;
                addressFieldRange *= 10;
            }

            for (int i = 0; i < actualInstructions; i++)
            {
                JavaInstruction instruction = _disassembledMethod.Instructions[_currentInstructionIndex + i];
                int instructionStart = instruction.Offset;
                int instructionSize = instruction.Size;

#if false // bstrAddress is supposed to refer to an absolute offset
                if (dwFields.GetAddress())
                {
                    prgDisassembly[i].bstrAddress = string.Format("{0," + addressFieldWidth + "}", instructionStart);
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_ADDRESS;
                }
#endif

                if (dwFields.GetCodeBytes())
                {
                    prgDisassembly[i].bstrCodeBytes = string.Join(" ", _bytecode.Skip(instructionStart).Take(instructionSize).Select(x => x.ToString("X2")));
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_CODEBYTES;
                }

                if (dwFields.GetCodeLocationId())
                {
                    prgDisassembly[i].uCodeLocationId = (ulong)instructionStart;
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_CODELOCATIONID;
                }

                if (dwFields.GetOpCode())
                {
                    string depth = _evaluationStackDepths != null ? string.Format("[{0}]", _evaluationStackDepths[_currentInstructionIndex + i]) : string.Empty;
                    prgDisassembly[i].bstrOpcode = string.Format("{0}{1}", depth, instruction.OpCode.Name ?? "???");
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPCODE;
                }

                if (dwFields.GetAddressOffset())
                {
                    prgDisassembly[i].bstrAddressOffset = string.Format("{0," + addressFieldWidth + "}", instructionStart);
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_ADDRESSOFFSET;
                }

                if (dwFields.GetDocumentUrl())
                {
                    // "For text documents that can be represented as file names, the bstrDocumentUrl
                    // field is filled in with the file name where the source can be found, using the
                    // format file://file name."
                    string sourcePath = _executionContext.Location.GetSourcePath();
                    Uri sourceUri;
                    if (!string.IsNullOrEmpty(sourcePath) && Uri.TryCreate(sourcePath, UriKind.Absolute, out sourceUri))
                    {
                        prgDisassembly[i].bstrDocumentUrl = sourceUri.AbsoluteUri;
                        // if it starts with file:/// one of the / characters will show in disassembly view
                        if (prgDisassembly[i].bstrDocumentUrl.StartsWith("file:///"))
                            prgDisassembly[i].bstrDocumentUrl = "file://" + prgDisassembly[i].bstrDocumentUrl.Substring("file:///".Length);

                        prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_DOCUMENTURL;
                    }
                }

                if (dwFields.GetOperands())
                {
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPERANDS;

                    bool includeSymbolNames = dwFields.GetOperandsSymbols();
                    if (includeSymbolNames)
                        prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPERANDS_SYMBOLS;

                    // operand 0
                    switch (instruction.OpCode.OperandType)
                    {
                    case JavaOperandType.InlineI1:
                        prgDisassembly[i].bstrOperands = instruction.Operands.InlineSByte.ToString();
                        break;

                    case JavaOperandType.InlineI2:
                        prgDisassembly[i].bstrOperands = instruction.Operands.InlineInt16.ToString();
                        break;

                    case JavaOperandType.InlineShortBranchTarget:
                    case JavaOperandType.InlineBranchTarget:
                        prgDisassembly[i].bstrOperands = (instructionStart + instruction.Operands.Operand1).ToString();
                        break;

                    case JavaOperandType.InlineLookupSwitch:
                        prgDisassembly[i].bstrOperands = "Switch?";
                        break;

                    case JavaOperandType.InlineTableSwitch:
                        prgDisassembly[i].bstrOperands = "TableSwitch?";
                        break;

                    case JavaOperandType.InlineVar:
                    case JavaOperandType.InlineVar_I1:
                        if (includeSymbolNames)
                        {
                            int localIndex = _bytecode[instructionStart + 1];
                            int testLocation = instructionStart;
                            if (instruction.OpCode.StackBehaviorPop != JavaStackBehavior.Pop0)
                            {
                                // this is a store instruction - the variable might not be visible until the following instruction
                                testLocation += instruction.Size;
                            }

                            ILocation currentLocation = _executionContext.Location.GetMethod().GetLocationOfCodeIndex(testLocation);
                            var local = localVariables.SingleOrDefault(variable => variable.GetSlot() == localIndex && variable.GetIsVisible(currentLocation));
                            if (local != null)
                            {
                                prgDisassembly[i].bstrOperands = local.GetName();
                            }
                        }

                        if (string.IsNullOrEmpty(prgDisassembly[i].bstrOperands))
                        {
                            prgDisassembly[i].bstrOperands = "#" + instruction.Operands.VariableSlot.ToString();
                        }

                        break;

                    case JavaOperandType.InlineShortConst:
                    case JavaOperandType.InlineConst:
                    case JavaOperandType.InlineField:
                    case JavaOperandType.InlineMethod:
                    case JavaOperandType.InlineMethod_U1_0:
                    case JavaOperandType.InlineType:
                    case JavaOperandType.InlineType_U1:
                        if (includeSymbolNames)
                        {
                            int index = instruction.Operands.ConstantPoolIndex;
                            var entry = constantPool[index - 1];
                            prgDisassembly[i].bstrOperands = entry.ToString(constantPool);
                        }

                        if (string.IsNullOrEmpty(prgDisassembly[i].bstrOperands))
                        {
                            prgDisassembly[i].bstrOperands = "#" + instruction.Operands.ConstantPoolIndex.ToString();
                        }

                        break;

                    case JavaOperandType.InlineArrayType:
                        prgDisassembly[i].bstrOperands = "T_" + instruction.Operands.ArrayType.ToString().ToUpperInvariant();
                        break;

                    case JavaOperandType.InlineNone:
                        if (includeSymbolNames)
                        {
                            int? localIndex = null;

                            switch (instruction.OpCode.OpCode)
                            {
                            case JavaOpCodeTag.Aload_0:
                            case JavaOpCodeTag.Astore_0:
                            case JavaOpCodeTag.Dload_0:
                            case JavaOpCodeTag.Dstore_0:
                            case JavaOpCodeTag.Fload_0:
                            case JavaOpCodeTag.Fstore_0:
                            case JavaOpCodeTag.Iload_0:
                            case JavaOpCodeTag.Istore_0:
                            case JavaOpCodeTag.Lload_0:
                            case JavaOpCodeTag.Lstore_0:
                                localIndex = 0;
                                break;

                            case JavaOpCodeTag.Aload_1:
                            case JavaOpCodeTag.Astore_1:
                            case JavaOpCodeTag.Dload_1:
                            case JavaOpCodeTag.Dstore_1:
                            case JavaOpCodeTag.Fload_1:
                            case JavaOpCodeTag.Fstore_1:
                            case JavaOpCodeTag.Iload_1:
                            case JavaOpCodeTag.Istore_1:
                            case JavaOpCodeTag.Lload_1:
                            case JavaOpCodeTag.Lstore_1:
                                localIndex = 1;
                                break;

                            case JavaOpCodeTag.Aload_2:
                            case JavaOpCodeTag.Astore_2:
                            case JavaOpCodeTag.Dload_2:
                            case JavaOpCodeTag.Dstore_2:
                            case JavaOpCodeTag.Fload_2:
                            case JavaOpCodeTag.Fstore_2:
                            case JavaOpCodeTag.Iload_2:
                            case JavaOpCodeTag.Istore_2:
                            case JavaOpCodeTag.Lload_2:
                            case JavaOpCodeTag.Lstore_2:
                                localIndex = 2;
                                break;

                            case JavaOpCodeTag.Aload_3:
                            case JavaOpCodeTag.Astore_3:
                            case JavaOpCodeTag.Dload_3:
                            case JavaOpCodeTag.Dstore_3:
                            case JavaOpCodeTag.Fload_3:
                            case JavaOpCodeTag.Fstore_3:
                            case JavaOpCodeTag.Iload_3:
                            case JavaOpCodeTag.Istore_3:
                            case JavaOpCodeTag.Lload_3:
                            case JavaOpCodeTag.Lstore_3:
                                localIndex = 3;
                                break;
                            }

                            if (localIndex.HasValue)
                            {
                                int testLocation = instructionStart;
                                if (instruction.OpCode.StackBehaviorPop != JavaStackBehavior.Pop0)
                                {
                                    // this is a store instruction - the variable might not be visible until the following instruction
                                    testLocation += instruction.Size;
                                }

                                ILocation currentLocation = _executionContext.Location.GetMethod().GetLocationOfCodeIndex(testLocation);
                                var local = localVariables.SingleOrDefault(variable => variable.GetSlot() == localIndex && variable.GetIsVisible(currentLocation));
                                if (local != null)
                                {
                                    prgDisassembly[i].bstrOperands = local.GetName();
                                }
                            }
                        }

                        break;

                    default:
                        prgDisassembly[i].bstrOperands = string.Empty;
                        break;
                    }

                    // operand 1
                    switch (instruction.OpCode.OperandType)
                    {
                    case JavaOperandType.InlineVar_I1:
                        prgDisassembly[i].bstrOperands += " " + instruction.Operands.Increment.ToString();
                        break;

                    case JavaOperandType.InlineMethod_U1_0:
                        prgDisassembly[i].bstrOperands += " " + instruction.Operands.Operand2.ToString();
                        break;

                    case JavaOperandType.InlineType_U1:
                        prgDisassembly[i].bstrOperands += " " + instruction.Operands.Dimensions.ToString();
                        break;

                    default:
                        break;
                    }

                    // operand 2
                    if (instruction.OpCode.OperandType == JavaOperandType.InlineMethod_U1_0)
                    {
                        prgDisassembly[i].bstrOperands += " 0";
                    }
                }

                if (dwFields.GetPosition())
                {
                    try
                    {
                        ILocation location = _executionContext.Location.GetMethod().GetLocationOfCodeIndex(instructionStart);
                        prgDisassembly[i].posBeg.dwLine = (uint)location.GetLineNumber();
                        prgDisassembly[i].posBeg.dwColumn = 0;
                        prgDisassembly[i].posEnd = prgDisassembly[i].posBeg;
                        prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_POSITION;
                    }
                    catch (Exception e)
                    {
                        if (ErrorHandler.IsCriticalException(e))
                            throw;

                        prgDisassembly[i].posBeg = default(TEXT_POSITION);
                        prgDisassembly[i].posEnd = default(TEXT_POSITION);
                        prgDisassembly[i].dwFields &= ~enum_DISASSEMBLY_STREAM_FIELDS.DSF_POSITION;
                    }
                }

                if (dwFields.GetFlags())
                {
                    // TODO: determine when the following condition is met?
                    //   "Indicates that this instruction is one of the next instructions to be executed (there may be more than one)."
                    bool active = false;
                    if (active)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_INSTRUCTION_ACTIVE;

                    // Check for location information to determine when this condition is met:
                    //   "Indicates that this instruction has source. Some instructions, such as profiling or garbage collection code, have no corresponding source."
                    bool hasSource = prgDisassembly[i].dwFields.GetPosition();
                    if (hasSource)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_HASSOURCE;

                    // The current single-method disassembly only includes source from a single file, so this is for all but the first line:
                    //   "Indicates that this instruction is in a different document than the previous one."
                    bool documentChange = i == 0 && _currentInstructionIndex == 0;
                    if (documentChange)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_DOCUMENTCHANGE;

                    // Javac removes code that is statically determined to be unreachable (even in debug mode), so this is false:
                    //   "Indicates that this instruction will not be executed."
                    bool disabled = false;
                    if (disabled)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_DISABLED;

                    // This is always false in bytecode:
                    //   "Indicates that this instruction is really data (not code)."
                    bool data = false;
                    if (data)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_DATA;

                    // The Java debugging information format does not include document checksum information
                    bool documentChecksum = false;
                    if (documentChecksum)
                        prgDisassembly[i].dwFlags |= enum_DISASSEMBLY_FLAGS.DF_DOCUMENT_CHECKSUM;

                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_FLAGS;
                }

                if (dwFields.GetByteOffset() && prgDisassembly[i].dwFields.GetPosition())
                {
                    //   "The number of bytes the instruction is from the beginning of the code line."
                    int byteOffset = 0;
                    for (int j = _currentInstructionIndex + i - 1; j >= 0; j--)
                    {
                        JavaInstruction priorInstruction = _disassembledMethod.Instructions[j];
                        try
                        {
                            ILocation location = _executionContext.Location.GetMethod().GetLocationOfCodeIndex(priorInstruction.Offset);
                            if (location.GetLineNumber() != prgDisassembly[i].posBeg.dwLine)
                                break;

                            byteOffset = instruction.Offset - priorInstruction.Offset;
                        }
                        catch (Exception e)
                        {
                            if (ErrorHandler.IsCriticalException(e))
                                throw;

                            break;
                        }
                    }

                    prgDisassembly[i].dwByteOffset = (uint)Math.Max(0, byteOffset);
                    prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_BYTEOFFSET;
                }

                if (dwFields.GetSymbol())
                {
                    switch (instruction.OpCode.OperandType)
                    {
                    case JavaOperandType.InlineLookupSwitch:
                        prgDisassembly[i].bstrSymbol = "// switch";
                        break;

                    case JavaOperandType.InlineTableSwitch:
                        prgDisassembly[i].bstrSymbol = "// table switch";
                        break;

                    default:
                        break;
                    }

                    if (prgDisassembly[i].bstrSymbol != null)
                        prgDisassembly[i].dwFields |= enum_DISASSEMBLY_STREAM_FIELDS.DSF_SYMBOL;
                }
            }

            _currentInstructionIndex += (int)actualInstructions;
            pdwInstructionsRead = actualInstructions;
            return actualInstructions == dwInstructions ? VSConstants.S_OK : VSConstants.S_FALSE;
        }

        /// <summary>
        /// Moves the read pointer in the disassembly stream a given number of instructions relative to a specified position.
        /// </summary>
        /// <param name="dwSeekStart">
        /// [in] A value from the SEEK_START enumeration that specifies the relative position to begin the seek process.
        /// </param>
        /// <param name="pCodeContext">
        /// [in] The IDebugCodeContext2 object representing the code context that the seek operation is relative to. This
        /// parameter is used only if dwSeekStart = SEEK_START_CODECONTEXT; otherwise, this parameter is ignored and can be
        /// a null value.
        /// </param>
        /// <param name="uCodeLocationId">
        /// [in] The code location identifier that the seek operation is relative to. This parameter is used if
        /// dwSeekStart = SEEK_START_CODELOCID; otherwise, this parameter is ignored and can be set to 0. See the Remarks
        /// section for the IDebugDisassemblyStream2.GetCodeLocationId method for a description of a code location identifier.
        /// </param>
        /// <param name="iInstructions">
        /// [in] The number of instructions to move relative to the position specified in dwSeekStart. This value can be
        /// negative to move backwards.
        /// </param>
        /// <returns>
        /// If successful, returns S_OK. Returns S_FALSE if the seek position was to a point beyond the list of available
        /// instructions. Otherwise, returns an error code.
        /// </returns>
        /// <remarks>
        /// If the seek was to a position before the beginning of the list, the read position is set to the first instruction
        /// in the list. If the see was to a position after the end of the list, the read position is set to the last
        /// instruction in the list.
        /// </remarks>
        public int Seek(enum_SEEK_START dwSeekStart, IDebugCodeContext2 pCodeContext, ulong uCodeLocationId, long iInstructions)
        {
            switch (dwSeekStart)
            {
            case enum_SEEK_START.SEEK_START_BEGIN:
                _currentInstructionIndex = 0;
                break;

            case enum_SEEK_START.SEEK_START_CODECONTEXT:
                int error = GetCodeLocationId(pCodeContext, out uCodeLocationId);
                if (!ErrorHandler.Succeeded(error))
                    return error;

                goto case enum_SEEK_START.SEEK_START_CODELOCID;

            case enum_SEEK_START.SEEK_START_CODELOCID:
                _currentInstructionIndex = _disassembledMethod.Instructions.FindIndex(i => i.Offset == (int)uCodeLocationId);
                if (_currentInstructionIndex < 0)
                    throw new ArgumentException();

                break;

            case enum_SEEK_START.SEEK_START_CURRENT:
                break;

            case enum_SEEK_START.SEEK_START_END:
                _currentInstructionIndex = _disassembledMethod.Instructions.Count;
                break;

            default:
                throw new ArgumentException("Invalid seek start location.");
            }

            _currentInstructionIndex += (int)iInstructions;
            if (_currentInstructionIndex >= 0 && _currentInstructionIndex <= _disassembledMethod.Instructions.Count)
                return VSConstants.S_OK;

            _currentInstructionIndex = Math.Max(0, _currentInstructionIndex);
            _currentInstructionIndex = Math.Min(_disassembledMethod.Instructions.Count, _currentInstructionIndex);
            return VSConstants.S_FALSE;
        }

        #endregion
    }
}
