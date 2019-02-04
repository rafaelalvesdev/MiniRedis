using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class DbSizeCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "DBSIZE";

        public override string SyntaxPattern => @"^(DBSIZE)$";


        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var result = database.GetKeysCount();
            return new EvaluationResult(result.Data);
        }
    }
}
