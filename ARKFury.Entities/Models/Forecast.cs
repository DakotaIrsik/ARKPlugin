using ArkFury.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkFury.Entities.Models
{
    [Table("Forecast")]
    public class ITem : Trackable
    {
        public int Temperature { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public double WindSpeed { get; set; }
        public int WindChill { get; set; }
    }
}