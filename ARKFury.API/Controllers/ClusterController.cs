using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClusterController : BaseController
    {
        private readonly IServerDomain _serverDomain;
        private readonly IRCONService _rconService;
        public ClusterController(IServerDomain serverDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<ClusterController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                IRCONService rconService,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)serverDomain, configuration, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _serverDomain = serverDomain;
            _rconService = rconService;
        }

        [HttpPost("Broadcast")]
        public async Task<ActionResult<List<string>>> Broadcast(int cluster, string message)
        {
            var responses = new List<string>();
            foreach (var server in await AllServers(cluster))
            {
                responses.Add(await _serverDomain.Broadcast(message, server.NormalizedName));
            }
            return GetCustomResponse(responses);
        }

        [HttpPost("RConn")]
        public async Task<ActionResult> RConn(int cluster, string command)
        {
            var responses = new List<string>();
            foreach (var server in await AllServers(cluster))
            {
                var request = new RCONRequest();
                request.Command = command;
                request.Server = server.IPAddress;
                request.Port = server.RConnPort;
                request.Password = server.RConPassword;

                responses.Add(await _rconService.ExecuteAsync(request));
            }
            return GetCustomResponse(responses);
        }

        private async Task<List<ServerDTO>> AllServers(int clusterNumber)
        {
            var servers = await _serverDomain.All();
            return servers.Data.Where(s => s.Cluster == clusterNumber).ToList();
        }                                                                                          
    }
}