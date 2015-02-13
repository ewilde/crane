using System;
using Crane.Core.Runners;

namespace Crane.Core.Api.Readers
{
    public class GitSourceControlInformationReader : ISourceControlInformationReader
    {
        public ISourceControlInformation ReadSourceControlInformation(ISolutionContext solutionContext)
        {
            try
            {
                var git = new Git();
                var result = git.Run("log --oneline -1", solutionContext.Path);

                if (result.ExitCode != 0)
                    return null;

                var message = result.StandardOutput;

                return new GitSourceControlInformation(message);
            }
            catch 
            {
                return null;
            }
        }
    }
}