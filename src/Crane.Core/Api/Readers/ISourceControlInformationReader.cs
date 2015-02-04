namespace Crane.Core.Api.Readers
{
    public interface ISourceControlInformationReader
    {
        ISourceControlInformation ReadSourceControlInformation(ISolutionContext solutionContext);
    }
}
