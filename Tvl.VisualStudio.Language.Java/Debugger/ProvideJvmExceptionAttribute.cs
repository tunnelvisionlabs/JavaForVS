namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Shell;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ProvideJvmExceptionAttribute : ProvideDebuggerExceptionAttribute
    {
        public ProvideJvmExceptionAttribute(string exceptionFullName)
            : base(JavaDebuggerConstants.JavaDebugEngineGuidString, JavaDebuggerConstants.JvmExceptionKind, GetNamespace(exceptionFullName), exceptionFullName)
        {
            Contract.Requires<ArgumentNullException>(exceptionFullName != null, "exceptionFullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(exceptionFullName));
        }

        private static string GetNamespace(string exceptionFullName)
        {
            Contract.Requires(!string.IsNullOrEmpty(exceptionFullName));
            return exceptionFullName.Substring(0, Math.Max(0, exceptionFullName.LastIndexOf('.')));
        }
    }
}
