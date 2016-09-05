﻿using Ntreev.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Commands.Commands.Test.Commands
{
    [Export(typeof(ICommand))]
    [GitSummary("StashSummary")]
    [GitDescription("StashDescription")]
    class StashCommand : ICommand
    {
        public bool HasSubCommand
        {
            get { return true; }
        }

        public string Name
        {
            get { return "stash"; }
        }

        public void Execute()
        {

        }

        [CommandMethod("list")]
        [GitDescription("ListDescription_StashCommand")]
        public void List(string options)
        {


        }

        [CommandMethod("show")]
        [CommandMethodSwitch("Path", "Port")]
        public void Show(int value)
        {


        }

        [CommandMethod("save")]
        [CommandMethodSwitch("Patch", "KeepIndex", "IncludeUntracked", "All", "Quit")]
        [GitDescription("SaveDescription_StashCommand")]
        public void Save(string message)
        {


        }

        [CommandSwitch(ShortName = 'p')]
        [GitDescription("PatchDescription_StashCommand")]
        public bool Patch
        {
            get; set;
        }

        [CommandSwitch(ShortName = 'k')]
        public bool KeepIndex
        {
            get; set;
        }

        [CommandSwitch(ShortName = 'u')]
        public bool IncludeUntracked
        {
            get; set;
        }

        [CommandSwitch(ShortName = 'a')]
        public bool All
        {
            get; set;
        }

        [CommandSwitch(ShortName = 'q')]
        public bool Quit
        {
            get; set;
        }

        [CommandSwitch]
        public int Path
        {
            get; set;
        }

        [CommandSwitch(Required = true)]
        public int Port
        {
            get; set;
        }
    }
}
