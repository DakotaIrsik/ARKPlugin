using System.Collections.Generic;

namespace ArkFury.Entities.ElasticSearch
{
    public class ElasticInfo : BaseElastic
    {
        public List<string> GeneralRules { get; set; } = new List<string>();
        public List<string> ServerRules { get; set; } = new List<string>();
        public List<string> BuildingRules { get; set; } = new List<string>();
        public List<string> BlockedBuildingLocations { get; set; } = new List<string>();
        public List<string> RaidingRules { get; set; } = new List<string>();
        public List<string> DinoPlatformRules { get; set; } = new List<string>();
    }
}
