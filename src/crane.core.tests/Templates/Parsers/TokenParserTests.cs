using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Templates.Parsers;
using Crane.Core.Tests.TestExtensions;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Templates.Parsers
{
    public class TokenParserTests
    {
        [Scenario]
        public void Given_a_template_and_tokens_correctly_replaces_them(MockContainer<TokenTemplateParser> tokenParser, string template)
        {
            string result = null;
            "Given I have a token template parser"
                ._(() => tokenParser = B.AutoMock<TokenTemplateParser>());
            
            "And I have a context with a project name"
                ._(() => ContextUtility.Configure(tokenParser.GetMock<ICraneContext>(), 
                                                    projectName: "ServiceStack", 
                                                    projectRootDirectory: new DirectoryInfo(@"c:\dev\servicestack")));
            
            "When I parse the template"
                ._(() => result = tokenParser.Subject.Parse("Hello %context.ProjectName% in directory %context.ProjectRootDirectory.FullName%", null));

            "Then the token should be replaced"
                ._(() => result.Should().Be(@"Hello ServiceStack in directory c:\dev\servicestack"));
        }
    }
}