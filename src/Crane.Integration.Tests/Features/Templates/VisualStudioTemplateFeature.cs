using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class VisualStudioTemplateFeature
    {
        [Scenario]
        public void creating_a_visual_studio_setup_using_the_template(DirectoryInfo root, ITemplate template, ICraneContext context, ITemplateInvoker templateInvoker)
        {
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ServiceLocator.Resolve<ICraneContext>(); var fileManager = ServiceLocator.Resolve<IFileManager>();
                    context.ProjectRootDirectory = new DirectoryInfo(fileManager.GetTemporaryDirectory());
                    templateInvoker = ServiceLocator.Resolve<ITemplateInvoker>();
                });

            "And I have a psake template builder"
                ._(() => template = ServiceLocator.Resolve<TemplateResolver>().Resolve(TemplateType.Source));
            
            "When I call create on the template with ServiceStack as the project name and solution name"
                ._(() => templateInvoker.InvokeTemplate(template, new ProjectContext{ProjectName = "ServiceStack", SolutionPath = "ServiceStack"}));

            "It should rename the class library folder name with the current project name"
                ._(() => Directory.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack")).Should().BeTrue());

            "It should rename the test class library folder name with the current project name"
                ._(() => Directory.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.UnitTests")).Should().BeTrue());

            "It should rename the solution file with the current project name"
                ._(() => File.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.sln")).Should().BeTrue());

            "It should replace the tokens in the solution file"
                ._(() => File.ReadAllText(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.sln"))
                    .Should().NotContain("%GUID-1%").And.NotContain("%GUID-2%"));

            "It should replace the tokens in the project file"
                ._(() => File.ReadAllText(Path.Combine(context.SourceDirectory.FullName, "ServiceStack", string.Format("{0}.csproj", "ServiceStack")))
                    .Should().NotContain("%GUID-1%"));

            "It should replace the tokens in the project unit test file"
                ._(() => File.ReadAllText(Path.Combine(context.SourceDirectory.FullName, string.Format("{0}.UnitTests", "ServiceStack"), string.Format("{0}.UnitTests.csproj", "ServiceStack")))
                    .Should().NotContain("%GUID-1%"));

            "It should rename the nuget specification file with the current project name"
                ._(() => File.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack", string.Format("{0}.nuspec", "ServiceStack"))).Should().BeTrue());

            "It should replace the tokens in the nuget specification file"
                ._(() =>File.ReadAllText(Path.Combine(context.SourceDirectory.FullName, "ServiceStack", string.Format("{0}.nuspec", "ServiceStack")))
                    .Should().NotContain("%username%")
                    .And.NotContain("%context.ProjectName%")
                    .And.NotContain("%DateTime.Now.Year%"))
                .Teardown(() => Directory.Delete(context.ProjectRootDirectory.FullName, recursive: true));
        }
    }
}
