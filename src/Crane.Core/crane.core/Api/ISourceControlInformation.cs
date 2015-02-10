namespace Crane.Core.Api
{
    public interface ISourceControlInformation
    {
        string ProviderName { get; }
        string LastCommitMessage { get; }
    }
}