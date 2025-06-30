using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Extensions;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServerController : BaseController
    {
        private readonly IServerDomain _serverDomain;

        public ServerController(IServerDomain ServerDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<ServerController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)ServerDomain, configuration, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _serverDomain = ServerDomain;
        }

        [HttpGet("Names")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IEnumerable<string>> Names()
        {
            var servers = await _serverDomain.All();
            return servers.Data.Select(d => d.NormalizedName);
        }

        [HttpPost("All")]
        [ProducesResponseType(typeof(IEnumerable<ServerDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<ServerDTO>>> All(PagingRequest request)
        {
            var servers = await _serverDomain.All();

            var result = servers.Data.SortBy(request)
                                             .Take(request.Size)
                                             .Skip(request.From)
                                             .ToList();

            return GetCustomResponse(result, request.Fields);
        }


        [HttpPost("Add")]
        [ProducesResponseType(typeof(IEnumerable<ServerDTO>), 200)]
        public async Task<ActionResult<ServerDTO>> Add(ServerDTO request)
        {
            //test if account can select player list.
            var validConnection = await ((IBaseDomain)_serverDomain).CheckMySqlConnection(request.MySqlConectionString);
            if (validConnection)
            {
                //write new config file with active server information listed above.
                var result = _serverDomain.AddUpdate(request);
                return GetCustomResponse(request, request.Fields);
                //this should acause the app to restart automatically, not sure if we'll get to the response...
            }

            return GetCustomResponse(request, request.Fields);
        }
    }
}