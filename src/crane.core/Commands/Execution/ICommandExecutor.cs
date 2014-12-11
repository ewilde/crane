namespace Crane.Core.Commands.Execution
{
    public interface ICommandExecutor
    {
        int ExecuteCommand(params string[] arguments);
    }
}
