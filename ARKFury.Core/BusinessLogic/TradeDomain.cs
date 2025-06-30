using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface ITradeDomain
    {
        Task<AdjustableDTO<TradeDTO>> AllLogs(PagingRequest request);
        TradeDTO AddSystemTrade(InventoryDTO item);
    }

    public class TradeDomain : BaseDomain, ITradeDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        public TradeDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elastic;
        }

        public async Task<AdjustableDTO<TradeDTO>> AllLogs(PagingRequest request)
        {
            return await _elasticSearchService.Search<TradeDTO>(request, "trade");
        }

        public TradeDTO AddSystemTrade(InventoryDTO item)
        {
            var trade = new TradeDTO()
            {
                Amount = item.Amount,
                Blueprint = item.Blueprint,
                Name = item.Name,
                CreateDate = item.CreateDate ?? DateTime.Now,
                From = "SYSTEM",
                Id = Guid.NewGuid().ToString(),
                InventoryId = item.Id,
                Price = item.Price,
                Randomness = item.ChanceAsPercentage,
                SteamId = item.SteamId,
                TheRoll = item.TheRoll
            };
            try

            {
                _elasticSearchService.Index(trade, "trade");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unable to save Trade Log for player {item.SteamId}'s item {item.Name} with Id {item.Id}");
            }
            return trade;
        }
    }
}
