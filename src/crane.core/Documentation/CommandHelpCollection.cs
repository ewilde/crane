using System.Collections.Generic;
using Crane.Core.Commands;

namespace Crane.Core.Documentation
{
    public class CommandHelpCollection : ICommandHelpCollection
    {
        private readonly IDictionary<string, ICommandHelp> _content;

        public CommandHelpCollection(IDictionary<string, ICommandHelp> content)
        {
            _content = content;
        }

        public ICommandHelp Get<T>() where T : ICraneCommand
        {
            var name = typeof(T).FullName;

            if (_content.ContainsKey(name))
            {
                return _content[name];
            }

            return null;
        }

        public int Count
        {
            get { return _content.Count; }
        }
    }
}