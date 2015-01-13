using System.Collections.Generic;
using System.IO;
using Crane.Core.Commands;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Extensions;
using Crane.Integration.Tests.TestUtilities;
using Crane.Integration.Tests.TestUtilities.Extensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class GenDocFeature
    {
        [Scenario]
        public void generate_markdown_dynamic_documentation_for_crane_commands(Run run, RunResult result, CraneTestContext craneTestContext, string docDirectory, string rootDirectory, IEnumerable<ICraneCommand> userCommands)
        {
            "Given I have my own private copy of the crane console"
               ._(() =>
               {
                   craneTestContext = ioc.Resolve<CraneTestContext>();
                   rootDirectory = craneTestContext.RootDirectory;
                   docDirectory = Path.Combine(rootDirectory, "doc");
                   userCommands = ioc.Resolve<IPublicCommandResolver>().Resolve();
                   if (Directory.Exists(docDirectory))
                       Directory.Delete(docDirectory, true);
               });

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane gendoc"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane gendoc"));

            "Then there should be no errors"
                ._(() => result.Should().BeErrorFree());

            "And there should be an index.md created in the doc folder"
                ._(() => File.Exists(Path.Combine(docDirectory , "index.md")).Should().BeTrue());

            "And the index page list all commands with links to command markdown file"
                ._(() =>
                {
                    var index = File.ReadAllText(Path.Combine(docDirectory , "index.md"));
                    userCommands.ForEach(
                        command =>
                            index.Should()
                                .Contain(string.Format("[`crane {0}`]({0}.md)", command.Name()),
                                    "missing link to command page {0} in index.md", command.Name()));
                });
            
            "And there should be a markdown file for each public crane command in the doc directory"
                ._(() => ioc.Resolve<IPublicCommandResolver>().Resolve().ForEach(
                    command => File.Exists(Path.Combine(docDirectory, command.Name() + ".md")).Should().BeTrue("missing {0} in directory {1}", command.Name() + ".md", docDirectory)));

            "And each command help file should have valid content"
                ._(() => userCommands.ForEach(
                    command =>
                        File.ReadAllText(Path.Combine(docDirectory, command.Name() + ".md"))
                            .Should()
                            .Contain(string.Format("usage: crane {0}", command.Name()))));

            "And it should update the mkdocs.yml file to configure add the command links to the navigation bar"
                ._(() =>
                {
                    var mkdocs = File.ReadAllText(Path.Combine(rootDirectory, "mkdocs.yml"));
                    userCommands.ForEach(command => mkdocs.Should().Contain(string.Format(" - ['{0}.md', 'Commands', '{0}']", command.Name())));
                });
        }
    }
}