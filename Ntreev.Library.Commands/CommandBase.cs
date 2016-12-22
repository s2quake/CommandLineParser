﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Commands
{
    public abstract class CommandBase : ICommand
    {
        private readonly string name;
        private readonly CommandTypes types;

        protected CommandBase(string name)
            : this(name, CommandTypes.None)
        {

        }

        protected CommandBase(string name, CommandTypes types)
        {
            this.name = name;
            this.types = types;
        }

        public string Name
        {
            get { return this.name; }
        }

        public CommandTypes Types
        {
            get { return this.types; }
        }

        protected abstract void OnExecute();

        #region ICommand

        void ICommand.Execute()
        {
            this.OnExecute();
        }

        #endregion
    }
}
