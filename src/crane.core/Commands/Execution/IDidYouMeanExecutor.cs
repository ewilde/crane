namespace Crane.Core.Commands.Execution
{
    public interface IDidYouMeanExecutor
    {
        void PrintHelp(ICraneCommand command, string[] arguments);
    }
}