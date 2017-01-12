﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Ntreev.Library.Commands.Properties;

namespace Ntreev.Library.Commands
{
    public class CommandMethodUsagePrinter
    {
        private readonly string name;
        private readonly object instance;
        private readonly string summary;
        private readonly string description;

        public CommandMethodUsagePrinter(string name, object instance)
        {
            var provider = CommandDescriptor.GetUsageDescriptionProvider(instance.GetType());
            this.name = name;
            this.instance = instance;
            this.summary = provider.GetSummary(instance);
            this.description = provider.GetDescription(instance);
        }

        public virtual void Print(TextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            using (var tw = new CommandTextWriter(writer))
            {
                this.Print(tw, descriptors);
            }
        }

        public virtual void Print(TextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            using (var tw = new CommandTextWriter(writer))
            {
                this.Print(tw, descriptor, switches);
            }
        }

        public virtual void Print(TextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor switchDescriptor)
        {
            using (var tw = new CommandTextWriter(writer))
            {
                this.Print(tw, descriptor, switchDescriptor);
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public object Instance
        {
            get { return this.instance; }
        }

        public string Summary
        {
            get { return this.summary; }
        }

        public string Description
        {
            get { return this.description; }
        }

        private void Print(CommandTextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            this.PrintSummary(writer, descriptors);
            this.PrintUsage(writer, descriptors);
            this.PrintDescription(writer, descriptors);
            this.PrintSubcommands(writer, descriptors);
        }

        private void Print(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            this.PrintSummary(writer, descriptor, switches);
            this.PrintUsage(writer, descriptor, switches);
            this.PrintDescription(writer, descriptor, switches);
            this.PrintRequirements(writer, descriptor, switches);
            this.PrintOptions(writer, descriptor, switches);
        }

        private void Print(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor switchDescriptor)
        {
            this.PrintSummary(writer, descriptor, switchDescriptor);
            this.PrintUsage(writer, descriptor, switchDescriptor);
            this.PrintDescription(writer, descriptor, switchDescriptor);
        }

        private void PrintSummary(CommandTextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            if (this.Summary == string.Empty)
                return;

            writer.WriteLine(Resources.Summary);
            writer.Indent++;
            writer.WriteLine(this.Summary);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintSummary(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            if (this.Description == string.Empty)
                return;

            writer.WriteLine(Resources.Summary);
            writer.Indent++;
            writer.WriteLine(descriptor.Summary);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintSummary(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor switchDescriptor)
        {
            writer.WriteLine(Resources.Summary);
            writer.Indent++;
            writer.WriteLine(switchDescriptor.Summary);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintDescription(CommandTextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            if (this.Description == string.Empty)
                return;

            writer.WriteLine(Resources.Description);
            writer.Indent++;
            writer.WriteMultiline(this.Description);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintDescription(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            if (descriptor.Description == string.Empty)
                return;

            writer.WriteLine(Resources.Description);
            writer.Indent++;
            writer.WriteMultiline(descriptor.Description);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintDescription(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor switchDescriptor)
        {
            writer.WriteLine(Resources.Description);
            writer.Indent++;
            writer.WriteMultiline(switchDescriptor.Description);
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintSubcommands(CommandTextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            writer.WriteLine(Resources.SubCommands);
            writer.Indent++;

            foreach (var item in descriptors)
            {
                writer.WriteLine(item.Name);
                writer.Indent++;
                if (item.Summary == string.Empty)
                    writer.WriteMultiline("*<empty>*");
                else
                    writer.WriteMultiline(item.Summary);
                writer.Indent--;
            }

            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintUsage(CommandTextWriter writer, CommandMethodDescriptor[] descriptors)
        {
            writer.WriteLine(Resources.Usage);
            writer.Indent++;

            writer.WriteLine("{0} <sub-command> [options...]", this.Name);

            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintUsage(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            writer.WriteLine(Resources.Usage);
            writer.Indent++;

            this.PrintMethodUsage(writer, descriptor, switches);

            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintUsage(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor switchDescriptor)
        {
            writer.WriteLine(Resources.Usage);
            writer.Indent++;

            this.PrintOption(writer, switchDescriptor);

            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintMethodUsage(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            var indent = writer.Indent;
            var query = from item in switches
                        orderby item.Required descending
                        select this.GetString(item);

            var maxWidth = writer.Width - (writer.TabString.Length * writer.Indent);

            var line = descriptor.Name;

            foreach (var item in query)
            {
                if (line != string.Empty)
                    line += " ";

                if (line.Length + item.Length >= maxWidth)
                {
                    writer.WriteLine(line);
                    line = string.Empty.PadLeft(descriptor.Name.Length + 1);
                }
                line += item;
            }

            writer.WriteLine(line);
            writer.Indent = indent;
        }

        private void PrintRequirements(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            var items = switches.Where(i => i.Required == true).ToArray();
            if (items.Any() == false)
                return;

            writer.WriteLine(Resources.Requirements);
            writer.Indent++;
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                this.PrintRequirement(writer, item);
                if (i + 1 < items.Length)
                    writer.WriteLine();
            }
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintOptions(CommandTextWriter writer, CommandMethodDescriptor descriptor, CommandMemberDescriptor[] switches)
        {
            var items = switches.Where(i => i.Required == false).ToArray();
            if (items.Any() == false)
                return;

            writer.WriteLine(Resources.Options);
            writer.Indent++;

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                this.PrintOption(writer, item);
                if (i + 1 < items.Length)
                    writer.WriteLine();
            }
            writer.Indent--;
            writer.WriteLine();
        }

        private void PrintRequirement(CommandTextWriter textWriter, CommandMemberDescriptor descriptor)
        {
            if (descriptor is CommandParameterDescriptor == true)
            {
                textWriter.WriteLine(descriptor.DisplayName);
            }
            else
            {
                if (descriptor.ShortNamePattern != string.Empty)
                    textWriter.WriteLine(descriptor.ShortNamePattern);
                if (descriptor.NamePattern != string.Empty)
                    textWriter.WriteLine(descriptor.NamePattern);
            }

            if (descriptor.Description != string.Empty)
            {
                textWriter.Indent++;
                textWriter.WriteMultiline(descriptor.Description);
                textWriter.Indent--;
            }
        }

        private void PrintOption(CommandTextWriter writer, CommandMemberDescriptor descriptor)
        {
            if (descriptor.ShortNamePattern != string.Empty)
                writer.WriteLine(descriptor.ShortNamePattern);
            if (descriptor.NamePattern != string.Empty)
                writer.WriteLine(descriptor.NamePattern);

            writer.Indent++;
            writer.WriteMultiline(descriptor.Description);
            writer.Indent--;
        }

        private string GetString(CommandMemberDescriptor descriptor)
        {
            if (descriptor.Required == true)
            {
                var text = string.Empty;
                if (descriptor is CommandParameterDescriptor == true)
                {
                    text = descriptor.DisplayName;
                }
                else
                {
                    var patternItems = new string[] { descriptor.ShortNamePattern, descriptor.NamePattern, };
                    text = string.Join(" | ", patternItems.Where(i => i != string.Empty));
                }
                if (descriptor.DefaultValue == DBNull.Value)
                    return string.Format("<{0}>", text);
                return string.Format("<{0}={1}>", text, descriptor.DefaultValue ?? "null");
            }
            else
            {
                var patternItems = new string[] { descriptor.ShortNamePattern, descriptor.NamePattern, };
                var patternText = string.Join(" | ", patternItems.Where(i => i != string.Empty));
                return string.Format("[{0}]", patternText);
            }
        }
    }
}