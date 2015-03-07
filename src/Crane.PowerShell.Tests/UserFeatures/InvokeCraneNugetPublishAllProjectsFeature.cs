using System;
using System.IO;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Core.Runners;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using Crane.Tests.Common.FluentExtensions;
using FluentAssertions;
using Xbehave;

namespace Crane.PowerShell.Tests.UserFeatures
{
    public class InvokeCraneNugetPublishAllProjectsFeature
    {
        [Scenario]
        public void invoking_command_packages_nuget_spec_file_correctly(
            CraneRunner craneRunner, 
            RunResult buildResult,
            RunResult commandResult, 
            CraneTestContext craneTestContext, 
            NuGetServerContext nuGetServer,
            string projectDir, 
            ICraneApi craneApi, 
            PowerShellApiRunner apiRunner)
        {
            "Given I have my own private copy of the crane console"
            ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<ICraneApi>());

            "And I have run crane init ServiceStack"
                ._(() => buildResult = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I have build the project"
                ._(() =>
                {
                    buildResult = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                    buildResult.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree();
                    projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");
                });
            
            "And I have my powershell api runner"
                ._(() => apiRunner = new PowerShellApiRunner(craneTestContext, TimeSpan.FromSeconds(15)));

            "And I have a nuGet server running"
               ._(() =>
               {
                   nuGetServer = new NuGetServerContext(craneTestContext);
                   nuGetServer.PackageCount.Should().BeGreaterThan(-1);
               });

            "And I have packed the nuget spec files"
                ._(() => 
                    apiRunner.Run(
                        @"$context = Get-CraneSolutionContext -Path '{0}'; " +
                         "Invoke-CraneNugetPackAllProjects -SolutionContext $context -BuildOutputPath '{1}' -NugetOutputPath '{2}' -Version {3}" +
                         "| % {{ $_.StandardOutput }}",
                        projectDir, Path.Combine(projectDir, "build-output"), Path.Combine(projectDir, "build-output", "nuGet"), "0.0.0.0").Should().BeErrorFree());

            "When I call invoke all nuget publish"
                ._(() => commandResult =
                    apiRunner.Run(
                        @"$context = Get-CraneSolutionContext -Path '{0}'; " +
                         "Invoke-CraneNugetPublishAllProjects -SolutionContext $context -NugetOutputPath '{1}' -Version '{2}' -Source '{3}' -ApiKey '{4}'" +
                         "| % {{ $_.StandardOutput }}",
                        projectDir, Path.Combine(projectDir, "build-output", "nuGet"), "0.0.0.0", nuGetServer.Source, nuGetServer.ApiKey));

            "Then there should be no error"
              ._(() => commandResult.Should().BeErrorFree());

            "And the nuget package should have been created"
                ._(() =>
                        File.Exists(Path.Combine(projectDir, "build-output", "nuGet",
                            "ServiceStack.0.0.0.0.nupkg")).Should().BeTrue())

                .Teardown(() =>
                {
                    nuGetServer.TearDown();                    
                    craneTestContext.TearDown();
                });
        }
    }
}