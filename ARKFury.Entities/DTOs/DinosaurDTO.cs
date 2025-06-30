using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class DinosaurDTO : InventoryDTO
    {
     
        public List<string> Maps = new List<string>();

        public long DinosaurId1 { get; set; }
        public long DinosaurId2 { get; set; }
    }
}