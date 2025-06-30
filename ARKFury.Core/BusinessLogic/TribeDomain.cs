using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.RCON.ARK;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface ITribeDomain
    {
        Task<List<PlayerDTO>> Players(ServerDTO server, string tribeId);
    }

    public class TribeDomain : BaseDomain, ITribeDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IRCONService _rConService;

        public TribeDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            IRCONService rConService,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elastic;
            _rConService = rConService;
        }

        public async Task<List<PlayerDTO>> Players(ServerDTO server, string tribeId)
        { 
            var tribePlayers = await _rConService.ExecuteAsync(new RCONRequest
            {
                Command = string.Format(RCONCommands.GET_TRIBE_ID_PLAYER_LIST, tribeId),
                Password = server.RConPassword,
                Port = server.RConnPort,
                Server = server.IPAddress
            });

            var result = new List<PlayerDTO>();
            return result;
        }
    }
}
