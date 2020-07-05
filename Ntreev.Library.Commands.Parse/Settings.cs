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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Commands.Parse
{
    // [CommandStaticProperty(typeof(GlobalSettings))]
    class Settings
    {
        public Settings()
        {
            // this.Libraries = new string[] { };
        }

        [CommandProperty(Usage = CommandPropertyUsage.Required)]
        public string Path
        {
            get; set;
        }

        [CommandProperty('b')]
        public bool IsBoolean
        {
            get; set;
        }

        // [CommandProperty(Usage = CommandPropertyUsage.Required)]
        // [Description("service name")]
        // public string ServiceName
        // {
        //     get; set;
        // }

        // [CommandProperty("path", Usage = CommandPropertyUsage.Required, IsExplicit = true)]
        // [Description("path to work")]
        // [DefaultValue("")]
        // public string WorkingPath
        // {
        //     get; set;
        // }

        // [CommandProperty(Usage = CommandPropertyUsage.Required)]
        // [DefaultValue("10001")]
        // [Description("port")]
        // [Browsable(true)]
        // public int Port
        // {
        //     get; set;
        // }

        // [CommandProperty('c')]
        // [Description("use cache")]
        // public bool UseCache
        // {
        //     get; set;
        // }

        // [CommandProperty("cache-size", IsExplicit = true)]
        // [Description("cache size. default is 1024")]
        // [DefaultValue("1024")]
        // public int CacheSize
        // {
        //     get; set;
        // }

        // [CommandPropertyArray]
        // [Description("library paths.")]
        // public string[] Libraries
        // {
        //     get; set;
        // }
    }
}
