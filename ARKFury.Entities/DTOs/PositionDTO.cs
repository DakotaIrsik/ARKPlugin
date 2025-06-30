using System;

namespace ArkFury.Entities.DTOs
{
    public class PositionDTO
    {
        public PositionDTO()
        {

        }

        public PositionDTO(string arkRConString)
        {

            X = Convert.ToDouble(arkRConString.Split(' ')[0]);
            Y = Convert.ToDouble(arkRConString.Split(' ')[1]);
            Z = Convert.ToDouble(arkRConString.Split(' ')[2]);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
