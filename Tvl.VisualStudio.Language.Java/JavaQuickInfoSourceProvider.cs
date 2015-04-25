namespace Tvl.VisualStudio.Language.Java
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;
    using IntelliSenseCache = Tvl.VisualStudio.Language.Java.SourceData.IntelliSenseCache;
    using IOutputWindowService = Tvl.VisualStudio.OutputWindow.Interfaces.IOutputWindowService;
    using IQuickInfoBroker = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoBroker;
    using IQuickInfoSource = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoSource;
    using IQuickInfoSourceProvider = Microsoft.VisualStudio.Language.Intellisense.IQuickInfoSourceProvider;
    using ITextBuffer = Microsoft.VisualStudio.Text.ITextBuffer;
    using PredefinedTaskSchedulers = Tvl.VisualStudio.Shell.PredefinedTaskSchedulers;
    using TaskScheduler = System.Threading.Tasks.TaskScheduler;

    [Export(typeof(IQuickInfoSourceProvider))]
    [Order]
    [ContentType(Constants.JavaContentType)]
    [Name("JavaQuickInfoSource")]
    internal class JavaQuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        public IOutputWindowService OutputWindowService
        {
            get;
            private set;
        }

        [Import(PredefinedTaskSchedulers.PriorityIntelliSense)]
        public TaskScheduler PriorityIntelliSenseTaskScheduler
        {
            get;
            private set;
        }

        [Import]
        public IQuickInfoBroker QuickInfoBroker
        {
            get;
            private set;
        }

        [Import]
        public IntelliSenseCache IntelliSenseCache
        {
            get;
            private set;
        }

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            // this functionality is currently disabled
            return null;
        }
    }
}
