using System;

namespace ArkFury.Entities.DTOs
{
    public class RollableDTO : BaseDTO
    {
        public void Roll()
        {
            var rnd = new Random();
            TheRoll = rnd.Next(1, 100);
        }

        public int ChanceAsPercentage { get; set; }
        public int TheRoll { get; set; }
    }
}
