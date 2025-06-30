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
using System;
using System.Threading.Tasks;

namespace ArkFury.API.General.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDomain _userDomain;
        private readonly IInventoryDomain _inventoryDomain;
        private readonly IRCONService _rConService;
        public UserController(IUserDomain UserDomain,
                                IInventoryDomain inventoryDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<UserController> logger,
                                IMapper mapper,
                                IOptions<AppSettings> settings,
                                ICacheService cache,
                                IRCONService rConService)
        {
            _userDomain = UserDomain;
            _inventoryDomain = inventoryDomain;
            _rConService = rConService;
        }

        //[HttpGet("{SteamId}/Login/{ServerIdentifier}")]
        //[ProducesResponseType(typeof(int), 200)]
        //public async Task<ActionResult<PlayerDTO>> Login(long SteamId)
        //{
        //   return await _userDomain.Login(SteamId);
        //}
    }
}