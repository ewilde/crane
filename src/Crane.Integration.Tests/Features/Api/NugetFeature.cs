using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Core.Runners;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using Crane.Tests.Common.FluentExtensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class NugetFeature
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_create_a_nuget_package(
            CraneRunner craneRunner, 
            RunResult result, 
            CraneTestContext craneTestContext,
            ISolutionContext solutionContext, 
            string projectDir, 
            ICraneApi craneApi,
            string buildOutputFolder,
            string nugetOutputFolder,
            IEnumerable<RunResult> runResults)
        {
            "Given I have my own private copy of the crane console"
            ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I have build the project"
                ._(() =>
                {
                    result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                    result.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree(); ;
                });

            "And I have the solution context using the api"
               ._(() =>
               {
                   projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");
                   craneApi = ServiceLocator.Resolve<ICraneApi>();
                   solutionContext = craneApi.GetSolutionContext(projectDir);
                   buildOutputFolder = Path.Combine(solutionContext.Path, "build-output");
               });

            "When I create nuGet packages using the api"
                ._(
                    () =>
                    {
                        nugetOutputFolder = Path.Combine(buildOutputFolder, "nuGet");
                        runResults = craneApi.NugetPack(solutionContext, buildOutputFolder, nugetOutputFolder, "0.0.0.0");
                    });

            "It should create nuGet packages for all the projects in the built solution"
                ._(() => File.Exists(Path.Combine(nugetOutputFolder, "ServiceStack.0.0.0.0.nupkg")).Should().BeTrue())
                .Teardown(() => craneTestContext.TearDown());
        }

        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_publish_build_to_nuget(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext, string projectDir, ICraneApi craneApi, NuGetServerContext nuGetServer)
        {
            "Given I have my own private copy of the crane console"
            ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a nuGet server running"
                ._(() =>
                {
                    nuGetServer = new NuGetServerContext(craneTestContext);
                    nuGetServer.PackageCount.Should().BeGreaterThan(-1);
                });    

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I have build the project"
                ._(() =>
                {
                    result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                    result.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree();;
                });

            "And I have the solution context using the api"
               ._(() =>
               {
                   projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");
                   craneApi = ServiceLocator.Resolve<ICraneApi>();
                   solutionContext = craneApi.GetSolutionContext(projectDir);
               });

            "And I have packaged the nuget spec"
                ._(() =>
                {
                    var buildOutputPath = Path.Combine(solutionContext.Path, "build-output");
                    craneApi.NugetPack(solutionContext, buildOutputPath, Path.Combine(buildOutputPath, "nuGet"), "0.0.0.0").First().Should().BeErrorFree();
                });

            "When I publish to nuGet using the api"
                ._(
                    () =>
                        craneApi.NugetPublish(solutionContext,
                            Path.Combine(solutionContext.Path, "build-output", "nuGet"), "0.0.0.0",
                            nuGetServer.Source.ToString(), nuGetServer.ApiKey).First().Should().BeErrorFree());

            "It should push the package to the nuGet server"
                ._(() => nuGetServer.PackageExists("ServiceStack", "0.0.0.0").Should().BeTrue())
               .Teardown(() =>
               {
                   nuGetServer.TearDown();
                   craneTestContext.TearDown();
               });
        }
    }
}