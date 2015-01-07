using System.Collections.Generic;

namespace Crane.Core.Commands.Resolvers
{
    public interface IVisibleCommandResolver
    {
        IEnumerable<ICraneCommand> Resolve();
    }
}