using System.IO;
using System.Linq;
using Crane.Core.Api.Builders;
using Crane.Integration.Tests.TestUtilities;
using Crane.Integration.Tests.TestUtilities.Extensions;
using FluentAssertions;
using Ionic.Zip;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{

    public class AssembleFeature
    {        
        [ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
        public void Assemble_with_a_folder_name_creates_a_build_when_folder_name_matches_solution_name(
            Run run, 
            RunResult result, 
            CraneTestContext craneTestContext,
            SolutionBuilderContext solutionBuilderContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "And I have a project called ServiceStack with no build"
                ._(() =>
                {
                    solutionBuilderContext = ioc.Resolve<SolutionBuilderContext>();
                    solutionBuilderContext
                        .CreateBuilder()
                        .WithSolution(solution => solution.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "ServiceStack.sln"))
                        .WithFile(text => AddSolutionPackagesConfigWithXUnitRunner(text, Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", ".nuget", "packages.config")))
                        .WithProject(project => project.Name = "ServiceStack.Core")
                        .Build();
                });

            "When I run crane assemble ServiceStack"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane Assemble ServiceStack"));

            "It should say 'Assemble success.'"
                ._(() =>
                {
                    result.ErrorOutput.Should().BeEmpty();
                    result.StandardOutput.Should().Be("Assemble success.");
                });

            "It should create a build.ps1 in the top level folder"
                ._(
                    () => new DirectoryInfo(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack")).GetFiles()
                            .Count(f => f.Name.ToLower() == "build.ps1")
                            .Should()
                            .Be(1)
                );

            "It should be able to be built successfuly with all tests passing"
                ._(() =>
                {
                    var buildResult = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                    buildResult.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree();
                });

            "It should create a build for the project with a reference to the solution file"
				._(() => File.ReadAllText(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack" , "build", "default.ps1")).Should().Contain("ServiceStack.sln"))
                .Teardown(() =>
                {
                    solutionBuilderContext.TearDown();
                    craneTestContext.TearDown();
                });
        }

        [ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
        public void Assemble_with_a_folder_name_creates_a_build_when_solution_is_a_different_name_and_in_different_location(
            Run run, 
            RunResult result, 
            CraneTestContext craneTestContext,
            SolutionBuilderContext solutionBuilderContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "And I have a project called SolutionInDirectoryProject with no build"
                ._(() =>
                {
                    solutionBuilderContext = ioc.Resolve<SolutionBuilderContext>();
                    solutionBuilderContext
                        .CreateBuilder()
                        .WithSolution(solution => solution.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "solutions", "MySolution.sln"))
                        .WithFile(text => AddSolutionPackagesConfigWithXUnitRunner(text, Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "solutions", ".nuget", "packages.config")))
                        .WithProject(project =>
                        {
                            project.Name = "ServiceStack";
                            project.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "ServiceStack", "ServiceStack.csproj");
                        })
                        .Build();
                });

            "When I run crane assemble SolutionInDirectoryProject"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane Assemble SolutionInDirectoryProject"));

            "It should say 'Assemble success.'"
                ._(() =>
                {
                    result.ErrorOutput.Should().BeEmpty();
                    result.StandardOutput.Should().Be("Assemble success.");
                });

            "It should create a build.ps1 in the top level folder"
                ._(
                    () => new DirectoryInfo(Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject")).GetFiles()
                            .Count(f => f.Name.ToLower() == "build.ps1")
                            .Should()
                            .Be(1)
                );

            "It should be able to be built successfully with all tests passing"
                ._(() =>
                {
                    var buildResult = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject"));
                    buildResult.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree();
                });


            "It should create a build for the project with a reference to the solution file"
                ._(() => File.ReadAllText(Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "build", "default.ps1")).Should().Contain("MySolution.sln"))
                .Teardown(() => craneTestContext.TearDown());
        }

        private static void AddSolutionPackagesConfigWithXUnitRunner(PlainFile text, string path)
        {
            text.Path = path;
            text.Text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""xunit.runners"" version=""1.9.2"" />
</packages>";
        }
    }

}
