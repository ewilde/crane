using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Mappers;
using Crane.Core.Api.Model;
using Crane.Core.Api.Model.Mappers;
using Crane.Core.Configuration;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Model.Mappers
{
    public class FubuSolutionMapperFeatures
    {
        [Scenario]
        public void map_fubu_solution_to_crane_solution(IFubuSolutionMapper solutionMapper, SolutionBuilderContext context, ISolutionContext solutionContext, Solution result)
        {
            "Given I have a solution mapper"
                ._(() => solutionMapper = ioc.Resolve<IFubuSolutionMapper>());

            "And I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithProject(item => item.Name = "FrodoFx.UnitTests")
                    .Build());

            "When I map the fubu solution"
                ._(() => result = solutionMapper.Map(FubuCsProjFile.Solution.LoadFrom(solutionContext.Solution.Path)));
            
            "It should return a solution"
                ._(() => result.Should().NotBeNull());

            "It should map the solutions name"
                ._(() => result.Name.Should().Be("Sally"));

            "It should map the solutions path"
                ._(() => result.Path.Should().Be(Path.Combine(context.RootDirectory, "Sally.sln")));

            "It should map the solutions projects"
                ._(() => result.Projects.Count().Should().Be(2));

            "It should map the solutions projects that reference their parent solution"
                ._(() => result.Projects.All(item => item.Solution.Equals(result)).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }
    }
}