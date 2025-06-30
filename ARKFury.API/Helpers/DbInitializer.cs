using ArkFury.Common;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.ElasticSearch;
using ArkFury.Entities.LookUps;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.API.Helpers
{
    public static class DbInitializer
    {
        private static string DeleteMessage(string indexName)
        {
            return $"Creating {indexName} ElasticSearch Index";
        }

        private static string CreateMessage(string indexName)
        {
            return $"Deleting {indexName} ElasticSearch Index";
        }

        private static string MappingFile(string elasticClassName)
        {
            return $"MappingFiles/{elasticClassName}.json";
        }

        private static string DeletingMSSQLDatabaseMessage => "Deleting MSSQL Database";
        private static string CreatingMSSSQLDatabaseMessage => "Creating MSSQL Database";
        private static string DataSuccessfullySeededMessage => "Data Successfully Seeded";

        private static string SeedingMessage(string indexName)
        {
            var dataStructureName = indexName.Replace("Elastic", "", StringComparison.InvariantCulture);
            dataStructureName = (dataStructureName.ToUpperInvariant().EndsWith('S')) ? dataStructureName + "es" : dataStructureName + "s";
            return $"Seeding {dataStructureName}";
        }

        public static async Task Seed(IWebHost host, bool shouldSeed)
        {
            using (var serviceScope = host?.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var elasticSearchService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>())
                    {
                        if (shouldSeed)
                        {
                            await DeleteElasticIndex(elasticSearchService, "info");
                            await DeleteElasticIndex(elasticSearchService, "server");
                            await DeleteElasticIndex(elasticSearchService, "dinosaur");
                            await DeleteElasticIndex(elasticSearchService, "item");
                            await DeleteElasticIndex(elasticSearchService, "player");
                            //await DeleteElasticIndex(elasticSearchService, "trade");
                            //await DeleteElasticIndex(elasticSearchService, "transaction");
                            await DeleteElasticIndex(elasticSearchService, "lootbox");
                            await DeleteElasticIndex(elasticSearchService, "inventory");

                            SeedInfo(elasticSearchService);
                            var configuration = services.GetRequiredService<IConfiguration>();
                            var mapper = services.GetRequiredService<IMapper>();
                            await SeedServer(elasticSearchService, configuration, mapper);
                            await SeedGeneralDinos(elasticSearchService);
                            await SeedItems(elasticSearchService);
                            await SeedLootboxes(elasticSearchService);
                        }
                    }
                }
            }
        }

        private static async Task DeleteElasticIndex(IElasticSearchService service, string index)
        {
            Console.WriteLine(DeleteMessage(index));
            await service.DeleteIndexAsync(index).ConfigureAwait(false);

            //for custom mapping files.
            //Console.WriteLine(CreateMessage(indexName));
            //await service.CreateIndexAsync(indexName, File.ReadAllText(MappingFile(type.Name))).ConfigureAwait(false);
        }

        private static void SeedInfo(IElasticSearchService es)
        {
            var dsi = new ElasticInfo();

            dsi.GeneralRules.Add("Bans will be enforced cluster wide. We will ban you as hard as we can; MachineID, SteamID, IP, and any other way we can figure out");

            dsi.ServerRules.Add("No racism, hate speech or bullying ingame");
            dsi.ServerRules.Add("No hacking /  No bug abuse /  No ingame exploits");
            dsi.ServerRules.Add("Dinos or items lost due to Ark issues will not be replaced. This includes lag, mod updates, game bugs/patches");
            dsi.ServerRules.Add("No Insiding");

            dsi.BuildingRules.Add("No building in the mesh");
            dsi.BuildingRules.Add("Cave building allowed");
            dsi.BuildingRules.Add("No structure spam except at base and fob. Keep it close or you will be wiped");
            dsi.BuildingRules.Add("No foundation/building exploits. Offenders will be perma-banned");
            dsi.BuildingRules.Add("No building in rat holes. Offenders will be perma-banned");

            dsi.BlockedBuildingLocations.Add("Wyvern egg nests, scar or wyvern cave");
            dsi.BlockedBuildingLocations.Add("Castle on Ragnarok near Viking Bay");
            dsi.BlockedBuildingLocations.Add("Obelisks");
            dsi.BlockedBuildingLocations.Add("Spawn locations");
            dsi.BlockedBuildingLocations.Add("Pearl Cave");
            dsi.BlockedBuildingLocations.Add("Lifes Labyrinth");
            dsi.BlockedBuildingLocations.Add("Lair Cave");
            dsi.BlockedBuildingLocations.Add("Island Snow Cave");
            dsi.BlockedBuildingLocations.Add("Island Swamp Cave");
            dsi.BlockedBuildingLocations.Add("All entrances and exits to caves listed");
            dsi.BlockedBuildingLocations.Add("Aberration surface and surface entrances");
            dsi.BlockedBuildingLocations.Add("Drop Spawns");
            dsi.BlockedBuildingLocations.Add("Bases found at the listed locations will be wiped. If it is deemed intentional (admin review), the offending tribe will wiped");

            dsi.RaidingRules.Add("No teaming");
            dsi.RaidingRules.Add("No tunnelling");

            dsi.DinoPlatformRules.Add("You can only block the rider, not the dino. If you are soaking bullets and not taking damage, you're doing it wrong");
            dsi.DinoPlatformRules.Add("No Tunnel / Poke builds");
            dsi.DinoPlatformRules.Add("Raiding is sacred to us. Anyone caught trying to bend the very few rules we have around raiding, will be perma-banned");

            es.Index(dsi, "info");
        }

        private static async Task SeedServer(IElasticSearchService es, IConfiguration configuration, IMapper mapper)
        {
            /*
                    CREATE USER 'ArkShopAdmin'@'localhost' IDENTIFIED BY 'bnmeT2c6Vpp&Kjk9';
                    GRANT ALL PRIVILEGES ON *.* TO 'ArkShopAdmin'@'localhost'
                    WITH GRANT OPTION;
                    CREATE USER 'ArkShopAdmin'@'%' IDENTIFIED BY 'bnmeT2c6Vpp&Kjk9';
                    GRANT ALL PRIVILEGES ON *.* TO 'ArkShopAdmin'@'%'
                    WITH GRANT OPTION;
             */

            var server = new ServerDTO();
            server.Id = "dab8edc9-84ff-424f-a2d2-7092a088f568";
            server.UniqueIdentifier = Guid.Parse("45e5d1d7-dd6d-489a-8be6-c1a3b88cf6e7");
            server.ArkShopMySqlDbName = "ArkShop";
            server.ArkShopDbPort = "3600";
            server.ArkShopMySqDbPassword = "bnmeT2c6Vpp&Kjk9";
            server.ArkShopMySqDbUsername = "ArkShopAdmin";
            server.RConnPort = 32331;
            server.RConPassword = "Shield22";
            server.IPAddress = "52.4.214.43";
            server.IsActive = true;
            server.CreateDate = DateTime.Now;
            server.CreatedBy = "Ender";
            server.Name = "ARKFury-Genesis";

            await es.IndexAsync(server, "server");
        }

        public static JToken Serialize(IConfiguration config)
        {
            JObject obj = new JObject();
            foreach (var child in config.GetChildren())
            {
                obj.Add(child.Key, Serialize(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
                return new JValue(section.Value);

            return obj;
        }

        private static async Task SeedGeneralDinos(IElasticSearchService elasticSearch)
        {
            var json = File.ReadAllText($@"Data\Dinosaurs.json");
            var dinosaurs = JsonConvert.DeserializeObject<List<DinosaurDTO>>(json);

            foreach (var dino in dinosaurs)
            {
                dino.Id = Guid.NewGuid().ToString();
                await elasticSearch.IndexAsync(dino, "dinosaur").ConfigureAwait(false);
            }
        }
        private static async Task SeedItems(IElasticSearchService elasticSearch)
        {
            var json = File.ReadAllText($@"Data\Items.json");
            var items = JsonConvert.DeserializeObject<List<ItemDTO>>(json);

            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Quality.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.Quality = Qualities.ToList.SingleOrDefault(q => q.Name == "Primitive");
                }
                await elasticSearch.IndexAsync(item, "item").ConfigureAwait(false);
            }
        }

        private static async Task SeedLootboxes(IElasticSearchService elasticSearch)
        {
            var json = File.ReadAllText($@"Data\Lootboxes.json");
            var items = JsonConvert.DeserializeObject<List<LootboxDTO>>(json);

            foreach (var item in items)
            {
                item.Id = Guid.NewGuid().ToString();
                await elasticSearch.IndexAsync(item, "lootbox").ConfigureAwait(false);
            }
        }
    }
}
