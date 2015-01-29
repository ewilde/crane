using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Mappers;
using Crane.Core.Api.Model;
using Crane.Core.Configuration;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Model.Mappers
{
    public class FubuProjectMapperFeatures
    {
        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void map_fubu_project_to_crane_project(IFubuProjectMapper projectMapper, SolutionBuilderContext context, ISolutionContext solutionContext, Project result)
        {
            "Given I have a project mapper"
                ._(() => projectMapper = ServiceLocator.Resolve<IFubuProjectMapper>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with a project"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item => item.Title = "FrodoFx")
                    .Build());

            "When I map the fubu project using the mapper"
                ._(() => result = projectMapper.Map(FubuCsProjFile.CsProjFile.LoadFrom(solutionContext.Solution.Projects.First().Path)));
            
            "It should return a project"
                ._(() => result.Should().NotBeNull());

            "It should map the projects name"
                ._(() => result.Name.Should().Be("FrodoFx"));

            "It should map the projects assemblyinfo"
                ._(() => result.AssemblyInfo.Title.Should().Be("FrodoFx", string.Format("File contents of {0} is {1}.", result.AssemblyInfo.Path, File.ReadAllText(result.AssemblyInfo.Path))));

            "It should map the projects path"
                ._(() => result.Path.Should().Be(Path.Combine(context.RootDirectory, "FrodoFx", "FrodoFx.csproj")))            
                .Teardown(() => context.TearDown());
        }
    }
}