using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.IO;

namespace Crane.Core.Commands.Handlers
{
    public class ListCommandsHandler : CommandHandler<ListCommands>
    {
        private readonly IEnumerable<ICraneCommand> _commands;
        private readonly IOutput _output;

        public ListCommandsHandler(IEnumerable<ICraneCommand> commands, IOutput output)
        {
            _commands = commands;
            _output = output;
        }


        protected override void DoHandle(ListCommands listCommands)
        {
            _output.WriteInfo("list of possible crane commands:");
            foreach (var command in _commands.Where(c => c.GetType() != typeof(UnknownCommand))
                                             .Select(c => c.GetType().Name.ToLowerInvariant())
                                             .OrderBy(n => n))
            {
                _output.WriteInfo("crane {0} ", command);
            }
        }
    }
}