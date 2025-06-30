using ArkFury.Common.Interfaces;
using ArkFury.Common.Models;
using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class ArkShopConfigurationDTO : BaseDTO 
    {
        public string Name { get; set; }
        public Mysql Mysql { get; set; }
        public General General { get; set; }
        public Kits Kits { get; set; }
        public ShopItems ShopItems { get; set; }
        public SellItems SellItems { get; set; }
        public Messages Messages { get; set; }
    }

    public class Mysql
    {
        public bool UseMysql { get; set; }
        public string MysqlHost { get; set; }
        public string MysqlUser { get; set; }
        public string MysqlPass { get; set; }
        public string MysqlDB { get; set; }
    }

    public class Default
    {
        public int Amount { get; set; }
    }

    public class Premiums
    {
        public int Amount { get; set; }
    }

    public class Groups
    {
        public Default Default { get; set; }
        public Premiums Premiums { get; set; }
    }

    public class TimedPointsReward
    {
        public bool Enabled { get; set; }
        public int Interval { get; set; }
        public Groups Groups { get; set; }
    }

    public class General
    {
        public TimedPointsReward TimedPointsReward { get; set; }
        public int ItemsPerPage { get; set; }
        public double ShopDisplayTime { get; set; }
        public double ShopTextSize { get; set; }
        public string DbPathOverride { get; set; }
        public string DefaultKit { get; set; }
    }

    public class Item
    {
        public int Amount { get; set; }
        public int Quality { get; set; }
        public bool ForceBlueprint { get; set; }
        public string Blueprint { get; set; }
    }

    public class Dino
    {
        public int Level { get; set; }
        public string Blueprint { get; set; }
    }

    public class Starter
    {
        public int DefaultAmount { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public bool OnlyFromSpawn { get; set; }
        public List<Item> Items { get; set; }
        public List<Dino> Dinos { get; set; }
    }

    public class Dino2
    {
        public int Level { get; set; }
        public bool Neutered { get; set; }
        public string Blueprint { get; set; }
    }

    public class Vip
    {
        public int DefaultAmount { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; }
        public List<Dino2> Dinos { get; set; }
    }

    public class Item2
    {
        public int Amount { get; set; }
        public int Quality { get; set; }
        public bool ForceBlueprint { get; set; }
        public string Blueprint { get; set; }
    }

    public class Tools
    {
        public int DefaultAmount { get; set; }
        public int Price { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string Description { get; set; }
        public List<Item2> Items { get; set; }
    }

    public class Kits
    {
        public Starter starter { get; set; }
        public Vip vip { get; set; }
        public Tools tools { get; set; }
    }

    public class Item3
    {
        public int Quality { get; set; }
        public bool ForceBlueprint { get; set; }
        public int Amount { get; set; }
        public string Blueprint { get; set; }
    }

    public class Ingots100
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<Item3> Items { get; set; }
    }

    public class Item4
    {
        public int Quality { get; set; }
        public bool ForceBlueprint { get; set; }
        public int Amount { get; set; }
        public string Blueprint { get; set; }
    }

    public class Tools2
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<Item4> Items { get; set; }
    }

    public class Para
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int Price { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string Blueprint { get; set; }
    }

    public class Carno
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int Price { get; set; }
        public bool Neutered { get; set; }
        public string Blueprint { get; set; }
    }

    public class Crate25
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ClassName { get; set; }
    }

    public class Crate2
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ClassName { get; set; }
    }

    public class Exp1000
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public bool GiveToDino { get; set; }
        public int Price { get; set; }
        public double Amount { get; set; }
    }

    public class Item5
    {
        public string Blueprint { get; set; }
    }

    public class Tekengram
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<Item5> Items { get; set; }
    }

    public class Item6
    {
        public string Command { get; set; }
    }

    public class Allengrams
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<Item6> Items { get; set; }
    }

    public class ShopItems
    {
        public Ingots100 ingots100 { get; set; }
        public Tools2 tools { get; set; }
        public Para para { get; set; }
        public Carno carno { get; set; }
        public Crate25 crate25 { get; set; }
        public Crate2 crate2 { get; set; }
        public Exp1000 exp1000 { get; set; }
        public Tekengram tekengram { get; set; }
        public Allengrams allengrams { get; set; }
    }

    public class Metal
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string Blueprint { get; set; }
    }

    public class SellItems
    {
        public Metal metal { get; set; }
    }

    public class Messages
    {
        public string Sender { get; set; }
        public string BoughtItem { get; set; }
        public string BoughtDino { get; set; }
        public string BoughtBeacon { get; set; }
        public string BoughtExp { get; set; }
        public string ReceivedPoints { get; set; }
        public string HavePoints { get; set; }
        public string NoPoints { get; set; }
        public string WrongId { get; set; }
        public string NoPermissionsKit { get; set; }
        public string CantBuyKit { get; set; }
        public string BoughtKit { get; set; }
        public string AvailableKits { get; set; }
        public string NoKits { get; set; }
        public string KitsLeft { get; set; }
        public string NoKitsLeft { get; set; }
        public string CantGivePoints { get; set; }
        public string RidingDino { get; set; }
        public string SentPoints { get; set; }
        public string GotPoints { get; set; }
        public string NoPlayer { get; set; }
        public string FoundMorePlayers { get; set; }
        public string BuyUsage { get; set; }
        public string ShopUsage { get; set; }
        public string KitUsage { get; set; }
        public string BuyKitUsage { get; set; }
        public string TradeUsage { get; set; }
        public string PointsCmd { get; set; }
        public string TradeCmd { get; set; }
        public string BuyCmd { get; set; }
        public string ShopCmd { get; set; }
        public string KitCmd { get; set; }
        public string BuyKitCmd { get; set; }
        public string SellCmd { get; set; }
        public string ShopSellCmd { get; set; }
        public string SellUsage { get; set; }
        public string NotEnoughItems { get; set; }
        public string SoldItems { get; set; }
        public string BadLevel { get; set; }
        public string KitsListPrice { get; set; }
        public string KitsListFormat { get; set; }
        public string StoreListDino { get; set; }
        public string StoreListItem { get; set; }
        public string StoreListFormat { get; set; }
        public string OnlyOnSpawnKit { get; set; }
        public string HelpCmd { get; set; }
        public string ShopMessage { get; set; }
        public string HelpMessage { get; set; }
    }
}
