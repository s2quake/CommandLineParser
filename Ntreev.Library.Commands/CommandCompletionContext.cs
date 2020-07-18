﻿//Released under the MIT License.
//
//Copyright (c) 2018 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ntreev.Library.Commands
{
    public class CommandCompletionContext
    {
        internal static object Create(ICommand command, IEnumerable<CommandMemberDescriptor> members, string[] args, string find)
        {
            var parser = new ParseDescriptor(members, args);
            var properties = new Dictionary<string, object>();
            var memberDescriptor = (CommandMemberDescriptor)null;
            var itemByDescriptor = parser.Items.ToDictionary(item => item.Descriptor);

            foreach (var item in itemByDescriptor)
            {
                var descriptor = item.Key;
                var parseInfo = item.Value;
                if (parseInfo.IsParsed == true)
                {
                    properties.Add(descriptor.DescriptorName, parseInfo.Value);
                    if (descriptor.Usage != CommandPropertyUsage.Variables)
                        itemByDescriptor.Remove(descriptor);
                }
            }

            if (args.Any() == true)
            {
                var arg = args.First();

                foreach (var item in itemByDescriptor)
                {
                    var descriptor = item.Key;
                    if (arg == descriptor.ShortNamePattern || arg == descriptor.NamePattern)
                    {
                        // int qer = 0;
                    }
                }
            }
            if (find.StartsWith(CommandSettings.Delimiter) == true)
            {
                var argList = new List<string>();
                foreach (var item in itemByDescriptor)
                {
                    var descriptor = item.Key;
                    if (descriptor.NamePattern != string.Empty)
                        argList.Add(descriptor.NamePattern);
                }
                return argList.ToArray();
            }
            else if (find.StartsWith(CommandSettings.ShortDelimiter) == true)
            {
                var argList = new List<string>();
                foreach (var item in itemByDescriptor)
                {
                    var descriptor = item.Key;
                    if (descriptor.ShortNamePattern != string.Empty)
                        argList.Add(descriptor.ShortNamePattern);
                }
                return argList.ToArray();
            }
            else
            {
                foreach (var item in itemByDescriptor)
                {
                    var descriptor = item.Key;
                    var parseInfo = item.Value;
                    if (memberDescriptor == null)
                    {
                        memberDescriptor = descriptor;
                    }
                }
                return new CommandCompletionContext(command, memberDescriptor, args.ToArray(), find, properties);
            }
        }

        private CommandCompletionContext(ICommand command, CommandMemberDescriptor member, string[] args, string find, Dictionary<string, object> properties)
        {
            this.Command = command;
            this.MemberDescriptor = member;
            this.Arguments = args;
            this.Find = find;
            this.Properties = properties;
        }

        public ICommand Command { get; }

        public CommandMemberDescriptor MemberDescriptor { get; }

        public string Find { get; }

        public string[] Arguments { get; }

        public IDictionary<string, object> Properties { get; }
    }
}
