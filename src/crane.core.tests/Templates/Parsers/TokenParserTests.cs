using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Templates.Parsers;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Templates.Parsers
{
    public class TokenParserTests
    {
        [Scenario]
        public void Correctly_replaces_standard_context_tokens(MockContainer<TokenTemplateParser> tokenParser, string template)
        {
            string result = null;
            "Given I have a token template parser"
                ._(() => tokenParser = B.AutoMock<TokenTemplateParser>());
            
            "And I have a context with a project name"
                ._(() => TokenDictionaryUtility.Defaults(tokenParser.GetMock<ITokenDictionary>(),
                    new Dictionary<string, Func<string>>
                    {
                        { "%context.ProjectName%", () => @"ServiceStack" },
                        { "%context.ProjectRootDirectory.FullName%", () => @"c:\dev\servicestack" }
                    }));
            
            "When I parse the template"
                ._(() => result = tokenParser.Subject.Parse("Hello %context.ProjectName% in directory %context.ProjectRootDirectory.FullName%", null));

            "Then the token should be replaced"
                ._(() => result.Should().Be(@"Hello ServiceStack in directory c:\dev\servicestack"));
        }

        [Scenario]
        public void Correctly_replaces_guid_tokens(MockContainer<TokenTemplateParser> tokenParser, string template)
        {
            string result = null; var guid1 = Guid.NewGuid(); var guid2 = Guid.NewGuid();
            "Given I have a token template parser"
                ._(() => tokenParser = B.AutoMock<TokenTemplateParser>());

            "And it has two guids"
                ._(() => A.CallTo(() => tokenParser.GetMock<IGuidGenerator>().Create()).ReturnsNextFromSequence(guid1, guid2));

            "When I parse a template with guid tokens"
                ._(() => result = tokenParser.Subject.Parse(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%", null));

            "Then the guid tokens should be replaced"
                ._(() => result.Should().Be(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%"
                    .Replace("%GUID-1%", guid1.ToString("B"))
                    .Replace("%GUID-2%", guid2.ToString("B"))));
        }
        [Scenario]
        public void Correctly_replaces_guid_tokens_across_files(MockContainer<TokenTemplateParser> tokenParser, string template)
        {
            string result1 = null; string result2 = null; var guid1 = Guid.NewGuid(); var guid2 = Guid.NewGuid(); var guid3 = Guid.NewGuid(); var guid4 = Guid.NewGuid();
            "Given I have a token template parser"
                ._(() => tokenParser = B.AutoMock<TokenTemplateParser>());

            "And it has two guids"
                ._(() => A.CallTo(() => tokenParser.GetMock<IGuidGenerator>().Create()).ReturnsNextFromSequence(guid1, guid2, guid3, guid4));

            "When I parse a template with guid tokens in one file"
                ._(() => result1 = tokenParser.Subject.Parse(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%", null));

            "When I parse a template with guid tokens in one file"
                ._(() => result2 = tokenParser.Subject.Parse(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%", null));

            "Then the guid tokens should be replaced in the first file"
                ._(() => result1.Should().Be(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%"
                    .Replace("%GUID-1%", guid1.ToString("B"))
                    .Replace("%GUID-2%", guid2.ToString("B"))));

            "Then the guid tokens should be replaced in the second file"
                ._(() => result2.Should().Be(@"proj1ID = %GUID-1%; proj2ID = %GUID-2%; lib1 = %GUID-1%"
                    .Replace("%GUID-1%", guid1.ToString("B"))
                    .Replace("%GUID-2%", guid2.ToString("B"))));

            
        }
    }
}