namespace Tvl.VisualStudio.Shell.OutputWindow.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Tvl.VisualStudio.Shell.OutputWindow.Interfaces;

    using Dispatcher = System.Windows.Threading.Dispatcher;
    using DispatcherPriority = System.Windows.Threading.DispatcherPriority;
    using ErrorHandler = Microsoft.VisualStudio.ErrorHandler;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
    using IVsOutputWindow = Microsoft.VisualStudio.Shell.Interop.IVsOutputWindow;
    using IVsOutputWindowPane = Microsoft.VisualStudio.Shell.Interop.IVsOutputWindowPane;
    using SVsOutputWindow = Microsoft.VisualStudio.Shell.Interop.SVsOutputWindow;
    using SVsServiceProvider = Microsoft.VisualStudio.Shell.SVsServiceProvider;
    using Thread = System.Threading.Thread;
    using VSConstants = Microsoft.VisualStudio.VSConstants;

    [Export(typeof(IOutputWindowService))]
    internal sealed class OutputWindowService : IOutputWindowService
    {
        public OutputWindowService()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        [Import]
        private SVsServiceProvider GlobalServiceProvider
        {
            get;
            set;
        }

        [ImportMany]
        private List<Lazy<OutputWindowDefinition, IOutputWindowDefinitionMetadata>> OutputWindowDefinitions
        {
            get;
            set;
        }

        private Dispatcher Dispatcher
        {
            get;
            set;
        }

        private readonly ConcurrentDictionary<string, Guid?> _outputWindows =
            new ConcurrentDictionary<string, Guid?>(
                new KeyValuePair<string, Guid?>[]
                {
                    new KeyValuePair<string, Guid?>(PredefinedOutputWindowPanes.Build, VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid),
                    new KeyValuePair<string, Guid?>(PredefinedOutputWindowPanes.Debug, VSConstants.OutputWindowPaneGuid.DebugPane_guid),
                    new KeyValuePair<string, Guid?>(PredefinedOutputWindowPanes.General, VSConstants.OutputWindowPaneGuid.GeneralPane_guid),
                });

        public IOutputWindowPane TryGetPane(string name)
        {
            var olesp = (IOleServiceProvider)GlobalServiceProvider.GetService(typeof(IOleServiceProvider));
            if (olesp == null)
                return null;

            var outputWindow = olesp.TryGetGlobalService<SVsOutputWindow, IVsOutputWindow>();
            if (outputWindow == null)
                return null;

            Guid? guid = _outputWindows.GetOrAdd(name, CreateWindowPaneOnMainThread);
            if (!guid.HasValue)
                return null;

            Guid guidValue = guid.Value;
            IVsOutputWindowPane vspane = null;
            if (ErrorHandler.Failed(ErrorHandler.CallWithCOMConvention(() => outputWindow.GetPane(ref guidValue, out vspane))))
                return null;

            IOutputWindowPane pane = new VsOutputWindowPaneAdapter(vspane);
            return pane;
        }

        private Guid? CreateWindowPaneOnMainThread(string name)
        {
            if (Dispatcher.Thread == Thread.CurrentThread)
                return CreateWindowPane(name);

            return (Guid?)Dispatcher.Invoke(DispatcherPriority.Normal, (Func<string, Guid?>)CreateWindowPane, name);
        }

        private Guid? CreateWindowPane(string name)
        {
            var olesp = (IOleServiceProvider)GlobalServiceProvider.GetService(typeof(IOleServiceProvider));
            var outputWindow = olesp.TryGetGlobalService<SVsOutputWindow, IVsOutputWindow>();
            if (outputWindow == null)
                return null;

            var definition = OutputWindowDefinitions.FirstOrDefault(lazy => lazy.Metadata.Name.Equals(name));
            if (definition == null)
                return null;

            Guid guid = Guid.NewGuid();
            // this controls whether the pane is listed in the output panes dropdown list, *not* whether the pane is initially selected
            bool visible = true;
            bool clearWithSolution = false;

            string displayName = definition.Metadata.Name;
            if (definition.Value != null && !string.IsNullOrEmpty(definition.Value.DisplayName))
                displayName = definition.Value.DisplayName;

            if (ErrorHandler.Failed(ErrorHandler.CallWithCOMConvention(() => outputWindow.CreatePane(ref guid, displayName, Convert.ToInt32(visible), Convert.ToInt32(clearWithSolution)))))
                return null;

            return guid;
        }
    }
}
