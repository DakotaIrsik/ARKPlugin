using ArkFury.API.Controllers;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PayPalController : BaseController
    {
        private readonly IPayPalDomain _payPalDomain;
        public PayPalController(IPayPalDomain PayPalDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<ServerController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                                    IElasticSearchService elasticSearchService,
                                ICacheService cache) : base((IBaseDomain)PayPalDomain, configuration, httpContext, logger, mapper, cache, elasticSearchService)
        {
            _payPalDomain = PayPalDomain;
        }

        [HttpPost("Complete")]
        public async Task<ActionResult<int>> Complete([FromBody]PayPalTransactionDTO transaction)
        {
            return await _payPalDomain.CompletePayment(transaction);
        }
    }
}