using MiniRedis.Common.Extensions;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Processor;
using MiniRedis.Core.Processor.Interfaces;
using System;
using System.Collections.Generic;

namespace MiniRedis.Core.Commands.Processor
{
    public class CommandResolver : ICommandResolver
    {
        private IReadOnlyList<ICommand> commands { get; set; }

        public CommandResolver(ICommandLibrary commandLibrary)
        {
            commands = commandLibrary.GetRegisteredCommands();
        }

        public CommandResolverResult ResolveCommand(string commandLine)
        {
            commandLine = commandLine?.Trim();
            var commandName = commandLine.Truncate(" ");

            foreach (var command in commands)
            {
                if (command.SyntaxMatchesCommandLine(commandLine))
                {
                    var args = command.GetArguments(commandLine);
                    var argsValidation = command.ValidateArguments(args);
                    if (!argsValidation.IsValid)
                    {
                        return new CommandResolverResult().Valid().Merge(argsValidation);
                    }

                    return new CommandResolverResult()
                    {
                        Command = command,
                        Arguments = args,
                    };
                }

                if (!string.IsNullOrWhiteSpace(command.CommandName) && command.CommandName.Equals(commandName, StringComparison.InvariantCultureIgnoreCase))
                    return new CommandResolverResult().WithError($"ERR wrong number of arguments for '{commandName?.ToLower()}' command");

            }

            return new CommandResolverResult().WithError($"Unknown or disabled command '{commandName}'");
        }
    }
}