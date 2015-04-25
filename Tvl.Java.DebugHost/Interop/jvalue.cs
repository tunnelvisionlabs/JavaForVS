namespace Tvl.Java.DebugHost.Interop
{
    using System;
    using System.Runtime.InteropServices;
    using ObjectId = Tvl.Java.DebugInterface.Types.ObjectId;
    using Tag = Tvl.Java.DebugInterface.Types.Tag;
    using Value = Tvl.Java.DebugInterface.Types.Value;

    [StructLayout(LayoutKind.Explicit)]
    public struct jvalue
    {
        [FieldOffset(0)]
        public byte ByteValue;

        [FieldOffset(0)]
        public char CharValue;

        [FieldOffset(0)]
        public short ShortValue;

        [FieldOffset(0)]
        public int IntValue;

        [FieldOffset(0)]
        public long LongValue;

        [FieldOffset(0)]
        public float FloatValue;

        [FieldOffset(0)]
        public double DoubleValue;

        [FieldOffset(0)]
        public jobject ObjectValue;

        public jvalue(JavaVM virtualMachine, JvmtiEnvironment environment, JniEnvironment nativeEnvironment, Value value)
            : this()
        {
            if (value.Data == 0)
                return;

            switch (value.Tag)
            {
            case Tag.Byte:
                ByteValue = (byte)value.Data;
                break;

            case Tag.Char:
                CharValue = (char)value.Data;
                break;

            case Tag.Float:
                IntValue = (int)(uint)value.Data;
                break;

            case Tag.Int:
                IntValue = (int)value.Data;
                break;

            case Tag.Double:
                LongValue = (long)(ulong)value.Data;
                break;

            case Tag.Long:
                LongValue = value.Data;
                break;

            case Tag.Short:
                ShortValue = (short)value.Data;
                break;

            case Tag.Boolean:
                break;

            case Tag.Array:
            case Tag.Object:
            case Tag.String:
            case Tag.Thread:
            case Tag.ThreadGroup:
            case Tag.ClassLoader:
            case Tag.ClassObject:
                if (value.Data == 0)
                    return;

                ObjectValue = virtualMachine.GetLocalReferenceForObject(nativeEnvironment, new ObjectId(value.Data)).Value;
                break;

            case Tag.Void:
            case Tag.Invalid:
            default:
                throw new ArgumentException();
            }
        }
    }
}
