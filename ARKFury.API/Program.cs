using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using ArkFury.API.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API
{
    public static class Program
    {
        private const string Seed = "/seed";

        public static async Task Main(string[] args)
        {
            var seed = args.Any(x => x == Seed);

            if (seed)
            {
                args = args.Except(new[] { Seed }).ToArray();
            }

            var host = BuildWebHost(args);
            //seed = true;
            await DbInitializer.Seed(host, seed).ConfigureAwait(false);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIIS()
                .UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); })
                .UseStartup<Startup>()
                .Build();
    }
}
