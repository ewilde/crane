namespace Crane.Core.Commands.Exceptions
{
    public class DirectoryExistsCraneException : CraneException
    {
        public DirectoryExistsCraneException(string directoryName)
            : base(string.Format("directory {0} already exists", directoryName))
        {
            
        }
    }
}