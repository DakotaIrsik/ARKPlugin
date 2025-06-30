using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Extensions;
using ArkFury.Core.BusinessLogic;
using System;
using Newtonsoft.Json;
using ArkFury.Entities.DTOs;
using System.Threading.Tasks;
using ArkFury.Common.Services;
using System.Linq;

namespace ArkFury.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IHttpContextAccessor Http { get; set; }
        protected IMapper Mapper { get; set; }
        protected ICacheService Cache { get; set; }
        protected AppSettings Settings { get; set; }
        protected ILogger Logger { get; set; }

        protected IElasticSearchService ElasticSearchService { get; set; }

        protected IBaseDomain Domain { get; set; }
        public BaseController(IBaseDomain domain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger logger,
                                IMapper mapper,
                                ICacheService cache,
                                IElasticSearchService elasticService)
        {
            Domain = domain;
            Http = httpContext;
            Mapper = mapper;
            Cache = cache;
            Settings = configuration?.Value;
            Logger = logger;
            ElasticSearchService = elasticService;
        }

        //protected void UpdateConfigurationFile(ServerDTO server)
        //{
        //    var settings = Settings;
        //    var client = settings.Clients.SingleOrDefault(s => s.UniqueIdentifier == server.UniqueIdentifier);
        //    client.ArkShopMySqDbUsername = server.ArkShopMySqDbUsername;
        //    client.ArkShopMySqDbPassword = server.ArkShopMySqDbPassword;
        //    client.ArkShopMySqlDbName = server.ArkShopMySqlDbName;
        //    client.ArkShopDbPort = server.ArkShopDbPort;
        //    client.IsActive = true;
        //    System.IO.File.WriteAllText($"WritingFile.txt", client.ArkShopMySqlDbName);
        //    System.IO.File.WriteAllText($"appsettings.{settings.Environment}.json", JsonConvert.SerializeObject(settings));
        //    System.IO.File.WriteAllText($"WroteFile.txt", client.ArkShopMySqlDbName);
        //}

        protected ActionResult GetCustomResponse(object o, string fields = null, Uri uri = null)
        {
            if (Domain.HasErrors == true)
            {
                return BadRequest(Domain.GetErrors());
            }
            if (o == null)
            {
                return NotFound();
            }
            else if (uri != null)
            {
                return Created(uri, o.FieldSelect(fields));
            }
            else
            {
                var x = o.FieldSelect(fields);
                var y = JsonConvert.SerializeObject(x, Formatting.Indented);
                return Ok(y);
            }
        }

        protected string GetCreatedLink(string id)
        {
            var request = Http.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            if (request.Host.Port.HasValue && request.Host.Port != 80 && request.Host.Port != 443)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return $"{uriBuilder.Uri}/{id}";
        }

        protected async Task<PlayerDTO> Player(long steamId)
        {
            var result = await ElasticSearchService.Search<PlayerDTO>(new PlayerDTO()
            {
                SteamId = steamId
            }, "player");

            return result.Data.SingleOrDefault();
        }
    }
}