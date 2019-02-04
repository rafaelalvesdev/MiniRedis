using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage.Interfaces;
using System.Linq;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class DbSizeCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^(DBSIZE)$";

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var result = database.GetAllKeys();
            return new EvaluationResult(result.Data.LongCount());
        }
    }
}
