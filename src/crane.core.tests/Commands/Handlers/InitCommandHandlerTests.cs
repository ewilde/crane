using Crane.Core.Commands;
using Crane.Core.Commands.Handlers;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Handlers
{
    public class InitCommandHandlerTests
    {
        [Scenario]
        public void calling_execute_to_create_a_blank_project(ICraneContext craneContext, 
            Init initCommand,
            MockContainer<InitCommandHandler> initCommandHandler)
        {
            var buildTemplate = A.Fake<ITemplate>();

            "Given I have an instance of the initCommandHandler"
                ._(() => initCommandHandler = B.AutoMock<InitCommandHandler>());

            "And I have an init command resolved with the project name FooStack"
                ._(() => initCommand = new Init {ProjectName = "FooStack"});

            "And I am in the working directory dev"
                ._(() => A.CallTo(() => initCommandHandler.GetMock<IFileManager>().CurrentDirectory).Returns(@"c:\dev"));

            "And the template resolver is configured to return at least a build template"
                ._(() => A.CallTo(() => initCommandHandler.GetMock<ITemplateResolver>().Resolve(TemplateType.Build)).Returns(buildTemplate));

            "When I handle the init command"
                ._(() => initCommandHandler.Subject.Handle(initCommand));

            "It sets the project directory to the current directory"
                ._(() => initCommandHandler.GetMock<ICraneContext>().ProjectRootDirectory.FullName.Should().Be(@"c:\dev\FooStack"));

            "It creates the project directory"
                ._(() => A.CallTo(() => initCommandHandler.GetMock<IFileManager>().CreateDirectory(@"c:\dev\FooStack")).MustHaveHappened());

            "It creates a new build script for that project"
                ._(() => A.CallTo(() => buildTemplate.Create()).MustHaveHappened());
        }

        [Scenario]
        public void init_command_handler_can_handle_init_command(MockContainer<InitCommandHandler> initCommandHandler, bool result)
        {
            "Given I have an instance of the initCommandHandler"
                ._(() => initCommandHandler = B.AutoMock<InitCommandHandler>());

            "When I see if it can handle an init command"
                ._(() => result = initCommandHandler.Subject.CanHandle(new Init()));

            "Then the result should be true"
                ._(() => result.Should().BeTrue());
        }

    }
}
