//Released under the MIT License.
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

using JSSoft.Library.Commands.Properties;
using System.Collections.Generic;
using System.Linq;

namespace JSSoft.Library.Commands
{
    public class CommandMethodDescriptorCollection : IEnumerable<CommandMethodDescriptor>
    {
        private readonly List<CommandMethodDescriptor> descriptors = new List<CommandMethodDescriptor>();

        internal CommandMethodDescriptorCollection()
        {

        }

        public bool Contains(string name)
        {
            var query = from item in descriptors
                        where item.Name == name
                        select item;
            return query.Any();
        }

        public CommandMethodDescriptor this[string name]
        {
            get
            {
                var query = from item in descriptors
                            where item.Name == name
                            select item;
                if (query.Any() == false)
                    throw new KeyNotFoundException(string.Format(Resources.Exception_MethodDoesNotExist_Format, name));
                return query.First();
            }
        }

        public CommandMethodDescriptor this[int index] => this.descriptors[index];

        public int Count => this.descriptors.Count;

        internal void Add(CommandMethodDescriptor item)
        {
            this.descriptors.Add(item);
        }

        internal void AddRange(IEnumerable<CommandMethodDescriptor> descriptors)
        {
            foreach (var item in descriptors)
            {
                this.Add(item);
            }
        }

        #region IEnumerable

        IEnumerator<CommandMethodDescriptor> IEnumerable<CommandMethodDescriptor>.GetEnumerator()
        {
            return this.descriptors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.descriptors.GetEnumerator();
        }

        #endregion
    }
}
