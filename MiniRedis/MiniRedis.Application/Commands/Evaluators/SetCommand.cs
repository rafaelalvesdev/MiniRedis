using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Enums;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class SetCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^SET\s*(?<Key>[^\s]*)\s(?<Value>[^\s|$]*)\s?(?<Options>EX\s*[0-9]*)?$";

        public override string[] ExpectedArgs => new[] { "Key", "Value", "Options" };

        private object lockObject = new object();

        public override CommandArguments GetArguments(string commandLine)
        {
            var args = base.GetArguments(commandLine);

            long ttl = -2; // default - seconds

            if (args.ContainsKey("Options"))
            {
                var options = args["Options"];
                args.Remove("Options");

                var timeRgx = new Regex(@"EX\s*(?<Seconds>[0-9]*)");
                var timeMatch = timeRgx.Match(options);

                if (timeMatch.Success)
                {
                    var time = timeMatch.Groups?.FirstOrDefault(x => x.Name == "Seconds")?.Value;
                    if (long.TryParse(time, out long seconds))
                        ttl = seconds > 0 ? seconds : ttl;
                }
            }

            if (ttl > -2)
                args["TTL"] = DateTimeOffset.Now.AddSeconds(ttl).Ticks.ToString();

            return args;
        }

        public override GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"];
            var value = args["Value"];

            DateTimeOffset? ttl = null;
            if (args.ContainsKey("TTL"))
                if (!string.IsNullOrWhiteSpace(args["TTL"]))
                    if (long.TryParse(args["TTL"], out long ticks))
                        ttl = new DateTimeOffset(new DateTime(ticks));

            GenericResult result;
            lock (lockObject)
                result = database.Save(key, new DatabaseValue(DatabaseValueType.Plain, value, ttl));

            return new GenericResult<DatabaseValue>().Valid(result.IsValid);
        }
    }
}
