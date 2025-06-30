namespace ArkFury.Entities.Models
{
    public partial class Arkshopplayers
    {
        public int Id { get; set; }
        public long SteamId { get; set; }
        public string Kits { get; set; }
        public int? Points { get; set; }
        public int? TotalSpent { get; set; }
    }
}
