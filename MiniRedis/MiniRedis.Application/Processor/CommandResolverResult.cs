using MiniRedis.Common.Model;
using MiniRedis.Services.Commands;
using MiniRedis.Services.Commands.Interfaces;

namespace MiniRedis.Services.Processor
{
    public class CommandResolverResult : GenericResult
    {
        public CommandArguments Arguments { get; set; }

        public ICommand Command { get; set; }
    }
}
