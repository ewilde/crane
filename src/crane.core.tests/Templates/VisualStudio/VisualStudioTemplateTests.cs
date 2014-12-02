using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;
using Crane.Core.Templates.VisualStudio;
using Crane.Core.Tests.TestExtensions;
using FakeItEasy;
using Xbehave;

namespace Crane.Core.Tests.Templates.VisualStudio
{
    public class VisualStudioTemplateTests
    {
        [Scenario]
        public void calling_create_copies_all_template_files_into_destination_directory()
        {
            MockContainer<VisualStudioTemplate> studioTemplate = null;
            ICraneContext context = null; IConfiguration configuration = null;
            IFileManager fileManager = null;

            "Given an instance of the psake build template"
               ._(() => studioTemplate = B.AutoMock<VisualStudioTemplate>());

            "And a configuration after a crane init"
                ._(() =>
                {
                    configuration = studioTemplate.GetMock<IConfiguration>();
                    DefaultConfigurationUtility.PostInit(configuration);
                });

            "And a current empty directory named after the project"
                ._(() =>
                {
                    context = studioTemplate.GetMock<ICraneContext>();
                    ContextUtility.Configure(context, projectName: "ServiceStack", projectRootDirectory: new DirectoryInfo(@"c:\dev\servicestack"));
                });

            "When I call create"
                ._(() => studioTemplate.Subject.Create());

            "It should create a src directory"
                 ._(() =>
                 {
                     fileManager = studioTemplate.GetMock<IFileManager>();
                     A.CallTo(() => fileManager
                         .CreateDirectory(string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultSourceFolderName)))
                         .MustHaveHappened();
                 });

            "It should copy all the files from the template directory to the src directory"
                ._(()=> A.CallTo(() => fileManager
                    .CopyFiles(
                        Path.Combine(studioTemplate.Subject.TemplateSourceDirectory.FullName, "2013"),
                        string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultSourceFolderName), true))
                    .MustHaveHappened());

            "It should rename all the tokenized directories"
                ._(
                    () =>
                        A.CallTo(
                            () => studioTemplate.GetMock<IFileAndDirectoryTokenParser>().Parse(context.ProjectRootDirectory))
                            .MustHaveHappened());
        }
    }
}