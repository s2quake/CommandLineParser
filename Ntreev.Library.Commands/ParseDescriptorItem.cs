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
    public class ParseDescriptorItem
    {
        private object value = DBNull.Value;
        
        public ParseDescriptorItem(CommandMemberDescriptor descriptor)
        {
            this.Descriptor = descriptor;

        }

        public CommandMemberDescriptor Descriptor { get; }

        public bool IsParsed => this.value != DBNull.Value;

        public object Value
        {
            get
            {
                if (this.value == DBNull.Value)
                {
                    if (this.Descriptor.IsExplicit == true && this.HasSwtich == true && this.Descriptor.DefaultValue != DBNull.Value)
                        return this.Descriptor.DefaultValue;
                    if (this.Descriptor.IsExplicit == false && this.Descriptor.DefaultValue != DBNull.Value)
                        return this.Descriptor.DefaultValue;
                    if (this.Descriptor.InitValue != DBNull.Value)
                        return this.Descriptor.InitValue;
                }
                return this.value;
            }
            set => this.value = value;
        }

        public object ActualValue
        {
            get
            {
                if (this.Value == DBNull.Value)
                {
                    if (this.Descriptor.MemberType.IsValueType == true)
                    {
                        return Activator.CreateInstance(this.Descriptor.MemberType);
                    }
                    else
                    {
                        return null;
                    }
                }
                return this.Value;
            }
        }

        public object InitValue
        {
            get
            {
                if (this.Descriptor.InitValue != DBNull.Value)
                    return this.Descriptor.InitValue;
                if (this.Descriptor.MemberType.IsValueType == true)
                    return Activator.CreateInstance(this.Descriptor.MemberType);
                return null;
            }
        }

        public bool HasSwtich { get; internal set; }
    }
}