using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class GenDocFeature
    {
        [Scenario]
        public void generate_markdown_dynamic_documentation_for_crane_commands(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
               ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane init"
                ._(() => result = run.Command(craneTestContext.Directory, "crane gendoc"));

            "Then there should be no errors"
                ._(() =>
                {
                    result.StandardOutput.Should().BeCraneOutputErrorFree();
                    result.ErrorOutput.Should().BeEmpty();
                });
        }
    }
}