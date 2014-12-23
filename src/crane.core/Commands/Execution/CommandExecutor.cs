using System;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Factories;
using Crane.Core.Commands.Handlers.Factories;
using Crane.Core.IO;

namespace Crane.Core.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ICommandHandlerFactory _commandHandlerFactory;
        private readonly IOutput _output;

        public CommandExecutor(ICommandFactory commandFactory, ICommandHandlerFactory commandHandlerFactory, IOutput output)
        {
            _commandFactory = commandFactory;
            _commandHandlerFactory = commandHandlerFactory;
            _output = output;
        }

        public int ExecuteCommand(params string[] args)
        {
            try
            {
                var command = _commandFactory.Create(args);
                var commandHandler = _commandHandlerFactory.Create(command);
                commandHandler.Handle(command);

                if (ShowSuccess(command))
                    _output.WriteSuccess("{0} success. ", command.GetType().Name);
            }
            catch (Exception exception)
            {
                _output.WriteError("error: {0}", exception.Message);
                return -1;
            }
            return 0;
        }

        private static bool ShowSuccess(ICraneCommand command)
        {
            return !(command is ListCommands || command is Help);
        }
    }

    
}