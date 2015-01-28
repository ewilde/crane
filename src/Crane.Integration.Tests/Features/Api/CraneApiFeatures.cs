using System;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Model;
using Crane.Core.Configuration;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
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
                ._(() => craneApi = ServiceLocator.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithProject(item => item.Name = "FrodoFx.UnitTests")
                    .Build());

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

        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void patch_assembly_info(ICraneApi craneApi,
            SolutionBuilderContext context, ISolutionContext solutionContext, Project project, AssemblyInfo updatedInfo, string updatedRawInfo)
        {
            "Given I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Sally";
                        item.Description = "Next generation web server";
                        item.Version = new Version(0,0,0,1);         
                        item.FileVersion = new Version(0,0,0,2);
                        item.InformationalVersion = "RELEASE";
                    }).Build());

            "And I have an updated assembly info with different version, file number and informational attribute"
                ._(() =>
                {
                    updatedInfo = solutionContext.Solution.Projects.First().AssemblyInfo;
                    updatedInfo.Version = new Version(0, 1, 0, 0);
                    updatedInfo.FileVersion = new Version(0, 1, 20);
                    updatedInfo.InformationalVersion = "DEBUG";
                });

            "When I patch the assemble info"
                ._(() =>
                {
                    craneApi.PatchAssemblyInfo(updatedInfo);

                    solutionContext = craneApi.GetSolutionContext(solutionContext.Path); // reload to get updated model
                    project = solutionContext.Solution.Projects.First();
                    updatedRawInfo = File.ReadAllText(project.AssemblyInfo.Path);
                });

            "Then it should update the assembly file version"
                ._(() =>
                {
                    updatedRawInfo.Should().Contain("[assembly: AssemblyFileVersionAttribute(\"0.1.20\")]");
                    project.AssemblyInfo.FileVersion.Should().Be(new Version(0, 1, 20));
                });

            "Then it should update the informational version attribute"
                ._(() =>
                {
                    updatedRawInfo.Should().Contain("[assembly: AssemblyInformationalVersionAttribute(\"DEBUG\")]");
                    project.AssemblyInfo.InformationalVersion.Should().Be("DEBUG");
                });

            "Then it should update the assembly version"
                ._(() =>
                {
                    updatedRawInfo.Should().Contain("[assembly: AssemblyVersionAttribute(\"0.1.0.0\")]");
                    project.AssemblyInfo.Version.Should().Be(new Version(0, 1, 0, 0));
                });

            "Then it not update the assembly title as it was not changed"
                ._(() =>
                {
                    updatedRawInfo.Should().Contain("[assembly: AssemblyTitleAttribute(\"Sally\")]");
                    project.AssemblyInfo.Title.Should().Be("Sally");
                })
                .Teardown(() => context.TearDown());
        }
    }
}
