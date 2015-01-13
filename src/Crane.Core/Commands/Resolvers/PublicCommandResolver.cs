using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crane.Core.Commands.Attributes;

namespace Crane.Core.Commands.Resolvers
{
    public class PublicCommandResolver : IPublicCommandResolver
    {
        private readonly IEnumerable<ICraneCommand> _commands;

        public PublicCommandResolver(IEnumerable<ICraneCommand> commands)
        {
            _commands = commands;
        }

        public IEnumerable<ICraneCommand> Resolve()
        {
            return _commands.Where(NotHidden);
        }

        private bool NotHidden(ICraneCommand arg)
        {
            return arg.GetType().GetCustomAttribute<HiddenCommandAttribute>() == null;
        }
    }
}