using MiniRedis.Services.Commands.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MiniRedis.Services.Commands.Library
{
    public class CommandLibrary : ICommandLibrary
    {
        private List<ICommand> _commands { get; set; } = new List<ICommand>();

        public IReadOnlyList<ICommand> GetRegisteredCommands() => _commands.ToList();

        public void RegisterCommand<TCommand>()
            where TCommand : ICommand, new()
                => _commands.Add(new TCommand());
    }
}