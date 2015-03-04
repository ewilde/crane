using System.IO;
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
        //[ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        [Scenario(Skip = "Not finished")]
        public void can_publish_build_to_nuget(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext, string projectDir, ICraneApi craneApi, NuGetServerContext nuGetServer)
        {
            "Given I have my own private copy of the crane console"
            ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a nuget server running"
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

            "When I publish to nuget using the api"
                ._(() => craneApi.NugetPublish(solutionContext));

            "It should push the package to the nuget server"
                ._(() => nuGetServer.PackageExists("SallyFx", "0.0.0.0").Should().BeTrue())
               .Teardown(() =>
               {
                   nuGetServer.TearDown();
                   craneTestContext.TearDown();
               });
        }
    }
}