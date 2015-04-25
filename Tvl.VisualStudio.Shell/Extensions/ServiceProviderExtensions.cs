namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    public static class ServiceProviderExtensions
    {
        public static SVsServiceProvider AsVsServiceProvider(this IServiceProvider sp)
        {
            Contract.Requires<ArgumentNullException>(sp != null, "sp");
            Contract.Ensures(Contract.Result<SVsServiceProvider>() != null);

            return new VsServiceProviderWrapper(sp);
        }

        public static TService GetService<TService>(this IServiceProvider sp)
        {
            Contract.Requires(sp != null);

            return GetService<TService, TService>(sp);
        }

        public static TServiceInterface GetService<TServiceClass, TServiceInterface>(this IServiceProvider sp)
        {
            Contract.Requires<ArgumentNullException>(sp != null, "sp");

            return (TServiceInterface)sp.GetService(typeof(TServiceClass));
        }

        public static IOleServiceProvider TryGetOleServiceProvider(this IServiceProvider sp)
        {
            Contract.Requires<ArgumentNullException>(sp != null, "sp");

            return (IOleServiceProvider)sp.GetService(typeof(IOleServiceProvider));
        }

        public static TServiceInterface TryGetGlobalService<TServiceClass, TServiceInterface>(this IOleServiceProvider sp)
            where TServiceInterface : class
        {
            Contract.Requires<ArgumentNullException>(sp != null, "sp");

            Guid guidService = typeof(TServiceClass).GUID;
            Guid riid = typeof(TServiceClass).GUID;
            IntPtr obj = IntPtr.Zero;
            int result = ErrorHandler.CallWithCOMConvention(() => sp.QueryService(ref guidService, ref riid, out obj));
            if (ErrorHandler.Failed(result) || obj == IntPtr.Zero)
                return null;

            try
            {
                TServiceInterface service = (TServiceInterface)Marshal.GetObjectForIUnknown(obj);
                return service;
            }
            finally
            {
                Marshal.Release(obj);
            }
        }
    }
}
