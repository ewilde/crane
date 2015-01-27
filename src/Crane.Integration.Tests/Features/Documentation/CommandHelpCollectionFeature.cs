using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Configuration;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Providers;
using Crane.Core.Extensions;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Documentation
{
    public class CommandHelpCollectionFeature
    {
        [Scenario]
        public void all_user_commands_should_have_help_documentation(ICommandHelpCollection collection, IEnumerable<ICraneCommand> userCommands)
        {
            "Given I have a command help collection"
                ._(() => collection = ioc.Resolve<IHelpProvider>().HelpCollection);

            "And I have all the user commands"
                ._(() => userCommands = ioc.Resolve<IPublicCommandResolver>().Resolve());

            "Then there should be a command help instance for each command"
                ._(
                    () =>
                        userCommands.ForEach(
                            command =>
                                collection.Get(command.Name())
                                    .Should()
                                    .NotBeNull("missing command help for command {0}", command.Name())));
        }
    }
}