using MiniRedis.Common.Extensions;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Processor;
using MiniRedis.Services.Processor.Interfaces;
using System.Collections.Generic;

namespace MiniRedis.Services.Commands.Processor
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

            foreach (var command in commands)
                if(command.SyntaxMatchesCommandLine(commandLine))
                {
                    return new CommandResolverResult()
                    {
                        Command = command,
                        Arguments = command.GetArguments(commandLine),
                    };
                }

            return new CommandResolverResult().WithError("Invalid command.");
        }
    }
}
