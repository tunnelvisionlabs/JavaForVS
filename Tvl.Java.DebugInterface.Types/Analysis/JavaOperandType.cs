namespace Tvl.Java.DebugInterface.Types.Analysis
{
    public enum JavaOperandType
    {
        InlineNone,

        InlineI1,
        InlineI2,

        InlineShortBranchTarget,
        InlineBranchTarget,

        InlineLookupSwitch,
        InlineTableSwitch,

        InlineShortConst,
        InlineConst,

        InlineVar,
        InlineField,
        InlineMethod,
        InlineType,
        InlineArrayType,

        InlineVar_I1,
        InlineMethod_U1_0,
        InlineType_U1,
    }
}
