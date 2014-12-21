namespace Crane.Core.Commands.Arguments
{
    public class CommandArgument : ICommandArgument
    {
        public string Name { get; set; }
        public bool Required { get; set; }
    }
}