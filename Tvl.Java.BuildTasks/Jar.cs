namespace Tvl.Java.BuildTasks
{
    using System;
    using System.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;

    public class Jar : JavaToolTask
    {
        public ITaskItem JarFile
        {
            get;
            set;
        }

        public ITaskItem[] Inputs
        {
            get;
            set;
        }

        public string Command
        {
            get;
            set;
        }

        public bool Verbose
        {
            get;
            set;
        }

        public bool Compress
        {
            get;
            set;
        }

        public bool CreateManifest
        {
            get;
            set;
        }

        public ITaskItem[] Manifest
        {
            get;
            set;
        }

        public string EntryPoint
        {
            get;
            set;
        }

        public string JvmOptions
        {
            get;
            set;
        }

        protected override string ToolName
        {
            get
            {
                return "jar.exe";
            }
        }

        protected override void AddCommandLineCommands(CommandLineBuilderExtension commandLine)
        {
            if (!string.IsNullOrEmpty(JvmOptions))
                commandLine.AppendTextUnquoted(" " + JvmOptions.Trim());
        }

        protected override void AddResponseFileCommands(CommandLineBuilderExtension commandLine)
        {
            JarCommand command;
            string flags = "";

            switch (Command.ToLowerInvariant())
            {
            case "create":
                command = JarCommand.Create;
                flags += "c";
                break;

            case "update":
                command = JarCommand.Update;
                flags += "u";
                break;

            case "extract":
                command = JarCommand.Extract;
                flags += "x";
                break;

            case "index":
                command = JarCommand.Index;
                flags += "i";
                break;

            case "list":
                command = JarCommand.List;
                flags += "t";
                break;

            default:
                throw new InvalidOperationException();
            }

            if (command != JarCommand.Index && JarFile != null && JarFile.ItemSpec != null)
                flags += 'f';

            if (Verbose && command != JarCommand.Index)
                flags += 'v';

            if (command == JarCommand.Create || command == JarCommand.Update)
            {
                if (!Compress)
                    flags += '0';

                if (!CreateManifest)
                    flags += 'M';

                if (Manifest != null && Manifest.Length != 0 && Manifest[0].ItemSpec != null)
                    flags += 'm';
            }

            commandLine.AppendSwitch(flags);

            if (JarFile != null && JarFile.ItemSpec != null)
                commandLine.AppendFileNameIfNotNull(JarFile);

            if (command == JarCommand.Create || command == JarCommand.Update)
            {
                if (Manifest != null && Manifest.Length != 0 && Manifest[0].ItemSpec != null)
                    commandLine.AppendFileNameIfNotNull(Manifest[0]);
            }

            ILookup<string, ITaskItem> inputs = Inputs.ToLookup(i => i.GetMetadata("BaseOutputDirectory") ?? string.Empty);

            commandLine.AppendFileNamesIfNotNull(inputs[string.Empty].ToArray(), " ");
            foreach (var pair in inputs)
            {
                if (pair.Key == string.Empty)
                    continue;

                foreach (var file in pair)
                {
                    commandLine.AppendSwitchIfNotNull("-C ", pair.Key);
                    commandLine.AppendFileNameIfNotNull(file.ItemSpec.StartsWith(pair.Key) ? file.ItemSpec.Substring(pair.Key.Length) : file.ItemSpec);
                }

                //commandLine.AppendSwitchIfNotNull("-C ", pair.Key);
                //commandLine.AppendFileNamesIfNotNull(pair.Select(i => i.ItemSpec.StartsWith(pair.Key) ? i.ItemSpec.Substring(pair.Key.Length) : i.ItemSpec).ToArray(), " ");
            }

            if (!string.IsNullOrEmpty(EntryPoint))
                commandLine.AppendSwitchIfNotNull("-e ", EntryPoint);
        }

        private enum JarCommand
        {
            Create,
            Update,
            Extract,
            Index,
            List
        }
    }
}
