using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class CartDTO
    {
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<DinosaurDTO> Dinosaurs { get; set; } = new List<DinosaurDTO>();
        public List<LootboxDTO> Lootboxes { get; set; } = new List<LootboxDTO>();
        public ServerDTO Server { get; set; } = new ServerDTO();
        public string Error { get; set; }
        public long SteamId { get; set; }
    }
}
