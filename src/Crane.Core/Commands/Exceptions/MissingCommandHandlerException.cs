namespace Crane.Core.Commands.Exceptions
{
    public class MissingCommandHandlerException : CraneException
    {
        public MissingCommandHandlerException(string message) : base(message)
        {
        }
    }
}