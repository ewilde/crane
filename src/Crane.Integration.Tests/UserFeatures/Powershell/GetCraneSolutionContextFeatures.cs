using System;
using System.IO;
using Crane.Core.Configuration;
using Crane.Integration.Tests.TestUtilities;
using Crane.Integration.Tests.TestUtilities.Extensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.Powershell
{
    public class GetCraneSolutionContextFeatures
    {
        [ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
        public void can_get_crane_context(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult, Run craneRunner)
        {
            "Given I have my own private copy of the crane console"
               ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have my powershell api runner"
                ._(() => apiRunner = new PowerShellApiRunner(craneTestContext, TimeSpan.FromSeconds(15)));

            "And I have initialized a project called ServiceStack"
                ._(() =>
                {
                    craneRunner = new Run();
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