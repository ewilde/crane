namespace Crane.Core.Runners
{
    public class Nuget : INuget
    {
        public RunResult Publish()
        {
            var result = GeneralProcessRunner.Run("nuget", "");
            return null;
        }
    }
}