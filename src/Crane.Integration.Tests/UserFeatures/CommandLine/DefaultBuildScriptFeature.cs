using System.IO;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class DefaultBuildScriptFeature
    {
        [Scenario]
        public void build_a_new_default_crane_project_sucessfully(Run run, RunResult result, CraneTestContext craneTestContext)
        {
             "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "Give I have a new crane project 'SallyFx'"
                ._(() => run.Command(craneTestContext.Directory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "When I build the project"
                ._(() => result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.Directory, "SallyFx")));

            "It should have build the main 'SallyFx' class library"
                ._(() => File.Exists(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.dll")));

            "It should have build the 'SallyFx' unit test library"
                ._(() => File.Exists(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.UnitTests.dll")));

            "It should not throw an error"
                ._(() => result.ErrorOutput.Should().BeEmpty())
                .Teardown(() => craneTestContext.TearDown()); 
        }
    }
}