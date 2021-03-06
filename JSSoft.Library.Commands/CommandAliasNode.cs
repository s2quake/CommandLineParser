﻿// Released under the MIT License.
// 
// Copyright (c) 2018 Ntreev Soft co., Ltd.
// Copyright (c) 2020 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Forked from https://github.com/NtreevSoft/CommandLineParser
// Namespaces and files starting with "Ntreev" have been renamed to "JSSoft".

using JSSoft.Library.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace JSSoft.Library.Commands
{
    class CommandAliasNode : ICommandNode
    {
        private readonly ICommandNode commandNode;

        public CommandAliasNode(ICommandNode commandNode, string alias)
        {
            this.commandNode = commandNode;
            this.Name = alias;
        }

        public ICommandNode Parent => this.commandNode.Parent;

        public IContainer<ICommandNode> Childs => this.commandNode.Childs;

        public IContainer<ICommandNode> ChildsByAlias => this.commandNode.ChildsByAlias;

        public string Name { get; }

        public string[] Aliases => this.commandNode.Aliases;

        public ICommand Command => this.commandNode.Command;

        public ICommandDescriptor Descriptor => this.commandNode.Descriptor;

        public CommandContextBase CommandContext => this.commandNode.CommandContext;

        public bool IsEnabled => this.commandNode.IsEnabled;

        public IEnumerable<ICommand> Commands => this.commandNode.Commands;
    }
}
