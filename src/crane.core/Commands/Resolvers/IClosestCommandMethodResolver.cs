using System.Reflection;

namespace Crane.Core.Commands.Resolvers
{
    public interface IClosestCommandMethodResolver
    {
        MethodInfo Resolve(ICraneCommand command, string[] arguments);
    }
}
