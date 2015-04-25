namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.InteropServices;

    public static class ValueHelper
    {
        public static int SingleToInt32Bits(float value)
        {
            FloatConversionStruct fcs = new FloatConversionStruct(value);
            return fcs.IntValue;
        }

        public static float Int32BitsToSingle(int value)
        {
            FloatConversionStruct fcs = new FloatConversionStruct(value);
            return fcs.FloatValue;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatConversionStruct
        {
            [FieldOffset(0)]
            public int IntValue;
            [FieldOffset(0)]
            public float FloatValue;

            public FloatConversionStruct(int value)
                : this()
            {
                IntValue = value;
            }

            public FloatConversionStruct(float value)
                : this()
            {
                FloatValue = value;
            }
        }
    }
}
