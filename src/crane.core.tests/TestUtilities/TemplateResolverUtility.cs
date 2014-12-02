using System.Collections.Generic;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;

namespace Crane.Core.Tests.TestExtensions
{
    public static class TemplateUtility
    {
        public static void Defaults(ITemplateResolver templateResolver)
        {
            templateResolver.Templates = ioc.Resolve<IEnumerable<ITemplate>>();
        }
    }
}