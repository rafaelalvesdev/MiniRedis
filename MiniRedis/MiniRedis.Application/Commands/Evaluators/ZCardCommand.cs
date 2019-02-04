using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class ZCardCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "ZCARD";

        public override string SyntaxPattern => @"^ZCARD\s*?(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'zcard' command");

            return new GenericResult().Valid();
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var item = database.Get(key);

            if (!item.IsValid)
                return new EvaluationResult(0);

            if (item.Data.Type != Storage.Enums.DatabaseValueType.ScoredCollection)
                return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");

            var collection = item.Data.Value as ScoredCollection;
            return new EvaluationResult(collection?.Collection?.Count ?? 0);
        }
    }
}
