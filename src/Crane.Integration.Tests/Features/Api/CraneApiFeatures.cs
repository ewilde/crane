using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class CraneApiFeatures
    {
        [Scenario]
        public void Get_context_with_root_folder_path_returns_all_projects(ICraneApi craneApi, SolutionBuilderContext context, ISolutionContext result, Project project)
        {
            "Given I have a crane api"
                ._(() => craneApi = ioc.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => result = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithProject(item => item.Name = "FrodoFx.UnitTests").Build());

            "When I get the solution context via the api"
                ._(() =>
                {
                    result = craneApi.GetSolutionContext(context.RootDirectory);
                });

            "And it should return a model representation of the solution on disk"
                ._(() => result.Solution.Should().NotBeNull());

            "And it should have the correct solution name"
                ._(() => result.Solution.Name.Should().Be("Sally"));

            "And it should have the correct solution path"
                ._(() => result.Solution.Path.Should().Be(Path.Combine(context.RootDirectory, "Sally.sln")));

            "And it should reference its parent context"
                ._(() => result.Solution.SolutionContext.Should().NotBeNull());

            "And it should have two projects"
                ._(() => result.Solution.Projects.Count().Should().Be(2));
                
            "And the projects should have the correct name"
                ._(() =>
                {
                    result.Solution.Projects.Any(item => item.Name.Equals("FrodoFx")).Should().BeTrue();
                    result.Solution.Projects.Any(item => item.Name.Equals("FrodoFx.UnitTests")).Should().BeTrue();
                });

            "And the projects should have the correct path"
                ._(() =>
                {
                    result.Solution.Projects.Any(item => item.Path.Equals(Path.Combine(context.RootDirectory, "FrodoFx", "FrodoFx.csproj"))).Should().BeTrue();
                    result.Solution.Projects.Any(item => item.Path.Equals(Path.Combine(context.RootDirectory, "FrodoFx.UnitTests", "FrodoFx.UnitTests.csproj"))).Should().BeTrue();
                });
                
            "And the projects should reference their solution"
                ._(() => result.Solution.Projects.All(item => item.Solution != null).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }

        [Scenario]
        public void patch_assembly_info_with_project_missing_original_assembly_info(ICraneApi craneApi,
            SolutionBuilderContext context, ISolutionContext result, Project project)
        {
            "Given I have a crane api"
                ._(() => craneApi = ioc.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => result = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx").Build());

            "When I patch the assemble info"
                ._(() =>
                {
                    result = craneApi.PatchAssemblyInfo(project);
                    project = result.Solution.Projects.First();
                });

            "Then it should create the assembly info on disk"
                ._(() => File.Exists(project.AssemblyInfo.Path))
                .Teardown(() => context.TearDown());
        }
    }
}
