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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface ILootboxDomain
    {
        Task<AdjustableDTO<LootboxDTO>> All();
        Task<LootboxDTO> Update(LootboxDTO model);
        Task<LootboxDTO> Insert(LootboxDTO lootbox);
        Task<List<LootboxDTO>> OpenAsync(CartDTO cart);
    }

    public class LootboxDomain : BaseDomain, ILootboxDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IInventoryService _inventoryService;
        public LootboxDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elasticsearchService,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            IInventoryService inventoryService,
            ICacheService cache) : base(context, mapper, elasticsearchService, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elasticsearchService;
            _inventoryService = inventoryService;
        }

        public async Task<AdjustableDTO<LootboxDTO>> All()
        {
            return await _cache.GetOrSetAsync("All-Lootboxes", async () =>
            await _elasticSearchService.Search<LootboxDTO>(new PagingRequest(true), "lootbox"));
        }

        public async Task<LootboxDTO> Insert(LootboxDTO lootbox)
        {
            _elasticSearchService.Index(lootbox, "lootbox");
            _cache.Remove("All-Items");
            await All();
            return lootbox;
        }

        public async Task<LootboxDTO> Update(LootboxDTO model)
        {
            var result = await All();
            var dino = result.Data.SingleOrDefault(d => d.Name == model.Name);
            dino.Price = model.Price;
            dino.IsActive = model.IsActive;
            int recordsChanged = 0;
            recordsChanged = await _elasticSearchService.UpdateAsync(dino, "lootbox");
            _cache.Remove("All-Lootboxes");
            await All();
            return (recordsChanged == 0) ? new LootboxDTO() : model;
        }

        public async Task<List<LootboxDTO>> OpenAsync(CartDTO cart)
        {
            List<LootboxDTO> result = new List<LootboxDTO>();
            var rnd = new Random();
            var theRecursiveBoxes = cart.Lootboxes;
            var boxed = await All();
            //while (theRecursiveBoxes?.Any() ?? false)
            //{

            // TODO Automap this nightmare.
            foreach (var item in theRecursiveBoxes)
            {
                var boxedItem = boxed.Data.SingleOrDefault(lb => lb.Name == item.Name);
                boxedItem.TheRoll = rnd.Next(1, 100);
                boxedItem.SteamId = cart.SteamId;
                boxedItem.Server = cart.Server;
                boxedItem.IsClaimed = true;
                boxedItem.LootType = "Lootbox";
                result.Add(boxedItem);

                await Update(boxedItem);

                foreach (var interiorDino in boxedItem.Dinosaurs)
                {
                    interiorDino.ChanceAsPercentage = boxedItem.ChanceAsPercentage;
                    interiorDino.SteamId = boxedItem.SteamId;
                    interiorDino.LootType = "Dinosaur";
                    interiorDino.Id = Guid.NewGuid().ToString();
                    await _inventoryService.AddTo(interiorDino);
                }

                foreach (var interiorLootbox in boxedItem.Lootboxes)
                {
                    interiorLootbox.ChanceAsPercentage = boxedItem.ChanceAsPercentage;
                    interiorLootbox.SteamId = boxedItem.SteamId;
                    interiorLootbox.LootType = "Lootbox";
                    interiorLootbox.Id = Guid.NewGuid().ToString();
                    await _inventoryService.AddTo(interiorLootbox);
                }

                foreach (var interiorItem in boxedItem.Items)
                {

                    interiorItem.ChanceAsPercentage = boxedItem.ChanceAsPercentage;
                    interiorItem.SteamId = boxedItem.SteamId;
                    interiorItem.LootType = "Item";
                    interiorItem.Id = Guid.NewGuid().ToString();
                    await _inventoryService.AddTo(interiorItem);
                }
            }

            //keep going until none left.
            //theRecursiveBoxes = theRecursiveBoxes.SelectMany(rb => rb.Lootboxes).Distinct().ToList();
            //}


            return result;
        }
    }
}
