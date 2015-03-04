namespace Crane.Core.Runners
{
    public class Nuget : INuget
    {
        public RunResult Publish(string packagePath, string source, string apiKey)
        {
            var result = GeneralProcessRunner.Run("nuget", string.Format("push -Source {0} -ApiKey {1}", source, apiKey));
            return result;
        }
    }
}