using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IUserDomain
    {
        Task<PlayerDTO> Login(long steamId);
    }

    public class UserDomain : BaseDomain, IUserDomain
    {
        private readonly ArkShopContext _arkShopContext;
        private readonly IRCONService _rCONService;
        private readonly IPlayerDomain _playerDomain;
        private readonly IServerDomain _serverDomain;
        public UserDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache,
            IRCONService rCONService,
            IPlayerDomain playerDomain,
            IServerDomain serverDomain) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _arkShopContext = context;
            _rCONService = rCONService;
            _serverDomain = serverDomain;
            _playerDomain = playerDomain;
        }

        public async Task<PlayerDTO> Login(long steamId)
        {
            var player = await _playerDomain.GetAsync(steamId);
            player.Id = player.SteamId.ToString();
            _elastic.Index(player, "player");
            return player;
        }
    }
}
