using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Psake;
using Crane.Core.Templates.VisualStudio;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class VisualStudioTemplateFeature
    {
        [Scenario]
        public void creating_a_visual_studio_setup_using_the_template(DirectoryInfo root, VisualStudioTemplate template, ICraneContext context, IFileManager fileManager)
        {
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ioc.Resolve<ICraneContext>(); fileManager = ioc.Resolve<IFileManager>();
                    context.ProjectRootDirectory = new DirectoryInfo(fileManager.GetTemporaryDirectory());
                });

            "And I have a psake template builder"
                ._(() => template = ioc.Resolve<VisualStudioTemplate>());

            "And I have been given a project name via init"
                ._(() => context.ProjectName = "ServiceStack");

            "When I call create on the template"
                ._(() => template.Create());

            "It should rename the class library folder name with the current project name"
                ._(() => Directory.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack")).Should().BeTrue());

            "It should rename the test class library folder name with the current project name"
                ._(() => Directory.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.Tests")).Should().BeTrue());

            "It should rename the solution file with the current project name"
                ._(() => File.Exists(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.sln")).Should().BeTrue());

            "It should replace the tokens in the solution file"
                ._(() => File.ReadAllText(Path.Combine(context.SourceDirectory.FullName, "ServiceStack.sln"))
                    .Should().NotContain("%GUID-1%").And.NotContain("%GUID-2%"))

                .Teardown(() => Directory.Delete(context.ProjectRootDirectory.FullName, recursive: true));
        }
    }
}
