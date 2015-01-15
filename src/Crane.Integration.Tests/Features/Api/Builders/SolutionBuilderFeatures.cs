using System.IO;
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
        public void build_project_with_name(SolutionBuilderContext context, ISolutionContext result, Project project)
        {
            "Given I have a solution builder context"
                ._(() =>
                {
                    context = ioc.Resolve<SolutionBuilderContext>();
                    context.CreateBuilder("Sally.sln");
                });

            "When I call build"
                ._(() => result = context.SolutionBuilder.WithProject(item => item.Name = "FrodoFx").Build());

            "It should return the build project"
                ._(() =>
                {
                    project = result.Solution.Projects.FirstOrDefault();
                    project.Should().NotBeNull();
                });

            "And the project file should have the name set correctly"
                ._(() => project.Name.Should().Be("FrodoFx"));

            "And the project file should exist on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "FrodoFx", "FrodoFx.csproj")).Should().BeTrue());
                
            "And it should create a solution file on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "Sally.sln")).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }
    }
}