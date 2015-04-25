namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System.Collections.ObjectModel;

    public struct JavaOpCode
    {
        /// <summary></summary>
        public static readonly JavaOpCode Aaload;
        /// <summary></summary>
        public static readonly JavaOpCode Aastore;
        /// <summary></summary>
        public static readonly JavaOpCode Aconst_null;
        /// <summary></summary>
        public static readonly JavaOpCode Aload;
        /// <summary></summary>
        public static readonly JavaOpCode Aload_0;
        /// <summary></summary>
        public static readonly JavaOpCode Aload_1;
        /// <summary></summary>
        public static readonly JavaOpCode Aload_2;
        /// <summary></summary>
        public static readonly JavaOpCode Aload_3;
        /// <summary></summary>
        public static readonly JavaOpCode Anewarray;
        /// <summary></summary>
        public static readonly JavaOpCode Areturn;
        /// <summary></summary>
        public static readonly JavaOpCode Arraylength;
        /// <summary></summary>
        public static readonly JavaOpCode Astore;
        /// <summary></summary>
        public static readonly JavaOpCode Astore_0;
        /// <summary></summary>
        public static readonly JavaOpCode Astore_1;
        /// <summary></summary>
        public static readonly JavaOpCode Astore_2;
        /// <summary></summary>
        public static readonly JavaOpCode Astore_3;
        /// <summary></summary>
        public static readonly JavaOpCode Athrow;
        /// <summary></summary>
        public static readonly JavaOpCode Baload;
        /// <summary></summary>
        public static readonly JavaOpCode Bastore;
        /// <summary></summary>
        public static readonly JavaOpCode Bipush;
        /// <summary></summary>
        public static readonly JavaOpCode Breakpoint;
        /// <summary></summary>
        public static readonly JavaOpCode Caload;
        /// <summary></summary>
        public static readonly JavaOpCode Castore;
        /// <summary></summary>
        public static readonly JavaOpCode Checkcast;
        /// <summary></summary>
        public static readonly JavaOpCode D2f;
        /// <summary></summary>
        public static readonly JavaOpCode D2i;
        /// <summary></summary>
        public static readonly JavaOpCode D2l;
        /// <summary></summary>
        public static readonly JavaOpCode Dadd;
        /// <summary></summary>
        public static readonly JavaOpCode Daload;
        /// <summary></summary>
        public static readonly JavaOpCode Dastore;
        /// <summary></summary>
        public static readonly JavaOpCode Dcmpg;
        /// <summary></summary>
        public static readonly JavaOpCode Dcmpl;
        /// <summary></summary>
        public static readonly JavaOpCode Dconst_0;
        /// <summary></summary>
        public static readonly JavaOpCode Dconst_1;
        /// <summary></summary>
        public static readonly JavaOpCode Ddiv;
        /// <summary></summary>
        public static readonly JavaOpCode Dload;
        /// <summary></summary>
        public static readonly JavaOpCode Dload_0;
        /// <summary></summary>
        public static readonly JavaOpCode Dload_1;
        /// <summary></summary>
        public static readonly JavaOpCode Dload_2;
        /// <summary></summary>
        public static readonly JavaOpCode Dload_3;
        /// <summary></summary>
        public static readonly JavaOpCode Dmul;
        /// <summary></summary>
        public static readonly JavaOpCode Dneg;
        /// <summary></summary>
        public static readonly JavaOpCode Drem;
        /// <summary></summary>
        public static readonly JavaOpCode Dreturn;
        /// <summary></summary>
        public static readonly JavaOpCode Dstore;
        /// <summary></summary>
        public static readonly JavaOpCode Dstore_0;
        /// <summary></summary>
        public static readonly JavaOpCode Dstore_1;
        /// <summary></summary>
        public static readonly JavaOpCode Dstore_2;
        /// <summary></summary>
        public static readonly JavaOpCode Dstore_3;
        /// <summary></summary>
        public static readonly JavaOpCode Dsub;
        /// <summary></summary>
        public static readonly JavaOpCode Dup;
        /// <summary></summary>
        public static readonly JavaOpCode Dup_x1;
        /// <summary></summary>
        public static readonly JavaOpCode Dup_x2;
        /// <summary></summary>
        public static readonly JavaOpCode Dup2;
        /// <summary></summary>
        public static readonly JavaOpCode Dup2_x1;
        /// <summary></summary>
        public static readonly JavaOpCode Dup2_x2;
        /// <summary></summary>
        public static readonly JavaOpCode F2d;
        /// <summary></summary>
        public static readonly JavaOpCode F2i;
        /// <summary></summary>
        public static readonly JavaOpCode F2l;
        /// <summary></summary>
        public static readonly JavaOpCode Fadd;
        /// <summary></summary>
        public static readonly JavaOpCode Faload;
        /// <summary></summary>
        public static readonly JavaOpCode Fastore;
        /// <summary></summary>
        public static readonly JavaOpCode Fcmpg;
        /// <summary></summary>
        public static readonly JavaOpCode Fcmpl;
        /// <summary></summary>
        public static readonly JavaOpCode Fconst_0;
        /// <summary></summary>
        public static readonly JavaOpCode Fconst_1;
        /// <summary></summary>
        public static readonly JavaOpCode Fconst_2;
        /// <summary></summary>
        public static readonly JavaOpCode Fdiv;
        /// <summary></summary>
        public static readonly JavaOpCode Fload;
        /// <summary></summary>
        public static readonly JavaOpCode Fload_0;
        /// <summary></summary>
        public static readonly JavaOpCode Fload_1;
        /// <summary></summary>
        public static readonly JavaOpCode Fload_2;
        /// <summary></summary>
        public static readonly JavaOpCode Fload_3;
        /// <summary></summary>
        public static readonly JavaOpCode Fmul;
        /// <summary></summary>
        public static readonly JavaOpCode Fneg;
        /// <summary></summary>
        public static readonly JavaOpCode Frem;
        /// <summary></summary>
        public static readonly JavaOpCode Freturn;
        /// <summary></summary>
        public static readonly JavaOpCode Fstore;
        /// <summary></summary>
        public static readonly JavaOpCode Fstore_0;
        /// <summary></summary>
        public static readonly JavaOpCode Fstore_1;
        /// <summary></summary>
        public static readonly JavaOpCode Fstore_2;
        /// <summary></summary>
        public static readonly JavaOpCode Fstore_3;
        /// <summary></summary>
        public static readonly JavaOpCode Fsub;
        /// <summary></summary>
        public static readonly JavaOpCode Getfield;
        /// <summary></summary>
        public static readonly JavaOpCode Getstatic;
        /// <summary></summary>
        public static readonly JavaOpCode Goto;
        /// <summary></summary>
        public static readonly JavaOpCode Goto_w;
        /// <summary></summary>
        public static readonly JavaOpCode I2b;
        /// <summary></summary>
        public static readonly JavaOpCode I2c;
        /// <summary></summary>
        public static readonly JavaOpCode I2d;
        /// <summary></summary>
        public static readonly JavaOpCode I2f;
        /// <summary></summary>
        public static readonly JavaOpCode I2l;
        /// <summary></summary>
        public static readonly JavaOpCode I2s;
        /// <summary></summary>
        public static readonly JavaOpCode Iadd;
        /// <summary></summary>
        public static readonly JavaOpCode Iaload;
        /// <summary></summary>
        public static readonly JavaOpCode Iand;
        /// <summary></summary>
        public static readonly JavaOpCode Iastore;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_0;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_1;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_2;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_3;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_4;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_5;
        /// <summary></summary>
        public static readonly JavaOpCode Iconst_m1;
        /// <summary></summary>
        public static readonly JavaOpCode Idiv;
        /// <summary></summary>
        public static readonly JavaOpCode If_acmpeq;
        /// <summary></summary>
        public static readonly JavaOpCode If_acmpne;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmpeq;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmpge;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmpgt;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmple;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmplt;
        /// <summary></summary>
        public static readonly JavaOpCode If_icmpne;
        /// <summary></summary>
        public static readonly JavaOpCode Ifeq;
        /// <summary></summary>
        public static readonly JavaOpCode Ifge;
        /// <summary></summary>
        public static readonly JavaOpCode Ifgt;
        /// <summary></summary>
        public static readonly JavaOpCode Ifle;
        /// <summary></summary>
        public static readonly JavaOpCode Iflt;
        /// <summary></summary>
        public static readonly JavaOpCode Ifne;
        /// <summary></summary>
        public static readonly JavaOpCode Ifnonnull;
        /// <summary></summary>
        public static readonly JavaOpCode Ifnull;
        /// <summary></summary>
        public static readonly JavaOpCode Iinc;
        /// <summary></summary>
        public static readonly JavaOpCode Iload;
        /// <summary></summary>
        public static readonly JavaOpCode Iload_0;
        /// <summary></summary>
        public static readonly JavaOpCode Iload_1;
        /// <summary></summary>
        public static readonly JavaOpCode Iload_2;
        /// <summary></summary>
        public static readonly JavaOpCode Iload_3;
        /// <summary></summary>
        public static readonly JavaOpCode Impdep1;
        /// <summary></summary>
        public static readonly JavaOpCode Impdep2;
        /// <summary></summary>
        public static readonly JavaOpCode Imul;
        /// <summary></summary>
        public static readonly JavaOpCode Ineg;
        /// <summary></summary>
        public static readonly JavaOpCode Instanceof;
        /// <summary></summary>
        public static readonly JavaOpCode Invokeinterface;
        /// <summary></summary>
        public static readonly JavaOpCode Invokespecial;
        /// <summary></summary>
        public static readonly JavaOpCode Invokestatic;
        /// <summary></summary>
        public static readonly JavaOpCode Invokevirtual;
        /// <summary></summary>
        public static readonly JavaOpCode Ior;
        /// <summary></summary>
        public static readonly JavaOpCode Irem;
        /// <summary></summary>
        public static readonly JavaOpCode Ireturn;
        /// <summary></summary>
        public static readonly JavaOpCode Ishl;
        /// <summary></summary>
        public static readonly JavaOpCode Ishr;
        /// <summary></summary>
        public static readonly JavaOpCode Istore;
        /// <summary></summary>
        public static readonly JavaOpCode Istore_0;
        /// <summary></summary>
        public static readonly JavaOpCode Istore_1;
        /// <summary></summary>
        public static readonly JavaOpCode Istore_2;
        /// <summary></summary>
        public static readonly JavaOpCode Istore_3;
        /// <summary></summary>
        public static readonly JavaOpCode Isub;
        /// <summary></summary>
        public static readonly JavaOpCode Iushr;
        /// <summary></summary>
        public static readonly JavaOpCode Ixor;
        /// <summary></summary>
        public static readonly JavaOpCode Jsr;
        /// <summary></summary>
        public static readonly JavaOpCode Jsr_w;
        /// <summary></summary>
        public static readonly JavaOpCode L2d;
        /// <summary></summary>
        public static readonly JavaOpCode L2f;
        /// <summary></summary>
        public static readonly JavaOpCode L2i;
        /// <summary></summary>
        public static readonly JavaOpCode Ladd;
        /// <summary></summary>
        public static readonly JavaOpCode Laload;
        /// <summary></summary>
        public static readonly JavaOpCode Land;
        /// <summary></summary>
        public static readonly JavaOpCode Lastore;
        /// <summary></summary>
        public static readonly JavaOpCode Lcmp;
        /// <summary></summary>
        public static readonly JavaOpCode Lconst_0;
        /// <summary></summary>
        public static readonly JavaOpCode Lconst_1;
        /// <summary></summary>
        public static readonly JavaOpCode Ldc;
        /// <summary></summary>
        public static readonly JavaOpCode Ldc_w;
        /// <summary></summary>
        public static readonly JavaOpCode Ldc2_w;
        /// <summary></summary>
        public static readonly JavaOpCode Ldiv;
        /// <summary></summary>
        public static readonly JavaOpCode Lload;
        /// <summary></summary>
        public static readonly JavaOpCode Lload_0;
        /// <summary></summary>
        public static readonly JavaOpCode Lload_1;
        /// <summary></summary>
        public static readonly JavaOpCode Lload_2;
        /// <summary></summary>
        public static readonly JavaOpCode Lload_3;
        /// <summary></summary>
        public static readonly JavaOpCode Lmul;
        /// <summary></summary>
        public static readonly JavaOpCode Lneg;
        /// <summary></summary>
        public static readonly JavaOpCode Lookupswitch;
        /// <summary></summary>
        public static readonly JavaOpCode Lor;
        /// <summary></summary>
        public static readonly JavaOpCode Lrem;
        /// <summary></summary>
        public static readonly JavaOpCode Lreturn;
        /// <summary></summary>
        public static readonly JavaOpCode Lshl;
        /// <summary></summary>
        public static readonly JavaOpCode Lshr;
        /// <summary></summary>
        public static readonly JavaOpCode Lstore;
        /// <summary></summary>
        public static readonly JavaOpCode Lstore_0;
        /// <summary></summary>
        public static readonly JavaOpCode Lstore_1;
        /// <summary></summary>
        public static readonly JavaOpCode Lstore_2;
        /// <summary></summary>
        public static readonly JavaOpCode Lstore_3;
        /// <summary></summary>
        public static readonly JavaOpCode Lsub;
        /// <summary></summary>
        public static readonly JavaOpCode Lushr;
        /// <summary></summary>
        public static readonly JavaOpCode Lxor;
        /// <summary></summary>
        public static readonly JavaOpCode Monitorenter;
        /// <summary></summary>
        public static readonly JavaOpCode Monitorexit;
        /// <summary></summary>
        public static readonly JavaOpCode Multianewarray;
        /// <summary></summary>
        public static readonly JavaOpCode New;
        /// <summary></summary>
        public static readonly JavaOpCode Newarray;
        /// <summary></summary>
        public static readonly JavaOpCode Nop;
        /// <summary></summary>
        public static readonly JavaOpCode Pop;
        /// <summary></summary>
        public static readonly JavaOpCode Pop2;
        /// <summary></summary>
        public static readonly JavaOpCode Putfield;
        /// <summary></summary>
        public static readonly JavaOpCode Putstatic;
        /// <summary></summary>
        public static readonly JavaOpCode Ret;
        /// <summary></summary>
        public static readonly JavaOpCode Return;
        /// <summary></summary>
        public static readonly JavaOpCode Saload;
        /// <summary></summary>
        public static readonly JavaOpCode Sastore;
        /// <summary></summary>
        public static readonly JavaOpCode Sipush;
        /// <summary></summary>
        public static readonly JavaOpCode Swap;
        /// <summary></summary>
        public static readonly JavaOpCode Tableswitch;
        /// <summary></summary>
        public static readonly JavaOpCode Wide;
        /// <summary></summary>
        public static readonly JavaOpCode Xxxunusedxxx1;

        private static readonly ReadOnlyCollection<JavaOpCode> _instructionLookup;

        public readonly JavaOpCodeTag OpCode;
        public readonly string Name;
        public readonly JavaOperandType OperandType;
        public readonly JavaFlowControl FlowControl;
        public readonly int Size;
        public readonly JavaStackBehavior StackBehaviorPop;
        public readonly JavaStackBehavior StackBehaviorPush;

        public JavaOpCode(JavaOpCodeTag opcode, string name, JavaOperandType operandType, JavaFlowControl flowControl, int size, JavaStackBehavior stackBehaviorPop, JavaStackBehavior stackBehaviorPush)
        {
            OpCode = opcode;
            Name = name;
            OperandType = operandType;
            FlowControl = flowControl;
            Size = size;
            StackBehaviorPop = stackBehaviorPop;
            StackBehaviorPush = stackBehaviorPush;
        }

        static JavaOpCode()
        {
            Aaload = new JavaOpCode(JavaOpCodeTag.Aaload, "aaload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushRef);
            Aastore = new JavaOpCode(JavaOpCodeTag.Aastore, "aastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopRef, JavaStackBehavior.Push0);
            Aconst_null = new JavaOpCode(JavaOpCodeTag.Aconst_null, "aconst_null", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Aload = new JavaOpCode(JavaOpCodeTag.Aload, "aload", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Aload_0 = new JavaOpCode(JavaOpCodeTag.Aload_0, "aload_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Aload_1 = new JavaOpCode(JavaOpCodeTag.Aload_1, "aload_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Aload_2 = new JavaOpCode(JavaOpCodeTag.Aload_2, "aload_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Aload_3 = new JavaOpCode(JavaOpCodeTag.Aload_3, "aload_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Anewarray = new JavaOpCode(JavaOpCodeTag.Anewarray, "anewarray", JavaOperandType.InlineType, JavaFlowControl.Next, 3, JavaStackBehavior.PopI, JavaStackBehavior.PushRef);
            Areturn = new JavaOpCode(JavaOpCodeTag.Areturn, "areturn", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Arraylength = new JavaOpCode(JavaOpCodeTag.Arraylength, "arraylength", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.PushI);
            Astore = new JavaOpCode(JavaOpCodeTag.Astore, "astore", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Astore_0 = new JavaOpCode(JavaOpCodeTag.Astore_0, "astore_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Astore_1 = new JavaOpCode(JavaOpCodeTag.Astore_1, "astore_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Astore_2 = new JavaOpCode(JavaOpCodeTag.Astore_2, "astore_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Astore_3 = new JavaOpCode(JavaOpCodeTag.Astore_3, "astore_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Athrow = new JavaOpCode(JavaOpCodeTag.Athrow, "athrow", JavaOperandType.InlineNone, JavaFlowControl.Throw, 1, JavaStackBehavior.PopRef, JavaStackBehavior.PushRef);
            Baload = new JavaOpCode(JavaOpCodeTag.Baload, "baload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushI);
            Bastore = new JavaOpCode(JavaOpCodeTag.Bastore, "bastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopI, JavaStackBehavior.Push0);
            Bipush = new JavaOpCode(JavaOpCodeTag.Bipush, "bipush", JavaOperandType.InlineI1, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Breakpoint = new JavaOpCode(JavaOpCodeTag.Breakpoint, "breakpoint", JavaOperandType.InlineNone, JavaFlowControl.Break, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Caload = new JavaOpCode(JavaOpCodeTag.Caload, "caload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushI);
            Castore = new JavaOpCode(JavaOpCodeTag.Castore, "castore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopI, JavaStackBehavior.Push0);
            Checkcast = new JavaOpCode(JavaOpCodeTag.Checkcast, "checkcast", JavaOperandType.InlineType, JavaFlowControl.Next, 3, JavaStackBehavior.PopRef, JavaStackBehavior.PushRef);
            D2f = new JavaOpCode(JavaOpCodeTag.D2f, "d2f", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.PushR4);
            D2i = new JavaOpCode(JavaOpCodeTag.D2i, "d2i", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.PushI);
            D2l = new JavaOpCode(JavaOpCodeTag.D2l, "d2l", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.PushI8);
            Dadd = new JavaOpCode(JavaOpCodeTag.Dadd, "dadd", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushR8);
            Daload = new JavaOpCode(JavaOpCodeTag.Daload, "daload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushR8);
            Dastore = new JavaOpCode(JavaOpCodeTag.Dastore, "dastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopR8, JavaStackBehavior.Push0);
            Dcmpg = new JavaOpCode(JavaOpCodeTag.Dcmpg, "dcmpg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushI);
            Dcmpl = new JavaOpCode(JavaOpCodeTag.Dcmpl, "dcmpl", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushI);
            Dconst_0 = new JavaOpCode(JavaOpCodeTag.Dconst_0, "dconst_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dconst_1 = new JavaOpCode(JavaOpCodeTag.Dconst_1, "dconst_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Ddiv = new JavaOpCode(JavaOpCodeTag.Ddiv, "ddiv", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushR8);
            Dload = new JavaOpCode(JavaOpCodeTag.Dload, "dload", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dload_0 = new JavaOpCode(JavaOpCodeTag.Dload_0, "dload_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dload_1 = new JavaOpCode(JavaOpCodeTag.Dload_1, "dload_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dload_2 = new JavaOpCode(JavaOpCodeTag.Dload_2, "dload_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dload_3 = new JavaOpCode(JavaOpCodeTag.Dload_3, "dload_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR8);
            Dmul = new JavaOpCode(JavaOpCodeTag.Dmul, "dmul", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushR8);
            Dneg = new JavaOpCode(JavaOpCodeTag.Dneg, "dneg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.PushR8);
            Drem = new JavaOpCode(JavaOpCodeTag.Drem, "drem", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8_PopR8, JavaStackBehavior.PushR8);
            Dreturn = new JavaOpCode(JavaOpCodeTag.Dreturn, "dreturn", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dstore = new JavaOpCode(JavaOpCodeTag.Dstore, "dstore", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dstore_0 = new JavaOpCode(JavaOpCodeTag.Dstore_0, "dstore_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dstore_1 = new JavaOpCode(JavaOpCodeTag.Dstore_1, "dstore_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dstore_2 = new JavaOpCode(JavaOpCodeTag.Dstore_2, "dstore_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dstore_3 = new JavaOpCode(JavaOpCodeTag.Dstore_3, "dstore_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.Push0);
            Dsub = new JavaOpCode(JavaOpCodeTag.Dsub, "dsub", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR8, JavaStackBehavior.PushR8);
            Dup = new JavaOpCode(JavaOpCodeTag.Dup, "dup", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop1, JavaStackBehavior.Push1_Push1);
            Dup_x1 = new JavaOpCode(JavaOpCodeTag.Dup_x1, "dup_x1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop1_Pop1, JavaStackBehavior.Push1_Push1);
            Dup_x2 = new JavaOpCode(JavaOpCodeTag.Dup_x2, "dup_x2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Dup2 = new JavaOpCode(JavaOpCodeTag.Dup2, "dup2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Dup2_x1 = new JavaOpCode(JavaOpCodeTag.Dup2_x1, "dup2_x1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Dup2_x2 = new JavaOpCode(JavaOpCodeTag.Dup2_x2, "dup2_x2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            F2d = new JavaOpCode(JavaOpCodeTag.F2d, "f2d", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.PushR8);
            F2i = new JavaOpCode(JavaOpCodeTag.F2i, "f2i", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.PushI);
            F2l = new JavaOpCode(JavaOpCodeTag.F2l, "f2l", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.PushI8);
            Fadd = new JavaOpCode(JavaOpCodeTag.Fadd, "fadd", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushR4);
            Faload = new JavaOpCode(JavaOpCodeTag.Faload, "faload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushR4);
            Fastore = new JavaOpCode(JavaOpCodeTag.Fastore, "fastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopR4, JavaStackBehavior.Push0);
            Fcmpg = new JavaOpCode(JavaOpCodeTag.Fcmpg, "fcmpg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushI);
            Fcmpl = new JavaOpCode(JavaOpCodeTag.Fcmpl, "fcmpl", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushI);
            Fconst_0 = new JavaOpCode(JavaOpCodeTag.Fconst_0, "fconst_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fconst_1 = new JavaOpCode(JavaOpCodeTag.Fconst_1, "fconst_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fconst_2 = new JavaOpCode(JavaOpCodeTag.Fconst_2, "fconst_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fdiv = new JavaOpCode(JavaOpCodeTag.Fdiv, "fdiv", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushR4);
            Fload = new JavaOpCode(JavaOpCodeTag.Fload, "fload", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fload_0 = new JavaOpCode(JavaOpCodeTag.Fload_0, "fload_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fload_1 = new JavaOpCode(JavaOpCodeTag.Fload_1, "fload_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fload_2 = new JavaOpCode(JavaOpCodeTag.Fload_2, "fload_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fload_3 = new JavaOpCode(JavaOpCodeTag.Fload_3, "fload_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushR4);
            Fmul = new JavaOpCode(JavaOpCodeTag.Fmul, "fmul", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushR4);
            Fneg = new JavaOpCode(JavaOpCodeTag.Fneg, "fneg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.PushR4);
            Frem = new JavaOpCode(JavaOpCodeTag.Frem, "frem", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushR4);
            Freturn = new JavaOpCode(JavaOpCodeTag.Freturn, "freturn", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fstore = new JavaOpCode(JavaOpCodeTag.Fstore, "fstore", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fstore_0 = new JavaOpCode(JavaOpCodeTag.Fstore_0, "fstore_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fstore_1 = new JavaOpCode(JavaOpCodeTag.Fstore_1, "fstore_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fstore_2 = new JavaOpCode(JavaOpCodeTag.Fstore_2, "fstore_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fstore_3 = new JavaOpCode(JavaOpCodeTag.Fstore_3, "fstore_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4, JavaStackBehavior.Push0);
            Fsub = new JavaOpCode(JavaOpCodeTag.Fsub, "fsub", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopR4_PopR4, JavaStackBehavior.PushR4);
            Getfield = new JavaOpCode(JavaOpCodeTag.Getfield, "getfield", JavaOperandType.InlineField, JavaFlowControl.Next, 3, JavaStackBehavior.PopRef, JavaStackBehavior.Push1);
            Getstatic = new JavaOpCode(JavaOpCodeTag.Getstatic, "getstatic", JavaOperandType.InlineField, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.Push1);
            Goto = new JavaOpCode(JavaOpCodeTag.Goto, "goto", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.Branch, 3, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Goto_w = new JavaOpCode(JavaOpCodeTag.Goto_w, "goto_w", JavaOperandType.InlineBranchTarget, JavaFlowControl.Branch, 5, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            I2b = new JavaOpCode(JavaOpCodeTag.I2b, "i2b", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushI);
            I2c = new JavaOpCode(JavaOpCodeTag.I2c, "i2c", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushI);
            I2d = new JavaOpCode(JavaOpCodeTag.I2d, "i2d", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushR8);
            I2f = new JavaOpCode(JavaOpCodeTag.I2f, "i2f", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushR4);
            I2l = new JavaOpCode(JavaOpCodeTag.I2l, "i2l", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushI8);
            I2s = new JavaOpCode(JavaOpCodeTag.I2s, "i2s", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushI);
            Iadd = new JavaOpCode(JavaOpCodeTag.Iadd, "iadd", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Iaload = new JavaOpCode(JavaOpCodeTag.Iaload, "iaload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushI);
            Iand = new JavaOpCode(JavaOpCodeTag.Iand, "iand", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Iastore = new JavaOpCode(JavaOpCodeTag.Iastore, "iastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopI, JavaStackBehavior.Push0);
            Iconst_0 = new JavaOpCode(JavaOpCodeTag.Iconst_0, "iconst_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_1 = new JavaOpCode(JavaOpCodeTag.Iconst_1, "iconst_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_2 = new JavaOpCode(JavaOpCodeTag.Iconst_2, "iconst_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_3 = new JavaOpCode(JavaOpCodeTag.Iconst_3, "iconst_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_4 = new JavaOpCode(JavaOpCodeTag.Iconst_4, "iconst_4", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_5 = new JavaOpCode(JavaOpCodeTag.Iconst_5, "iconst_5", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iconst_m1 = new JavaOpCode(JavaOpCodeTag.Iconst_m1, "iconst_m1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Idiv = new JavaOpCode(JavaOpCodeTag.Idiv, "idiv", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            If_acmpeq = new JavaOpCode(JavaOpCodeTag.If_acmpeq, "if_acmpeq", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopRef_PopRef, JavaStackBehavior.Push0);
            If_acmpne = new JavaOpCode(JavaOpCodeTag.If_acmpne, "if_acmpne", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopRef_PopRef, JavaStackBehavior.Push0);
            If_icmpeq = new JavaOpCode(JavaOpCodeTag.If_icmpeq, "if_icmpeq", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            If_icmpge = new JavaOpCode(JavaOpCodeTag.If_icmpge, "if_icmpge", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            If_icmpgt = new JavaOpCode(JavaOpCodeTag.If_icmpgt, "if_icmpgt", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            If_icmple = new JavaOpCode(JavaOpCodeTag.If_icmple, "if_icmple", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            If_icmplt = new JavaOpCode(JavaOpCodeTag.If_icmplt, "if_icmplt", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            If_icmpne = new JavaOpCode(JavaOpCodeTag.If_icmpne, "if_icmpne", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI_PopI, JavaStackBehavior.Push0);
            Ifeq = new JavaOpCode(JavaOpCodeTag.Ifeq, "ifeq", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ifge = new JavaOpCode(JavaOpCodeTag.Ifge, "ifge", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ifgt = new JavaOpCode(JavaOpCodeTag.Ifgt, "ifgt", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ifle = new JavaOpCode(JavaOpCodeTag.Ifle, "ifle", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Iflt = new JavaOpCode(JavaOpCodeTag.Iflt, "iflt", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ifne = new JavaOpCode(JavaOpCodeTag.Ifne, "ifne", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ifnonnull = new JavaOpCode(JavaOpCodeTag.Ifnonnull, "ifnonnull", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Ifnull = new JavaOpCode(JavaOpCodeTag.Ifnull, "ifnull", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.ConditionalBranch, 3, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Iinc = new JavaOpCode(JavaOpCodeTag.Iinc, "iinc", JavaOperandType.InlineVar_I1, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Iload = new JavaOpCode(JavaOpCodeTag.Iload, "iload", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iload_0 = new JavaOpCode(JavaOpCodeTag.Iload_0, "iload_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iload_1 = new JavaOpCode(JavaOpCodeTag.Iload_1, "iload_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iload_2 = new JavaOpCode(JavaOpCodeTag.Iload_2, "iload_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Iload_3 = new JavaOpCode(JavaOpCodeTag.Iload_3, "iload_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Impdep1 = new JavaOpCode(JavaOpCodeTag.Impdep1, "impdep1", JavaOperandType.InlineNone, JavaFlowControl.Special, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Impdep2 = new JavaOpCode(JavaOpCodeTag.Impdep2, "impdep2", JavaOperandType.InlineNone, JavaFlowControl.Special, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Imul = new JavaOpCode(JavaOpCodeTag.Imul, "imul", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Ineg = new JavaOpCode(JavaOpCodeTag.Ineg, "ineg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.PushI);
            Instanceof = new JavaOpCode(JavaOpCodeTag.Instanceof, "instanceof", JavaOperandType.InlineType, JavaFlowControl.Next, 3, JavaStackBehavior.PopRef, JavaStackBehavior.PushI);
            Invokeinterface = new JavaOpCode(JavaOpCodeTag.Invokeinterface, "invokeinterface", JavaOperandType.InlineMethod_U1_0, JavaFlowControl.Call, 5, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Invokespecial = new JavaOpCode(JavaOpCodeTag.Invokespecial, "invokespecial", JavaOperandType.InlineMethod, JavaFlowControl.Call, 3, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Invokestatic = new JavaOpCode(JavaOpCodeTag.Invokestatic, "invokestatic", JavaOperandType.InlineMethod, JavaFlowControl.Call, 3, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Invokevirtual = new JavaOpCode(JavaOpCodeTag.Invokevirtual, "invokevirtual", JavaOperandType.InlineMethod, JavaFlowControl.Call, 3, JavaStackBehavior.PopVar, JavaStackBehavior.PushVar);
            Ior = new JavaOpCode(JavaOpCodeTag.Ior, "ior", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Irem = new JavaOpCode(JavaOpCodeTag.Irem, "irem", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Ireturn = new JavaOpCode(JavaOpCodeTag.Ireturn, "ireturn", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Ishl = new JavaOpCode(JavaOpCodeTag.Ishl, "ishl", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Ishr = new JavaOpCode(JavaOpCodeTag.Ishr, "ishr", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Istore = new JavaOpCode(JavaOpCodeTag.Istore, "istore", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Istore_0 = new JavaOpCode(JavaOpCodeTag.Istore_0, "istore_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Istore_1 = new JavaOpCode(JavaOpCodeTag.Istore_1, "istore_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Istore_2 = new JavaOpCode(JavaOpCodeTag.Istore_2, "istore_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Istore_3 = new JavaOpCode(JavaOpCodeTag.Istore_3, "istore_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Isub = new JavaOpCode(JavaOpCodeTag.Isub, "isub", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Iushr = new JavaOpCode(JavaOpCodeTag.Iushr, "iushr", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Ixor = new JavaOpCode(JavaOpCodeTag.Ixor, "ixor", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI_PopI, JavaStackBehavior.PushI);
            Jsr = new JavaOpCode(JavaOpCodeTag.Jsr, "jsr", JavaOperandType.InlineShortBranchTarget, JavaFlowControl.Branch, 3, JavaStackBehavior.Pop0, JavaStackBehavior.PushRet);
            Jsr_w = new JavaOpCode(JavaOpCodeTag.Jsr_w, "jsr_w", JavaOperandType.InlineBranchTarget, JavaFlowControl.Branch, 5, JavaStackBehavior.Pop0, JavaStackBehavior.PushRet);
            L2d = new JavaOpCode(JavaOpCodeTag.L2d, "l2d", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.PushR8);
            L2f = new JavaOpCode(JavaOpCodeTag.L2f, "l2f", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.PushR4);
            L2i = new JavaOpCode(JavaOpCodeTag.L2i, "l2i", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.PushI);
            Ladd = new JavaOpCode(JavaOpCodeTag.Ladd, "ladd", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Laload = new JavaOpCode(JavaOpCodeTag.Laload, "laload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushI8);
            Land = new JavaOpCode(JavaOpCodeTag.Land, "land", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lastore = new JavaOpCode(JavaOpCodeTag.Lastore, "lastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopI8, JavaStackBehavior.Push0);
            Lcmp = new JavaOpCode(JavaOpCodeTag.Lcmp, "lcmp", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI);
            Lconst_0 = new JavaOpCode(JavaOpCodeTag.Lconst_0, "lconst_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lconst_1 = new JavaOpCode(JavaOpCodeTag.Lconst_1, "lconst_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Ldc = new JavaOpCode(JavaOpCodeTag.Ldc, "ldc", JavaOperandType.InlineShortConst, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.Push1);
            Ldc_w = new JavaOpCode(JavaOpCodeTag.Ldc_w, "ldc_w", JavaOperandType.InlineConst, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.Push1);
            Ldc2_w = new JavaOpCode(JavaOpCodeTag.Ldc2_w, "ldc2_w", JavaOperandType.InlineConst, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.Push1);
            Ldiv = new JavaOpCode(JavaOpCodeTag.Ldiv, "ldiv", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lload = new JavaOpCode(JavaOpCodeTag.Lload, "lload", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lload_0 = new JavaOpCode(JavaOpCodeTag.Lload_0, "lload_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lload_1 = new JavaOpCode(JavaOpCodeTag.Lload_1, "lload_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lload_2 = new JavaOpCode(JavaOpCodeTag.Lload_2, "lload_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lload_3 = new JavaOpCode(JavaOpCodeTag.Lload_3, "lload_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.PushI8);
            Lmul = new JavaOpCode(JavaOpCodeTag.Lmul, "lmul", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lneg = new JavaOpCode(JavaOpCodeTag.Lneg, "lneg", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.PushI8);
            Lookupswitch = new JavaOpCode(JavaOpCodeTag.Lookupswitch, "lookupswitch", JavaOperandType.InlineLookupSwitch, JavaFlowControl.ConditionalBranch, 0, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Lor = new JavaOpCode(JavaOpCodeTag.Lor, "lor", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lrem = new JavaOpCode(JavaOpCodeTag.Lrem, "lrem", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lreturn = new JavaOpCode(JavaOpCodeTag.Lreturn, "lreturn", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lshl = new JavaOpCode(JavaOpCodeTag.Lshl, "lshl", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI, JavaStackBehavior.PushI8);
            Lshr = new JavaOpCode(JavaOpCodeTag.Lshr, "lshr", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI, JavaStackBehavior.PushI8);
            Lstore = new JavaOpCode(JavaOpCodeTag.Lstore, "lstore", JavaOperandType.InlineVar, JavaFlowControl.Next, 2, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lstore_0 = new JavaOpCode(JavaOpCodeTag.Lstore_0, "lstore_0", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lstore_1 = new JavaOpCode(JavaOpCodeTag.Lstore_1, "lstore_1", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lstore_2 = new JavaOpCode(JavaOpCodeTag.Lstore_2, "lstore_2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lstore_3 = new JavaOpCode(JavaOpCodeTag.Lstore_3, "lstore_3", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8, JavaStackBehavior.Push0);
            Lsub = new JavaOpCode(JavaOpCodeTag.Lsub, "lsub", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Lushr = new JavaOpCode(JavaOpCodeTag.Lushr, "lushr", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI, JavaStackBehavior.PushI8);
            Lxor = new JavaOpCode(JavaOpCodeTag.Lxor, "lxor", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopI8_PopI8, JavaStackBehavior.PushI8);
            Monitorenter = new JavaOpCode(JavaOpCodeTag.Monitorenter, "monitorenter", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Monitorexit = new JavaOpCode(JavaOpCodeTag.Monitorexit, "monitorexit", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef, JavaStackBehavior.Push0);
            Multianewarray = new JavaOpCode(JavaOpCodeTag.Multianewarray, "multianewarray", JavaOperandType.InlineType_U1, JavaFlowControl.Next, 4, JavaStackBehavior.PopVar, JavaStackBehavior.PushRef);
            New = new JavaOpCode(JavaOpCodeTag.New, "new", JavaOperandType.InlineType, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.PushRef);
            Newarray = new JavaOpCode(JavaOpCodeTag.Newarray, "newarray", JavaOperandType.InlineArrayType, JavaFlowControl.Next, 2, JavaStackBehavior.PopI, JavaStackBehavior.PushRef);
            Nop = new JavaOpCode(JavaOpCodeTag.Nop, "nop", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Pop = new JavaOpCode(JavaOpCodeTag.Pop, "pop", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop1, JavaStackBehavior.Push0);
            Pop2 = new JavaOpCode(JavaOpCodeTag.Pop2, "pop2", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop1, JavaStackBehavior.Push0);
            Putfield = new JavaOpCode(JavaOpCodeTag.Putfield, "putfield", JavaOperandType.InlineField, JavaFlowControl.Next, 3, JavaStackBehavior.PopRef_Pop1, JavaStackBehavior.Push0);
            Putstatic = new JavaOpCode(JavaOpCodeTag.Putstatic, "putstatic", JavaOperandType.InlineField, JavaFlowControl.Next, 3, JavaStackBehavior.Pop1, JavaStackBehavior.Push0);
            Ret = new JavaOpCode(JavaOpCodeTag.Ret, "ret", JavaOperandType.InlineVar, JavaFlowControl.Return, 2, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Return = new JavaOpCode(JavaOpCodeTag.Return, "return", JavaOperandType.InlineNone, JavaFlowControl.Return, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Saload = new JavaOpCode(JavaOpCodeTag.Saload, "saload", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI, JavaStackBehavior.PushI);
            Sastore = new JavaOpCode(JavaOpCodeTag.Sastore, "sastore", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.PopRef_PopI_PopI, JavaStackBehavior.Push0);
            Sipush = new JavaOpCode(JavaOpCodeTag.Sipush, "sipush", JavaOperandType.InlineI2, JavaFlowControl.Next, 3, JavaStackBehavior.Pop0, JavaStackBehavior.PushI);
            Swap = new JavaOpCode(JavaOpCodeTag.Swap, "swap", JavaOperandType.InlineNone, JavaFlowControl.Next, 1, JavaStackBehavior.Pop1_Pop1, JavaStackBehavior.Push1);
            Tableswitch = new JavaOpCode(JavaOpCodeTag.Tableswitch, "tableswitch", JavaOperandType.InlineTableSwitch, JavaFlowControl.ConditionalBranch, 0, JavaStackBehavior.PopI, JavaStackBehavior.Push0);
            Wide = new JavaOpCode(JavaOpCodeTag.Wide, "wide", JavaOperandType.InlineNone, JavaFlowControl.Meta, 0, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);
            Xxxunusedxxx1 = new JavaOpCode(JavaOpCodeTag.Xxxunusedxxx1, "xxxunusedxxx1", JavaOperandType.InlineNone, JavaFlowControl.Special, 1, JavaStackBehavior.Pop0, JavaStackBehavior.Push0);

            JavaOpCode[] instructionLookup = new JavaOpCode[byte.MaxValue + 1];
            instructionLookup[0x32] = Aaload;
            instructionLookup[0x53] = Aastore;
            instructionLookup[0x01] = Aconst_null;
            instructionLookup[0x19] = Aload;
            instructionLookup[0x2A] = Aload_0;
            instructionLookup[0x2B] = Aload_1;
            instructionLookup[0x2C] = Aload_2;
            instructionLookup[0x2D] = Aload_3;
            instructionLookup[0xBD] = Anewarray;
            instructionLookup[0xB0] = Areturn;
            instructionLookup[0xBE] = Arraylength;
            instructionLookup[0x3A] = Astore;
            instructionLookup[0x4B] = Astore_0;
            instructionLookup[0x4C] = Astore_1;
            instructionLookup[0x4D] = Astore_2;
            instructionLookup[0x4E] = Astore_3;
            instructionLookup[0xBF] = Athrow;
            instructionLookup[0x33] = Baload;
            instructionLookup[0x54] = Bastore;
            instructionLookup[0x10] = Bipush;
            instructionLookup[0xCA] = Breakpoint;
            instructionLookup[0x34] = Caload;
            instructionLookup[0x55] = Castore;
            instructionLookup[0xC0] = Checkcast;
            instructionLookup[0x90] = D2f;
            instructionLookup[0x8E] = D2i;
            instructionLookup[0x8F] = D2l;
            instructionLookup[0x63] = Dadd;
            instructionLookup[0x31] = Daload;
            instructionLookup[0x52] = Dastore;
            instructionLookup[0x98] = Dcmpg;
            instructionLookup[0x97] = Dcmpl;
            instructionLookup[0x0E] = Dconst_0;
            instructionLookup[0x0F] = Dconst_1;
            instructionLookup[0x6F] = Ddiv;
            instructionLookup[0x18] = Dload;
            instructionLookup[0x26] = Dload_0;
            instructionLookup[0x27] = Dload_1;
            instructionLookup[0x28] = Dload_2;
            instructionLookup[0x29] = Dload_3;
            instructionLookup[0x6B] = Dmul;
            instructionLookup[0x77] = Dneg;
            instructionLookup[0x73] = Drem;
            instructionLookup[0xAF] = Dreturn;
            instructionLookup[0x39] = Dstore;
            instructionLookup[0x47] = Dstore_0;
            instructionLookup[0x48] = Dstore_1;
            instructionLookup[0x49] = Dstore_2;
            instructionLookup[0x4A] = Dstore_3;
            instructionLookup[0x67] = Dsub;
            instructionLookup[0x59] = Dup;
            instructionLookup[0x5A] = Dup_x1;
            instructionLookup[0x5B] = Dup_x2;
            instructionLookup[0x5C] = Dup2;
            instructionLookup[0x5D] = Dup2_x1;
            instructionLookup[0x5E] = Dup2_x2;
            instructionLookup[0x8D] = F2d;
            instructionLookup[0x8B] = F2i;
            instructionLookup[0x8C] = F2l;
            instructionLookup[0x62] = Fadd;
            instructionLookup[0x30] = Faload;
            instructionLookup[0x51] = Fastore;
            instructionLookup[0x96] = Fcmpg;
            instructionLookup[0x95] = Fcmpl;
            instructionLookup[0x0B] = Fconst_0;
            instructionLookup[0x0C] = Fconst_1;
            instructionLookup[0x0D] = Fconst_2;
            instructionLookup[0x6E] = Fdiv;
            instructionLookup[0x17] = Fload;
            instructionLookup[0x22] = Fload_0;
            instructionLookup[0x23] = Fload_1;
            instructionLookup[0x24] = Fload_2;
            instructionLookup[0x25] = Fload_3;
            instructionLookup[0x6A] = Fmul;
            instructionLookup[0x76] = Fneg;
            instructionLookup[0x72] = Frem;
            instructionLookup[0xAE] = Freturn;
            instructionLookup[0x38] = Fstore;
            instructionLookup[0x43] = Fstore_0;
            instructionLookup[0x44] = Fstore_1;
            instructionLookup[0x45] = Fstore_2;
            instructionLookup[0x46] = Fstore_3;
            instructionLookup[0x66] = Fsub;
            instructionLookup[0xB4] = Getfield;
            instructionLookup[0xB2] = Getstatic;
            instructionLookup[0xA7] = Goto;
            instructionLookup[0xC8] = Goto_w;
            instructionLookup[0x91] = I2b;
            instructionLookup[0x92] = I2c;
            instructionLookup[0x87] = I2d;
            instructionLookup[0x86] = I2f;
            instructionLookup[0x85] = I2l;
            instructionLookup[0x93] = I2s;
            instructionLookup[0x60] = Iadd;
            instructionLookup[0x2E] = Iaload;
            instructionLookup[0x7E] = Iand;
            instructionLookup[0x4F] = Iastore;
            instructionLookup[0x03] = Iconst_0;
            instructionLookup[0x04] = Iconst_1;
            instructionLookup[0x05] = Iconst_2;
            instructionLookup[0x06] = Iconst_3;
            instructionLookup[0x07] = Iconst_4;
            instructionLookup[0x08] = Iconst_5;
            instructionLookup[0x02] = Iconst_m1;
            instructionLookup[0x6C] = Idiv;
            instructionLookup[0xA5] = If_acmpeq;
            instructionLookup[0xA6] = If_acmpne;
            instructionLookup[0x9F] = If_icmpeq;
            instructionLookup[0xA2] = If_icmpge;
            instructionLookup[0xA3] = If_icmpgt;
            instructionLookup[0xA4] = If_icmple;
            instructionLookup[0xA1] = If_icmplt;
            instructionLookup[0xA0] = If_icmpne;
            instructionLookup[0x99] = Ifeq;
            instructionLookup[0x9C] = Ifge;
            instructionLookup[0x9D] = Ifgt;
            instructionLookup[0x9E] = Ifle;
            instructionLookup[0x9B] = Iflt;
            instructionLookup[0x9A] = Ifne;
            instructionLookup[0xC7] = Ifnonnull;
            instructionLookup[0xC6] = Ifnull;
            instructionLookup[0x84] = Iinc;
            instructionLookup[0x15] = Iload;
            instructionLookup[0x1A] = Iload_0;
            instructionLookup[0x1B] = Iload_1;
            instructionLookup[0x1C] = Iload_2;
            instructionLookup[0x1D] = Iload_3;
            instructionLookup[0xFE] = Impdep1;
            instructionLookup[0xFF] = Impdep2;
            instructionLookup[0x68] = Imul;
            instructionLookup[0x74] = Ineg;
            instructionLookup[0xC1] = Instanceof;
            instructionLookup[0xB9] = Invokeinterface;
            instructionLookup[0xB7] = Invokespecial;
            instructionLookup[0xB8] = Invokestatic;
            instructionLookup[0xB6] = Invokevirtual;
            instructionLookup[0x80] = Ior;
            instructionLookup[0x70] = Irem;
            instructionLookup[0xAC] = Ireturn;
            instructionLookup[0x78] = Ishl;
            instructionLookup[0x7A] = Ishr;
            instructionLookup[0x36] = Istore;
            instructionLookup[0x3B] = Istore_0;
            instructionLookup[0x3C] = Istore_1;
            instructionLookup[0x3D] = Istore_2;
            instructionLookup[0x3E] = Istore_3;
            instructionLookup[0x64] = Isub;
            instructionLookup[0x7C] = Iushr;
            instructionLookup[0x82] = Ixor;
            instructionLookup[0xA8] = Jsr;
            instructionLookup[0xC9] = Jsr_w;
            instructionLookup[0x8A] = L2d;
            instructionLookup[0x89] = L2f;
            instructionLookup[0x88] = L2i;
            instructionLookup[0x61] = Ladd;
            instructionLookup[0x2F] = Laload;
            instructionLookup[0x7F] = Land;
            instructionLookup[0x50] = Lastore;
            instructionLookup[0x94] = Lcmp;
            instructionLookup[0x09] = Lconst_0;
            instructionLookup[0x0A] = Lconst_1;
            instructionLookup[0x12] = Ldc;
            instructionLookup[0x13] = Ldc_w;
            instructionLookup[0x14] = Ldc2_w;
            instructionLookup[0x6D] = Ldiv;
            instructionLookup[0x16] = Lload;
            instructionLookup[0x1E] = Lload_0;
            instructionLookup[0x1F] = Lload_1;
            instructionLookup[0x20] = Lload_2;
            instructionLookup[0x21] = Lload_3;
            instructionLookup[0x69] = Lmul;
            instructionLookup[0x75] = Lneg;
            instructionLookup[0xAB] = Lookupswitch;
            instructionLookup[0x81] = Lor;
            instructionLookup[0x71] = Lrem;
            instructionLookup[0xAD] = Lreturn;
            instructionLookup[0x79] = Lshl;
            instructionLookup[0x7B] = Lshr;
            instructionLookup[0x37] = Lstore;
            instructionLookup[0x3F] = Lstore_0;
            instructionLookup[0x40] = Lstore_1;
            instructionLookup[0x41] = Lstore_2;
            instructionLookup[0x42] = Lstore_3;
            instructionLookup[0x65] = Lsub;
            instructionLookup[0x7D] = Lushr;
            instructionLookup[0x83] = Lxor;
            instructionLookup[0xC2] = Monitorenter;
            instructionLookup[0xC3] = Monitorexit;
            instructionLookup[0xC5] = Multianewarray;
            instructionLookup[0xBB] = New;
            instructionLookup[0xBC] = Newarray;
            instructionLookup[0x00] = Nop;
            instructionLookup[0x57] = Pop;
            instructionLookup[0x58] = Pop2;
            instructionLookup[0xB5] = Putfield;
            instructionLookup[0xB3] = Putstatic;
            instructionLookup[0xA9] = Ret;
            instructionLookup[0xB1] = Return;
            instructionLookup[0x35] = Saload;
            instructionLookup[0x56] = Sastore;
            instructionLookup[0x11] = Sipush;
            instructionLookup[0x5F] = Swap;
            instructionLookup[0xAA] = Tableswitch;
            instructionLookup[0xC4] = Wide;
            instructionLookup[0xBA] = Xxxunusedxxx1;

            _instructionLookup = new ReadOnlyCollection<JavaOpCode>(instructionLookup);
        }

        public static ReadOnlyCollection<JavaOpCode> InstructionLookup
        {
            get
            {
                return _instructionLookup;
            }
        }
    }
}
