using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;
using Crane.Core.Tests.Builders;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using Xbehave;

namespace Crane.Core.Tests.Templates.Parsers
{
    public class FileAndDirectoryTokenParserTests
    {
        [Scenario]
        public void RenamesDirectoryContainingToken(MockContainer<FileAndDirectoryTokenParser> parser, DirectoryInfo directoryPath, ITokenDictionary tokenDictionary)
        {
            "Given I have a file and directory token parser"
                ._(() => parser = B.AutoMock<FileAndDirectoryTokenParser>());

            "And I have a directory with a token"
                ._(() => directoryPath = new DirectoryInfo(@"%context.ProjectName%"));

            "And I have a token dictionary with the %context.ProjectName% set"
                ._(() => tokenDictionary = BuildA.TokenDictionary.WithToken("%context.ProjectName%", "ServiceStack").Build());
            
            "When I call parse on the file and directory parser"
                ._(() => parser.Subject.Parse(directoryPath, tokenDictionary));

            "Then is should rename the directory using the current project name"
                ._(
                    () =>
                        A.CallTo(
                            () => parser.GetMock<IFileManager>().RenameDirectory(directoryPath.FullName, "ServiceStack"))
                            .MustHaveHappened());
        }

        [Scenario]
        public void RenamingAFileIgnoresTokenInFolderName(MockContainer<FileAndDirectoryTokenParser> parser, 
            DirectoryInfo directoryPath, string directory, ITokenDictionary tokenDictionary)
        {
            "Given I have a file and directory token parser"
                ._(() => parser = B.AutoMock<FileAndDirectoryTokenParser>());

            "And I have a directory on disk"
                ._(() =>
                {
                    directory = ServiceLocator.Resolve<IFileManager>().GetTemporaryDirectory();
                    directoryPath = Directory.CreateDirectory("%context.ProjectName%");
               });

            "And I have a file in that directory"
                ._(() => File.Create(Path.Combine(directoryPath.FullName, "Class1.cs")));

            "And I have a token dictionary with the %context.ProjectName% set"
                ._(() => tokenDictionary = BuildA.TokenDictionary.WithToken("%context.ProjectName%", "ServiceStack").Build());
        

            "When I call parse on the file and directory parser"
                ._(() => parser.Subject.Parse(directoryPath, tokenDictionary));

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
