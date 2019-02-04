using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class DelCommand : AbstractCommand, ICommand
    {
        public override string CommandName => "DEL";

        public override string SyntaxPattern => @"^DEL\s*?(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().WithError("ERR wrong number of arguments for 'del' command");

            return base.ValidateArguments(args);
        }

        public override EvaluationResult Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"]?.Trim();            
            var result = database.Remove(key);

            return new EvaluationResult(result.IsValid ? 1 : 0);
        }
    }
}
