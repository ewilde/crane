using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Builders;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api.Builders
{
    public class SolutionBuilderTests
    {
        [Scenario]
        public void build_project_with_name(SolutionBuilderContext context, Project result)
        {
            "Given I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "When I call build"
                ._(() => result = context.SolutionBuilder.WithProject(project => project.Name = "FrodoFx").Build().Projects.FirstOrDefault());

            "It should return the build project"
                ._(() => result.Should().NotBeNull());

            "And it should set the name correctly"
                ._(() => result.Name.Should().Be("FrodoFx"))
                .Teardown(() => context.TearDown());
        }
    }
}