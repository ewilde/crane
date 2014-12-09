namespace Crane.Core.Commands.Handlers
{
    public abstract class CommandHandler<TCommand> : ICommandHandler where TCommand : class, ICraneCommand
    {
        public bool CanHandle(ICraneCommand command)
        {
            return command is TCommand;
        }

        public void Handle(ICraneCommand craneCommand)
        {
            var command = craneCommand as TCommand;
            DoHandle(command);
        }

        protected abstract void DoHandle(TCommand command);
    }
}