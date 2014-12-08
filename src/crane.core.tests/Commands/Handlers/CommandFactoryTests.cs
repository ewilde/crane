using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Commands;
using Crane.Core.Commands.Handlers;
using Crane.Core.Commands.Resolvers;
using FakeItEasy;
using FluentAssertions;
using PowerArgs;
using Xbehave;

namespace Crane.Core.Tests.Commands.Handlers
{
    public class CommandFactoryTests
    {
        public class DummyCommand : ICraneCommand
        {
            [ArgPosition(0)]
            public string ProjectName { get; set; }

            public string Name { get { return "Dummy";  } }
        }

        [Scenario]
        public void Can_fill_arguments_when_passed_by_position(CommandFactory commandFactory, ICraneCommand craneCommand)
        {
            "Given I have a command factory that is going to build a DummyCommand"
                ._(() =>
                {
                    var fakeCommandResolver = A.Fake<ICommandResolver>();
                    A.CallTo(() => fakeCommandResolver.Resolve(A<IEnumerable<ICraneCommand>>.Ignored, A<string>.Ignored))
                        .Returns(typeof (DummyCommand));
                    commandFactory = new CommandFactory(fakeCommandResolver, A.Fake<IEnumerable<ICraneCommand>>());
                });

            "When I create a command with arguments passed by position"
                ._(() => craneCommand = commandFactory.Create(new[] {"dummy", "testproject"}));

            "Then a command should be created with the ProjectName set to testproject"
                ._(() => ((DummyCommand) craneCommand).ProjectName.Should().Be("testproject"));
        }
    }
}
