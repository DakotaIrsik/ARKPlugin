using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models.Requests;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlayerController : BaseController
    {
        private readonly IPlayerDomain _playerDomain;
        private readonly IInventoryDomain _inventoryDomain;
        private readonly IDinosaurDomain _dinosaurDomain;
        private readonly IRCONService _rConService;
        private readonly ITribeDomain _tribeDomain;

        public PlayerController(IPlayerDomain PlayerDomain,
                                IInventoryDomain inventoryDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<PlayerController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                ICacheService cache,
                                IElasticSearchService elasticSearchService,
                                ITribeDomain tribeDomain,
                                IRCONService rConService,
                                IDinosaurDomain dinosaurDomain) : base((BaseDomain)PlayerDomain, configuration, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _playerDomain = PlayerDomain;
            _inventoryDomain = inventoryDomain;
            _rConService = rConService;
            _dinosaurDomain = dinosaurDomain;
            _tribeDomain = tribeDomain;
        }

        [HttpGet("{SteamId}/Points")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> Get(long SteamId)
        {
            var result = await _playerDomain.Points(SteamId);
            return result;
        }

        [HttpGet("{SteamId}/Inventory")]
        [ProducesResponseType(typeof(List<InventoryDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<InventoryDTO>>> InventoryAsync(long SteamId, [FromQuery]InventorySearchRequest request)
        {
            var result = await _inventoryDomain.AllAsync(SteamId, request);
            return result;
        }

        [HttpPost("{steamId}/ChangePoints")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> ChangePoints(long steamId, int points)
        {
            var result = await _playerDomain.ChangePoints(steamId, points);
            return result;
        }

        [HttpPost("{SteamId}/TeleportToPlayer/{targetPlayer}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> TeleportToPlayer(long SteamId, long targetPlayer)
        {
            return await _playerDomain.TeleportTo(SteamId, targetPlayer);
        }

        [HttpPost("{SteamId}/CallPlayer/{targetPlayer}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> CallPlayer(long SteamId, long targetPlayer)
        {
            return await _playerDomain.TeleportTo(targetPlayer, SteamId);
        }

        [HttpPost("{SteamId}/TeleportTribeToPlayer")]
        [ProducesResponseType(typeof(List<PlayerDTO>), 200)]
        public async Task<ActionResult<List<PlayerDTO>>> TeleportTribeToPlayer(long SteamId)
        {
            var tribe = await _playerDomain.TribeInformation(SteamId);
            var player = await Player(SteamId);
            var clanMembers = await _tribeDomain.Players(player.Server, tribe.Id);


            foreach (var clanMember in clanMembers)
            {
                await _playerDomain.TeleportTo(clanMember.SteamId, SteamId);
            }

            return clanMembers;
        }

        [HttpPost("{SteamId}/WhistleAll")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<ActionResult<List<string>>> WhistleAll(long SteamId)
        {
            return await _playerDomain.CallAllDinos(SteamId);
        }

        [HttpPost("{SteamId}/Dinosaurs/{DinosaurId1}/{DinosaurId2}/Call")]
        [ProducesResponseType(typeof(OkResult), 200)]
        public async Task<ActionResult<object>> CallDino(long SteamId, long DinosaurId1, long DinosaurId2)
        {
            var action = await _playerDomain.CallDino(SteamId, DinosaurId1, DinosaurId2);
            return GetCustomResponse(action);
        }

        [HttpGet("{steamId}/Dinosaurs")]
        [ProducesResponseType(typeof(List<DinosaurDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<DinosaurDTO>>> Dinosaurs(long steamId, [FromQuery]PagingRequest request)
        {
            var result = await _playerDomain.GetDinosaurs(steamId);
            return GetCustomResponse(result, request.Fields);
        }
    }
}
