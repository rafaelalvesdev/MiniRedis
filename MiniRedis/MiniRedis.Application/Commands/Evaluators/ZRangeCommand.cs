using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;
using System.Linq;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class ZRangeCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^ZRANGE\s*?(?<Key>[^\s]*)?\s*?(?<Start>[^\s]*)?\s*?(?<Stop>[^\s]*)?$";

        public override string[] ExpectedArgs => new[] { "Key", "Start", "Stop" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]) ||
                !args.ContainsKey("Start") || string.IsNullOrWhiteSpace(args["Start"]) ||
                !args.ContainsKey("Stop") || string.IsNullOrWhiteSpace(args["Stop"]))
                return new GenericResult().Invalid();

            if (!int.TryParse(args["Start"], out int start))
                return new GenericResult().Invalid().WithError("ERR value is not an integer or out of range");

            if (!int.TryParse(args["Stop"], out int stop))
                return new GenericResult().Invalid().WithError("ERR value is not an integer or out of range");

            return new GenericResult().Valid();
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();
            var start = int.Parse(args["Start"]?.Trim());
            var stop = int.Parse(args["Stop"]?.Trim());
            var item = database.Get(key);

            if (!item.IsValid)
                return new EvaluationResult((string)null);

            if (item.Data.Type != Storage.Enums.DatabaseValueType.ScoredCollection)
                return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");
                       
            var list = (item.Data.Value as ScoredCollection)?.SortedList;

            start = start < 0 ? list.Count + start : start;
            stop = stop < 0 ? list.Count : stop < start ? 0 : stop;

            list = list.Take(stop).Skip(start).ToList();

            return new EvaluationResult(list.Aggregate((a, b) => $"{a}\n{b}"));
        }
    }
}
