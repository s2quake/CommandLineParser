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

using System;
using System.ComponentModel;

namespace JSSoft.Library.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UsageDescriptionProviderAttribute : Attribute
    {
        public UsageDescriptionProviderAttribute(string typeName)
            : this(Type.GetType(typeName))
        {
        }

        public UsageDescriptionProviderAttribute(Type type)
        {
            if (typeof(IUsageDescriptionProvider).IsAssignableFrom(type) == false)
                throw new ArgumentException();
            this.ProviderType = type;
        }

        public Type ProviderType { get; private set; }

        protected virtual IUsageDescriptionProvider CreateInstance(Type type)
        {
            return TypeDescriptor.CreateInstance(null, this.ProviderType, null, null) as IUsageDescriptionProvider;
        }

        internal IUsageDescriptionProvider CreateInstanceInternal(Type type)
        {
            return this.CreateInstance(type);
        }
    }
}
