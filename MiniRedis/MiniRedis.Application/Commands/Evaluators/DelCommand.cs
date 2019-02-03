using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services.Commands.Evaluators
{
    public class DelCommand : AbstractCommand, ICommand
    {
        public override string SyntaxPattern => @"^DEL\s*(?<Key>[^$]*)?$";

        public override string[] ExpectedArgs => new[] { "Key" };

        public override GenericResult ValidateArguments(CommandArguments args)
        {
            if (!args.ContainsKey("Key") || string.IsNullOrWhiteSpace(args["Key"]))
                return new GenericResult().Invalid();

            return new GenericResult().Valid();
        }

        public override GenericResult<DatabaseValue> Evaluate(IDatabase database, CommandArguments args)
        {
            var key = args["Key"];            
            var result = database.Remove(key);

            return new GenericResult<DatabaseValue>().Valid().WithMessage(!result.IsValid ? "0" : "1");
        }
    }
}
