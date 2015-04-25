namespace Tvl.VisualStudio.Shell
{
    public enum CommandType
    {
        Invalid = -1,

        Separator = 0,
        Button = 1,
        MenuButton = 2,
        Swatch = 3,
        SplitDropDown = 4,

        DropDownCombo = 0x10,
        MRUCombo = 0x20,
        DynamicCombo = 0x30,
        OwnerDropDownCombo = 0x40,
        IndexCombo = 80,
        ComboMask = 0xf0,
        ControlMask = 0xff,

        Menu = 0x100,
        MenuController = 0x200,
        MenuToolbar = 0x300,
        MenuContext = 0x400,
        MenuToolWindowToolbar = 0x500,
        MenuControllerLatched = 0x600,
        MenuMask = 0xf00,

        Shared = 0x10000000,
        AppId = 0x20000000,
        SharedMask = unchecked((int)0xf0000000),
    }
}
