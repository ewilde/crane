namespace Crane.Core.Commands.Exceptions
{
    public class MissingArgumentCraneException : CraneException
    {
        
        public MissingArgumentCraneException(string message)
            : base(message)
        {
            
        }
    }
}