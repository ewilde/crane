using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crane.Core.Commands.Attributes;

namespace Crane.Core.Commands.Resolvers
{
    public class VisibleCommandResolver : IVisibleCommandResolver
    {
        private readonly IEnumerable<ICraneCommand> _commands;

        public VisibleCommandResolver(IEnumerable<ICraneCommand> commands)
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