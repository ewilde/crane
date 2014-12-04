using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Psake;
using Crane.Core.Tests.TestExtensions;
using FakeItEasy;
using Xbehave;

namespace Crane.Core.Tests.Templates.Psake
{
    public class PsakeBuildTemplateTests
    {
        [Scenario]
        public void calling_create_copies_all_template_files_into_destination_directory(MockContainer<PsakeBuildTemplate> buildTemplate)
        {
            "Given an instance of the psake build template"
                ._(() => buildTemplate = B.AutoMock<PsakeBuildTemplate>());

            "And a configuration after a crane init"
                ._(() => DefaultConfigurationUtility.PostInit(buildTemplate.GetMock<IConfiguration>()));

            "And a current empty directory named after the project"
                ._(() => ContextUtility.Configure(buildTemplate.GetMock<ICraneContext>(), projectName: "ServiceStack", projectRootDirectory: new DirectoryInfo(@"c:\dev\servicestack")));

            "When I call create"
                ._(() => buildTemplate.Subject.Create());

            "It should create a build directory"
                ._(() =>
                        A.CallTo(() => buildTemplate.GetMock<IFileManager>()
                                .CreateDirectory(string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultBuildFolderName)))
                                .MustHaveHappened());

            "It should copy all the template files to the new project's build directory"
                ._(() => A.CallTo(() => buildTemplate.GetMock<IFileManager>()
                    .CopyFiles(
                            Path.Combine(buildTemplate.Subject.TemplateSourceDirectory.FullName, "build"),
                            string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultBuildFolderName), true))
                    .MustHaveHappened());
        }
    }
}