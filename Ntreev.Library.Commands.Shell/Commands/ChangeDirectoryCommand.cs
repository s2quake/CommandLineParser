﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Commands.Shell.Commands
{
    [Export(typeof(ICommand))]
    [UsageDescriptionProvider(typeof(ResourceUsageDescriptionProvider))]
    class ChangeDirectoryCommand : Command
    {
        [Import]
        private Lazy<IShell> shell = null;
        [Import]
        private Lazy<CommandContext> commandContext = null;

        public ChangeDirectoryCommand()
            : base("cd", CommandTypes.AllowEmptyArgument)
        {
            this.DirectoryName = string.Empty;
        }

        public override void Execute()
        {
            var shell = this.shell.Value;
            if (this.DirectoryName == string.Empty)
            {
                this.Out.WriteLine(shell.Prompt);
            }
            else if (this.DirectoryName == "..")
            {
                var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
                Directory.SetCurrentDirectory(dir);
                shell.Prompt = dir;
            }
            else if(Directory.Exists(this.DirectoryName) == true)
            {
                var dir = new DirectoryInfo(this.DirectoryName).FullName;
                Directory.SetCurrentDirectory(dir);
                shell.Prompt = dir;
            }
            else
            {
                throw new DirectoryNotFoundException(string.Format("'{0}'은(는) 존재하지 않는 경로입니다.", this.DirectoryName));
            }
        }

        [CommandSwitch(Name = "dir", Required = true)]
        public string DirectoryName
        {
            get; set;
        }

        public TextWriter Out
        {
            get { return this.commandContext.Value.Out; }
        }
    }
}
