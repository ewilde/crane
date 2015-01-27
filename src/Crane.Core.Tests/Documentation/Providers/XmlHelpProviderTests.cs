using Crane.Core.Commands;
using Crane.Core.Configuration;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Providers;
using Crane.Core.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation.Providers
{
    public class XmlHelpProviderTests
    {
        [Scenario]
        public void xml_help_provider_returns_crane_help_collection(IHelpProvider xmlProvider, ICommandHelpCollection helpCollection)
        {
            "Given I have an instance of the xml help provider"
                ._(() => xmlProvider = ioc.Resolve<XmlHelpProvider>());

            "It should return a help collection"
                ._(() =>
                {
                    helpCollection = xmlProvider.HelpCollection;
                    helpCollection.Should().NotBeNull();
                });

            "It should contain help for the crane commands"
                ._(() => helpCollection.Get("init").Should().NotBeNull());
        }
    }
}