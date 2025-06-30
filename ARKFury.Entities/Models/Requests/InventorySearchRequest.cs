using ArkFury.Common.Models;

namespace ArkFury.Entities.Models.Requests
{
    public class InventorySearchRequest : PagingRequest
    {
        public string Term { get; set; }
    }
}
