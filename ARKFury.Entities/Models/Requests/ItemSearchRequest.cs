using ArkFury.Common.Constants;
using ArkFury.Common.Models;

namespace ArkFury.Entities.Models.Requests
{
    public class ItemSearchRequest : SearchRequestBase
    {
        public ItemSearchRequest() : base(true)
        {
        }
        public ItemSearchRequest(PagingRequest request)
        {
        }

        public ItemSearchRequest(string id) : base(id)
        {
        }

        public ItemSearchRequest(bool includeInactive) : base(includeInactive)
        {
        }

        public string Name { get; set; }

        public bool IncludeInActive { get; set; }

    }
}
