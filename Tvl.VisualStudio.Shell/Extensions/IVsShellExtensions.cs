namespace Tvl.VisualStudio.Shell
{
    using ArgumentNullException = System.ArgumentNullException;
    using Contract = System.Diagnostics.Contracts.Contract;
    using ErrorHandler = Microsoft.VisualStudio.ErrorHandler;
    using Guid = System.Guid;
    using IVsPackage = Microsoft.VisualStudio.Shell.Interop.IVsPackage;
    using IVsShell = Microsoft.VisualStudio.Shell.Interop.IVsShell;
    using Package = Microsoft.VisualStudio.Shell.Package;

    public static class IVsShellExtensions
    {
        public static T LoadPackage<T>(this IVsShell shell)
            where T : Package
        {
            Contract.Requires<ArgumentNullException>(shell != null, "shell");

            Guid guid = typeof(T).GUID;
            IVsPackage package;
            ErrorHandler.ThrowOnFailure(shell.LoadPackage(ref guid, out package));
            return (T)package;
        }
    }
}
