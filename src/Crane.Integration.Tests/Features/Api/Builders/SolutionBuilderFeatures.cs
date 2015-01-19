using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Builders;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api.Builders
{
    public class SolutionBuilderTests
    {
        [Scenario]
        public void build_project_with_name(SolutionBuilderContext context, ISolutionContext result, Project project)
        {
            "Given I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "When I call build"
                ._(() => result = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx").Build());

            "It should return the build project"
                ._(() =>
                {
                    project = result.Solution.Projects.FirstOrDefault();
                    project.Should().NotBeNull();
                });

            "And the project should have a name"
                ._(() => project.Name.Should().Be("FrodoFx"));
            
            "And the project should have a path"
                ._(() => project.Path.Should().Be(Path.Combine(context.RootDirectory, "FrodoFx", "FrodoFx.csproj")));

            "And the project file should exist on disk"
                ._(() => File.Exists(project.Path).Should().BeTrue());
                
            "And it should create a solution file on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "Sally.sln")).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }

        /// <summary>
        /// To be clear this is not a practice we like, but we want to be able to build it 
        /// so we can test crane assemble with unusual source code structures
        /// </summary>
        /// <example>
        /// C:\DEV\TEMP\SOLUTIONINDIRECTORYPROJECT
        /// └───src
        ///     ├───.nuget
        ///     │       packages.config
        ///     │
        ///     ├───ServiceStack
        ///     │   │   Calculator.cs
        ///     │   │   ServiceStack.csproj
        ///     │   └───Properties
        ///     │           AssemblyInfo.cs
        ///     │
        ///     ├───ServiceStack.UnitTests
        ///     │   │   CalculatorFeature.cs
        ///     │   │   packages.config
        ///     │   │   ServiceStack.UnitTests.csproj
        ///     │   └───Properties
        ///     │           AssemblyInfo.cs
        ///     │
        ///     └───Solutions
        ///             MySolution.sln
        /// </example>
        [Scenario]
        public void build_solution_with_projects_in_sibling_directories(SolutionBuilderContext context, ISolutionContext result, Project project)
        {
            "Given I have a solution builder context"
               ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "When I call build"
                ._(() => result = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Solutions", "MySolution.sln"))
                    .WithProject(item =>
                    {
                        item.Name = "ServiceStack";
                        item.Path = Path.Combine(context.RootDirectory, "ServiceStack", "ServiceStack.csproj");
                    }).Build());

            "It should return the build project"
                ._(() =>
                {
                    project = result.Solution.Projects.FirstOrDefault();
                    project.Should().NotBeNull();
                });

            "And the project file should have the name set correctly"
                ._(() => project.Name.Should().Be("ServiceStack"));

            "And the project file should exist on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "ServiceStack", "ServiceStack.csproj")).Should().BeTrue());

            "And the solution file should have the correct name"
                ._(() => result.Solution.Name.Should().Be("MySolution"));

            "And it should create a solution file on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "Solutions", "MySolution.sln")).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }
    }
}