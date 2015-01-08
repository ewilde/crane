using System.Xml.Linq;
using Crane.Core.Extensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Extensions
{
    public class XDocumentExtensionsTests
    {
        [Scenario]
        public void inner_xml_should_return_inner_xml_of_xelement(XElement element, string result)
        {
            "Given I have an xelement with child nodes"
                ._(() => element = XElement.Parse("<Summary>This <code>item</code></Summary>"));

            "When I call inner xml"
                ._(() => result = element.InnerXml());

            "It should return the correct inner xml"
                ._(() => result.Should().Be("This <code>item</code>"));
        }
    }
}