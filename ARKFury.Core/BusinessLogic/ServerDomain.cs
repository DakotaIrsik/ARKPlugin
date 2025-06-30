using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IServerDomain
    {
        Task<AdjustableDTO<ServerDTO>> All();
        Task Shutdown(string server);
        Task<string> Broadcast(string message, string serverName);
        ServerDTO AddUpdate(ServerDTO server);
    }

    public class ServerDomain : BaseDomain, IServerDomain
    {
        private IRCONService _rconService;
        public ServerDomain(ArkShopContext context, IMapper mapper,
           IElasticSearchService elastic,
           ILogger logger,
           IHttpContextAccessor httpContextAccessor,
           IOptions<AppSettings> settings,
           IRCONService rconService,
           ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _rconService = rconService;
        }

        public async Task<AdjustableDTO<ServerDTO>> All()
        {
           return await _elastic.Search<ServerDTO>(new PagingRequest(true), "server");
        }

        public ServerDTO AddUpdate(ServerDTO server)
        {
            var result = _elastic.Index(server, "server");
            return server;
        }

        public async Task<string> Broadcast(string message, string serverName)
        {
            var servers = await All();
            var server = servers.Data.FirstOrDefault(d => d.NormalizedName == serverName);
            var request = new RCONRequest();
            request.Command = $"broadcast {message}";
            request.Server = server.IPAddress;
            request.Port = server.RConnPort;
            request.Password = server.RConPassword;
            return await _rconService.ExecuteAsync(request);
        }

        public async Task Shutdown(string serverName)
        {
            var servers = await All();
            var server = servers.Data.FirstOrDefault(d => d.NormalizedName == serverName);
            var request = new RCONRequest();
            request.Command = $"broadcast Server will restart in 15 minutes.The restart will take around 15 minutes.";
            request.Server = server.IPAddress;
            request.Port = server.RConnPort;
            request.Password = server.RConPassword;

            await _rconService.ExecuteAsync(request);
            await Task.Delay(300000);

            request.Command = $"broadcast Server will restart in 10 minutes.The restart will take around 15 minutes.";
            await _rconService.ExecuteAsync(request);
            await Task.Delay(300000);

            request.Command = $"broadcast Server will restart in 5 minutes.The restart will take around 15 minutes.";
            await _rconService.ExecuteAsync(request);
            await Task.Delay(300000);

            await _rconService.ExecuteAsync(request);
            request.Command = $"doShutdown .";
            await _rconService.ExecuteAsync(request);
        }
    }
}
