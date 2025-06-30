using ArkFury.Entities.DTOs;
using Nest;
using System;

namespace ArkFury.Entities.ElasticSearch
{
    public class ElasticConfiguration : BaseElastic
    {
        public Mysql Mysql { get; set; }
        public General General { get; set; }
        public Kits Kits { get; set; }
        public ShopItems ShopItems { get; set; }
        public SellItems SellItems { get; set; }
        public Messages Messages { get; set; }
    }
}
