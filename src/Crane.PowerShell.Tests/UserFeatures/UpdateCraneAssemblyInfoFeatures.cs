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

    public class UpdateCraneAllProjectsAssemblyInfosFeature
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void should_update_all_code_assembly_infos(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult,
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

            "When I update the solution assembly infos"
                ._(() => commandResult = apiRunner.Run(@"Update-CraneAllProjectsAssemblyInfos -Path '{0}' -Version '4.5.6.7'", projectDir));

            "Then there should be no error"
               ._(() => commandResult.Should().BeErrorFree());

            "And the assembly info file should be patched for each code project"
                ._(() =>
                    craneApi.GetSolutionContext(projectDir).Solution.Projects.Where(p => !p.TestProject)
                        .All(p => p.AssemblyInfo.Version.ToString() == "4.5.6.7")
                        .Should()
                        .BeTrue()
                ).Teardown(() => craneTestContext.TearDown());

           
        }
    }
}