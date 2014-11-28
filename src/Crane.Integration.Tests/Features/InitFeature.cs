using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features
{
    public class InitFeature
    {
        [Scenario]
        public void Init_with_no_arguments_returns_did_you_mean_init_projectname(Run run, RunResult result)
        {
            "Given I have crane in my path"
                ._(() => run = new Run());

            "When I run crane init"
                ._(() => result = run.Command("crane init"));

            "Then I receive the text 'did you mean crane init projectname"
                ._(() => result.StandardOutput.Should().Be("did you mean crane init projectname"));
        }

        
    }
}
