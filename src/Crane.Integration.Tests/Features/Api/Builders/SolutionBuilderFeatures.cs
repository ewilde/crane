using System;
using System.IO;
using System.Linq;
using Crane.Core.Api;
using Crane.Core.Api.Builders;
using Crane.Core.Api.Model;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using FubuCsProjFile;
using Xbehave;
using AssemblyInfo = Crane.Core.Api.Model.AssemblyInfo;

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

            "And the solution context path should be the root directory"
                ._(() => result.Path.Should().Be(context.RootDirectory));
            
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


        [Scenario]
        public void build_project_with_assembly_info(SolutionBuilderContext context, ISolutionContext result, Project project, AssemblyInfo assemblyInfo)
        {
            "Given I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "When I call build with a solution, a project and an assembly info"
                ._(() => result = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "FrodoFx";
                        item.Description = "Middle earth web server";
                        item.FileVersion = new Version(0, 0, 1, 2049);
                        item.Version = new Version(0, 0, 1);
                        item.InformationalVersion = "release";
                    })
                    .Build());

            "It should return the build project with an assembly info"
                ._(() =>
                {
                    project = result.Solution.Projects.FirstOrDefault();
                    project.Should().NotBeNull("the project should not be null");
                    assemblyInfo = project.AssemblyInfo;
                    assemblyInfo.Should().NotBeNull("the assembly info should not be null");
                });

            "And the assembly info title should be set"
                ._(() => assemblyInfo.Title.Should().Be("FrodoFx"));

            "And the assembly info version should be set"
                ._(() => assemblyInfo.Version.Should().Be(new Version(0, 0, 1)));

            "And the assembly info file version should be set"
                ._(() => assemblyInfo.FileVersion.Should().Be(new Version(0, 0, 1, 2049)));

            "And the assembly informational attribute should be set"
                ._(() => assemblyInfo.InformationalVersion.Should().Be("release"));

            "And the assembly info file should exist on disk"
                ._(() => File.Exists(assemblyInfo.Path).Should().BeTrue(string.Format("assembly info with path: {0} did not exist on disk", assemblyInfo.Path)));
            
            "And it should create a solution file on disk"
                ._(() => File.Exists(Path.Combine(context.RootDirectory, "Sally.sln")).Should().BeTrue())
                .Teardown(() => context.TearDown());
        }


        [Scenario]
        public void build_project_with_assembly_info_fubu_test(SolutionBuilderContext context, ISolutionContext solutionContext, FubuCsProjFile.CsProjFile project)
        {
            "Given I have a solution builder context"
                ._(() => context = ioc.Resolve<SolutionBuilderContext>());

            "When I call build with a solution, a project and an assembly info"
                ._(() => solutionContext = context.CreateBuilder()
                    .WithSolution(item => item.Path = Path.Combine(context.RootDirectory, "Sally.sln"))
                    .WithProject(item => item.Name = "FrodoFx")
                    .WithFile<AssemblyInfo>(item =>
                    {
                        item.Title = "FrodoFx";
                        item.Description = "Middle earth web server";
                        item.FileVersion = new Version(0, 0, 1, 2049);
                        item.Version = new Version(0, 0, 1);
                        item.InformationalVersion = "release";
                    })
                    .Build());

            "It should return the build project with an assembly info"
                ._(() =>
                {
                    project = new CsProjFile(solutionContext.Solution.Projects.FirstOrDefault().Path);
                    project.Should().NotBeNull("the project should not be null");
                    
                    project.AssemblyInfo.Should().NotBeNull("the assembly info should not be null");
                });

            "And the assembly info title should be set"
                ._(() => project.AssemblyInfo.AssemblyTitle.Should().Be("FrodoFx"))
                .Teardown(() => context.TearDown());
        }
    }
}