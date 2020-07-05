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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using Ntreev.Library.Commands.Properties;

namespace Ntreev.Library.Commands
{
    class ParseDescriptor
    {
        private readonly Dictionary<string, string> unparsedArguments = new Dictionary<string, string>();

        private readonly Dictionary<CommandMemberDescriptor, ParseDescriptorItem> itemList = new Dictionary<CommandMemberDescriptor, ParseDescriptorItem>();

        /// <param name="type">
        /// 스위치를 직접 명시하지 않아도 되는 타입
        /// </param>
        /// <param name="members"></param>
        /// <param name="commandLine"></param>
        public ParseDescriptor(Type type, IEnumerable<CommandMemberDescriptor> members, string commandLine)
            : this(type, members, CommandStringUtility.SplitAll(commandLine))
        {


        }

        public ParseDescriptor(Type type, IEnumerable<CommandMemberDescriptor> members, IEnumerable<string> args)
        {
            foreach (var item in members)
            {
                this.itemList.Add(item, new ParseDescriptorItem(item));
            }

            var descriptors = new Dictionary<string, CommandMemberDescriptor>();
            foreach (var item in members)
            {
                if (item.IsExplicit == false)
                    continue;
                if (item.NamePattern != string.Empty)
                    descriptors.Add(item.NamePattern, item);
                if (item.ShortNamePattern != string.Empty)
                    descriptors.Add(item.ShortNamePattern, item);
            }

            var variableList = new List<string>();
            var variables = members.Where(item => item is CommandMemberArrayDescriptor).FirstOrDefault();
            var arguments = new Queue<string>(args);

            while (arguments.Any())
            {
                var arg = arguments.Dequeue();
                if (descriptors.ContainsKey(arg) == true)
                {
                    var descriptor = descriptors[arg];
                    var nextArg = arguments.FirstOrDefault() ?? string.Empty;
                    var isValue = nextArg != string.Empty && CommandStringUtility.IsSwitch(nextArg) == false && nextArg != "--";
                    if (isValue == true)
                    {
                        var textValue = arguments.Dequeue();
                        if (CommandStringUtility.IsWrappedOfQuote(textValue) == true)
                            textValue = Regex.Unescape(textValue);
                        this.itemList[descriptor].Desiredvalue = Parser.Parse(descriptor, textValue);
                    }
                    this.itemList[descriptor].MemberName = arg;
                }
                else if (arg == "--")
                {
                    if (variables != null)
                    {
                        foreach (var item in arguments)
                        {
                            variableList.Add(item);
                        }
                    }
                    else
                    {
                        foreach (var item in arguments)
                        {
                            this.unparsedArguments.Add(item, null);
                        }
                    }
                    arguments.Clear();
                }
                else if (CommandStringUtility.IsSwitch(arg) == true)
                {
                    this.unparsedArguments.Add(arg, null);
                }
                else if (CommandStringUtility.IsSwitch(arg) == false)
                {
                    var requiredDescriptor = this.itemList.Where(item => item.Key.IsRequired == true && item.Key.IsExplicit == false && item.Value.IsParsed == false)
                                                          .Select(item => item.Key).FirstOrDefault();
                    if (requiredDescriptor != null)
                    {
                        var parseInfo = this.itemList[requiredDescriptor];
                        var value = Parser.Parse(requiredDescriptor, arg);
                        parseInfo.Desiredvalue = value;
                    }
                    else if (variables != null)
                    {
                        variableList.Add(arg);
                    }
                    else
                    {
                        var nextArg = arguments.FirstOrDefault();
                        if (nextArg != null && CommandStringUtility.IsSwitch(nextArg) == false)
                            this.unparsedArguments.Add(arg, arguments.Dequeue());
                        else
                            this.unparsedArguments.Add(arg, null);
                    }
                }
                else
                {
                    this.unparsedArguments.Add(arg, null);
                }
            }

            if (variables != null)
            {
                this.itemList[variables].Desiredvalue = Parser.ParseArray(variables, variableList);
            }
        }

        public void SetValue(object instance)
        {
            this.ValidateSetValue(instance);

            foreach (var item in this.itemList)
            {
                var descriptor = item.Key;
                descriptor.ValidateTrigger(this.itemList);
            }

            foreach (var item in this.itemList)
            {
                var descriptor = item.Key;
                var parseInfo = item.Value;

                if (parseInfo.Desiredvalue != DBNull.Value)
                {
                    descriptor.SetValueInternal(instance, parseInfo.Desiredvalue);
                }
                else if (parseInfo.DefaultValue != DBNull.Value)
                {
                    descriptor.SetValueInternal(instance, parseInfo.DefaultValue);
                }
                else if (parseInfo.ExplicitValue != DBNull.Value)
                {
                    descriptor.SetValueInternal(instance, parseInfo.ExplicitValue);
                }
                else if (descriptor.MemberType.IsValueType == true)
                {
                    descriptor.SetValueInternal(instance, Activator.CreateInstance(descriptor.MemberType));
                }
                else
                {
                    descriptor.SetValueInternal(instance, null);
                }
            }
        }

        public IDictionary<CommandMemberDescriptor, ParseDescriptorItem> Descriptors => this.itemList;

        private void ValidateSetValue(object instance)
        {
            if (this.unparsedArguments.Any())
            {
                var items = new Dictionary<string, string>(this.unparsedArguments);
                if (items.Any() == true)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("처리되지 않은 인자가 포함되어 있습니다.");
                    foreach (var item in items)
                    {
                        if (item.Value != null)
                        {
                            sb.AppendLine($"    {item.Key} {item.Value}");
                        }
                        else
                        {
                            sb.AppendLine($"    {item.Key}");
                        }
                    }
                    throw new ArgumentException(sb.ToString());
                }
            }

            foreach (var item in this.itemList)
            {
                var descriptor = item.Key;
                var parseInfo = item.Value;
                if (descriptor.IsExplicit == true)
                {
                    if (parseInfo.MemberName != string.Empty &&  parseInfo.Desiredvalue == DBNull.Value && descriptor.ExplicitValue == DBNull.Value)
                        throw new ArgumentException($"{descriptor.DisplayName}에 값이 설정되지 않았습니다.");
                }
                if (parseInfo.IsParsed == true)
                    continue;
                if (descriptor.IsRequired == true)
                {
                    if (descriptor.IsExplicit == false && descriptor.DefaultValue != DBNull.Value)
                        continue;

                    throw new ArgumentException($"필수 인자 {descriptor.DisplayName}가 빠져있습니다");
                }
            }
        }
    }
}
