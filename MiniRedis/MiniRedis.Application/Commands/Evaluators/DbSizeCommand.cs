using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Linq;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class DbSizeCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^(DBSIZE)$";

        public override GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args)
        {
            var result = database.GetAllKeys();

            return new GenericResult<DatabaseValue>()
                .WithMessage(Convert.ToString(result.Data.LongCount()))
                .Valid();
        }
    }
}
