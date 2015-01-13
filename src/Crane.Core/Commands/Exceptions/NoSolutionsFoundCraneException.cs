namespace Crane.Core.Commands.Exceptions
{
    public class NoSolutionsFoundCraneException : CraneException
    {
        public NoSolutionsFoundCraneException(string folder)
            : base(string.Format("No solutions were found in {0} you need to have one solution in order for crane to be able to create a build", folder))
        {
            
        }
    }
}