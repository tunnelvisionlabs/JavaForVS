namespace Tvl
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        public static T HandleNonCriticalExceptions<T>(this T task)
            where T : Task
        {
            Contract.Requires<ArgumentNullException>(task != null, "task");
            Contract.Ensures(Contract.Result<T>() != null);

            task.ContinueWith(HandleNonCriticalExceptionsCore, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        private static void HandleNonCriticalExceptionsCore(Task task)
        {
            Contract.Requires<ArgumentNullException>(task != null, "task");

            AggregateException exception = task.Exception;
            if (HasCriticalException(exception))
                throw exception;
        }

        private static bool HasCriticalException(Exception exception)
        {
            Contract.Requires<ArgumentNullException>(exception != null, "exception");

            AggregateException aggregate = exception as AggregateException;
            if (aggregate != null)
                return aggregate.InnerExceptions != null && aggregate.InnerExceptions.Any(HasCriticalException);

            return exception.IsCritical();
        }
    }
}
