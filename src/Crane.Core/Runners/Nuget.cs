namespace Crane.Core.Runners
{
    public class Nuget : INuget
    {
        public RunResult Publish(string packagePath, string source, string apiKey)
        {
            var result = GeneralProcessRunner.Run("nuget", string.Format("push {0} -Source {1} -ApiKey {2}", packagePath, source, apiKey));
            return result;
        }
    }
}