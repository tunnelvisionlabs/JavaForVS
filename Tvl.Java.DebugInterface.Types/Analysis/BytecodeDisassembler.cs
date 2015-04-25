namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Collections;
    using Tvl.Java.DebugInterface.Types.Loader;

    public static class BytecodeDisassembler
    {
        public static DisassembledMethod Disassemble(byte[] bytecode)
        {
            Contract.Requires<ArgumentNullException>(bytecode != null, "bytecode");

            List<JavaInstruction> instructions = new List<JavaInstruction>();
            List<SwitchData> switchData = new List<SwitchData>();
            for (int i = 0; i < bytecode.Length; /*increment in loop*/)
            {
                int instructionStart = i;
                JavaOpCode opcode = JavaOpCode.InstructionLookup[bytecode[instructionStart]];
                if (opcode.Name == null)
                    throw new FormatException(string.Format("Encountered unrecognized opcode {0} at offset {1}.", bytecode[instructionStart], instructionStart));

                int instructionLength = opcode.Size;
                JavaOperandData operands = default(JavaOperandData);
                switch (opcode.OperandType)
                {
                case JavaOperandType.InlineNone:
                    break;

                case JavaOperandType.InlineI1:
                    operands = new JavaOperandData(ReadSByte(bytecode, instructionStart + 1));
                    break;

                case JavaOperandType.InlineI2:
                case JavaOperandType.InlineShortBranchTarget:
                    operands = new JavaOperandData(ReadInt16(bytecode, instructionStart + 1));
                    break;

                case JavaOperandType.InlineBranchTarget:
                    operands = new JavaOperandData(ReadInt32(bytecode, instructionStart + 1));
                    break;

                case JavaOperandType.InlineLookupSwitch:
                    {
                        int defaultStart = (instructionStart + 4) & (~3);
                        int defaultValue = ReadInt32(bytecode, defaultStart);
                        int pairsCount = ReadInt32(bytecode, defaultStart + sizeof(int));
                        if (pairsCount < 0)
                            throw new FormatException();

                        List<KeyValuePair<int, int>> pairs = new List<KeyValuePair<int, int>>();
                        for (int j = 0; j < pairsCount; j++)
                        {
                            int pairStart = defaultStart + (sizeof(int) * (2 + 2 * j));
                            pairs.Add(new KeyValuePair<int, int>(ReadInt32(bytecode, pairStart), ReadInt32(bytecode, pairStart + sizeof(int))));
                        }

                        switchData.Add(new LookupSwitchData(defaultValue, pairs));
                        int instructionSize = (defaultStart - instructionStart) + (sizeof(int) * (2 + 2 * pairsCount));
                        operands = new JavaOperandData((ushort)(switchData.Count - 1), instructionSize);
                        break;
                    }


                case JavaOperandType.InlineTableSwitch:
                    {
                        int defaultStart = (instructionStart + 4) & (~3);
                        int defaultValue = ReadInt32(bytecode, defaultStart);
                        int lowValue = ReadInt32(bytecode, defaultStart + 4);
                        int highValue = ReadInt32(bytecode, defaultStart + 8);
                        if (highValue < lowValue)
                            throw new FormatException();

                        List<int> offsets = new List<int>();
                        for (int j = 0; j < highValue - lowValue + 1; j++)
                        {
                            int valueStart = defaultStart + (sizeof(int) * (3 + j));
                            offsets.Add(ReadInt32(bytecode, valueStart));
                        }

                        switchData.Add(new TableSwitchData(defaultValue, lowValue, highValue, offsets));
                        int instructionSize = (defaultStart - instructionStart) + (sizeof(int) * (3 + offsets.Count));
                        operands = new JavaOperandData((ushort)(switchData.Count - 1), instructionSize);
                        break;
                    }

                case JavaOperandType.InlineShortConst:
                case JavaOperandType.InlineVar:
                case JavaOperandType.InlineArrayType:
                    operands = new JavaOperandData(ReadByte(bytecode, instructionStart + 1));
                    break;

                case JavaOperandType.InlineConst:
                case JavaOperandType.InlineField:
                case JavaOperandType.InlineMethod:
                case JavaOperandType.InlineType:
                    operands = new JavaOperandData(ReadUInt16(bytecode, instructionStart + 1));
                    break;

                case JavaOperandType.InlineVar_I1:
                    operands = new JavaOperandData(ReadByte(bytecode, instructionStart + 1), ReadSByte(bytecode, instructionStart + 2));
                    break;

                case JavaOperandType.InlineMethod_U1_0:
                case JavaOperandType.InlineType_U1:
                    operands = new JavaOperandData(ReadUInt16(bytecode, instructionStart + 1), ReadByte(bytecode, instructionStart + 3));
                    break;

                default:
                    throw new FormatException();
                }

                instructions.Add(new JavaInstruction(instructionStart, opcode, operands));
                if (opcode.Size > 0)
                {
                    i += opcode.Size;
                }
                else
                {
                    switch (opcode.OpCode)
                    {
                    case JavaOpCodeTag.Tableswitch:
                    case JavaOpCodeTag.Lookupswitch:
                        i += operands.SwitchInstructionSize;
                        break;

                    case JavaOpCodeTag.Wide:
                        throw new NotImplementedException();

                    default:
                        throw new FormatException();
                    }
                }
            }

            return new DisassembledMethod(instructions, switchData);
        }

        public static ImmutableList<int?> GetEvaluationStackDepths(DisassembledMethod disassembledMethod, ReadOnlyCollection<ConstantPoolEntry> constantPool, ReadOnlyCollection<ExceptionTableEntry> exceptionTable)
        {
            Contract.Requires<ArgumentNullException>(disassembledMethod != null, "disassembledMethod");
            Contract.Requires<ArgumentNullException>(constantPool != null, "constantPool");
            Contract.Requires<ArgumentNullException>(exceptionTable != null, "exceptionTable");

            Contract.Ensures(Contract.Result<ImmutableList<int?>>() != null);
            Contract.Ensures(Contract.Result<ImmutableList<int?>>().Count == disassembledMethod.Instructions.Count);

            int?[] depths = new int?[disassembledMethod.Instructions.Count];
            Queue<int> workQueue = new Queue<int>();

            // can obviously start at the beginning of the method
            depths[0] = 0;
            workQueue.Enqueue(0);
            // can also start inside each exception handler
            foreach (var entry in exceptionTable)
            {
                int nextIndex = disassembledMethod.Instructions.FindIndex(i => i.Offset == entry.HandlerOffset);
                if (!depths[nextIndex].HasValue)
                {
                    depths[nextIndex] = 1;
                    workQueue.Enqueue(nextIndex);
                }
            }

            while (workQueue.Count > 0)
            {
                int index = workQueue.Dequeue();
                JavaInstruction instruction = disassembledMethod.Instructions[index];
                int netImpact;

                List<string> argumentSignatures = null;
                string returnTypeSignature = null;
                if (instruction.OpCode.FlowControl == JavaFlowControl.Call)
                {
                    IConstantMemberReference memberReference = (IConstantMemberReference)constantPool[instruction.Operands.ConstantPoolIndex - 1];
                    ConstantNameAndType nameAndType = (ConstantNameAndType)constantPool[memberReference.NameAndTypeIndex - 1];
                    ConstantUtf8 descriptor = (ConstantUtf8)constantPool[nameAndType.DescriptorIndex - 1];
                    string signature = descriptor.Value;
                    SignatureHelper.ParseMethodSignature(signature, out argumentSignatures, out returnTypeSignature);
                }

                switch (instruction.OpCode.StackBehaviorPop)
                {
                case JavaStackBehavior.Pop0:
                    netImpact = 0;
                    break;

                case JavaStackBehavior.Pop1:
                case JavaStackBehavior.PopI:
                case JavaStackBehavior.PopI8:
                case JavaStackBehavior.PopR4:
                case JavaStackBehavior.PopR8:
                case JavaStackBehavior.PopRef:
                    netImpact = -1;
                    break;

                case JavaStackBehavior.Pop1_Pop1:
                case JavaStackBehavior.PopI_PopI:
                case JavaStackBehavior.PopI8_PopI8:
                case JavaStackBehavior.PopI8_PopI:
                case JavaStackBehavior.PopR4_PopR4:
                case JavaStackBehavior.PopR8_PopR8:
                case JavaStackBehavior.PopRef_Pop1:
                case JavaStackBehavior.PopRef_PopI:
                    netImpact = -2;
                    break;

                case JavaStackBehavior.PopRef_PopI_PopI:
                case JavaStackBehavior.PopRef_PopI_PopI8:
                case JavaStackBehavior.PopRef_PopI_PopR4:
                case JavaStackBehavior.PopRef_PopI_PopR8:
                case JavaStackBehavior.PopRef_PopI_PopRef:
                case JavaStackBehavior.PopRef_PopRef:
                    netImpact = -3;
                    break;

                case JavaStackBehavior.PopVar:
                    switch (instruction.OpCode.OpCode)
                    {
                    case JavaOpCodeTag.Dup_x2:
                    case JavaOpCodeTag.Dup2:
                    case JavaOpCodeTag.Dup2_x1:
                    case JavaOpCodeTag.Dup2_x2:
                        netImpact = 1;
                        break;

                    case JavaOpCodeTag.Invokestatic:
                        netImpact = -argumentSignatures.Count;
                        if (returnTypeSignature != "V")
                            netImpact++;
                        break;

                    case JavaOpCodeTag.Invokeinterface:
                    case JavaOpCodeTag.Invokespecial:
                    case JavaOpCodeTag.Invokevirtual:
                        netImpact = -argumentSignatures.Count - 1;
                        if (returnTypeSignature != "V")
                            netImpact++;
                        break;

                    case JavaOpCodeTag.Multianewarray:
                        netImpact = -instruction.Operands.Dimensions;
                        break;

                    default:
                        throw new FormatException();
                    }

                    break;

                default:
                    throw new FormatException();
                }

                switch (instruction.OpCode.StackBehaviorPush)
                {
                case JavaStackBehavior.Push0:
                    break;

                case JavaStackBehavior.Push1:
                case JavaStackBehavior.PushI:
                case JavaStackBehavior.PushI8:
                case JavaStackBehavior.PushR4:
                case JavaStackBehavior.PushR8:
                case JavaStackBehavior.PushRef:
                case JavaStackBehavior.PushRet:
                    netImpact++;
                    break;

                case JavaStackBehavior.Push1_Push1:
                    netImpact += 2;
                    break;

                case JavaStackBehavior.PushVar:
                    // these are all handled in the pop section
                    break;

                default:
                    throw new FormatException();
                }

                switch (instruction.OpCode.FlowControl)
                {
                case JavaFlowControl.Next:
                case JavaFlowControl.Break:
                case JavaFlowControl.Call:
                case JavaFlowControl.ConditionalBranch:
                case JavaFlowControl.Special:
                    if (!depths[index + 1].HasValue)
                    {
                        depths[index + 1] = depths[index] + netImpact;
                        workQueue.Enqueue(index + 1);
                    }

                    if (instruction.OpCode.FlowControl == JavaFlowControl.ConditionalBranch)
                        goto case JavaFlowControl.Branch;

                    break;

                case JavaFlowControl.Branch:
                    switch (instruction.OpCode.OpCode)
                    {
                    case JavaOpCodeTag.Lookupswitch:
                        {
                            LookupSwitchData switchData = (LookupSwitchData)disassembledMethod.SwitchData[instruction.Operands.SwitchDataIndex];
                            foreach (var pair in switchData.Pairs)
                            {
                                int nextIndex = disassembledMethod.Instructions.FindIndex(i => i.Offset == instruction.Offset + pair.Value);
                                if (!depths[nextIndex].HasValue)
                                {
                                    depths[nextIndex] = depths[index] + netImpact;
                                    workQueue.Enqueue(nextIndex);
                                }
                            }

                            break;
                        }

                    case JavaOpCodeTag.Tableswitch:
                        {
                            TableSwitchData switchData = (TableSwitchData)disassembledMethod.SwitchData[instruction.Operands.SwitchDataIndex];
                            foreach (var offset in switchData.Offsets)
                            {
                                int nextIndex = disassembledMethod.Instructions.FindIndex(i => i.Offset == instruction.Offset + offset);
                                if (!depths[nextIndex].HasValue)
                                {
                                    depths[nextIndex] = depths[index] + netImpact;
                                    workQueue.Enqueue(nextIndex);
                                }
                            }

                            break;
                        }

                    default:
                        {
                            // single branch target
                            int nextIndex = disassembledMethod.Instructions.FindIndex(i => i.Offset == instruction.Offset + instruction.Operands.BranchTarget);
                            if (!depths[nextIndex].HasValue)
                            {
                                depths[nextIndex] = depths[index] + netImpact;
                                workQueue.Enqueue(nextIndex);
                            }

                            break;
                        }
                    }

                    break;

                case JavaFlowControl.Return:
                    // no work in this method following this instruction
                    break;

                case JavaFlowControl.Throw:
                    // 'catch' blocks are handled separately
                    break;

                case JavaFlowControl.Meta:
                    throw new NotImplementedException();

                default:
                    throw new FormatException();
                }

                if (workQueue.Count == 0)
                {
                    for (int i = 0; i < depths.Length; i++)
                    {
                        if (depths[i].HasValue)
                            continue;

                        depths[i] = 1;
                        workQueue.Enqueue(i);
                        break;
                    }
                }
            }

            return new ImmutableList<int?>(depths);
        }

        private static byte ReadByte(byte[] data, int offset)
        {
            return data[offset];
        }

        private static sbyte ReadSByte(byte[] data, int offset)
        {
            return (sbyte)data[offset];
        }

        private static ushort ReadUInt16(byte[] data, int offset)
        {
            return (ushort)((data[offset] << 8) + data[offset + 1]);
        }

        private static short ReadInt16(byte[] data, int offset)
        {
            return (short)ReadUInt16(data, offset);
        }

        private static int ReadInt32(byte[] data, int offset)
        {
            return ((data[offset] << 24) + (data[offset + 1] << 16) + (data[offset + 2] << 8) + data[offset + 3]);
        }
    }
}
