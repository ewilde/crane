using Crane.Core.Configuration;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using Crane.Core.Templates.VisualStudio;
using Crane.Core.Tests.TestExtensions;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Templates
{
    public class TemplateResolverTests
    {
        [Scenario]
        public void Template_resolver_build_template_correctly(MockContainer<TemplateResolver> templateResolver, ITemplate buildTemplate)
        {
            "Given I have a template resolver"
                ._(() => templateResolver = B.AutoMock<TemplateResolver>());

            "And default configuration"
                ._(() => DefaultConfigurationUtility.PostInit(templateResolver.GetMock<IConfiguration>()));

            "And a set of templates"
                ._(() => TemplateUtility.Defaults(templateResolver.Subject));

            "When I call resolve"
                ._(() => buildTemplate = templateResolver.Subject.Resolve(TemplateType.Build));

            "Then I get a build template back"
                ._(() => buildTemplate.Should().BeAssignableTo<IBuildTemplate>());
        }

        [Scenario]
        public void Template_resolver_visual_studio_template_correctly(MockContainer<TemplateResolver> templateResolver, ITemplate buildTemplate)
        {
            "Given I have a template resolver"
                ._(() => templateResolver = B.AutoMock<TemplateResolver>());

            "And default configuration"
                ._(() => DefaultConfigurationUtility.PostInit(templateResolver.GetMock<IConfiguration>()));

            "And a set of templates"
                ._(() => TemplateUtility.Defaults(templateResolver.Subject));

            "When I call resolve"
                ._(() => buildTemplate = templateResolver.Subject.Resolve(TemplateType.Source));

            "Then I get a build template back"
                ._(() => buildTemplate.Should().BeOfType<VisualStudioTemplate>());
        }
    }
}