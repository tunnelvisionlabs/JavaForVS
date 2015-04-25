namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System;

    public class JavaInstruction
    {
        private readonly int _offset;
        private readonly JavaOpCode _opcode;
        private readonly JavaOperandData _operands;

        public JavaInstruction(int offset, JavaOpCode opcode, JavaOperandData operands)
        {
            _offset = offset;
            _opcode = opcode;
            _operands = operands;
        }

        public int Offset
        {
            get
            {
                return _offset;
            }
        }

        public JavaOpCode OpCode
        {
            get
            {
                return _opcode;
            }
        }

        public JavaOperandData Operands
        {
            get
            {
                return _operands;
            }
        }

        public int Size
        {
            get
            {
                if (_opcode.Size > 0)
                {
                    return _opcode.Size;
                }
                else
                {
                    switch (_opcode.OpCode)
                    {
                    case JavaOpCodeTag.Lookupswitch:
                    case JavaOpCodeTag.Tableswitch:
                        return _operands.SwitchInstructionSize;

                    case JavaOpCodeTag.Wide:
                        throw new NotImplementedException();

                    default:
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}
