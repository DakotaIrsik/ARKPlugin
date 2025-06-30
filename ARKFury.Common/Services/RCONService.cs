using ArkFury.Common.Models;
using CoreRCON;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ArkFury.Common.Services
{
    public interface IRCONService
    {
        Task<string> ExecuteAsync(RCONRequest request);
    }
    public class RCONService : IRCONService
    {
        private readonly AppSettings _settings;
        public RCONService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> ExecuteAsync(RCONRequest request)
        {
            if (_settings.Environment == "Development")
            {
                //if debugging, change the real LAN ip's it would use as config and instead use what I know to be my dev test server.
                request.Server = "52.4.214.43"; //atl
            }

            var rcon = new RCON(IPAddress.Parse(request.Server), Convert.ToUInt16(request.Port), request.Password);
            await rcon.ConnectAsync();
            Thread.Sleep(100);
            return await rcon.SendCommandAsync(request.Command.ToLower()).ConfigureAwait(false);
        }
    }
}
