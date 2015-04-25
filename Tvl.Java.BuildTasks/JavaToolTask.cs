namespace Tvl.Java.BuildTasks
{
    using System;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;
    using Directory = System.IO.Directory;
    using File = System.IO.File;
    using Path = System.IO.Path;
    using SecurityException = System.Security.SecurityException;

    public abstract class JavaToolTask : ToolTask
    {
        [Required]
        public string JvmRegistryBase
        {
            get;
            set;
        }

        [Required]
        public string Platform
        {
            get;
            set;
        }

        protected override Encoding ResponseFileEncoding
        {
            get
            {
                return new UTF8Encoding(false);
            }
        }

        protected override string GenerateFullPathToTool()
        {
            if (File.Exists(ToolPath))
                return Path.GetFullPath(ToolPath);

            return FindJavaBinary(ToolName, true);
        }
        
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilderExtension commandLine = new CommandLineBuilderExtension();
            AddCommandLineCommands(commandLine);
            return commandLine.ToString().Replace('\\', '/');
        }

        protected override string GenerateResponseFileCommands()
        {
            CommandLineBuilderExtension commandLine = new CommandLineBuilderExtension();
            AddResponseFileCommands(commandLine);
            return commandLine.ToString().Replace('\\', '/');
        }

        protected virtual void AddCommandLineCommands(CommandLineBuilderExtension commandLine)
        {
        }

        protected virtual void AddResponseFileCommands(CommandLineBuilderExtension commandLine)
        {
        }

        protected virtual string FindJavaBinary(string fileName, bool developmentKit)
        {
            string vendorBase = JvmRegistryBase;

            bool allow64bit = Platform.Equals("X64", StringComparison.OrdinalIgnoreCase) || Platform.Equals("AnyCPU", StringComparison.OrdinalIgnoreCase);
            bool allow32bit = Platform.Equals("X86", StringComparison.OrdinalIgnoreCase) || Platform.Equals("AnyCPU", StringComparison.OrdinalIgnoreCase);

            string softwareRegKey = @"SOFTWARE\";
            string installation = developmentKit ? "Java Development Kit" : "Java Runtime Environment";

            if (allow64bit)
            {
                string registryRoot = softwareRegKey + JvmRegistryBase + @"\" + installation;
                string path = FindJavaPath(RegistryView.Registry64, registryRoot, fileName);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            if (allow32bit)
            {
                string registryRoot = softwareRegKey + JvmRegistryBase + @"\" + installation;
                string path = FindJavaPath(RegistryView.Registry32, registryRoot, fileName);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            return null;
        }

        private static string FindJavaPath(RegistryView view, string registryRoot, string fileName)
        {
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                using (RegistryKey jdk = baseKey.OpenSubKey(registryRoot, RegistryKeyPermissionCheck.ReadSubTree))
                {
                    if (jdk == null)
                        return null;

                    string currentVersion = jdk.GetValue("CurrentVersion") as string;
                    if (currentVersion == null)
                        return null;

                    using (RegistryKey jdkVersion = jdk.OpenSubKey(currentVersion, RegistryKeyPermissionCheck.ReadSubTree))
                    {
                        if (jdkVersion == null)
                            return null;

                        string javaHome = jdkVersion.GetValue("JavaHome") as string;
                        if (!Directory.Exists(javaHome))
                            return null;

                        string javac = Path.Combine(javaHome, "bin", fileName);
                        if (!File.Exists(javac))
                            return null;

                        return javac;
                    }
                }
            }
            catch (SecurityException)
            {
                return null;
            }
        }
    }
}
