using System.Collections.Generic;

namespace ArkFury.Entities.LookUps
{
    public static class Qualities
    {
        public static List<Quality> ToList => new List<Quality>
        {
            new Quality { Id = "10b9df3c-700a-4576-ac1e-6120fcaca58a", Name = "Primitive", RandomThreshold = 1, CraftingXP = 1, RepairingXP = 1, CraftingResourcesRequirements = 1 },
            new Quality { Id = "83733e5d-1e2b-4851-94a6-396718152883", Name = "Ramshackle", RandomThreshold = 1.25, CraftingXP = 2, RepairingXP = 2, CraftingResourcesRequirements = 1.33 },
            new Quality { Id = "8528b3bd-3ad9-46f2-99ff-9014cb994b8a", Name = "Apprentice", RandomThreshold = 2.5, CraftingXP = 3, RepairingXP = 3, CraftingResourcesRequirements = 1.67 },
            new Quality { Id = "26907e00-82c2-47e1-b365-1c7c6269ba4f", Name = "Journeyman", RandomThreshold = 4.5, CraftingXP = 4, RepairingXP = 4, CraftingResourcesRequirements = 2 },
            new Quality { Id = "9b64c530-bafd-4a5a-b554-0847207643ee", Name = "Mastercraft", RandomThreshold = 7, CraftingXP = 5, RepairingXP = 5, CraftingResourcesRequirements = 2.5 },
            new Quality { Id = "cf5828ec-7023-48f5-b617-09b8cee2b856", Name = "Ascendant", RandomThreshold = 10, CraftingXP = 6, RepairingXP = 6, CraftingResourcesRequirements = 3.5 },
        };
    }


    public class Quality
    {
        public string Id { get; set; }
        public double? Damage { get; set; }
        public double? Armor { get; set; }
        public double? Durability { get; set; }
        public string Name { get; set; }
        public double RandomThreshold { get; set; }
        public double CraftingXP { get; set; }
        public double RepairingXP { get; set; }
        public double CraftingResourcesRequirements { get; set; }
    }
}
