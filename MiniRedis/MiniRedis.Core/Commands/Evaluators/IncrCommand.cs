using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Storage;
using MiniRedis.Core.Storage.Interfaces;
using System;

namespace MiniRedis.Core.Commands.Evaluators
{
    public class IncrCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "INCR";

        public override string SyntaxPattern => @"^INCR\s*?(?<Key>[^\s|$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'incr' command");

            return base.ValidateArguments(args);
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var item = database.Get(key);

            long number = 0;
            if (item.IsValid && !long.TryParse(Convert.ToString(item.Data.Value), out number))
                return new EvaluationResult().WithError("value is not an integer or out of range");

            var newValue = new DatabaseValue((++number).ToString(), item.Data?.TTL);
            var saveResult = database.Save(key, newValue);

            if (saveResult.IsValid)
                return new EvaluationResult(number);

            return new EvaluationResult().Invalid();
        }
    }
}
