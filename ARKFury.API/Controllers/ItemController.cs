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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemController : BaseController
    {
        private readonly IItemDomain _itemDomain;
        public ItemController(IItemDomain itemDomain,
                                IOptions<AppSettings> shop,
                                IHttpContextAccessor httpContext,
                                ILogger<ItemController> logger,
                                IMapper mapper,
                                IElasticSearchService elasticSearchService,
                                IOptions<AppSettings> settings,
                                ICacheService cache) : base((IBaseDomain)itemDomain, shop, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _itemDomain = itemDomain;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdjustableDTO<ItemDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<ItemDTO>>> All([FromQuery]ItemSearchRequest request)
        {
            var items = await _itemDomain.Search(request);

            if (!request.IncludeInActive)
            {
                items.Data = items.Data.Where(d => d.IsActive).ToList();
            }

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(items.Data, request.Fields);

        }

        [HttpPost("AllAsJson")]
        [ProducesResponseType(typeof(AdjustableDTO<DinosaurDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<DinosaurDTO>>> AllAsJson(ItemSearchRequest request)
        {
            return new JsonResult(await All(request));
        }

        [HttpPost("Update")]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<ActionResult<ItemDTO>> Update(ItemDTO request)
        {
            return await _itemDomain.Update(request);
        }

        [HttpPost("ByType")]
        [ProducesResponseType(typeof(AdjustableDTO<ItemDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<ItemDTO>>> ByType(ItemSearchRequest request)
        {
            var items = await _itemDomain.Search(request);

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);
        }

        [HttpGet("Types")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IEnumerable<string>> Types([FromQuery]ItemSearchRequest request)
        {
            var response = await _itemDomain.Search(request);

            var result = response.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return result.DistinctBy(d => d.Type).Select(r => r.Type).OrderBy(r => r);
        }
    }
}