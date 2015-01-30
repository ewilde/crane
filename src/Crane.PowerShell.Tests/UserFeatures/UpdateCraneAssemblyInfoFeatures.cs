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
    public class UpdateCraneAssemblyInfoFeatures
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void can_get_crane_context(CraneTestContext craneTestContext, PowerShellApiRunner apiRunner, RunResult commandResult, string assemblyInfoPath,
            ICraneApi craneApi)
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
                    
                });

            "And I know the path to a assembly info file in the project"
                ._(() =>
                {
                    craneApi = ServiceLocator.Resolve<CraneApi>();
                    assemblyInfoPath =
                        craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"))
                            .Solution.Projects.First(p => p.Name == "ServiceStack")
                            .AssemblyInfo.Path;
                });

            "When I update the assembly info using powershell"
                ._(() => commandResult = apiRunner.Run("Update-CraneAssemblyInfo -Path {0} -Version \"1.2.3.4\"", assemblyInfoPath));

            "Then the assembly info file should be patched"
                ._(
                    () =>
                        craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"))
                            .Solution.Projects.First(p => p.Name == "ServiceStack")
                            .AssemblyInfo.Version.ToString().Should().Be("1.2.3.4"));

            "And there should be no error"
                ._(() => commandResult.Should().BeErrorFree());

            "It should write the solution context to the powershell pipeline"
                ._(() => commandResult.StandardOutput.Should()
                    .Contain(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack")))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}