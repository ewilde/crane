using System.Reflection;

namespace Crane.Core.Commands.Resolvers
{
    public interface ICommandMethodResolver
    {
        MethodInfo Resolve(ICraneCommand command, string[] arguments);
    }
}