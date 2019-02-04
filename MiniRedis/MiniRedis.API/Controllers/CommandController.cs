using Microsoft.AspNetCore.Mvc;
using MiniRedis.Core.Processor.Interfaces;
using MiniRedis.Core.Storage.Interfaces;
using System.Linq;

namespace MiniRedis.API.Controllers
{
    [Route("")]
    [Controller]
    public class CommandController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult<string> RunCommand(
            [FromServices] ICommandResolver commandResolver,
            [FromServices] IDatabase database,
            [FromQuery] string cmd)
        {
            var commandResult = commandResolver.ResolveCommand(cmd);

            if (!commandResult.IsValid)
                return $"ERR {commandResult.Errors.FirstOrDefault()?.Message}";

            var evalResult = commandResult.Command.Evaluate(database, commandResult.Arguments);

            if(!evalResult.IsValid)
                return $"ERR {evalResult.Errors.FirstOrDefault()?.Message}";

            return evalResult.ToString();
        }
    }
}
