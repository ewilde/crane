namespace Crane.Core.Api
{
    public class GitSourceControlInformation : ISourceControlInformation
    {

        public GitSourceControlInformation(string lastCommitMessage)
        {
            LastCommitMessage = lastCommitMessage;
        }

        public string ProviderName { get { return "git"; } }
        public string LastCommitMessage { get; private set; }
    }
}