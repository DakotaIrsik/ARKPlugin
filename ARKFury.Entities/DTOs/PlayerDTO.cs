using System;
using System.Collections.Generic;
using System.Text;

namespace ArkFury.Entities.DTOs
{
    public class PlayerDTO : BaseDTO
    {
        public PlayerDTO()
        {

        }

        public PlayerDTO(long steamId)
        {
            SteamId = steamId;
        }

        public long SteamId { get; set; }
        public string Kits { get; set; }
        public int? Points { get; set; }
        public int? TotalSpent { get; set; }
        public ServerDTO Server { get; set; }
        public TribeDTO Clan { get; set; }
        public List<DinosaurDTO> Dinosaurs { get; set; }
    }
}
