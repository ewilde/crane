using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Mappers;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Model.Mappers
{
    public class FubuProjectMapperFeatures
    {
        [Scenario]
        public void map_fubu_project_to_crane_project(IFubuProjectMapper projectMapper, SolutionBuilderContext context, ISolutionContext solutionContext, Project result)
        {
            "Given I have a project mapper"
                ._(() => projectMapper = ioc.Resolve<IFubuProjectMapper>());

            "And I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "And I have a solution with a project"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx").Build());

            "When I map the fubu project using the mapper"
                ._(() => result = projectMapper.Map(FubuCsProjFile.CsProjFile.LoadFrom(solutionContext.Solution.Projects.First().Path)));
            
            "It should return a project"
                ._(() => result.Should().NotBeNull());

            "It should map the projects name"
                ._(() => result.Name.Should().Be("FrodoFx"));

            "It should map the projects path"
                ._(() => result.Path.Should().Be(Path.Combine(context.RootDirectory, "FrodoFx", "FrodoFx.csproj")))            
                .Teardown(() => context.TearDown());
        }
    }
}