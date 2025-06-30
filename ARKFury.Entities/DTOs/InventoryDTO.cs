using ArkFury.Entities.LookUps;
using System;

namespace ArkFury.Entities.DTOs
{
    public class InventoryDTO : RollableDTO
    {
        public InventoryDTO()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string LootType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ItemId { get; set; }
        public int Amount { get; set; }
        public int StackSize { get; set; }
        public string Blueprint { get; set; }
        public string BlueprintPath { get; set; }
        public string Map { get; set; }
        public string Error { get; set; }
        public long SteamId { get; set; }
        public bool? IsClaimed { get; set; } = false;
        public int Price { get; set; }
        //public bool? Won => TheRoll > (100 - ChanceAsPercentage);
        public Quality Quality { get; set; } = new Quality();
        public DinosaurStatsDTO Stats { get; set; } = new DinosaurStatsDTO();
        public ServerDTO Server { get; set; } = new ServerDTO();

        private string _image;
        public string Image
        {
            get
            {
                var bp = Name?.Replace(" ", "_") ?? "";
                return (string.IsNullOrWhiteSpace(bp)) ? "" : $@"https://arkfury.com/images/120px-{bp}.png";
            }
            set
            {
                _image = value;
            }
        }
    }
}