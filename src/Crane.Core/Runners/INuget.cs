namespace Crane.Core.Runners
{
    public interface INuget
    {
        RunResult Publish(string packagePath,string source, string apiKey);
    }
}