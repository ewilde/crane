using System;
using System.IO;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Resolvers;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class SolutionPathResolverTests
    {
        [Scenario]
        public void Resolves_when_solution_is_at_same_level_at_build_folder(string result, string path, SolutionPathResolver solutionPathResolver)
        {
            "Given I have a path to a project where the solution is at the same level as the build dir"
                ._(() =>
                {
                    solutionPathResolver = new SolutionPathResolver();
                    path = "TestProjects/SameLevel";
                });

            "When I resolve the path relative to the build folder"
                ._(() => result = solutionPathResolver.GetPathRelativeFromBuildFolder(path));

            "Then the resolved path should be '..\\test.sln"
                ._(() => result.Should().Be(string.Format("..{0}test.sln", Path.DirectorySeparatorChar)));
        }

        [Scenario]
               
        public void Resolves_when_solution_is_at_a_deeper_level_than_build_folder(string result, string path, SolutionPathResolver solutionPathResolver)
        {
            "Given I have a path to a project where the solution is at the a deeper level than the build dir"
                ._(() =>
                {
                    solutionPathResolver = new SolutionPathResolver();
                    path = "TestProjects/DeeperLevel";
                });

            "When I resolve the path relative to the build folder"
                ._(() => result = solutionPathResolver.GetPathRelativeFromBuildFolder(path));

            "Then the resolved path should be '..\\Solutions\\sln\\deep.sln"
                ._(() => result.Should().Be(string.Format("..{0}Solutions{0}sln{0}deep.sln", Path.DirectorySeparatorChar)));
        }
      

        [Scenario]
        public void Resolves_throws_a_multiple_solutions_found_exception_when_more_than_one_solution_found(Exception result, string path, SolutionPathResolver solutionPathResolver)
        {
            "Given I have a path to a project where there are multiple solutions"
                ._(() =>
                {
                    solutionPathResolver = new SolutionPathResolver();
                    path = "TestProjects/MultipleSolutions";
                });

            "When I resolve the path relative to the build folder"
                ._(() => result = Throws.Exception(() => solutionPathResolver.GetPathRelativeFromBuildFolder(path)));

            "Then a MultipleSolutionsFoundCraneException should be thrown"
                ._(() => result.Should().BeOfType<MultipleSolutionsFoundCraneException>());

        }

        [Scenario]
        public void Resolves_throws_a_no_solutions_found_exception_when_no_solutions_found(Exception result, string path, SolutionPathResolver solutionPathResolver)
        {
            "Given I have a path to a project where there are no solutions"
                ._(() =>
                {
                    solutionPathResolver = new SolutionPathResolver();
                    path = "TestProjects/NoSolutions";
                });

            "When I resolve the path relative to the build folder"
                ._(() => result = Throws.Exception(() => solutionPathResolver.GetPathRelativeFromBuildFolder(path)));

            "Then a NoSolutionsFoundCraneException should be thrown"
                ._(() => result.Should().BeOfType<NoSolutionsFoundCraneException>());
        }
    }
}