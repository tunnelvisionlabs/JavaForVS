namespace Tvl.VisualStudio.Shell
{
    using System;

    [Flags]
    public enum CommandOptions
    {
        None = 0,
        NoKeyCustomize = 1,
        NoButtonCustomize = 2,
        NoCustomize = 3,
        TextContextUseButton = 4,
        TextChanges = 8,
        DefaultDisabled = 0x10,
        DefaultInvisible = 0x20,
        DynamicVisibility = 0x40,
        Repeat = 0x80,
        AllowParams = 0x1000,
        DynamicItemStart = 0x100,
        CommandWellOnly = 0x200,
        Picture = 0x400,
        Text = 0x800,
        PictureAndText = 0xc00,
        FilterKeys = 0x2000,
        PostExec = 0x4000,
        DontCache = 0x8000,
        FixMenuController = 0x10000,
        NoShowOnMenuController = 0x20000,
        RouteToDocuments = 0x40000,
        NoAutoComplete = 0x80000,
        TextMenuUseBtn = 0x100000,
        TextMenuCtrlUseMnu = 0x200000,
        TextCascadeUseBtn = 0x400000,
        CaseSensitive = 0x800000,

        MenuDefaultDocked = 0x1000000,
        MenuNoToolbarClose = 0x2000000,
        MenuNotInTbList = 0x4000000,
        MenuAlwaysCreate = 0x8000000,

        ProfferedCommand = 0x10000000,
        TextIsAnchor = 0x20000000,
        StretchHorizontally = 0x40000000,

        ValidFlags = 0x7fffffff
    }
}
