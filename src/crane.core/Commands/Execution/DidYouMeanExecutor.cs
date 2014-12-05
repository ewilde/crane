using System;
using System.Linq;
using System.Text;
using Crane.Core.Commands.Resolvers;
using Crane.Core.IO;

namespace Crane.Core.Commands.Execution
{
    public class DidYouMeanExecutor : IDidYouMeanExecutor
    {
        private readonly IClosestCommandMethodResolver _closestCommandMethodResolver;
        private readonly IOutput _output;

        public DidYouMeanExecutor(IClosestCommandMethodResolver closestCommandMethodResolver, IOutput output)
        {
            _closestCommandMethodResolver = closestCommandMethodResolver;
            _output = output;
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

            _output.WriteInfo(stringBuilder.ToString());
        }
    }
}