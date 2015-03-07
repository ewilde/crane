using Crane.Core.Configuration;
using Crane.Core.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Runners
{
    public class NuGetTests
    {
        [Scenario]
        public void nuget_command_fails_validations_on_non_zero_exit_code(
            INuGet nuGetRunner,
            RunResult runResult,
            bool validateResult)
        {
            "Given I have a nuget runner"
                ._(() => nuGetRunner = ServiceLocator.Resolve<INuGet>());

            "And I have ran nuget and it returned a non-zero exit code"
                ._(() => runResult = new RunResult("nuget pack file.nuspec", null, null, 110));

            "When I validate the run result"
                ._(() => validateResult = nuGetRunner.ValidateResult(runResult));

            "Then it should fail validation"
                ._(() => validateResult.Should().BeFalse());
        }

        [Scenario]
        public void nuget_command_fails_validations_when_command_output_prints_error(
            INuGet nuGetRunner,
            RunResult runResult,
            bool validateResult)
        {
            "Given I have a nuget runner"
                ._(() => nuGetRunner = ServiceLocator.Resolve<INuGet>());

            "And I have ran nuget and it returned a non-zero exit code"
                ._(() => runResult = new RunResult("nuget pack file.nuspec", "nuget pack contains invalid arguments blah blah", null, 0));

            "When I validate the run result"
                ._(() => validateResult = nuGetRunner.ValidateResult(runResult));

            "Then it should fail validation"
                ._(() => validateResult.Should().BeFalse());
        }
    }
}