using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
            var buildTemplate = B.AutoMock<BaseTemplate>().Subject;
            buildTemplate.Name = "Psake";
            buildTemplate.TemplateType = TemplateType.Build;
            
            var sourceTemplate = B.AutoMock<BaseTemplate>().Subject;
            sourceTemplate.Name = "VisualStudio";
            sourceTemplate.TemplateType = TemplateType.Source;
            
            templateResolver.Templates = new[] { buildTemplate, sourceTemplate };
        }
    }
}