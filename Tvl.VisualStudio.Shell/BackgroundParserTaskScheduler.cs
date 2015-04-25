namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio;
    using Tvl.VisualStudio.OutputWindow.Interfaces;

    using CancellationTokenSource = System.Threading.CancellationTokenSource;
    using Thread = System.Threading.Thread;

    public class BackgroundParserTaskScheduler : TaskScheduler
    {
#if DEBUG
        public static readonly int DefaultConcurrencyLevel = 1;
#else
        public static readonly int DefaultConcurrencyLevel = Environment.ProcessorCount;
#endif

        private readonly int _maximumConcurrencyLevel;
        private readonly BlockingCollection<Task> _blockingQueue;
        private readonly Thread[] _workerThreads;
        private readonly IOutputWindowService _outputWindowService;
        private CancellationTokenSource _shutdownParser = new CancellationTokenSource();

        public BackgroundParserTaskScheduler(IOutputWindowService outputWindowService)
            : this("TVL IntelliSense", DefaultConcurrencyLevel, outputWindowService)
        {
            Contract.Requires(outputWindowService != null);
        }

        public BackgroundParserTaskScheduler(string name, int maximumConcurrencyLevel, IOutputWindowService outputWindowService)
        {
            Contract.Requires<ArgumentOutOfRangeException>(maximumConcurrencyLevel > 0);
            Contract.Requires<ArgumentNullException>(outputWindowService != null, "outputWindowService");

            _blockingQueue = new BlockingCollection<Task>();

            _outputWindowService = outputWindowService;
            _maximumConcurrencyLevel = maximumConcurrencyLevel;
            _workerThreads = new Thread[_maximumConcurrencyLevel];
            for (int i = 0; i < maximumConcurrencyLevel; i++)
            {
                _workerThreads[i] = new Thread(DispatchLoop)
                {
                    IsBackground = true,
                    Name = string.Format("{0} Thread {1}", name, i),
                };

                _workerThreads[i].Start();
            }
        }

        public override int MaximumConcurrencyLevel
        {
            get
            {
                return _maximumConcurrencyLevel;
            }
        }

        protected IOutputWindowService OutputWindowService
        {
            get
            {
                return _outputWindowService;
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _blockingQueue.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            _blockingQueue.Add(task);
        }

        protected override bool TryDequeue(Task task)
        {
            return false;
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        private void DispatchLoop()
        {
            try
            {
                while (true)
                {
                    Task task = _blockingQueue.Take(_shutdownParser.Token);

                    int remaining = _blockingQueue.Count;
                    if ((remaining % 200) == 0)
                        OutputLine(string.Format("Parser queue has {0} items remaining", remaining));

                    TryExecuteTask(task);
                    if (_shutdownParser.IsCancellationRequested)
                        return;
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                if (ErrorHandler.IsCriticalException(ex))
                    throw;
            }
        }

        private void OutputLine(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var outputWindow = OutputWindowService.TryGetPane(PredefinedOutputWindowPanes.TvlDiagnostics);
            if (outputWindow == null)
                return;

            outputWindow.WriteLine(message);
        }
    }
}
