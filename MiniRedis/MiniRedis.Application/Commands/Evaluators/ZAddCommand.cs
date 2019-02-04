using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Enums;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class ZAddCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "ZADD";

        public override string SyntaxPattern => @"^ZADD\s*?(?<Key>[^\s]*)?\s*?(?<Score>[^\s]*)?\s*?(?<Member>[^\s]*)?$";

        public override string[] ExpectedArgs => new[] { "Key", "Score", "Member" };

        private object lockObject = new object();

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]) ||
                !args.ContainsKey("Score") || string.IsNullOrWhiteSpace(args["Score"]) ||
                !args.ContainsKey("Member") || string.IsNullOrWhiteSpace(args["Member"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'zadd' command");

            if(!float.TryParse(args["Score"], out float score))
                return new GenericResult().Invalid().WithError("ERR value is not a valid float");

            return base.ValidateArguments(args);
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var member = args["Member"]?.Trim();
            var score = float.Parse(args["Score"]?.Replace(".", ","));
            
            GenericResult result;
            ScoredCollection collection = null;
            lock (lockObject)
            {
                var itemResult = database.Get(key);
                if (itemResult.IsValid)
                {
                    if (itemResult.Data.Type != DatabaseValueType.ScoredCollection)
                        return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");

                    collection = itemResult.Data.Value as ScoredCollection;
                }

                collection = collection ?? new ScoredCollection();
                collection.Collection[member] = score;
                collection.UpdateSortedCollection();

                result = database.Save(key, new DatabaseValue(collection));
            }

            return new EvaluationResult(result.IsValid ? 1 : 0);
        }
    }
}
