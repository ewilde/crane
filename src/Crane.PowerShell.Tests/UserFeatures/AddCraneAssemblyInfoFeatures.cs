using System;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.FluentExtensions;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.PowerShell.Tests.UserFeatures
{
    public class AddCraneAssemblyInfoFeatures
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_get_crane_context(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult, 
            ICraneApi craneApi, string projectDir)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have my powershell api runner"
                ._(() => apiRunner = new PowerShellApiRunner(craneTestContext, TimeSpan.FromSeconds(15)));

            "And I have initialized a project called ServiceStack"
                ._(() =>
                {
                    var craneRunner = new CraneRunner();
                    craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack");
                    projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");

                });

            "When I update the assembly info using powershell"
                ._(() => commandResult = apiRunner.Run(@"(Get-CraneSolutionContext -Path {0}).Solution.Projects ", projectDir));

            "Then the assembly info file should be patched"
                ._(
                    () =>
                        craneApi.GetSolutionContext(projectDir)
                            .Solution.Projects.First(p => p.Name == "ServiceStack")
                            .AssemblyInfo.Description.Should().Be("Test Description"));

            "And there should be no error"
                ._(() => commandResult.Should().BeErrorFree());

            "It should write the solution context to the powershell pipeline"
                ._(() => commandResult.StandardOutput.Should()
                    .Contain(projectDir))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}