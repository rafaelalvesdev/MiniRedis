using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Storage.Interfaces;

namespace MiniRedis.Core.Commands.Evaluators
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
