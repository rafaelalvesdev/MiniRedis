using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Services.Commands.Evaluators;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Commands.Library;
using MiniRedis.Services.Commands.Processor;
using MiniRedis.Services.Processor.Interfaces;
using MiniRedis.Services.Storage.InMemory;
using MiniRedis.Services.Storage.Interfaces;
using System;


namespace MiniRedis.Services.Test.Fixtures
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

            services.AddSingleton<IDatabase, InMemoryDatabase>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        { }
    }
}
