using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.RCON.ARK;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IUtilityDomain
    {
        Task<bool> TeleportPlayerToPlayer(long steamId, long destinationSteamId);
    }

    public class UtilityDomain : BaseDomain, IUtilityDomain
    {
        private readonly IRCONService _rCon;

        private readonly IPlayerDomain _playerDomain;

        private readonly IServerDomain _serverDomain;
            
        public UtilityDomain(ArkShopContext context,
                                IMapper mapper,
                                IElasticSearchService elastic,
                                ILogger logger,
                                IHttpContextAccessor httpContextAccessor,
                                IOptions<AppSettings> settings,
                                ICacheService cache,
                                IRCONService rCon,
                                IPlayerDomain playerDomain,
                                IServerDomain serverDomain) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _playerDomain = playerDomain;
            _serverDomain = serverDomain;
            _rCon = rCon;
        }

        public async Task<bool> TeleportPlayerToPlayer(long steamId, long destinationSteamId)
        {
            var player = await _playerDomain.GetAsync(steamId);
            var result = await _rCon.ExecuteAsync(
                new RCONRequest 
                { 
                    Command = string.Format(RCONCommands.TELEPORT_PLAYER_TO_PLAYER, 
                                            steamId, 
                                            destinationSteamId),
                    Password = player.Server.RConPassword,
                    Port = player.Server.RConnPort,
                    Server = player.Server.IPAddress
                });

            return (result != "Failure");
        }

        public async Task<bool> CallDinos(long steamId)
        {
            var player = await _playerDomain.GetAsync(steamId);
            var inGamePlayer = await _rCon.ExecuteAsync(
                new RCONRequest
                {
                    Command = string.Format(RCONCommands.GET_PLAYER_POSITION,
                                            steamId),
                    Password = player.Server.RConPassword,
                    Port = player.Server.RConnPort,
                    Server = player.Server.IPAddress
                });

            var result = await _rCon.ExecuteAsync(
                new RCONRequest
                {
                    Command = string.Format(RCONCommands.LIST_PLAYER_DINOS)
                });

            return (result != "Failure");
        }
    }
}
