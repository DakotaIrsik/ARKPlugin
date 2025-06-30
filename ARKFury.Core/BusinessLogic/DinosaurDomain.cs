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
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IDinosaurDomain
    {
        Task<AdjustableDTO<DinosaurDTO>> Search(DinosaurSearchRequest request);
        Task<DinosaurDTO> Update(DinosaurDTO model);
        Task<IEnumerable<DinosaurDTO>> OpenAsync(CartDTO cart);
    }

    public class DinosaurDomain : BaseDomain, IDinosaurDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IRCONService _rConService;
        public DinosaurDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elasticsearchService,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache,
            IRCONService rConService) : base(context, mapper, elasticsearchService, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elasticsearchService;
            _rConService = rConService;
        }

        public async Task<AdjustableDTO<DinosaurDTO>> Search(DinosaurSearchRequest request)
        {
            return await _cache.GetOrSetAsync("All-Dinosaurs", async () =>
            await _elasticSearchService.Search<DinosaurDTO>(new PagingRequest(true), "dinosaur"));
        }

        public async Task<DinosaurDTO> Update(DinosaurDTO model)
        {
            var result = await Search(new DinosaurSearchRequest());
            var dino = result.Data.SingleOrDefault(d => d.Name == model.Name);
            dino.Price = model.Price;
            dino.IsActive = model.IsActive;
            dino.StackSize = model.StackSize;
            dino.Stats.Level = model.Stats.Level;
            int recordsChanged = 0;
            recordsChanged = await _elasticSearchService.UpdateAsync(dino, "dinosaur");
            _cache.Remove("All-Dinosaurs");
            await Search(new DinosaurSearchRequest());
            return (recordsChanged == 0) ? new DinosaurDTO() : model;
        }

        public async Task<IEnumerable<DinosaurDTO>> OpenAsync(CartDTO cart)
        {
            List<DinosaurDTO> result = new List<DinosaurDTO>();
            var rnd = new Random();
            var items = cart.Dinosaurs;
            var boxed = await Search(new DinosaurSearchRequest());
            foreach (var item in items)
            {
                var boxedItem = boxed.Data.SingleOrDefault(lb => lb.Name == item.Name);
                boxedItem.TheRoll = rnd.Next(1, 100);
                boxedItem.SteamId = cart.SteamId;
                boxedItem.Server = cart.Server;
                boxedItem.Id = Guid.NewGuid().ToString();
                result.Add(boxedItem);

                var cartItem = cart.Dinosaurs.SingleOrDefault(d => d.Name == boxedItem.Name);
                cartItem.TheRoll = boxedItem.TheRoll;
                cartItem.ChanceAsPercentage = boxedItem.ChanceAsPercentage;
                cartItem.IsClaimed = true;
                cartItem.Id = Guid.NewGuid().ToString();
            }
            return result;
        }
    }
}
