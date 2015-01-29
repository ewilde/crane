using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class PsakeBuildTemplateFeature
    {
        [Scenario]
        public void creating_a_new_psake_build(DirectoryInfo root, ITemplate template, ICraneContext context, ITemplateInvoker templateInvoker)
        {            
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ServiceLocator.Resolve<ICraneContext>();
                    var fileManager = ServiceLocator.Resolve<IFileManager>();
                    templateInvoker = ServiceLocator.Resolve<ITemplateInvoker>();
                    context.ProjectRootDirectory = new DirectoryInfo(fileManager.GetTemporaryDirectory());
                });

            "And I have a psake template builder"
                ._(() => template = ServiceLocator.Resolve<ITemplateResolver>().Resolve(TemplateType.Build));
            
            "When I call create on the template with service stack as the project name and solution name"
                ._(() => templateInvoker.InvokeTemplate(template, new ProjectContext {ProjectName = "ServiceStack", SolutionPath = "../ServiceStack.sln"}));

            "It place a build.ps1 in the root project directory"
                ._(() => File.Exists(Path.Combine(context.ProjectRootDirectory.FullName, "build.ps1")).Should().BeTrue("build.ps1 should be in root directory"));

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText(Path.Combine(context.BuildDirectory.FullName, "default.ps1")).Should().Contain("../ServiceStack.sln"))
                .Teardown(() => Directory.Delete(context.ProjectRootDirectory.FullName, recursive: true));            
        }
    }
}