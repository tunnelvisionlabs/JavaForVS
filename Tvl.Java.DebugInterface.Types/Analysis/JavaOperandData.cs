namespace Tvl.Java.DebugInterface.Types.Analysis
{
    public struct JavaOperandData
    {
        private readonly int _operand1;
        private readonly int _operand2;

        public JavaOperandData(byte operand1)
            : this()
        {
            _operand1 = operand1;
        }

        public JavaOperandData(sbyte operand1)
            : this()
        {
            _operand1 = operand1;
        }

        public JavaOperandData(short operand1)
            : this()
        {
            _operand1 = operand1;
        }

        public JavaOperandData(ushort operand1)
            : this()
        {
            _operand1 = operand1;
        }

        public JavaOperandData(int operand1)
            : this()
        {
            _operand1 = operand1;
        }

        public JavaOperandData(byte operand1, sbyte operand2)
            : this()
        {
            _operand1 = operand1;
            _operand2 = (byte)operand2;
        }

        public JavaOperandData(ushort operand1, byte operand2)
            : this()
        {
            _operand1 = operand1;
            _operand2 = operand2;
        }

        public JavaOperandData(ushort operand1, int operand2)
            : this()
        {
            _operand1 = operand1;
            _operand2 = operand2;
        }

        public int Operand1
        {
            get
            {
                return _operand1;
            }
        }

        public int Operand2
        {
            get
            {
                return _operand2;
            }
        }

        public sbyte InlineSByte
        {
            get
            {
                return (sbyte)_operand1;
            }
        }

        public short InlineInt16
        {
            get
            {
                return (short)_operand1;
            }
        }

        public JavaArrayType ArrayType
        {
            get
            {
                return (JavaArrayType)_operand1;
            }
        }

        public int BranchTarget
        {
            get
            {
                return _operand1;
            }
        }

        public ushort ConstantPoolIndex
        {
            get
            {
                return (ushort)_operand1;
            }
        }

        public byte VariableSlot
        {
            get
            {
                return (byte)_operand1;
            }
        }

        public ushort SwitchDataIndex
        {
            get
            {
                return (ushort)_operand1;
            }
        }

        public sbyte Increment
        {
            get
            {
                return (sbyte)_operand2;
            }
        }

        public byte Dimensions
        {
            get
            {
                return (byte)_operand2;
            }
        }

        public int SwitchInstructionSize
        {
            get
            {
                return _operand2;
            }
        }
    }
}
