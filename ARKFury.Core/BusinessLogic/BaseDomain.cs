using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IBaseDomain
    {
        bool HasErrors { get; }
        bool HasMessages { get; }
        List<Error> GetErrors();
        Task<bool> CheckMySqlConnection(string connectionString);
    }

    public class BaseDomain : IBaseDomain
    {
        protected readonly IElasticSearchService _elastic;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IHttpContextAccessor _http;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ICacheService _cache;

        public List<Error> Errors { get; set; } = new List<Error>();
        public List<Message> Messages { get; set; } = new List<Message>();

        public BaseDomain(ArkShopContext context,
                            IMapper mapper,
                            IElasticSearchService elastic,
                            ILogger logger,
                            IHttpContextAccessor httpContext,
                            IOptions<AppSettings> settings,
                            ICacheService cache)
        {
            _elastic = elastic;
            _mapper = mapper;
            _logger = logger;
            _http = httpContext;
            _settings = settings;
            _cache = cache;
        }
        protected string UserId => _http?.HttpContext?.User?.Claims?.SingleOrDefault(c => c.Type == "sub")?.Value;
        public bool HasErrors => Errors.Any();
        public bool HasMessages => Messages.Any();
        public List<Error> GetErrors()
        {
            return Errors;
        }

        public async Task<PlayerDTO> Player(long steamId)
        {
            var result = await _elastic.Search<PlayerDTO>(new PlayerDTO()
            {
                SteamId = steamId
            }, "player");

            return result.Data.SingleOrDefault();
        }

        public async Task<bool> CheckMySqlConnection(string connectionString)
        {
            MySqlConnection conx = null;
            try
            {
                using (conx = new MySqlConnection(connectionString))
                {
                    await conx.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error establishing connection to server: {connectionString}");
                return false;
            }
        }
    }
}
