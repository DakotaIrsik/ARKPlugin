using ArkFury.Common;

namespace ArkFury.Entities.Models
{
    public class City : Trackable
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
