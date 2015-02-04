using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Core.Runners;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class CraneApiGetSourceControlInformationFeature
    {
        [Scenario]
        public void Api_can_read_the_latest_git_commit_from_a_solution_in_git(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext, string projectDir, Git git, ISourceControlInformation sourceControlInformation,
            ICraneApi craneApi)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I initialize that as a git repository"
                ._(() =>
                {
                    projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");
                    git = ServiceLocator.Resolve<Git>();
                    git.Run("init", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", projectDir).ErrorOutput.Should().BeEmpty();
                });

            "And I have a previous commit"
                ._(() =>
                {
                    git.Run("add -A", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("commit -m \"First commit of ServiceStack\"", projectDir).ErrorOutput.Should().BeEmpty();
                });


            "And I have the solution context using the api"
                ._(() =>
                {
                    craneApi = ServiceLocator.Resolve<ICraneApi>();
                    solutionContext = craneApi.GetSolutionContext(projectDir);
                });
            "When I get the source information using the api"
                ._(() => sourceControlInformation = craneApi.GetSourceControlInformation(solutionContext));

            "It should set the provider name to git"
                ._(() => sourceControlInformation.ProviderName.Should().Be("git"));

            "It should return the latest commit message as 'First commit of ServiceStack'"
                ._(() => sourceControlInformation.LastCommitMessage.Should().Contain("First commit of ServiceStack"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Api_returns_null_when_source_control_is_not_being_used(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext, ISourceControlInformation sourceControlInformation,
            ICraneApi craneApi)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "And I have the solution context using the api"
                ._(() =>
                {
                    craneApi = ServiceLocator.Resolve<ICraneApi>();
                    solutionContext = craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                });

            "When I get the source information using the api"
                ._(() => sourceControlInformation = craneApi.GetSourceControlInformation(solutionContext));

            "It should return null as the source control information"
                ._(() => sourceControlInformation.Should().BeNull())
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}