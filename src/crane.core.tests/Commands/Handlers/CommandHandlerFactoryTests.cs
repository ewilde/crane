using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Commands;
using Crane.Core.Commands.Handlers;
using Crane.Core.Commands.Handlers.Factories;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Handlers
{
    public class CommandHandlerFactoryTests
    {
        [Scenario]
        public void Gives_handler_that_can_handle_command(CommandHandlerFactory commandHandlerFactory,
            ICommandHandler commandHandler)
        {
            "Given I have a 2 command handlers one of which can handle a dummy command"
                ._(
                    () =>
                        commandHandlerFactory =
                            new CommandHandlerFactory(new ICommandHandler[]
                            {
                                new OtherCommandHandler(),
                                new DummyCommandHandler()
                            }));

            "When I create a command handler for a dummy command"
                ._(() => commandHandler = commandHandlerFactory.Create(new DummyCommand()));

            "Then the dummy command handler is returned"
                ._(() => commandHandler.Should().BeOfType<DummyCommandHandler>());
        }

        public class DummyCommand : ICraneCommand
        {
            public string Name { get { return "Dummy"; }}
        }

        public class DummyCommandHandler : ICommandHandler
        {
            public bool CanHandle(ICraneCommand command)
            {
                return true;
            }

            public void Handle(ICraneCommand craneCommand)
            {
                throw new NotImplementedException();
            }
        }

        public class OtherCommandHandler : ICommandHandler
        {
            public bool CanHandle(ICraneCommand command)
            {
                return false;
            }

            public void Handle(ICraneCommand craneCommand)
            {
                throw new NotImplementedException();
            }
        }
    }

   
}
