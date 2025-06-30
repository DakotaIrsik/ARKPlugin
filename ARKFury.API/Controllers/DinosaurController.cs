using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
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
using ArkFury.Common.Extensions;
using ArkFury.Common.Services;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DinosaurController : BaseController
    {
        private readonly IDinosaurDomain _dinosaurDomain;
        public DinosaurController(IDinosaurDomain DinosaurDomain,
                                IOptions<AppSettings> shop,
                                IHttpContextAccessor httpContext,
                                ILogger<DinosaurController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)DinosaurDomain, shop, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _dinosaurDomain = DinosaurDomain;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdjustableDTO<DinosaurDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<ItemDTO>>> All([FromQuery]DinosaurSearchRequest request)
        {
            var items = await _dinosaurDomain.Search(request);

            if (!request.IncludeInActive)
            {
                items.Data = items.Data.Where(d => d.IsActive).ToList();
            }

            var result = items.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);

        }


        [HttpPost("AllAsJson")]
        [ProducesResponseType(typeof(AdjustableDTO<DinosaurDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<DinosaurDTO>>> AllAsJson(DinosaurSearchRequest request)
        {
            var items = await _dinosaurDomain.Search(new DinosaurSearchRequest());

            if (!request.IncludeInActive)
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
        [ProducesResponseType(typeof(DinosaurDTO), 200)]
        public async Task<ActionResult<DinosaurDTO>> Update([FromBody]DinosaurDTO request)
        {
            return await _dinosaurDomain.Update(request);
        }

        [HttpPost("ByType")]
        [ProducesResponseType(typeof(AdjustableDTO<DinosaurDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<DinosaurDTO>>> ByType(DinosaurSearchRequest request)
        {
            var items = await _dinosaurDomain.Search(new DinosaurSearchRequest());

            var result = items.Data.Where(d => d.Type == request.Type)
                                    .SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return GetCustomResponse(result, request.Fields);
        }

        [HttpGet("Types")]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IEnumerable<string>> Types([FromQuery]DinosaurSearchRequest request)
        {
            var response = await _dinosaurDomain.Search(request);

            var result = response.Data.SortBy(request)
                                    .Take(request.Size)
                                    .Skip(request.From)
                                    .ToList();

            return result.DistinctBy(d => d.Type).Select(r => r.Type).OrderBy(r => r);
        }
    }
}