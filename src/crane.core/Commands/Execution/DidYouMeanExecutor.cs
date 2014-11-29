using System;
using System.Linq;
using System.Text;
using Crane.Core.Commands.Resolvers;

namespace Crane.Core.Commands.Execution
{
    public class DidYouMeanExecutor : IDidYouMeanExecutor
    {
        private readonly IClosestCommandMethodResolver _closestCommandMethodResolver;
        private readonly Action<string> _writeLine; 

        public DidYouMeanExecutor(IClosestCommandMethodResolver closestCommandMethodResolver, Action<string> writeLine)
        {
            _closestCommandMethodResolver = closestCommandMethodResolver;
            _writeLine = writeLine;
        }

        public void PrintHelp(ICraneCommand command, string[] arguments)
        {
            var method = _closestCommandMethodResolver.Resolve(command, arguments);


            var methodParamets = method.GetParameters();

            var stringBuilder = new StringBuilder("did you mean 'crane ");
            stringBuilder.Append(command.GetType().Name.ToLowerInvariant());

            if (methodParamets.Length > 0)
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(string.Join(" ", methodParamets.Select(m => m.Name.ToLowerInvariant())));
            }

            stringBuilder.Append("'?");

            _writeLine(stringBuilder.ToString());
        }
    }
}