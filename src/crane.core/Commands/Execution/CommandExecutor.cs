﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crane.Core.Commands.Resolvers;
using Crane.Core.IO;

namespace Crane.Core.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IEnumerable<ICraneCommand> _commands;
        private readonly ICommandResolver _commandResolver;
        private readonly ICommandMethodResolver _commandMethodResolver;
        private readonly IDidYouMeanExecutor _didYouMeanExecutor;
        private readonly IOutput _output;

        public CommandExecutor(IEnumerable<ICraneCommand> commands, 
                               ICommandResolver commandResolver, 
                               ICommandMethodResolver commandMethodResolver, 
                               IDidYouMeanExecutor didYouMeanExecutor, IOutput output)
        {
            _commands = commands;
            _commandResolver = commandResolver;
            _commandMethodResolver = commandMethodResolver;
            _didYouMeanExecutor = didYouMeanExecutor;
            _output = output;
        }


        public int ExecuteCommand(string[] arguments)
        {
            var command = _commandResolver.Resolve(_commands, arguments[0]);
            var methodArgs = arguments.Skip(1).ToArray();
            var method = _commandMethodResolver.Resolve(command, methodArgs);

            if (method != null)
            {
                try
                {
                    method.Invoke(command, methodArgs.Cast<object>().ToArray());
                    _output.WriteSuccess("{0} success.", command.Name);
                }
                catch(TargetInvocationException targetInvocationException)
                {
                    var message = targetInvocationException.InnerException != null
                        ? targetInvocationException.InnerException.Message
                        : targetInvocationException.Message;

                    _output.WriteError("error: {0} ", message);
                    return -1;
                }
            }
            else
            {
                _didYouMeanExecutor.PrintHelp(command, arguments);    
            }

            return 0;
        }
    }
}