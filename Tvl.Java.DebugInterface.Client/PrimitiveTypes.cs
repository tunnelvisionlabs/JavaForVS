namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;

    internal sealed class PrimitiveTypes
    {
        private readonly BooleanType _boolean;
        private readonly ByteType _byte;
        private readonly IntegerType _integer;
        private readonly LongType _long;
        private readonly FloatType _float;
        private readonly DoubleType _double;
        private readonly ShortType _short;
        private readonly CharType _char;
        private readonly VoidType _void;

        private readonly VoidValue _voidValue;

        public PrimitiveTypes(VirtualMachine virtualMachine)
        {
            Contract.Requires<ArgumentNullException>(virtualMachine != null, "virtualMachine");

            _boolean = new BooleanType(virtualMachine);
            _byte = new ByteType(virtualMachine);
            _integer = new IntegerType(virtualMachine);
            _long = new LongType(virtualMachine);
            _float = new FloatType(virtualMachine);
            _double = new DoubleType(virtualMachine);
            _short = new ShortType(virtualMachine);
            _char = new CharType(virtualMachine);
            _void = new VoidType(virtualMachine);

            _voidValue = new VoidValue(virtualMachine);
        }

        public BooleanType Boolean
        {
            get
            {
                Contract.Ensures(Contract.Result<BooleanType>() != null);
                return _boolean;
            }
        }

        public ByteType Byte
        {
            get
            {
                Contract.Ensures(Contract.Result<ByteType>() != null);
                return _byte;
            }
        }

        public IntegerType Integer
        {
            get
            {
                Contract.Ensures(Contract.Result<IntegerType>() != null);
                return _integer;
            }
        }

        public LongType Long
        {
            get
            {
                Contract.Ensures(Contract.Result<LongType>() != null);
                return _long;
            }
        }

        public FloatType Float
        {
            get
            {
                Contract.Ensures(Contract.Result<FloatType>() != null);
                return _float;
            }
        }

        public DoubleType Double
        {
            get
            {
                Contract.Ensures(Contract.Result<DoubleType>() != null);
                return _double;
            }
        }

        public ShortType Short
        {
            get
            {
                Contract.Ensures(Contract.Result<ShortType>() != null);
                return _short;
            }
        }

        public CharType Char
        {
            get
            {
                Contract.Ensures(Contract.Result<CharType>() != null);
                return _char;
            }
        }

        public VoidType Void
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidType>() != null);
                return _void;
            }
        }

        public VoidValue VoidValue
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidValue>() != null);
                return _voidValue;
            }
        }
    }
}
