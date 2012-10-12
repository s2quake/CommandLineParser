﻿#region License
//Ntreev CommandLineParser for .Net 1.0.4548.25168
//https://github.com/NtreevSoft/CommandLineParser

//Released under the MIT License.

//Copyright (c) 2010 Ntreev Soft co., Ltd.

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ntreev.Library;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace SampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();

            CommandLineParser parser = new CommandLineParser();
            CommandLineInvoker invoker = new CommandLineInvoker();
            try
            {
                invoker.Invoke(Environment.CommandLine, options);
                //invoker.Invoke("wow database /Path e /ip e /port e /number 2", options);
                invoker.PrintUsage();
                Environment.Exit(0);
            }
            catch (SwitchException e)
            {
                Environment.Exit(1);
            }
            catch (ArgumentException)
            {
                parser.PrintUsage();
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                invoker.PrintUsage();
                Environment.Exit(1);
            }
        }
      
        class Options
        {
            private int number;

            [CommandSwitch("n")]
            [Description("백업 여부를 설정합니다.")]
            public bool NoBackup { get; set; }

            public string Text { get; set; }

            public int Number 
            {
                get { return this.number; }
                set { this.number = value; }
            }

            public int Number2 { get; set; }

            public TypeCode TypeCode { get; set; }

            [CommandSwitch("a", ArgSeperator='\0')]
            public AttributeTargets AttributeTargets { get; set; }

            [TypeConverter(typeof(FileInfoTypeConverter))]
            [Description("파일 경로를 나타냅니다.")]
            [DisplayName("File Path")]
            public FileInfo Path { get; set; }

            [Description("list of paths")]
            public List<int> PathList { get; set; }

            public string[] TextArray { get; set; }

            public Options()
            {
                
            }

            [CommandMethod]
            public void Show()
            {
                int qwer = 0;
            }

            [CommandMethod]
            public void TestMethod(int wow)
            {
                int qwer = 0;
            }

            [CommandMethod]
            public void TestMethod1(int wow, [DefaultValue(false)]bool test)
            {
                int qwer = 0;
            }

            [CommandMethod("database")]
            [CommandMethodSwitch("Path", "Number")]
            [Description("데이터 베이스에 직접 연결합니다.")]
            public void SetDataBase([Description("주소입니다.")]string ip, [DefaultValue("4004")]string port)
            {
                int qwer = 0;
            }

            class PathListTypeConverter : TypeConverter
            {
                public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                {
                    if (sourceType == typeof(string))
                        return true;
                    return base.CanConvertFrom(context, sourceType);
                }

                public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }

            class FileInfoTypeConverter : TypeConverter
            {
                public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
                {
                    if (sourceType == typeof(string))
                        return true;
                    return base.CanConvertFrom(context, sourceType);
                }

                public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
                {
                    FileInfo fileInfo = new FileInfo((string)value);
                    if (fileInfo.Exists == false)
                        return null;
                    return fileInfo;
                }
            }
        }

        static class wow
        {
            public static int Data { get; set; }

            [CommandMethod]
            public static int GetData(int n, float f)
            {
                throw new Exception();
                return wow.Data;
            }
        }
    }
}