using System.IO;
using System.Linq;
using Crane.Core.Configuration;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.FluentExtensions;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{

    public class AssembleFeature
    {        
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void Assemble_with_a_folder_name_creates_a_build_when_folder_name_matches_solution_name(
            CraneRunner craneRunner, 
            RunResult result, 
            CraneTestContext craneTestContext,
            SolutionBuilderContext solutionBuilderContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have a project called ServiceStack with no build"
                ._(() =>
                {
                    solutionBuilderContext = ServiceLocator.Resolve<SolutionBuilderContext>();
                    solutionBuilderContext
                        .CreateBuilder()
                        .WithSolution(solution => solution.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "ServiceStack.sln"))
                        .WithFile(file => file.AddSolutionPackagesConfigWithXUnitRunner(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", ".nuget", "packages.config")))
                        .WithProject(project => project.Name = "ServiceStack.Core")
                        .Build();
                });

            "When I run crane assemble ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane Assemble ServiceStack"));

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

        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void Assemble_with_a_folder_name_creates_a_build_when_solution_is_a_different_name_and_in_different_location(
            CraneRunner craneRunner, 
            RunResult result, 
            CraneTestContext craneTestContext,
            SolutionBuilderContext solutionBuilderContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have a project called SolutionInDirectoryProject with no build"
                ._(() =>
                {
                    solutionBuilderContext = ServiceLocator.Resolve<SolutionBuilderContext>();
                    solutionBuilderContext
                        .CreateBuilder()
                        .WithSolution(solution => solution.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "solutions", "MySolution.sln"))
                        .WithFile(file => file.AddSolutionPackagesConfigWithXUnitRunner(Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "solutions", ".nuget", "packages.config")))
                        .WithProject(project =>
                        {
                            project.Name = "ServiceStack";
                            project.Path = Path.Combine(craneTestContext.BuildOutputDirectory, "SolutionInDirectoryProject", "src", "ServiceStack", "ServiceStack.csproj");
                        })
                        .Build();
                });

            "When I run crane assemble SolutionInDirectoryProject"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane Assemble SolutionInDirectoryProject"));

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
    }

}
