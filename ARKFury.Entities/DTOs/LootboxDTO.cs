using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class LootboxDTO : InventoryDTO
    {
        public List<string> Description { get
            {
                var x = new List<string>();
                foreach (var item in Items)
                {
                    x.Add($"{item.ChanceAsPercentage}% - {item.Amount * item.StackSize} x (Quality {item.Quality}) {item.Name}");
                }

                foreach (var item in Dinosaurs)
                {
                    x.Add($"{item.ChanceAsPercentage}% - {item.Amount} x (Level {item.Stats.Level}) {item.Name}");
                }

                foreach (var item in Lootboxes)
                {
                    x.Add($"{item.ChanceAsPercentage}% - {item.Amount} x {item.Name}");
                }

                return x;
            } }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public List<DinosaurDTO> Dinosaurs { get; set; } = new List<DinosaurDTO>();
        public List<LootboxDTO> Lootboxes { get; set; } = new List<LootboxDTO>();
        public PointDTO Point { get; set; }
        public VipDTO Vip { get; set; }

    }
}