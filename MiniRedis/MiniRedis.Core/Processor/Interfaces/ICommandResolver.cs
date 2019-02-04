namespace MiniRedis.Core.Processor.Interfaces
{
    public interface ICommandResolver
    {
        CommandResolverResult ResolveCommand(string command);
    }
}
