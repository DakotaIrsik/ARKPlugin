using System;

namespace ArkFury.Entities.DTOs
{
    public class TradeDTO
    {
        public string Id { get; set; }
        public long SteamId { get; set; }
        public string Blueprint { get; set; }
        public int Amount { get; set; }
        public int Randomness { get; set; }
        public int TheRoll { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string InventoryId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
