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

        /// <summary>
        /// Gets a command help using the command's short name
        /// </summary>
        /// <param name="command">Command short name</param>
        /// <returns>Command help for the corresponding <paramref name="command"/> short name</returns>
        public ICommandHelp Get(string command)
        {

            if (_content.ContainsKey(command))
            {
                return _content[command];
            }

            return null;
        }

        public int Count
        {
            get { return _content.Count; }
        }
    }
}