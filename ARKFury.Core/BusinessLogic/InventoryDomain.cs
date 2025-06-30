using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.RCON.ARK;
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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IInventoryDomain
    {
        Task<AdjustableDTO<InventoryDTO>> AllAsync(long steamId, InventorySearchRequest request);
        Task<CartDTO> AddAsync(CartDTO model);
        Task<InventoryDTO> UpdateAsync(InventoryDTO model);
        Task<List<string>> ClaimItemsAsync(List<InventoryDTO> items);
        Task<string> ClaimInventoryAsync(InventoryDTO item);
    }

    public class InventoryDomain : BaseDomain, IInventoryDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IInventoryService _inventoryService;
        private readonly ILootboxDomain _lootboxDomain;
        private readonly IItemDomain _itemDomain;
        private readonly IDinosaurDomain _dinosaurDomain;
        private readonly IRCONService _rconService;
        private readonly IPlayerDomain _playerDomain;

        public InventoryDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            IInventoryService inventoryService,
            ILootboxDomain lootboxDomain,
            IItemDomain itemDomain,
            IDinosaurDomain dinosaurDomain,
            IPlayerDomain playerDomain,
            ICacheService cache,
            IRCONService rconService) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elastic;
            _inventoryService = inventoryService;
            _itemDomain = itemDomain;
            _dinosaurDomain = dinosaurDomain;
            _rconService = rconService;
            _playerDomain = playerDomain;
            _lootboxDomain = lootboxDomain;
        }

        public async Task<AdjustableDTO<InventoryDTO>> AllAsync(long steamId, InventorySearchRequest request)
        {
            //return await _cache.GetOrSetAsync($"Inventory-{steamId}", async () =>
            //{
            var searchRequest = new InventoryDTO
            {
                SteamId = steamId,
                IsClaimed = null,
                Server = null,
                Stats = null,
                Quality = null,
                Size = request.Size,
                From = request.From
            };
            var result = await _elasticSearchService.Search<InventoryDTO>(searchRequest, "inventory");
            return result;
            //});
        }

        public async Task<InventoryDTO> UpdateAsync(InventoryDTO model)
        {
            var result = await AllAsync(model.SteamId, new InventorySearchRequest());
            var row = result.Data.SingleOrDefault(d => d.Id == model.Id);
            row.Price = model.Price;
            row.IsActive = model.IsActive;
            row.Quality = model.Quality;
            row.IsClaimed = model.IsClaimed;
            row.Map = model.Map;
            row.UpdateDate = DateTime.Now;
            row.UpdatedBy = "SYSTEM";
            int recordsChanged = 0;
            recordsChanged = await _elasticSearchService.UpdateAsync(row, "inventory");
            if (recordsChanged > 0)
            {
                _cache.Remove("All-Items");
                await AllAsync(model.SteamId, new InventorySearchRequest());
            }
            return (recordsChanged == 0) ? new InventoryDTO() : model;
        }

        public async Task<CartDTO> AddAsync(CartDTO cart)
        {
            var player = await _playerDomain.GetAsync(cart.SteamId);
            if (player.Points < cart.Items.Sum(i => i.Price * i.Amount) + 
                                cart.Dinosaurs.Sum(i => i.Price * i.Amount) +
                                cart.Lootboxes.Sum(i => i.Price * i.Amount))
            {
                cart.Error = $"Insuccifient Points, current balance: {player.Points}";
                return cart;
            }

            var allItems = new CartDTO();
            allItems.SteamId = cart.SteamId;

            var openLootboxes = await _lootboxDomain.OpenAsync(cart);
            allItems.Lootboxes?.AddRange(openLootboxes);

            var openItems = await _itemDomain.OpenAsync(cart);
            allItems.Items?.AddRange(openItems);

            var openDinosaurs = await _dinosaurDomain.OpenAsync(cart);
            allItems.Dinosaurs?.AddRange(openDinosaurs);

            var dil = new List<InventoryDTO>();
            dil.AddRange(_mapper.Map<List<InventoryDTO>>(allItems.Dinosaurs));
            dil.AddRange(_mapper.Map<List<InventoryDTO>>(allItems.Items));
            dil.AddRange(_mapper.Map<List<InventoryDTO>>(allItems.Lootboxes));

            foreach (var doc in dil)
            {
                doc.SteamId = cart.SteamId;
                doc.IsClaimed = doc.LootType == "Lootbox" ? true : false;
                await _inventoryService.AddTo(doc);
                await _playerDomain.ChangePoints(doc.SteamId, doc.Price * -1);
            }

            _cache.Remove($"Inventory-{cart.SteamId}");
            await AllAsync(cart.SteamId, new InventorySearchRequest());

            return cart;
        }

        public async Task<string> ClaimInventoryAsync(InventoryDTO item)
        {
            var response = $"Failed to claim item: {item.Name}";
            try
            {
                var result = await _rconService.ExecuteAsync(new RCONRequest
                {
                    Command = LootTypeRconCommandResolver(item),
                    Password = item.Server.RConPassword,
                    Port = item.Server.RConnPort,
                    Server = item.Server.IPAddress
                });

                if (response != null) //do some real verification here.
                {
                    item.IsClaimed = true;
                    await UpdateAsync(item);
                }
                response = $"{item.Name} Claimed";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error claiming player {item.SteamId}'s item {item.Name}  on MySQL Server: {item.Server.MySqlConectionString}");
            }

            return response;
        }

        public async Task<List<string>> ClaimItemsAsync(List<InventoryDTO> items)
        {
            var results = new List<string>();
            foreach (var item in items)
            {
                results.Add(await ClaimInventoryAsync(item));
            }
            return results;
        }

        private string LootTypeRconCommandResolver(InventoryDTO item)
        {
            var theCommand = "";
            switch (item.LootType)
            {
                case "Item":
                    theCommand = string.Format(RCONCommands.GIVE_ITEM,
                        item.SteamId,
                        item.BlueprintPath,
                        item.StackSize * item.Amount,
                        item.Quality.RandomThreshold);
                    break;
                case "Dinosaur":
                    theCommand = string.Format(RCONCommands.GIVE_DINO,
                        item.SteamId,
                        item.BlueprintPath,
                        item.Stats.Level);
                    break;
            }
            return theCommand;
        }
    }
}