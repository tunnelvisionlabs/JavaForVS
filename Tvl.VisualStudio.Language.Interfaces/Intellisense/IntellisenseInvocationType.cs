namespace Tvl.VisualStudio.Language.Intellisense
{
    public enum IntellisenseInvocationType
    {
        Default,
        IdentifierChar,
        Sharp, // preprocessor directives
        BackspaceDeleteOrBackTab,
        Space,
        //QMark,
        ShowMemberList,
    }
}
