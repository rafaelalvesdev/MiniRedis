using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Storage;
using MiniRedis.Core.Storage.Interfaces;
using System;

namespace MiniRedis.Core.Commands.Evaluators
{
    public class GetCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "GET";

        public override string SyntaxPattern => @"^GET\s*(?<Key>[^\s|$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'get' command");

            return base.ValidateArguments(args);
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"];
            var value = database.Get(key);

            if (!value.IsValid)
                return new EvaluationResult((string)null);

            if (value.Data.Type != Storage.Enums.DatabaseValueType.Plain)
                return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");

            return new EvaluationResult(Convert.ToString(value.Data.Value));
        }
    }
}
