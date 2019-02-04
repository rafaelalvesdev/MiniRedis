using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Storage;
using MiniRedis.Core.Storage.Interfaces;

namespace MiniRedis.Core.Commands.Evaluators
{
    public class ZRankCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "ZRANK";

        public override string SyntaxPattern => @"^ZRANK\s*?(?<Key>[^\s]*)?\s*?(?<Member>[^\s|$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key", "Member" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]) ||
                !args.ContainsKey("Member") || string.IsNullOrWhiteSpace(args["Member"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'zrank' command");

            return new GenericResult().Valid();
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var member = args["Member"]?.Trim();
            var item = database.Get(key);

            if (!item.IsValid)
                return new EvaluationResult((string)null);

            if (item.Data.Type != Storage.Enums.DatabaseValueType.ScoredCollection)
                return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");

            var collection = item.Data.Value as ScoredCollection;

            var rank = collection.SortedList.IndexOf(member);

            if (rank == -1)
                return new EvaluationResult((string)null);

            return new EvaluationResult(rank);
        }
    }
}