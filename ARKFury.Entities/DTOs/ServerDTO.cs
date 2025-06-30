using System;

namespace ArkFury.Entities.DTOs
{
    public class ServerDTO : BaseDTO
    {
        public ServerDTO(int cluster,
            string map,
            int mapNumber,
            string region,
            string ipAddress,
            string gameFolderLocation,
            int queryPort,
            int gamePort,
            int rConnPort,
            string url,
            string password,
            Guid uniqueIdentifier,
            string name,
            string id)
        {
            Cluster = cluster;
            MapNumber = mapNumber;
            Map = map;
            Region = region;
            QueryPort = queryPort;
            GamePort = gamePort;
            RConnPort = rConnPort;
            IPAddress = ipAddress;
            RConPassword = password;
            GameFolderLocation = gameFolderLocation;
            ArkShopFolder = $"{gameFolderLocation}/ShooterGame/Binaries/Win64/ArkApi/Plugins/ArkShop";
            CreateDate = DateTime.Now;
            Id = id;
            Url = url;
            UniqueIdentifier = uniqueIdentifier;
            Name = name;
        }

        public ServerDTO()
        {

        }

        public string ArkShopMySqlDbName { get; set; }
        public string ArkShopMySqDbPassword { get; set; }
        public string ArkShopMySqDbUsername { get; set; }
        public string ArkShopDbPort { get; set; }
        public string MySqlConectionString => $"server={IPAddress};" +
                                                $"database={ArkShopMySqlDbName};" +
                                                $"user={ArkShopMySqDbUsername};" +
                                                $"password={ArkShopMySqDbPassword};";
        public string IPAddress { get; set; }
        public string NormalizedName => $"{Region} Cluster-{Cluster} {Map}-{MapNumber}";
        public int Cluster { get; set; }
        public string Map { get; set; }
        public int MapNumber { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string GameUserSettings { get; set; }
        public int QueryPort { get; set; }
        public int GamePort { get; set; }
        public int RConnPort { get; set; }
        public string GameFolderLocation { get; set; }
        public string ArkShopFolder { get; set; }
        public string Url { get; set; }
        public string RConPassword { get; set; }
        public Guid UniqueIdentifier { get; set; }
    }
}