using System;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Core.Runners;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.FluentExtensions;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.PowerShell.Tests.UserFeatures
{
    public class UpdateCraneAssemblyInfoFeatures
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_get_crane_context(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult, 
            ICraneApi craneApi, string projectDir)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have my powershell api runner"
                ._(() => apiRunner = new PowerShellApiRunner(craneTestContext, TimeSpan.FromSeconds(15)));

            "And I have a crane api"
                ._(() => craneApi = ServiceLocator.Resolve<ICraneApi>());

            "And I have initialized a project called ServiceStack"
                ._(() =>
                {
                    var craneRunner = new CraneRunner();
                    craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack");
                    projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");

                });

            "When I update the assembly info using powershell"
                ._(() => commandResult = apiRunner.Run(@"(Get-CraneSolutionContext -Path '{0}').Solution.Projects | % {{ $_.AssemblyInfo.Description = $_.Name + ' Project'; $_.AssemblyInfo.Version = '1.2.3.4'; Update-CraneAssemblyInfo $_ }}", projectDir));

            "Then the assembly info file should be patched for each project"
                ._(
                    () =>
                    { 
                        var projects =
                        craneApi.GetSolutionContext(projectDir)
                            .Solution.Projects.ToList();

                        foreach (var project in projects)
                        {
                            project.AssemblyInfo.Description.Should().Be(string.Format("{0} Project", project.Name));
                            project.AssemblyInfo.Version.Should().Be(new Version(1, 2, 3, 4));
                        }
                            
                    });

            "And there should be no error"
                ._(() => commandResult.Should().BeErrorFree())
                .Teardown(() => craneTestContext.TearDown());

        
        }
    }
}