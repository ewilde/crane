using System;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Model;
using Crane.Core.Configuration;
using Crane.Core.Runners;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class PatchAssemblyInfoFeature
    {
        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void patch_assembly_info(ICraneApi craneApi,
            SolutionBuilderContext context, ISolutionContext solutionContext, Project project, AssemblyInfo updatedInfo, string updatedRawInfo)
        {
            "Given I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with a project and an assembly info file"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Sally";
                        item.Description = "Next generation web server";
                        item.Version = new Version(0, 0, 0, 1);
                        item.FileVersion = new Version(0, 0, 0, 2);
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

        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void patch_solution_assembly_info(ICraneApi craneApi,
            SolutionBuilderContext context, ISolutionContext solutionContext, Project project, 
            AssemblyInfo updatedInfo, string updatedRawInfo)
        {
            "Given I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Sally";
                        item.Description = "Next generation web server";
                        item.Version = new Version(0, 0, 0, 1);
                        item.FileVersion = new Version(0, 0, 0, 2);
                        item.InformationalVersion = "RELEASE";
                    })
                    .WithProject(item => item.Name = "BobFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Bob";
                        item.Description = "Old school";
                        item.Version = new Version(0, 0, 0, 1);
                        item.FileVersion = new Version(0, 0, 0, 2);
                        item.InformationalVersion = "RELEASE";
                    }).Build());

            "And I have got the solution context"
                ._(() => solutionContext = craneApi.GetSolutionContext(context.RootDirectory));

            "When I path the solution assembly info for all projects"
                ._(() => craneApi.PatchSolutionAssemblyInfo(solutionContext, "1.2.3.4"));

            "Then file version should be set to 1.2.3.4 in all assembly info files"
                ._(() =>
                {
                    solutionContext = craneApi.GetSolutionContext(context.RootDirectory);
                    solutionContext.Solution.Projects.All(p => p.AssemblyInfo.FileVersion.ToString() == "1.2.3.4")
                        .Should()
                        .BeTrue();
                });

            "Then version should be set to 1.2.3.4 in all assembly info files"
                ._(() => solutionContext.Solution.Projects.All(p => p.AssemblyInfo.Version.ToString() == "1.2.3.4")
                                    .Should()
                                    .BeTrue());

            "Then file informational version should be set to 1.2.3.4"
                ._(() => solutionContext.Solution.Projects.All(p => p.AssemblyInfo.InformationalVersion.ToString() == "1.2.3.4")
                                    .Should()
                                    .BeTrue()
                         ).Teardown(() => context.TearDown());
        }

        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void patch_solution_assembly_info_when_project_in_git(ICraneApi craneApi,
            SolutionBuilderContext context, ISolutionContext solutionContext, Project project,
            AssemblyInfo updatedInfo, string updatedRawInfo, Git git)
        {
            "Given I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<CraneApi>());

            "And I have a solution builder context"
                ._(() => context = ServiceLocator.Resolve<SolutionBuilderContext>());

            "And I have a solution with two projects"
                ._(() => context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Sally";
                        item.Description = "Next generation web server";
                        item.Version = new Version(0, 0, 0, 1);
                        item.FileVersion = new Version(0, 0, 0, 2);
                        item.InformationalVersion = "RELEASE";
                    })
                    .WithProject(item => item.Name = "BobFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "Bob";
                        item.Description = "Old school";
                        item.Version = new Version(0, 0, 0, 1);
                        item.FileVersion = new Version(0, 0, 0, 2);
                        item.InformationalVersion = "RELEASE";
                    }).Build());

            "And I have got the solution context"
                ._(() => solutionContext = craneApi.GetSolutionContext(context.RootDirectory));

            "And I initialize that as a git repository"
                ._(() =>
                {
                    git = ServiceLocator.Resolve<Git>();
                    git.Run("init", context.RootDirectory).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", context.RootDirectory).ErrorOutput.Should().BeEmpty();
                });

            "And I have a previous commit"
                ._(() =>
                {
                    git.Run("add -A", context.RootDirectory).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", context.RootDirectory).ErrorOutput.Should().BeEmpty();
                    git.Run("commit -m \"My brand new project\"", context.RootDirectory).ErrorOutput.Should().BeEmpty();
                });

            "When I path the solution assembly info for all projects"
                ._(() => craneApi.PatchSolutionAssemblyInfo(solutionContext, "1.2.3.4"));

            "Then file version should be set to 1.2.3.4 in all assembly info files"
                ._(() =>
                {
                    solutionContext = craneApi.GetSolutionContext(context.RootDirectory);
                    solutionContext.Solution.Projects.All(p => p.AssemblyInfo.FileVersion.ToString() == "1.2.3.4")
                        .Should()
                        .BeTrue();
                });

            "Then version should be set to 1.2.3.4 in all assembly info files"
                ._(() => solutionContext.Solution.Projects.All(p => p.AssemblyInfo.Version.ToString() == "1.2.3.4")
                                    .Should()
                                    .BeTrue());

            "Then file informational version should start with 1.2.3.4"
                ._(() => solutionContext.Solution.Projects.All(p => p.AssemblyInfo.InformationalVersion.ToString().StartsWith("1.2.3.4"))
                                    .Should()
                                    .BeTrue())
                  .Teardown(() => context.TearDown());

            "Then file informational version should end with the commit message 'My brand new project'"
                ._(() => solutionContext.Solution.Projects.All(p => p.AssemblyInfo.InformationalVersion.ToString().EndsWith("My brand new project"))
                                    .Should()
                                    .BeTrue())
                  .Teardown(() => context.TearDown());
        }

        [ScenarioIgnoreOnMono("suspect fubucsprojfile does not work on mono")]
        public void patch_solution_assembly_should_only_patch_non_test_projects(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext, ICraneApi craneApi, AssemblyInfo originalAssemblyInfo)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have an instance of the crane api"
                ._(() => craneApi = ServiceLocator.Resolve<ICraneApi>());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I have got the solution context using the api"
                ._(() =>
                {
                    craneApi = ServiceLocator.Resolve<ICraneApi>();
                    solutionContext = craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                    originalAssemblyInfo =
                        solutionContext.Solution.Projects.First(p => p.Name == "ServiceStack.UnitTests").AssemblyInfo;
                });

            "And I have created one a solution with one code project called ServiceStack"
                ._(() => solutionContext.Solution.Projects.First(p => p.Name == "ServiceStack").TestProject.Should().BeFalse());

            "And I have created one a solution with one test project called ServiceStack.UnitTests"
                ._(() => solutionContext.Solution.Projects.First(p => p.Name == "ServiceStack.UnitTests").TestProject.Should().BeTrue());

            "When I path the solution assembly info for all projects"
                ._(() =>
                {
                    craneApi.PatchSolutionAssemblyInfo(solutionContext, "1.2.3.4");
                    solutionContext = craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                });

            "It should patch the code project's assemblyinfo version"
                ._(() => solutionContext.Solution.Projects.First(p => !p.TestProject)
                    .AssemblyInfo.Version.ToString().Should().Be( "1.2.3.4"));

            "It should patch the code project's assemblyinfo file version"
                ._(() => solutionContext.Solution.Projects.First(p => !p.TestProject)
                    .AssemblyInfo.FileVersion.ToString().Should().Be("1.2.3.4"));

            "It should patch the code project's assemblyinfo file informational version"
                ._(() => solutionContext.Solution.Projects.First(p => !p.TestProject)
                    .AssemblyInfo.InformationalVersion.Should().Be("1.2.3.4"));

            "It should not patch the test project's assemblyinfo version"
                ._(() => solutionContext.Solution.Projects.First(p => p.TestProject)
                    .AssemblyInfo.Version.Should().Be(originalAssemblyInfo.Version));

            "It should not patch the test project's assemblyinfo file version"
                ._(() => solutionContext.Solution.Projects.First(p => p.TestProject)
                    .AssemblyInfo.FileVersion.Should().Be(originalAssemblyInfo.FileVersion));

            "It should not patch the test project's file informational version"
                ._(() => solutionContext.Solution.Projects.First(p => p.TestProject)
                    .AssemblyInfo.InformationalVersion.Should().Be(originalAssemblyInfo.InformationalVersion))
                    .Teardown(() => craneTestContext.TearDown());
        }
    }
}