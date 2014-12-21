using Autofac;
using Crane.Core.Configuration.Modules;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Providers;
using Crane.Core.Templates.Parsers;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Configuration.Modules
{
    public class DocumentationModuleTests
    {
        [Scenario]
        public void Xml_help_provider_is_configured_as_the_default_help_provider(IContainer container)
        {
            "Given we have bootstraped the IoC"
                ._(() => container = BootStrap.Start());

            "Then it should resolve the help provider to be an xml help provider"
                ._(() => container.Resolve<IHelpProvider>().Should().BeOfType<XmlHelpProvider>());

            "And it should be a singleton instance" // Is there a better way to verify lifecycle in Autofac?
               ._(
                   () =>
                       ReferenceEquals(container.Resolve<IHelpProvider>(),
                           container.Resolve<IHelpProvider>())
                           .Should().BeTrue());
        } 
    }
}