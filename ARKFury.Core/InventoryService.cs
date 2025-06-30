using ArkFury.Common.Services;
using ArkFury.Core.BusinessLogic;
using ArkFury.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.Core
{
    public interface IInventoryService
    {
        Task<int> AddTo(InventoryDTO item);
        Task<List<InventoryDTO>> AddTo(List<InventoryDTO> items);
    }

    public class InventoryService : IInventoryService
    {
        private const string INDEX_NAME = "inventory";

        private readonly IElasticSearchService _elasticSearchService;
        private readonly ITradeDomain _tradeDomain;

        public InventoryService(IElasticSearchService elasticSearchService, ITradeDomain tradeDomain)
        {
            _elasticSearchService = elasticSearchService;
            _tradeDomain = tradeDomain;
        }
        public async Task<int> AddTo(InventoryDTO item)
        {
            var result = await _elasticSearchService.IndexAsync(item, INDEX_NAME);
            if (result > 0)
            {
                _tradeDomain.AddSystemTrade(item);
            }

            return result;
        }

        public async Task<List<InventoryDTO>> AddTo(List<InventoryDTO> items)
        {
            foreach (var item in items)
            {
                item.CreateDate = DateTime.Now;
                item.Id = Guid.NewGuid().ToString();
                await AddTo(item);
            }
            return items;
        }
    }
}
