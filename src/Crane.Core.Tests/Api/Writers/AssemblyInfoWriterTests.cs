using System;
using Crane.Core.Api;
using Crane.Core.Api.Model;
using Crane.Core.Api.Writers;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using FluentAssertions;
using PowerArgs;
using Xbehave;

namespace Crane.Core.Tests.Api.Writers
{
    public class AssemblyInfoWriterTests
    {
        [Scenario]
        public void create_assembly_info_replaces_tokens(IAssemblyInfoWriter assemblyInfoWriter, MockFileManager fileManager)
        {
            "Given I have a file manager"
                ._(() => fileManager = new MockFileManager());

            "And I have an assembly info writer"
                ._(() => assemblyInfoWriter =  new AssemblyInfoWriter(fileManager));

            "When I call create"
                ._(() =>  assemblyInfoWriter.Create(@"c:\dev\sallyfx\properties\AssemblyInfo.cs", "SallyFx", "SallyFx is a new generation web server", "0.0.2", "0.0.2/f1214 x64", "0.0.2.1"));

            "It should replace the title token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyTitleAttribute(\"SallyFx\")]"));

            "It should replace the description token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyDescriptionAttribute(\"SallyFx is a new generation web server\")]"));

            "It should replace the version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyVersionAttribute(\"0.0.2\")]"));

            "It should replace the informational version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyInformationalVersionAttribute(\"0.0.2/f1214 x64\")]"));

            "It should replace the file version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyFileVersionAttribute(\"0.0.2.1\")]"));
        }

        [Scenario]
        public void patch_updates_assembly_info(IAssemblyInfoWriter assemblyInfoWriter, AssemblyInfo assemblyInfo, MockFileManager fileManager)
        {
            string path = @"c:\dev\sallyfx\properties\AssemblyInfo.cs";

            "Given I have a file manager"
                ._(() => fileManager = new MockFileManager());

            "And I have an assembly info writer"
                ._(() => assemblyInfoWriter = new AssemblyInfoWriter(fileManager));

            "And I have an assembly info on disk"
                ._(() => A.CallTo(() => fileManager.UnderlyingFake.ReadAllText(path)).Returns(Crane.Core.Properties.Resources.AssemblyInfoTemplate));

            "And I have a data to patch the assembly info file with"
                ._(() => assemblyInfo = new AssemblyInfo()
                {
                    Title = "Norman.Chat",
                    Description = "Norman is a chat bot",
                    FileVersion = new Version(0, 0, 3, 1),
                    Version = new Version(0, 0, 3, 0),
                    InformationalVersion = "RELEASE",
                    RootDirectory = @"c:\dev\sallyfx",
                    Include = @"properties\AssemblyInfo.cs"                    
                });

            "When I patch the assembly"
                ._(() => assemblyInfoWriter.Patch(assemblyInfo));

            "It should replace the title token"
               ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyTitleAttribute(\"Norman.Chat\")]"));

            "It should replace the description token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyDescriptionAttribute(\"Norman is a chat bot\")]"));

            "It should replace the version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyVersionAttribute(\"0.0.3.0\")]"));

            "It should replace the informational version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyInformationalVersionAttribute(\"RELEASE\")]"));

            "It should replace the file version token"
                ._(() => fileManager.Output.Should().Contain("[assembly: AssemblyFileVersionAttribute(\"0.0.3.1\")]"));
        }
    }
}