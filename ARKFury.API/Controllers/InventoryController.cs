using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
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
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventoryController : BaseController
    {
        private readonly IInventoryDomain _inventoryDomain;
        private readonly IServerDomain _serverDomain;
        private readonly ILootboxDomain _lootboxDomain;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryDomain InventoryDomain,
                                    ILootboxDomain lootboxDomain,
                                    IOptions<AppSettings> settings,
                                    IHttpContextAccessor context,
                                    ILogger<InventoryController> logger,
                                    IMapper mapper,
                                    IElasticSearchService elasticSearchService,
                                    ICacheService cacheService,
                                    IServerDomain serverDomain) : base((IBaseDomain)InventoryDomain, settings, context, logger, mapper, cacheService, elasticSearchService)
        {
            _inventoryDomain = InventoryDomain;
            _lootboxDomain = lootboxDomain;
            _mapper = mapper;
            _cacheService = cacheService;
            _serverDomain = serverDomain;
        }

        [HttpPost("{SteamId}/Add")]
        [ProducesResponseType(typeof(List<CartDTO>), 200)]
        public async Task<ActionResult<CartDTO>> Add(long SteamId, [FromBody]CartDTO request)
        {
            request.SteamId = SteamId;
            var result = await _inventoryDomain.AddAsync(request);
            return result;
        }


        [HttpGet("{SteamId}/Claim/{InventoryId}")]
        [ProducesResponseType(typeof(List<CartDTO>), 200)]
        public async Task<ActionResult<CartDTO>> Claim(long SteamId, string InventoryId)
        {
            var response = await _inventoryDomain.AllAsync(SteamId, new InventorySearchRequest { Size = 500, From = 0 });
            var request = response.Data.FirstOrDefault(i => i.Id == InventoryId);
            request.SteamId = SteamId;
            var player = await Player(SteamId);
            request.Server = player.Server ?? response.Data.FirstOrDefault().Server;
            

            var cart = new CartDTO();
            if (request.LootType == "Item")
            {
                var item = _mapper.Map<ItemDTO>(request);
                cart.Items.Add(item);
                await _inventoryDomain.ClaimInventoryAsync(request);
            }
            if (request.LootType == "Dinosaur")
            {
                var item = _mapper.Map<DinosaurDTO>(request);
                cart.Dinosaurs.Add(item);
                await _inventoryDomain.ClaimInventoryAsync(request);
            }
            if (request.LootType == "Lootbox")
            {
                var tempCart = new CartDTO();
                tempCart.SteamId = request.SteamId;
                tempCart.Server = request.Server;
                var lb = await _lootboxDomain.All();
                var slb = lb.Data.SingleOrDefault(d => d.Name == d.Name);
                slb.IsClaimed = true;
                tempCart.Lootboxes.Add(slb);
                var lootboxes = await _lootboxDomain.OpenAsync(tempCart);

                slb.Id = request.Id;

                var invRecs = await _inventoryDomain.AllAsync(request.SteamId, new InventorySearchRequest());
                var invRec = invRecs.Data.SingleOrDefault(i => i.Id == request.Id);

                await _inventoryDomain.UpdateAsync(invRec);
                cart.Lootboxes.AddRange(lootboxes);
            }

            //_cacheService.Remove($"Inventory-{cart.SteamId}");
            await _inventoryDomain.AllAsync(cart.SteamId, new InventorySearchRequest());

            return cart;
        }
    }
}