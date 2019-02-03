using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Enums;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Linq;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class IncrCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^INCR\s*(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().Invalid();

            return new GenericResult().Valid();
        }

        public override GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"];
            var result = database.Get(key);

            if (!result.IsValid)
                return result;
            
            if(!long.TryParse(Convert.ToString(result.Data.Value), out long number))
                return new GenericResult<DatabaseValue>().WithError("value is not an integer or out of range");

            var newValue = new DatabaseValue(DatabaseValueType.Plain, ++number, result.Data.TTL);
            var saveResult = database.Save(key, newValue);

            if (saveResult.IsValid)
                return new GenericResult<DatabaseValue>(newValue).Valid();
            else
                return new GenericResult<DatabaseValue>().Invalid();
        }
    }
}
