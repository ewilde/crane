namespace Crane.Integration.Tests.TestUtilities.Extensions
{
    public static class ShouldExtensions
    {    
        public static GenericAssertions<RunResult> Should(this RunResult actualValue)
        {
            return new GenericAssertions<RunResult>(actualValue);
        }
        
    }
}