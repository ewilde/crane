using Crane.PowerShell;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.PowerShell
{
    public class GetCraneSolutionContextFeatures
    {
        [Scenario]
        public void creating_an_instance_of_the_cmdlet(GetCraneSolutionContext cmdlet)
        {
            "When I have an instance of Get-CraneSolutionContext cmdlet"
                ._(() => cmdlet = new GetCraneSolutionContext());

            "It should have it's dependecies injected"
                ._(() => cmdlet.Api.Should().NotBeNull());
        }
    }
}