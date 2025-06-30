using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using ArkFury.Entities.Models.Requests;
using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IItemDomain
    {
        Task<AdjustableDTO<ItemDTO>> Search(ItemSearchRequest request);
        Task<ItemDTO> Update(ItemDTO model);
        Task<List<ItemDTO>> OpenAsync(CartDTO cart);
    }

    public class ItemDomain : BaseDomain, IItemDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        public ItemDomain(ArkShopContext context, 
            IMapper mapper,
            IElasticSearchService elasticsearchService,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elasticsearchService, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elasticsearchService;
        }

        public async Task<AdjustableDTO<ItemDTO>> Search(ItemSearchRequest request)
        {
           return await _cache.GetOrSetAsync("All-Items", async () =>
                 await _elasticSearchService.Search<ItemDTO>(request, "item"));
        }

        public async Task<ItemDTO> Update(ItemDTO model)
        {
            var result = await Search(new ItemSearchRequest(model.Id));
            var dino = result.Data.SingleOrDefault(d => d.Id == model.Id);
            dino.Price = model.Price;
            dino.IsActive = model.IsActive;
            dino.StackSize = model.StackSize;
            dino.Quality = model.Quality;
            int recordsChanged = 0;
            recordsChanged = await _elasticSearchService.UpdateAsync(dino, "item");
            _cache.Remove("All-Items");
            await Search(new ItemSearchRequest(model.Id));
            return (recordsChanged == 0) ? new ItemDTO() : model;
        }

        public async Task<List<ItemDTO>> OpenAsync(CartDTO cart)
        {
            List<ItemDTO> result = new List<ItemDTO>();
            var rnd = new Random();
            var items = cart.Items;
            var boxed = await Search(new ItemSearchRequest(true));
            foreach (var item in items)
            {
                var boxedItem = boxed.Data.SingleOrDefault(lb => lb.Name == item.Name);
                boxedItem.TheRoll = rnd.Next(1, 100);
                boxedItem.SteamId = cart.SteamId;
                boxedItem.Server = cart.Server;
                result.Add(boxedItem);

                var cartItem = cart.Items.SingleOrDefault(d => d.Name == boxedItem.Name);
                cartItem.TheRoll = boxedItem.TheRoll;
                cartItem.ChanceAsPercentage = boxedItem.ChanceAsPercentage;
                cartItem.IsClaimed = true;
            }
            return result;
        }
    }
}
