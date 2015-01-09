namespace Crane.Integration.Tests
{
    public class ScenarioRunOnlyOnMonoAttribute : Xbehave.ScenarioAttribute
    {
        public ScenarioRunOnlyOnMonoAttribute()
            : this("Only run on mono")
        {

        }

        public ScenarioRunOnlyOnMonoAttribute(string reason)
        {
            if (!TestEnvironment.IsRunningOnMono())
            {
                Skip = reason;
            }
        }


    }
}