using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crane.Core.Commands.Attributes;
using Crane.Core.Commands.Resolvers;
using Crane.Core.IO;

namespace Crane.Core.Commands.Handlers
{
    public class ListCommandsHandler : CommandHandler<ListCommands>
    {
        private readonly IEnumerable<ICraneCommand> _commands;
        private readonly IOutput _output;

        public ListCommandsHandler(IVisibleCommandResolver commandResolver, IOutput output)
        {
            _commands = commandResolver.Resolve();
            _output = output;
        }


        protected override void DoHandle(ListCommands listCommands)
        {
            _output.WriteInfo("list of possible crane commands:");
            foreach (var command in _commands.Select(c => c.GetType().Name.ToLowerInvariant())
                                             .OrderBy(n => n))
            {
                _output.WriteInfo("crane {0} ", command);
            }
        }
    }
}