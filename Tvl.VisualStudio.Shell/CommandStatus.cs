namespace Tvl.VisualStudio.Shell
{
    using System;

    [Flags]
    public enum CommandStatus
    {
        None,
        Supported=1,
        Enabled=2,
        Latched=4,
        Ninched=8,
        Invisible=16,
        DefaultHideOnContextMenu =32
    }
}
