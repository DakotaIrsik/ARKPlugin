using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeDomain _tradeDomain;
        public TradeController(ITradeDomain tradeDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<TradeController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                ICacheService cache)
        {
            _tradeDomain = tradeDomain;
        }

        [HttpPost("Logs")]
        [ProducesResponseType(typeof(AdjustableDTO<TradeDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<TradeDTO>>> Logs(PagingRequest request)
        {
            return await _tradeDomain.AllLogs(request);
        }
    }
}