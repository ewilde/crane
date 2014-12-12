using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using Xbehave;

namespace Crane.Core.Tests.Templates.Parsers
{
    public class FileAndDirectoryTokenParserTests
    {
        [Scenario]
        public void RenamesDirectoryContainingToken(MockContainer<FileAndDirectoryTokenParser> parser, DirectoryInfo directoryPath)
        {
            "Given I have a file and directory token parser"
                ._(() => parser = B.AutoMock<FileAndDirectoryTokenParser>());

            "And I have a directory with a token"
                ._(() => directoryPath = new DirectoryInfo(@"c:\dev\%context.ProjectName%"));

            "And I have a token dictionary"
                ._(
                    () =>
                        TokenDictionaryUtility.Defaults(parser.GetMock<ITokenDictionary>(),
                            new Dictionary<string, Func<string>> {{"%context.ProjectName%", () => "ServiceStack"}}));

            "When I call parse on the file and directory parser"
                ._(() => parser.Subject.Parse(directoryPath));

            "Then is should rename the directory using the current project name"
                ._(
                    () =>
                        A.CallTo(
                            () => parser.GetMock<IFileManager>().RenameDirectory(directoryPath.FullName, "ServiceStack"))
                            .MustHaveHappened());
        }

        [Scenario]
        public void RenamingAFileIgnoresTokenInFolderName(MockContainer<FileAndDirectoryTokenParser> parser, 
            DirectoryInfo directoryPath, string directory)
        {
            "Given I have a file and directory token parser"
                ._(() => parser = B.AutoMock<FileAndDirectoryTokenParser>());

            "And I have a directory on disk"
                ._(() =>
                {
                    directory = new FileManager().GetTemporaryDirectory();
                    directoryPath = Directory.CreateDirectory("%context.ProjectName%");
               });

            "And I have a file in that directory"
                ._(() => File.Create(Path.Combine(directoryPath.FullName, "Class1.cs")));

            "And I have a token dictionary"
                ._(
                    () =>
                        TokenDictionaryUtility.Defaults(parser.GetMock<ITokenDictionary>(),
                            new Dictionary<string, Func<string>> {{"%context.ProjectName%", () => "ServiceStack"}}));


            "When I call parse on the file and directory parser"
                ._(() => parser.Subject.Parse(directoryPath));

            "Then is should not try to rename the file"
                ._(
                    () =>
                        A.CallTo(
                            () => parser.GetMock<IFileManager>().RenameFile(A<string>.Ignored, A<string>.Ignored))
                            .MustNotHaveHappened())
                .Teardown(()=> Directory.Delete(directory, true));
        }
    }
}
