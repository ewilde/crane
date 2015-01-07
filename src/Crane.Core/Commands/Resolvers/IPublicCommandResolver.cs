using System.Collections.Generic;

namespace Crane.Core.Commands.Resolvers
{
    public interface IPublicCommandResolver
    {
        IEnumerable<ICraneCommand> Resolve();
    }
}