namespace Tvl.VisualStudio.Shell
{
    using System;

    [Flags]
    public enum PropertySheetPageFlags : uint
    {
        /// <summary>
        /// Uses the default meaning for all structure members. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_DEFAULT = 0x0000,

        /// <summary>
        /// Creates the page from the dialog box template in memory pointed to by the pResource member. The PropertySheet function assumes that the template that is in memory is not write-protected. A read-only template will cause an exception in some versions of Windows.
        /// </summary>
        PSP_DLGINDIRECT = 0x0001,

        /// <summary>
        /// Uses hIcon as the small icon on the tab for the page. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_USEHICON = 0x0002,

        /// <summary>
        /// Uses pszIcon as the name of the icon resource to load and use as the small icon on the tab for the page. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_USEICONID = 0x0004,

        /// <summary>
        /// Uses the pszTitle member as the title of the property sheet dialog box instead of the title stored in the dialog box template. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_USETITLE = 0x0008,

        /// <summary>
        /// Reverses the direction in which pszTitle is displayed. Normal windows display all text, including pszTitle, left-to-right (LTR). For languages such as Hebrew or Arabic that read right-to-left (RTL), a window can be mirrored and all text will be displayed RTL. If PSP_RTLREADING is set, pszTitle will instead read RTL in a normal parent window, and LTR in a mirrored parent window.
        /// </summary>
        PSP_RTLREADING = 0x0010,

        /// <summary>
        /// Enables the property sheet Help button when the page is active. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_HASHELP = 0x0020,

        /// <summary>
        /// Maintains the reference count specified by the pcRefParent member for the lifetime of the property sheet page created from this structure.
        /// </summary>
        PSP_USEREFPARENT = 0x0040,

        /// <summary>
        /// Calls the function specified by the pfnCallback member when creating or destroying the property sheet page defined by this structure.
        /// </summary>
        PSP_USECALLBACK = 0x0080,

        /// <summary>
        /// Causes the page to be created when the property sheet is created. If this flag is not specified, the page will not be created until it is selected the first time. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_PREMATURE = 0x0400,

        /// <summary>
        /// Causes the wizard property sheet to hide the header area when the page is selected. If a watermark has been provided, it will be painted on the left side of the page. This flag should be set for welcome and completion pages, and omitted for interior pages. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_HIDEHEADER = 0x0800,

        /// <summary>
        /// Displays the string pointed to by the pszHeaderTitle member as the title in the header of a Wizard97 interior page. You must also set the PSH_WIZARD97 flag in the dwFlags member of the associated PROPSHEETHEADER structure. The PSP_USEHEADERTITLE flag is ignored if PSP_HIDEHEADER is set. This flag is not supported when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        PSP_USEHEADERTITLE = 0x1000,

        /// <summary>
        /// Displays the string pointed to by the pszHeaderSubTitle member as the subtitle of the header area of a Wizard97 page. To use this flag, you must also set the PSH_WIZARD97 flag in the dwFlags member of the associated PROPSHEETHEADER structure. The PSP_USEHEADERSUBTITLE flag is ignored if PSP_HIDEHEADER is set. In Aero-style wizards, the title appears near the top of the client area.
        /// </summary>
        PSP_USEHEADERSUBTITLE = 0x2000,

        /// <summary>
        /// Use an activation context. To use an activation context, you must set this flag and assign the activation context handle to hActCtx. See the Remarks.
        /// </summary>
        PSP_USEFUSIONCONTEXT = 0x4000,
    }
}
