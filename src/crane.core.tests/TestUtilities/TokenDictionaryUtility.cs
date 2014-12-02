using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Templates.Parsers;
using FakeItEasy;

namespace Crane.Core.Tests.TestUtilities
{
    public static class TokenDictionaryUtility
    {
        public static void Defaults(ITokenDictionary dictionary, Dictionary<string, Func<string>> tokens = null)
        {
            A.CallTo(() => dictionary.Tokens)
                .Returns(tokens ?? 
                         new Dictionary<string, Func<string>> {{"%context.ProjectName%", () => "FooProjectName"}});
        }
    }
}
