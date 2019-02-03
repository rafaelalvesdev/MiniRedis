namespace MiniRedis.Services.Processor.Interfaces
{
    public interface ICommandResolver
    {
        CommandResolverResult ResolveCommand(string command);
    }
}
