namespace Tvl.VisualStudio.Language.Java.Project
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Build.Utilities;

    public static class CommandLineBuilderExtensions
    {
        public static void AppendSwitchIfNotNullOrEmpty(this CommandLineBuilder commandLine, string switchName, string parameter)
        {
            Contract.Requires<ArgumentNullException>(commandLine != null, "commandLine");
            Contract.Requires<ArgumentNullException>(switchName != null, "switchName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(switchName));

            if (!string.IsNullOrEmpty(parameter))
                commandLine.AppendSwitchIfNotNull(switchName, parameter);
        }
    }
}
