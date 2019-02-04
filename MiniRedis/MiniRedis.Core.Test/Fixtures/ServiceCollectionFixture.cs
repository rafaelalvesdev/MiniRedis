using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Core.Commands.Evaluators;
using MiniRedis.Core.Commands.Interfaces;
using MiniRedis.Core.Commands.Library;
using MiniRedis.Core.Commands.Processor;
using MiniRedis.Core.Processor.Interfaces;
using MiniRedis.Core.Storage.InMemory;
using MiniRedis.Core.Storage.Interfaces;
using System;


namespace MiniRedis.Core.Test.Fixtures
{
    public class ServiceCollectionFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get;  }

        public ServiceCollectionFixture()
        {
            var services = new ServiceCollection();

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

            services.AddSingleton<IDatabase>(provider => {
                return new InMemoryDatabase(
                    new MemoryCache(
                        new MemoryCacheOptions()));
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        { }
    }
}
