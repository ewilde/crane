namespace Crane.Integration.Tests
{
	public class ScenarioIgnoreOnMonoAttribute : Xbehave.ScenarioAttribute
	{
		public ScenarioIgnoreOnMonoAttribute() : this("Ignored on Mono")
		{

		}

		public ScenarioIgnoreOnMonoAttribute(string reason)
		{
			if(TestEnvironment.IsRunningOnMono()) {
				Skip = reason;
			}
		}

		
	}
}

