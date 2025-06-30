using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Core.RCON.ARK;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IPlayerDomain
    {
        Task<int> Points(long steamId);
        Task<int> ChangePoints(long steamId, int points);
        Task<PlayerDTO> GetAsync(long steamId);
        Task<string> TeleportTo(long steamId, long targetPlayer);
        Task<string> CallDino(long steamId, DinosaurDTO dinosaur, PositionDTO position = null);
        Task<string> CallDino(long steamId, long dinosaurId1, long dinosaurId2, PositionDTO position = null);
        Task<List<string>> CallAllDinos(long steamId);
        Task<List<DinosaurDTO>> GetDinosaurs(long steamId);
        Task<TribeDTO> TribeInformation(long steamId);
    }

    public class PlayerDomain : BaseDomain, IPlayerDomain
    {
        private readonly ArkShopContext _arkShopContext;
        private readonly IRCONService _rCONService;
        private readonly ITribeDomain _tribeDomain;
        private readonly IServerDomain _serverDomain;
        public PlayerDomain(ArkShopContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache,
            ITribeDomain tribeDomain,
            IRCONService rCONService,
            IServerDomain serverDomain) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _arkShopContext = context;
            _rCONService = rCONService;
            _tribeDomain = tribeDomain;
            _serverDomain = serverDomain;
        }

        public async Task<int> Points(long steamId)
        {
            var result = await GetAsync(steamId);
            return result.Points ?? 0;
        }

        public async Task<int> ChangePoints(long steamId, int points)
        {
            int result = 0;
            try
            {
                var player = await GetAsync(steamId);
                player.Points += points;
                if (points < 0)
                {
                    player.TotalSpent += Math.Abs(points);
                }

                result = UpdatePointsToMySQL(player.Points ?? 0, steamId, player.Server.MySqlConectionString);
            }
            catch(Exception ex)
            {

            }
            return (result > 0) ? points : 0;
        }

        public async Task<PlayerDTO> GetAsync(long steamId)
        {
            var esPlayerResponse = await _elastic.Search<PlayerDTO>(new PlayerDTO { SteamId = steamId }, "player");
            var player = esPlayerResponse.Data.FirstOrDefault() ?? new PlayerDTO(steamId);
            if (player.Server == null)
            {
                var allServers = await _serverDomain.All();
                var suggestedServer = allServers.Data.FirstOrDefault(s => s.IsActive && s.Name == "ARKFury-Genesis") ?? allServers.Data.FirstOrDefault();
                player.Server = suggestedServer;
            }

            var shopPlayer = GetPlayerFromMySqlServer(player.Server.MySqlConectionString, player.SteamId);
            var clanInfo = await TribeInformation(steamId);
            player.Clan = clanInfo;
            player.SteamId = steamId;
            player.Points = shopPlayer.Points;
            player.Kits = shopPlayer.Kits;
            player.IsActive = true;
            player.TotalSpent = shopPlayer.TotalSpent;
            player.Id = steamId.ToString();


            _elastic.Index(player, "player");
            return player;
        }

        private Arkshopplayers GetPlayerFromMySqlServer(string connectionString, long steamId)
        {
            Arkshopplayers player = new Arkshopplayers();
            player.SteamId = steamId;
            try
            {
                
                using (var conx = new MySqlConnection(connectionString))
                {
                    conx.Open();
                    var x = conx.CreateCommand();
                    x.CommandText = "Select * from Arkshopplayers WHERE SteamId = " + steamId;
                    var playerser = x.ExecuteReader();
                    while (playerser.Read())
                    {
                        player.Id = Convert.ToInt32(playerser["id"]);
                        player.Kits = playerser["kits"].ToString();
                        player.Points = Convert.ToInt32(playerser["points"]);
                        player.SteamId = steamId;
                        player.TotalSpent = Convert.ToInt32(playerser["totalspent"]);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving points from MySQL Server: {connectionString}");
            }

            return player;
        }

        private int UpdatePointsToMySQL(int points, long steamId, string connectionString)
        {
            int rowsUpdated = 0;
            try
            {
                using (var conx = new MySqlConnection(connectionString))
                {
                    conx.Open();
                    var x = conx.CreateCommand();
                    x.CommandText = $"UPDATE Arkshopplayers SET Points = {points} WHERE SteamId = {steamId}";
                    rowsUpdated = x.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating {points} points for player {steamId} on MySQL Server: {connectionString}");
            }
            return rowsUpdated;
        }

        public async Task<TribeDTO> TribeInformation(long steamId)
        {
            var tribe = new TribeDTO();
            try
            {
                var player = await Player(steamId);
                string result = "";
                if (player?.SteamId == steamId)
                {
                    result = await _rCONService.ExecuteAsync(new RCONRequest
                    {
                        Command = string.Format(RCONCommands.GET_PLAYER_TRIBE, steamId),
                        Password = player.Server.RConPassword,
                        Port = player.Server.RConnPort,
                        Server = player.Server.IPAddress
                    });
                }

                tribe.Id = result.Replace("Tribe ID - ", "");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error obtaining Tribe information for player: {steamId}");
            }

            return tribe;
        }

        public async Task<string> TeleportTo(long steamId, long targetPlayer)
        {
            var player = await GetAsync(steamId);
            return await _rCONService.ExecuteAsync(
                    new RCONRequest
                    {
                        Command = string.Format(RCONCommands.TELEPORT_PLAYER_TO_PLAYER,
                                                steamId,
                                                targetPlayer),
                        Password = player.Server.RConPassword,
                        Port = player.Server.RConnPort,
                        Server = player.Server.IPAddress
                    });

        }

        public async Task<string> CallDino(long steamId, DinosaurDTO dinosaur, PositionDTO position = null)
        {
            var player = await GetAsync(steamId);
            position = position == null ? await GetPosition(steamId) : position;
            return await _rCONService.ExecuteAsync(
                    new RCONRequest
                    {
                        Command = string.Format(RCONCommands.TELEPORT_DINOSAUR_TO_COORDINATE,
                                                dinosaur.DinosaurId1,
                                                dinosaur.DinosaurId2,
                                                position.X,
                                                position.Y,
                                                position.Z),
                        Password = player.Server.RConPassword,
                        Port = player.Server.RConnPort,
                        Server = player.Server.IPAddress
                    });
        }

        public async Task<string> CallDino(long steamId, long dinosaurId1, long dinosaurId2, PositionDTO position = null)
        {
            return await CallDino(steamId, new DinosaurDTO { DinosaurId1 = dinosaurId1, DinosaurId2 = dinosaurId2 });
        }

        public async Task<List<string>> CallAllDinos(long steamId)
        {
            var responses = new List<string>();
            var player = await GetAsync(steamId);
            var playerPosition = await GetPosition(steamId);
            var playerDinosaurs = await GetDinosaurs(steamId);
            foreach (var dinosaur in playerDinosaurs)
            {
                responses.Add(await CallDino(player.SteamId, dinosaur, playerPosition));
            }

            return responses;
        }

        public async Task<PositionDTO> GetPosition(long steamId)
        {
            var player = await GetAsync(steamId);
            var result = await _rCONService.ExecuteAsync(
                    new RCONRequest
                    {
                        Command = string.Format(RCONCommands.GET_PLAYER_POSITION,
                                                steamId),
                        Password = player.Server.RConPassword,
                        Port = player.Server.RConnPort,
                        Server = player.Server.IPAddress
                    });

            var position = new PositionDTO();

            position.X = Convert.ToDouble(
                result.Split(' ')[0]
                .Replace("X=", "")
                .Replace("Y=", "")
                .Replace("Z=", ""));

            position.Y = Convert.ToDouble(
             result.Split(' ')[1]
             .Replace("X=", "")
             .Replace("Y=", "")
             .Replace("Z=", ""));


            position.Z = Convert.ToDouble(
             result.Split(' ')[2]
             .Replace("X=", "")
             .Replace("Y=", "")
             .Replace("Z=", ""));

            return position;
        }

        public async Task<List<DinosaurDTO>> GetDinosaurs(long steamId)
        {
            var dinos = new List<DinosaurDTO>();

            var player = await GetAsync(steamId);
            var result = await _rCONService.ExecuteAsync(
                    new RCONRequest
                    {
                        Command = string.Format(RCONCommands.LIST_PLAYER_DINOS,
                                                steamId),
                        Password = player.Server.RConPassword,
                        Port = player.Server.RConnPort,
                        Server = player.Server.IPAddress
                    });

            string[] lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var dino = new DinosaurDTO();
                // TODO split out name and such correctly, populate BP path, image etc.
                dino.Name = line.Split(',')[0];
                dino.DinosaurId1 = Convert.ToInt64(line.Split(',')[1].Replace("ID1=", "").Replace("ID2=", ""));
                dino.DinosaurId2 = Convert.ToInt64(line.Split(',')[2].Replace("ID1=", "").Replace("ID2=", ""));

                dinos.Add(dino);
            }

            return dinos;
        }
    }
}
