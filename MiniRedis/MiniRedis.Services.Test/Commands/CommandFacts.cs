using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Common.Extensions;
using MiniRedis.Services.Commands;
using MiniRedis.Services.Commands.Enums;
using MiniRedis.Services.Processor.Interfaces;
using MiniRedis.Services.Storage.Interfaces;
using MiniRedis.Services.Test.Fixtures;
using System.Linq;
using System.Threading;
using Xunit;

namespace MiniRedis.Services.Test.Commands
{
    [TestCaseOrderer("MiniRedis.Services.Test.PriorityOrderer", "MiniRedis.Services.Test")]
    public class CommandFacts : IClassFixture<ServiceCollectionFixture>
    {
        ServiceCollectionFixture fixture;
        ICommandResolver commandResolver;
        IDatabase database;

        public CommandFacts(ServiceCollectionFixture fixture)
        {
            this.fixture = fixture;
            commandResolver = fixture.ServiceProvider.GetService<ICommandResolver>();
            database = fixture.ServiceProvider.GetService<IDatabase>();
        }

        private EvaluationResult RunCommand(string commandLine)
        {
            var commandResult = commandResolver.ResolveCommand(commandLine);

            if (!commandResult.IsValid)
                return new EvaluationResult().WithError(commandResult.Errors.FirstOrDefault()?.Message);

            return commandResult.Command.Evaluate(database, commandResult.Arguments);
        }


        [Fact, TestPriority(0)]
        public void NullGetCommand()
        {
            var commandLine = "GET Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().BeNull();
        }

        [Fact, TestPriority(0)]
        public void EmptyDelCommand()
        {
            var commandLine = "DEL Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(0);
        }


        [Fact, TestPriority(1)]
        public void SimpleTestSetCommand()
        {
            var commandLine = "SET Abc 123";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().Be("OK");
        }

        [Fact, TestPriority(2)]
        public void SimpleTestGetCommand()
        {
            var commandLine = "GET Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().Be("123");
        }

        [Fact, TestPriority(3)]
        public void SimpleTestDelCommand()
        {
            var commandLine = "DEL Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }


        [Fact, TestPriority(4)]
        public void DelayedTestSetCommand()
        {
            var commandLine = "SET Abc 123 EX 2";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().Be("OK");
        }

        [Fact, TestPriority(5)]
        public void TestDbSizeCommand()
        {
            var commandLine = "DBSIZE";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(6)]
        public void DelayedTestGetCommand()
        {
            Thread.Sleep(2000);
            var commandLine = "GET Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().BeNull();
        }

        [Fact, TestPriority(7)]
        public void TestIncrCommand()
        {
            var commandLine = "INCR Abcd";

            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(8)]
        public void TestGetIncrCommand()
        {
            var commandLine = "GET Abcd";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.String);
            result.String.Should().Be("1");
        }

        [Fact, TestPriority(9)]
        public void TestZAddCommand()
        {
            var commandLine = "ZADD TstKey 10 TstMember";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(10)]
        public void TestZAddCommand2()
        {
            var commandLine = "ZADD TstKey 15.4 TstMember2";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(11)]
        public void TestZAddCommand3()
        {
            var commandLine = "ZADD TstKey 10 Abc";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(12)]
        public void TestZCardCommand()
        {
            var commandLine = "ZCARD TstKey";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(3);
        }

        [Fact, TestPriority(13)]
        public void TestZRankCommand()
        {
            var commandLine = "ZRANK TstKey TstMember";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.Number);
            result.Number.Should().Be(1);
        }

        [Fact, TestPriority(14)]
        public void TestZRangeCommand()
        {
            var commandLine = "ZRANGE TstKey 0 10";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.StringArray);
            result.StringArray.Should().NotBeNullOrEmpty();
            result.StringArray.Should().Contain("Abc");
            result.StringArray.Should().Contain("TstMember");
            result.StringArray.Should().Contain("TstMember2");
        }

        [Fact, TestPriority(15)]
        public void TestZRangeCommand2()
        {
            var commandLine = "ZRANGE TstKey -2 10";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.StringArray);
            result.StringArray.Should().NotBeNullOrEmpty();
            result.StringArray.Should().Contain("TstMember");
            result.StringArray.Should().Contain("TstMember2");
        }

        [Fact, TestPriority(16)]
        public void TestZRangeCommand3()
        {
            var commandLine = "ZRANGE TstKey -2 -1";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.StringArray);
            result.StringArray.Should().NotBeNullOrEmpty();
            result.StringArray.Should().Contain("TstMember");
            result.StringArray.Should().Contain("TstMember2");
        }

        [Fact, TestPriority(17)]
        public void TestZRangeCommand4()
        {
            var commandLine = "ZRANGE TstKey -1 -1";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.StringArray);
            result.StringArray.Should().NotBeNullOrEmpty();
            result.StringArray.Should().Contain("TstMember2");
        }

        [Fact, TestPriority(18)]
        public void TestZRangeCommand5()
        {
            var commandLine = "ZRANGE TstKey 0 0";
            var result = RunCommand(commandLine);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.ValueType.Should().Be(ResultValueType.StringArray);
            result.StringArray.Should().BeNullOrEmpty();
        }
    }
}