using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Services.Processor.Interfaces;
using MiniRedis.Services.Storage.Interfaces;
using MiniRedis.Services.Test.Fixtures;
using System.Threading;
using Xunit;

namespace MiniRedis.Services.Test.Commands
{

    [TestCaseOrderer("MiniRedis.Services.Test.PriorityOrderer", "MiniRedis.Services.Test")]
    public class CommandFacts : IClassFixture<ServiceCollectionFixture>
    {
        ServiceCollectionFixture fixture;

        public CommandFacts(ServiceCollectionFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact, TestPriority(1)]
        public void SimpleTestSetCommand()
        {
            var commandLine = "SET Abc 123";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

        [Fact, TestPriority(2)]
        public void SimpleTestGetCommand()
        {
            var commandLine = "GET Abc";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

        [Fact, TestPriority(3)]
        public void SimpleTestDelCommand()
        {
            var commandLine = "DEL Abc";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }


        [Fact, TestPriority(4)]
        public void DelayedTestSetCommand()
        {
            var commandLine = "SET Abc 123 EX 2";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

        [Fact, TestPriority(5)]
        public void DelayedTestGetCommand()
        {
            Thread.Sleep(3000);

            var commandLine = "GET Abc";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

        [Fact, TestPriority(6)]
        public void TestDbSizeCommand()
        {
            var commandLine = "DBSIZE";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }
        
        [Fact, TestPriority(7)]
        public void TestIncrSetCommand()
        {
            var commandLine = "SET Abcd 10";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

        [Fact, TestPriority(8)]
        public void TestIncrCommand()
        {
            var commandLine = "INCR Abcd";

            var resolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            var commandResult = resolver.ResolveCommand(commandLine);

            var database = fixture.ServiceProvider.GetService<IDatabase>();

            var result = commandResult.Command.Evaluate(database, commandResult.Arguments);
        }

    }
}
