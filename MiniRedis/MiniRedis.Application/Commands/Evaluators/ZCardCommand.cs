using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class ZCardCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^ZCARD\s*?(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().Invalid();

            return new GenericResult().Valid();
        }

        public override GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var item = database.Get(key);

            if (!item.IsValid)
                return new GenericResult<DatabaseValue>().WithMessage("0");

            if (item.Data.Type != Storage.Enums.DatabaseValueType.ScoredCollection)
                return new GenericResult<DatabaseValue>().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");

            var collection = item.Data.Value as ScoredCollection;
            return new GenericResult<DatabaseValue>().WithMessage((collection?.Collection?.Count ?? 0).ToString());
        }
    }
}
