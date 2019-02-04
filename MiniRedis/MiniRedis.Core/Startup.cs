using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Core.Commands.Evaluators;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Commands.Library;
using MiniRedis.Core.Commands.Processor;
using MiniRedis.Core.Processor.Interfaces;
using MiniRedis.Core.Storage.InMemory;
using MiniRedis.Core.Storage.Interfaces;

namespace MiniRedis.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICommandLibrary, CommandLibrary>(provider =>
            {
                var library = new CommandLibrary();

                library.RegisterCommand<SetCommand>();
                library.RegisterCommand<GetCommand>();
                library.RegisterCommand<DelCommand>();
                library.RegisterCommand<DbSizeCommand>();
                library.RegisterCommand<IncrCommand>();
                library.RegisterCommand<ZAddCommand>();
                library.RegisterCommand<ZCardCommand>();
                library.RegisterCommand<ZRankCommand>();
                library.RegisterCommand<ZRangeCommand>();

                return library;
            });

            services.AddSingleton<ICommandResolver, CommandResolver>();

            services.AddSingleton<IDatabase, InMemoryDatabase>();
        }
    }
}
