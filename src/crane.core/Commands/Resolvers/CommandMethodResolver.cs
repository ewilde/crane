using System.Linq;
using System.Reflection;

namespace Crane.Core.Commands.Resolvers
{
    public class CommandMethodResolver : ICommandMethodResolver
    {
        public MethodInfo Resolve(ICraneCommand command, string[] arguments)
        {

            var craneCommandInterfaceMethodNames =
                typeof (ICraneCommand).GetMethods().Select(m => m.Name.ToLowerInvariant()).ToList();

            var method = command.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).FirstOrDefault(m => !craneCommandInterfaceMethodNames.Contains(m.Name.ToLowerInvariant())
                && m.GetParameters().Length == arguments.Length && (m.Attributes & MethodAttributes.SpecialName) != MethodAttributes.SpecialName);

            return method;
        }
    }
}