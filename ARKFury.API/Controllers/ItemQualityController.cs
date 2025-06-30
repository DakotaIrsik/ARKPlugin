using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Extensions;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.LookUps;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemQualityController : BaseController
    {
        public ItemQualityController(IItemDomain itemDomain,
                                IOptions<AppSettings> shop,
                                IHttpContextAccessor httpContext,
                                ILogger<ItemController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)itemDomain, shop, httpContext, logger, mapper, cache, elasticSearchService)
        {
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(AdjustableDTO<Quality>), 200)]
        public ActionResult<AdjustableDTO<Quality>> All([FromQuery]PagingRequest request)
        {
            var items = new AdjustableDTO<Quality>(request, Qualities.ToList);

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);
        }

        [HttpPost("AllAsJson")]
        [ProducesResponseType(typeof(AdjustableDTO<Quality>), 200)]
        public ActionResult<AdjustableDTO<Quality>> AllAsJson(PagingRequest request)
        {
            var items = new AdjustableDTO<Quality>(request, Qualities.ToList);

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);
        }

        [HttpGet("Names")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public IEnumerable<string> Names()
        {
            var result = Qualities.ToList;
            return result.Select(d => d.Name);
        }
    }
}