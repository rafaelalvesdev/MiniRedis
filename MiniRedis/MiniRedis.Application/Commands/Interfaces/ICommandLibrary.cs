using System.Collections.Generic;

namespace MiniRedis.Services.Commands.Interfaces
{
    public interface ICommandLibrary
    {
        IReadOnlyList<ICommand> GetRegisteredCommands();

        void RegisterCommand<TCommand>()
            where TCommand : ICommand, new();
    }
}
