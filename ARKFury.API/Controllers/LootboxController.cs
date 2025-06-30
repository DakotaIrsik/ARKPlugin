using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Extensions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LootboxController : BaseController
    {
        private readonly ILootboxDomain _lootboxDomain;
        public LootboxController(ILootboxDomain LootboxDomain,
                                IOptions<AppSettings> shop,
                                IHttpContextAccessor httpContext,
                                ILogger<LootboxController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)LootboxDomain, shop, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _lootboxDomain = LootboxDomain;
        }

        [HttpPost("All")]
        [ProducesResponseType(typeof(AdjustableDTO<LootboxDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<LootboxDTO>>> All(LootboxSearchRequest request)
        {
            var items = await _lootboxDomain.All();

            if (!request.IncludeInactive)
            {
                items.Data = items.Data.Where(d => d.IsActive).ToList();
            }

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);
        }

        [HttpPost("Save")]
        [ProducesResponseType(typeof(LootboxDTO), 200)]
        public async Task<ActionResult<LootboxDTO>> Save(CartDTO cart)
        {
            var guid = Guid.NewGuid().ToString();
            var lootbox = new LootboxDTO()
            {
                Dinosaurs = cart.Dinosaurs ?? new List<DinosaurDTO>(),
                Items = cart.Items ?? new List<ItemDTO>(),
                Lootboxes = cart.Lootboxes ?? new List<LootboxDTO>(),
                LootType = "Lootbox",
                Amount = 1,
                CreateDate = DateTime.Now,
                CreatedBy = cart.SteamId.ToString(),
                Id = guid,
                StackSize = 1,
                Name = $"{cart.SteamId.ToString().Substring(cart.SteamId.ToString().Length - 4)}-{guid.ToString().Substring(guid.ToString().Length - 4)}"
            };
                
            return await _lootboxDomain.Insert(lootbox);
        }


        [HttpPost("AllAsJson")]
        [ProducesResponseType(typeof(AdjustableDTO<LootboxDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<LootboxDTO>>> AllAsJson(LootboxSearchRequest request)
        {
            var items = await _lootboxDomain.All();

            if (!request.IncludeInactive)
            {
                items.Data = items.Data.Where(d => d.IsActive).ToList();
            }

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return new JsonResult(result);

        }

        [HttpPost("Update")]
        [ProducesResponseType(typeof(LootboxDTO), 200)]
        public async Task<ActionResult<LootboxDTO>> Update([FromBody]LootboxDTO request)
        {
            return await _lootboxDomain.Update(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IEnumerable<string>> Names()
        {

            var result = await _lootboxDomain.All();
            return result.Data.Select(d => d.Name);
        }
    }
}