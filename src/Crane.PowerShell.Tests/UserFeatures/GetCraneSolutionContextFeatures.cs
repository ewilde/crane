using System;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.FluentExtensions;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.PowerShell.Tests.UserFeatures
{
    public class GetCraneSolutionContextFeatures
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_get_crane_context(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult, CraneRunner craneRunner)
        {
            "Given I have my own private copy of the crane console"
               ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have my powershell api runner"
                ._(() => apiRunner = new PowerShellApiRunner(craneTestContext, TimeSpan.FromSeconds(15)));

            "And I have initialized a project called ServiceStack"
                ._(() =>
                {
                    craneRunner = new CraneRunner();
                    craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack");
                });

            "When I get the context using powershell"
                ._(() => commandResult = apiRunner.Run("Get-CraneSolutionContext -Path {0} | FL", Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack")));

            "Then there should be no error"
                ._(() => commandResult.Should().BeErrorFree());

            "It should write the solution context to the powershell pipeline"
                ._(() => commandResult.StandardOutput.Should()
                    .Contain(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack")))
                .Teardown(() =>craneTestContext.TearDown());
        }
    }
}