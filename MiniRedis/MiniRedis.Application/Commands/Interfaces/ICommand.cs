using MiniRedis.Common.Model;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Interfaces
{
    public interface ICommand
    {
        string SyntaxPattern { get; }

        string[] ExpectedArgs { get; }

        bool SyntaxMatchesCommandLine(string commandLine);

        CommandArguments GetArguments(string commandLine);

        GenericResult ValidateArguments(CommandArguments args);

        GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args);
    }
}
