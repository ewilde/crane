using Crane.Core.Documentation;
using Crane.Core.Documentation.Formatters;
using Crane.Core.Documentation.Providers;
using Crane.Core.IO;

namespace Crane.Core.Commands.Handlers
{
    public class HelpCommandHandler : CommandHandler<Help>
    {
        private readonly IOutput _output;
        private readonly IHelpProvider _helpProvider;
        private readonly IHelpFormatter _helpFormatter;

        public HelpCommandHandler(IOutput output, IHelpProvider helpProvider, IHelpFormatter helpFormatter)
        {
            _output = output;
            _helpProvider = helpProvider;
            _helpFormatter = helpFormatter;
        }

        protected override void DoHandle(Help command)
        {
            var commandHelp = _helpProvider.HelpCollection.Get(command.Command);
            _output.WriteInfo(_helpFormatter.Format(commandHelp));
        }
    }
}