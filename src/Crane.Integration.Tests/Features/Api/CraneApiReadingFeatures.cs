using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class CraneApiReadingFeatures
    {
        [Scenario]
        public void Api_can_read_a_solution_with_multiple_projects(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "When I get the solution context using the api"
                ._(() =>
                {
                    var craneApi = ServiceLocator.Resolve<ICraneApi>();
                    solutionContext = craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"));
                });

            "It should have the correct path to the solution"
                ._(() => solutionContext.Solution.Path.Should()
                            .Be(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "src",
                                "ServiceStack.sln")));

            "It should have 2 projects"
                ._(() => solutionContext.Solution.Projects.Count().Should().Be(2));

            "It should have one project called ServiceStack"
                ._(() => solutionContext.Solution.Projects.Count(p => p.Name == "ServiceStack").Should().Be(1));

            "It should have one project called ServiceStack.UnitTests"
                ._(() => solutionContext.Solution.Projects.Count(p => p.Name == "ServiceStack.UnitTests").Should().Be(1))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Api_can_read_a_solution_by_path_direct_to_sln_file(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
            ISolutionContext solutionContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "When I get the solution context using the api"
                ._(() =>
                {
                    var craneApi = ServiceLocator.Resolve<ICraneApi>();
                    solutionContext = craneApi.GetSolutionContext(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "src", "ServiceStack.sln"));
                });

            "It should have the correct path to the solution"
                ._(() => solutionContext.Solution.Path.Should()
                            .Be(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "src",
                                "ServiceStack.sln")));

            "It should have 2 projects"
                ._(() => solutionContext.Solution.Projects.Count().Should().Be(2));

            "It should have one project called ServiceStack"
                ._(() => solutionContext.Solution.Projects.Count(p => p.Name == "ServiceStack").Should().Be(1));

            "It should have one project called ServiceStack.UnitTests"
                ._(() => solutionContext.Solution.Projects.Count(p => p.Name == "ServiceStack.UnitTests").Should().Be(1))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}