namespace Tvl
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public static class ExceptionExtensions
    {
        private static readonly Action<Exception> _internalPreserveStackTrace =
            (Action<Exception>)Delegate.CreateDelegate(
                typeof(Action<Exception>),
                typeof(Exception).GetMethod(
                    "InternalPreserveStackTrace",
                    BindingFlags.Instance | BindingFlags.NonPublic));

#pragma warning disable 618
        public static bool IsCritical(this Exception e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

            if (e is AccessViolationException
                || e is StackOverflowException
                || e is ExecutionEngineException
                || e is OutOfMemoryException
                || e is BadImageFormatException
                || e is AppDomainUnloadedException)
            {
                return true;
            }

            return false;
        }
#pragma warning restore 618

        public static void PreserveStackTrace(this Exception e)
        {
            Contract.Requires<ArgumentNullException>(e != null, "e");

            _internalPreserveStackTrace(e);
        }
    }
}
