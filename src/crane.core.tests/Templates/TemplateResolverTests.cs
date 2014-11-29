using Crane.Core.Configuration;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using Crane.Core.Tests.TestExtensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Templates
{
    public class TemplateResolverTests
    {
        [Scenario]
        public void Template_resolver_build_template_correctly_using_configuration(TemplateResolver templateResolver, IConfiguration configuration, ITemplate buildTemplate)
        {
            "Given I have a template resolver"
                ._(() => templateResolver = a.New<TemplateResolver>());

            "And default configuration"
                ._(() => configuration = a.New<DefaultConfiguration>());

            "When I call resolve"
                ._(() => buildTemplate = templateResolver.Resolve(TemplateType.Build));

            "Then I get a build template back"
                ._(() => buildTemplate.Should().BeAssignableTo<IBuildTemplate>());
        }
    }
}