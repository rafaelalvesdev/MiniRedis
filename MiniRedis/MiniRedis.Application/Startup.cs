using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Services.Commands.Evaluators;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Commands.Library;
using MiniRedis.Services.Commands.Processor;
using MiniRedis.Services.Processor.Interfaces;
using MiniRedis.Services.Storage.InMemory;
using MiniRedis.Services.Storage.Interfaces;

namespace MiniRedis.Services
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
