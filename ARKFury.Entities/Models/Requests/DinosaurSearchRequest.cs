using ArkFury.Common.Models;

namespace ArkFury.Entities.Models.Requests
{
    public class DinosaurSearchRequest : SearchRequestBase
    {

        public DinosaurSearchRequest() : base(true)
        {
        }
        public DinosaurSearchRequest(PagingRequest request)
        {
        }

        public DinosaurSearchRequest(string id) : base(id)
        {
        }

        public DinosaurSearchRequest(bool includeInactive) : base(includeInactive)
        {
        }

        public string Name { get; set; }
        public bool IncludeInActive { get; set; }

    }
}
