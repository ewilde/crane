using System.IO;
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

            "Then I receive the text did you mean 'crane init projectname'?"
                ._(() => result.StandardOutput.Should().Be("did you mean 'crane init projectname'?"));
        }

        [Scenario]
        public void Init_with_a_project_name_creates_a_project(Run run, RunResult result)
        {
            "Given I have crane in my path"
                ._(() => run = new Run());

            "When I run crane init ServiceStack"
                ._(() => result = run.Command("crane init ServiceStack"));

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText("./ServiceStack/build/default.ps1").Should().Contain("ServiceStack.sln"))
                .Teardown(() =>
                {
                    Directory.Delete("./ServiceStack/build", recursive: true);
                    Directory.Delete("./ServiceStack", recursive: true);
                });


        }

        
    }
}
