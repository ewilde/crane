namespace Crane.Core.Runners
{
    public interface IChocolatey
    {
        RunResult Pack(string chocolateyExePath, string chocolateySpecPath);
    }
}