using ArkFury.Common.Models;

namespace ArkFury.Entities.Models.Requests
{
    public class LootboxSearchRequest : PagingRequest
    {
        public string Type { get; set; }

        public string SearchTerm { get; set; }

        public string Name { get; set; }

        public bool IncludeInactive { get; set; }
    }
}
