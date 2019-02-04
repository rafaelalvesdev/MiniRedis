using MiniRedis.Common.Extensions;
using MiniRedis.Common.Model;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Storage;
using MiniRedis.Services.Storage.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiniRedis.Services.Commands.Evaluators
{
    public abstract class AbstractCommand : ICommand
    {
        public abstract string SyntaxPattern { get; }
        public virtual string[] ExpectedArgs => new string[0];

        private Regex syntaxRegex;

        public AbstractCommand()
        {
            if (string.IsNullOrWhiteSpace(SyntaxPattern))
                throw new Exception("Síntaxe de comando indefinida.");

            syntaxRegex = new Regex(SyntaxPattern);
        }

        public bool SyntaxMatchesCommandLine(string commandLine)
        {
            return syntaxRegex.IsMatch(commandLine);
        }

        public virtual CommandArguments GetArguments(string commandLine)
        {
            var match = syntaxRegex.Match(commandLine);

            var args = new CommandArguments();
            foreach (var group in match.Groups.Where(x => ExpectedArgs.Contains(x.Name)))
                args.Add(group.Name, group.Value);

            return args;
        }

        public virtual GenericResult ValidateArguments(CommandArguments args) => new GenericResult().Valid();

        public abstract EvaluationResult Evaluate(IDatabase database, CommandArguments args);
    }
}
