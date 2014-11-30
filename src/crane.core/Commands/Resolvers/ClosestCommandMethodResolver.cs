using System.Linq;
using System.Reflection;

namespace Crane.Core.Commands.Resolvers
{
    public class ClosestCommandMethodResolver : IClosestCommandMethodResolver
    {
        public MethodInfo Resolve(ICraneCommand command, string[] arguments)
        {
            var craneCommandInterfaceMethodNames =
                typeof(ICraneCommand).GetMethods().Select(m => m.Name.ToLowerInvariant()).ToList();

            var method = command.GetType()
                                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                .Where(m => !craneCommandInterfaceMethodNames.Contains(m.Name.ToLowerInvariant())
                                                    && (m.Attributes & MethodAttributes.SpecialName) != MethodAttributes.SpecialName)
                                .OrderBy(m => m.GetParameters().Length)
                                .First();

            return method;
        }
    }
}