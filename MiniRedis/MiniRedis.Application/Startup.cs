using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniRedis.Services.Commands.Evaluators;
using MiniRedis.Services.Commands.Interfaces;
using MiniRedis.Services.Commands.Library;

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
        }
    }
}
