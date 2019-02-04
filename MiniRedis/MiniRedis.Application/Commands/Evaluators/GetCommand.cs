using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;
using System;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class GetCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^GET\s*(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().Invalid();

            return new GenericResult().Valid();
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"];
            var value = database.Get(key);

            if (!value.IsValid)
                return new EvaluationResult((string)null);

            if (value.Data.Type != Storage.Enums.DatabaseValueType.Plain)
                return new EvaluationResult().WithError("WRONGTYPE Operation against a key holding the wrong kind of value");
            
            return new EvaluationResult(Convert.ToString(value.Data));
        }
    }
}
