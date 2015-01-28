using Crane.Tests.Common.Runners;

namespace Crane.Tests.Common.FluentExtensions
{
    public static class ShouldExtensions
    {    
        public static GenericAssertions<RunResult> Should(this RunResult actualValue)
        {
            return new GenericAssertions<RunResult>(actualValue);
        }
        
    }
}