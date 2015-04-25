namespace Tvl.VisualStudio.Text.Navigation
{
    using System;

    [Flags, Serializable]
    public enum NavigationTargetStyle
    {
        None = 0,
        Bold = 0x0001,
        Gray = 0x0002,
        Italic = 0x0004,
        Underlined = 0x0008,
    }
}
