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
    public class CraneApiGetSolutionContextFeature
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

        [Scenario]
        public void Api_can_tell_which_projects_in_a_solution_are_test_projects(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext,
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

            "It should determine that the ServiceStack project is not a test project"
                ._(() => solutionContext.Solution.Projects.First(p => p.Name == "ServiceStack").TestProject.Should().BeFalse());

            "It should determine that the ServiceStack.UnitTests is a test project"
                ._(() => solutionContext.Solution.Projects.First(p => p.Name == "ServiceStack.UnitTests").TestProject.Should().BeTrue());            

            "It should have only the ServiceStack project in the code projects collection"
                ._(() =>
                {
                    solutionContext.Solution.CodeProjects.Count().Should().Be(1);
                    solutionContext.Solution.CodeProjects.First().Name.Should().Be("ServiceStack");
                }).Teardown(() => craneTestContext.TearDown());
        }
    }
}