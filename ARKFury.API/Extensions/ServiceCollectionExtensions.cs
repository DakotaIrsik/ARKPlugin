using Microsoft.Extensions.DependencyInjection;
using ArkFury.Core.BusinessLogic;
using ArkFury.Core;
using ArkFury.Entities.Models;

namespace ArkFury.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<IServerDomain, ServerDomain>();
            services.AddTransient<IPayPalDomain, PayPalDomain>();
            services.AddTransient<IDinosaurDomain, DinosaurDomain>();
            services.AddTransient<IItemDomain, ItemDomain>();
            services.AddTransient<IServerDomain, ServerDomain>();
            services.AddTransient<ITradeDomain, TradeDomain>();
            services.AddTransient<IInfoDomain, InfoDomain>();
            services.AddTransient<IInventoryDomain, InventoryDomain>();
            services.AddTransient<IPlayerDomain, PlayerDomain>();
            services.AddTransient<ITribeDomain, TribeDomain>();
            services.AddTransient<ILootboxDomain, LootboxDomain>();
            services.AddTransient<IInventoryDomain, InventoryDomain>();
            services.AddTransient<IBaseDomain, BaseDomain>();
            services.AddTransient<IUtilityDomain, UtilityDomain>();
            services.AddTransient<IUserDomain, UserDomain>();

            services.AddTransient<IInventoryService, InventoryService>();




            return services;
        }
    }
}