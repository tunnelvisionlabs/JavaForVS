namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DISASSEMBLY_STREAM_FIELDSExtensions
    {
        /// <summary>
        /// Initialize/use the bstrAddress field.
        /// </summary>
        public static bool GetAddress(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_ADDRESS) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrAddressOffset field.
        /// </summary>
        public static bool GetAddressOffset(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_ADDRESSOFFSET) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrCodeBytes field.
        /// </summary>
        public static bool GetCodeBytes(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_CODEBYTES) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrOpCode field.
        /// </summary>
        public static bool GetOpCode(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPCODE) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrOperands field.
        /// </summary>
        public static bool GetOperands(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPERANDS) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrSymbol field.
        /// </summary>
        public static bool GetSymbol(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_SYMBOL) != 0;
        }

        /// <summary>
        /// Initialize/use the uCodeLocationId field.
        /// </summary>
        public static bool GetCodeLocationId(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_CODELOCATIONID) != 0;
        }

        /// <summary>
        /// Initialize/use the posBeg and posEnd fields.
        /// </summary>
        public static bool GetPosition(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_POSITION) != 0;
        }

        /// <summary>
        /// Initialize/use the bstrDocumentUrl field.
        /// </summary>
        public static bool GetDocumentUrl(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_DOCUMENTURL) != 0;
        }

        /// <summary>
        /// Initialize/use the dwByteOffset field.
        /// </summary>
        public static bool GetByteOffset(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_BYTEOFFSET) != 0;
        }

        /// <summary>
        /// Initialize/use the dwFlags field.
        /// </summary>
        public static bool GetFlags(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_FLAGS) != 0;
        }

        /// <summary>
        /// Include symbol names in the bstrOperands field.
        /// </summary>
        public static bool GetOperandsSymbols(this enum_DISASSEMBLY_STREAM_FIELDS fields)
        {
            return (fields & enum_DISASSEMBLY_STREAM_FIELDS.DSF_OPERANDS_SYMBOLS) != 0;
        }
    }
}
