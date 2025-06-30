using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class TribeDTO : BaseDTO
    {
        public string Name { get; set; }

        public List<PlayerDTO> Members { get; set; }
    }
}
