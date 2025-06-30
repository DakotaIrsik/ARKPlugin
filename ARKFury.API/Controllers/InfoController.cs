using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    //test
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InfoController : BaseController
    {
        private readonly IInfoDomain _infoDomain;
        public InfoController(IInfoDomain InfoDomain,
                                IOptions<AppSettings> shop,
                                IHttpContextAccessor httpContext,
                                ILogger<InfoController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)InfoDomain, shop, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _infoDomain = InfoDomain;
        }


        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<string> Get()
        {
            return await _infoDomain.AsString();
        }
    }
}