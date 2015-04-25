namespace Tvl.VisualStudio.Shell.Implementation
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Tvl.VisualStudio.OutputWindow.Interfaces;

    public class StandardTaskSchedulers
    {
        [Export(PredefinedTaskSchedulers.ProjectCacheIntelliSense, typeof(TaskScheduler))]
        private readonly BackgroundParserTaskScheduler ProjectCacheIntelliSenseScheduler;

        [Export(PredefinedTaskSchedulers.BackgroundIntelliSense, typeof(TaskScheduler))]
        private readonly BackgroundParserTaskScheduler BackgroundIntelliSenseScheduler;

        [Export(PredefinedTaskSchedulers.PriorityIntelliSense, typeof(TaskScheduler))]
        private readonly BackgroundParserTaskScheduler PriorityIntelliSenseScheduler;

        [ImportingConstructor]
        public StandardTaskSchedulers(IOutputWindowService outputWindowService)
        {
            ProjectCacheIntelliSenseScheduler = new BackgroundParserTaskScheduler("TVL Low Priority Background", BackgroundParserTaskScheduler.DefaultConcurrencyLevel, outputWindowService);
            BackgroundIntelliSenseScheduler = new BackgroundParserTaskScheduler(outputWindowService);
            PriorityIntelliSenseScheduler = new BackgroundParserTaskScheduler("TVL Priority IntelliSense", 2, outputWindowService);
        }
    }
}
