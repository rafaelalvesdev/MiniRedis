using MiniRedis.Common.Model;
using MiniRedis.Core.Commands;
using MiniRedis.Core.Commands.Interfaces;

namespace MiniRedis.Core.Processor
{
    public class CommandResolverResult : GenericResult
    {
        public CommandArguments Arguments { get; set; }

        public ICommand Command { get; set; }
    }
}
