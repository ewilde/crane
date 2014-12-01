using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.VisualStudio;
using Crane.Core.Tests.TestExtensions;
using FakeItEasy;
using Xbehave;

namespace Crane.Core.Tests.Templates.VisualStudio
{
    public class VisualStudioTemplateTests
    {
        [Scenario]
        public void calling_create_copies_all_template_files_into_destination_directory(
            MockContainer<VisualStudioTemplate> studioTemplate)
        {
            "Given an instance of the psake build template"
               ._(() => studioTemplate = B.AutoMock<VisualStudioTemplate>());

            "And a configuration after a crane init"
                ._(() => DefaultConfigurationUtility.PostInit(studioTemplate.GetMock<IConfiguration>()));

            "And a current empty directory named after the project"
                ._(() => ContextUtility.Configure(studioTemplate.GetMock<ICraneContext>(), projectName: "ServiceStack", projectRootDirectory: new DirectoryInfo(@"c:\dev\servicestack")));

            "When I call create"
                ._(() => studioTemplate.Subject.Create());

            "It should create a src directory"
                 ._(() => A.CallTo(() => studioTemplate.GetMock<IFileManager>()
                    .CreateDirectory(string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultSourceFolderName)))
                    .MustHaveHappened());

            "It should copy all the files from the template directory to the src directory"
                ._(()=> A.CallTo(() => studioTemplate.GetMock<IFileManager>()
                    .CopyFiles(
                        studioTemplate.Subject.TemplateSourceDirectory.FullName,
                        string.Format(@"c:\dev\servicestack\{0}", CraneConfiguration.DefaultSourceFolderName), "*.*"))
                    .MustHaveHappened());

            //"It should rename the class library folder to the current project name"
            //    ._(()=> A.CallTo(() => studioTemplate.GetMock<IFileManager>()
            //        .RenameDirectory
        }
    }
}