using ArkFury.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ArkFury.Common.Models
{
    public class GeoDbPagingRequest : PagingRequest
    {
        [Range(1, Numbers.GeoDbMaximumPageSize, ErrorMessage = "Size is out of Range")]
        public override int Size { get; set; } = Numbers.GeoDbDefaultPageSize;

        [Required]
        [StringLength(20)]
        public string State { get; set; }
    }
}
