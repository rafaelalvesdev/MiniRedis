using MiniRedis.Common.Model;
using MiniRedis.Core.Storage;
using MiniRedis.Core.Storage.Interfaces;

namespace MiniRedis.Core.Commands.Interfaces
{
    public interface ICommand
    {
        string CommandName { get; }

        string SyntaxPattern { get; }

        string[] ExpectedArgs { get; }

        bool SyntaxMatchesCommandLine(string commandLine);

        CommandArguments GetArguments(string commandLine);

        GenericResult ValidateArguments(CommandArguments args);

        EvaluationResult Evaluate(IDatabase database, CommandArguments args);
    }
}
